// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Security;

namespace System.Net
{
    internal sealed class SafeDeleteSslContext : SafeDeleteContext
    {
#pragma warning disable IDE0060
        public SafeDeleteSslContext(SslAuthenticationOptions sslAuthenticationOptions)
            : base(IntPtr.Zero)
        {
        }
#pragma warning restore IDE0060
    }
}
