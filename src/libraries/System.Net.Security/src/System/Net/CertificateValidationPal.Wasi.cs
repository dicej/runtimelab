// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
    internal static partial class CertificateValidationPal
    {
        internal static SslPolicyErrors VerifyCertificateProperties(
            SafeDeleteContext? _ /*securityContext*/,
            X509Chain chain,
            X509Certificate2 remoteCertificate,
            bool checkCertName,
            bool isServer,
            string? hostName)
        {
            throw new Exception("todo");
        }

        //
        // Extracts a remote certificate upon request.
        //
        private static X509Certificate2? GetRemoteCertificate(
            SafeDeleteContext? securityContext,
            bool retrieveChainCertificates,
            ref X509Chain? chain,
            X509ChainPolicy? chainPolicy)
        {
            throw new Exception("todo");
        }

        internal static bool IsLocalCertificateUsed(SafeFreeCredentials? _1, SafeDeleteContext? ctx)
        {
            throw new Exception("todo");
        }

        //
        // Used only by client SSL code, never returns null.
        //
        internal static string[] GetRequestCertificateAuthorities(SafeDeleteContext securityContext)
        {
            throw new Exception("todo");
        }

        static partial void CheckSupportsStore(StoreLocation storeLocation, ref bool hasSupport)
        {
            throw new Exception("todo");
        }

        private static X509Store OpenStore(StoreLocation storeLocation)
        {
            throw new Exception("todo");
        }
    }
}
