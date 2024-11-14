// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime;
using System.Runtime.CompilerServices;

using Internal.DeveloperExperience;

namespace System.Diagnostics
{
    public partial class StackFrame
    {
        private IntPtr[] _eips;
        private int _eipIndex;

        internal StackFrame(IntPtr[] eips, int eipIndex, bool needFileInfo)
        {
            InitializeForEip(eips, eipIndex, needFileInfo);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe void BuildStackFrameViaNativeUnwind(int frameIndex, bool needFileInfo)
        {
            frameIndex += 1; // We may have a value that has already overflown or will overflow.
            if (frameIndex < 0)
                frameIndex = int.MaxValue;

            // We have to parse the entire stack to get just one frame...
            int eipCount = RuntimeImports.RhpGetCurrentBrowserThreadStackTrace(0, Exception.ReportAllFramesAsJS());
            IntPtr[] eips = new IntPtr[eipCount];
            fixed (void* pEips = eips)
                RuntimeImports.RhpGetCurrentBrowserThreadStackTrace((nuint)pEips, Exception.ReportAllFramesAsJS());

            for (int eipIndex = 0, actualFrameIndex = 0; eipIndex < eipCount; actualFrameIndex++)
            {
                int length = Exception.GetBrowserFrameLengthInChunks(eips[eipIndex]);
                if (frameIndex == actualFrameIndex)
                {
                    // Trim the EIP array to avoid rooting the whole thing.
                    IntPtr[] justFrameEips = eips.AsSpan(eipIndex, length).ToArray();
                    InitializeForEip(justFrameEips, 0, needFileInfo);
                    return;
                }

                eipIndex += length;
            }

            // Frame info not found, build a dummy instance.
            InitializeForIpAddress(IntPtr.Zero, needFileInfo);
        }

        private void InitializeForEip(IntPtr[] eips, int eipIndex, bool needFileInfo)
        {
            IntPtr ip;
            IntPtr eip = eips[eipIndex];
            if (eip == Exception.EdiSeparator)
            {
                ip = Exception.EdiSeparator;
            }
            else
            {
                // We (have to) use the biased function index as IP because "0" is a valid function index.
                if (Exception.GetBrowserFrameInfoWithBias(eip, out int wasmFunctionIndexWithBias) is not 0 ||
                    Exception.IsValidBiasedWasmFunctionIndex(wasmFunctionIndexWithBias))
                {
                    _eips = eips;
                    _eipIndex = eipIndex;
                }
                ip = wasmFunctionIndexWithBias;
            }
            InitializeForIpAddress(ip, needFileInfo);
        }

        private string CreateStackTraceStringForNativeUnwind(out bool isStackTraceHidden)
        {
            Debug.Assert(_ipAddress != Exception.EdiSeparator);
            isStackTraceHidden = false;

            // An unknown frame?
            if (_eips is null)
            {
                return UnknownStackFrameString;
            }

            // A JS frame?
            ReadOnlySpan<char> jsFrame = Exception.GetJSFrame(_eips, _eipIndex);
            if (!jsFrame.IsEmpty)
            {
                // Do a little munging here - our caller expects 'at'-less frames.
                const string At = "at ";
                int atIndex = jsFrame.IndexOf(At.AsSpan());
                if (atIndex >= 0)
                {
                    jsFrame = jsFrame.Slice(atIndex + At.Length);
                }
                return jsFrame.ToString();
            }

            // A known WASM frame?
            string methodName = DeveloperExperience.GetMethodName(_ipAddress, out _, out isStackTraceHidden);
            if (methodName is null)
            {
                // A WASM frame not recorded in our stack trace data (e. g. a runtime helper).
                Exception.GetBrowserFrameInfoWithoutBias(_eips[_eipIndex], out int wasmFunctionIndex);
                methodName = $"wasm-function[{wasmFunctionIndex}]";
            }

            // Add in the "true" IP, the file-relative offset, if available. This allows easily mapping back to
            // the file and line via tools like "emsymbolizer".
            int wasmFunctionOffset = Exception.GetWasmFunctionOffset(_eips, _eipIndex);
            if (wasmFunctionOffset != 0)
            {
                return $"{methodName}:0x{wasmFunctionOffset:x}";
            }
            return methodName;
        }
    }
}
