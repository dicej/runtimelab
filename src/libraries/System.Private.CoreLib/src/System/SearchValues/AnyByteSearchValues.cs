﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.Wasm;
using System.Runtime.Intrinsics.X86;

namespace System.Buffers
{
    internal sealed class AnyByteSearchValues : SearchValues<byte>
    {
        private IndexOfAnyAsciiSearcher.AnyByteState _state;

        public AnyByteSearchValues(ReadOnlySpan<byte> values) =>
            IndexOfAnyAsciiSearcher.ComputeAnyByteState(values, out _state);

        internal override byte[] GetValues() =>
            _state.Lookup.GetByteValues();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override bool ContainsCore(byte value) =>
            _state.Lookup.Contains(value);

        [CompExactlyDependsOn(typeof(Ssse3))]
        [CompExactlyDependsOn(typeof(AdvSimd))]
        [CompExactlyDependsOn(typeof(PackedSimd))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int IndexOfAny(ReadOnlySpan<byte> span) =>
            IndexOfAnyAsciiSearcher.IndexOfAnyVectorizedAnyByte<IndexOfAnyAsciiSearcher.DontNegate>(
                ref MemoryMarshal.GetReference(span), span.Length, ref _state);

        [CompExactlyDependsOn(typeof(Ssse3))]
        [CompExactlyDependsOn(typeof(AdvSimd))]
        [CompExactlyDependsOn(typeof(PackedSimd))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int IndexOfAnyExcept(ReadOnlySpan<byte> span) =>
            IndexOfAnyAsciiSearcher.IndexOfAnyVectorizedAnyByte<IndexOfAnyAsciiSearcher.Negate>(
                ref MemoryMarshal.GetReference(span), span.Length, ref _state);

        [CompExactlyDependsOn(typeof(Ssse3))]
        [CompExactlyDependsOn(typeof(AdvSimd))]
        [CompExactlyDependsOn(typeof(PackedSimd))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int LastIndexOfAny(ReadOnlySpan<byte> span) =>
            IndexOfAnyAsciiSearcher.LastIndexOfAnyVectorizedAnyByte<IndexOfAnyAsciiSearcher.DontNegate>(
                ref MemoryMarshal.GetReference(span), span.Length, ref _state);

        [CompExactlyDependsOn(typeof(Ssse3))]
        [CompExactlyDependsOn(typeof(AdvSimd))]
        [CompExactlyDependsOn(typeof(PackedSimd))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override int LastIndexOfAnyExcept(ReadOnlySpan<byte> span) =>
            IndexOfAnyAsciiSearcher.LastIndexOfAnyVectorizedAnyByte<IndexOfAnyAsciiSearcher.Negate>(
                ref MemoryMarshal.GetReference(span), span.Length, ref _state);
    }
}
