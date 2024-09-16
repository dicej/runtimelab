// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security
{
    internal static class SslStreamPal
    {
        public static Exception GetException(SecurityStatusPal status)
        {
            return status.Exception ?? new Exception();
        }

        internal const bool StartMutualAuthAsAnonymous = false;
        internal const bool CanEncryptEmptyMessage = false;

        public static void VerifyPackageInfo()
        {
        }

        public static SecurityStatusPal SelectApplicationProtocol(
            SafeFreeCredentials? credentialsHandle,
            SafeDeleteSslContext? context,
            SslAuthenticationOptions sslAuthenticationOptions,
            ReadOnlySpan<byte> clientProtocols)
        {
            throw new PlatformNotSupportedException(nameof(SelectApplicationProtocol));
        }

        public static ProtocolToken AcceptSecurityContext(
            ref SafeFreeCredentials? credential,
            ref SafeDeleteSslContext? context,
            ReadOnlySpan<byte> inputBuffer,
            out int consumed,
            SslAuthenticationOptions sslAuthenticationOptions)
        {
            throw new Exception("todo");
        }

        public static ProtocolToken InitializeSecurityContext(
            ref SafeFreeCredentials? credential,
            ref SafeDeleteSslContext? context,
            string? targetName,
            ReadOnlySpan<byte> inputBuffer,
            out int consumed,
            SslAuthenticationOptions sslAuthenticationOptions)
        {
            // TODO
            _ = (credential, context, targetName, sslAuthenticationOptions);
            _ = inputBuffer;
            consumed = 0;
            ProtocolToken token = default;
            token.Status = new SecurityStatusPal(SecurityStatusPalErrorCode.OK);
            return token;
        }

        public static SafeFreeCredentials? AcquireCredentialsHandle(SslAuthenticationOptions _1, bool _2)
        {
            return null;
        }

        public static ProtocolToken EncryptMessage(SafeDeleteSslContext securityContext, ReadOnlyMemory<byte> input, int _ /*headerSize*/, int _1 /*trailerSize*/)
        {
            throw new Exception("todo");
        }

        public static SecurityStatusPal DecryptMessage(SafeDeleteSslContext securityContext, Span<byte> buffer, out int offset, out int count)
        {
            throw new Exception("todo");
        }

        public static ChannelBinding? QueryContextChannelBinding(SafeDeleteSslContext securityContext, ChannelBindingKind attribute)
        {
            throw new Exception("todo");
        }

        public static ProtocolToken Renegotiate(
            ref SafeFreeCredentials? credentialsHandle,
            ref SafeDeleteSslContext context,
            SslAuthenticationOptions sslAuthenticationOptions)
        {
            throw new Exception("todo");
        }

        public static void QueryContextStreamSizes(SafeDeleteContext? _ /*securityContext*/, out StreamSizes streamSizes)
        {
            streamSizes = StreamSizes.Default;
        }

        public static void QueryContextConnectionInfo(SafeDeleteSslContext securityContext, ref SslConnectionInfo connectionInfo)
        {
            throw new Exception("todo");
        }

        public static bool TryUpdateClintCertificate(
            SafeFreeCredentials? _,
            SafeDeleteSslContext? context,
            SslAuthenticationOptions sslAuthenticationOptions)
        {
            throw new Exception("todo");
        }

        public static SecurityStatusPal ApplyAlertToken(SafeDeleteContext? securityContext, TlsAlertType alertType, TlsAlertMessage alertMessage)
        {
            throw new Exception("todo");
        }

        public static SecurityStatusPal ApplyShutdownToken(SafeDeleteSslContext context)
        {
            throw new Exception("todo");
        }
    }
}
