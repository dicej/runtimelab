// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Binary;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
    internal partial class NegotiateAuthenticationPal
    {
        public static NegotiateAuthenticationPal Create(NegotiateAuthenticationClientOptions clientOptions)
        {
            return new UnsupportedNegotiateAuthenticationPal(clientOptions);
        }

        public static NegotiateAuthenticationPal Create(NegotiateAuthenticationServerOptions serverOptions)
        {
            return new UnsupportedNegotiateAuthenticationPal(serverOptions);
        }
    }
}
