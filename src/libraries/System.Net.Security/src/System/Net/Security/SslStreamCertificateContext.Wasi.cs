// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security
{
    public partial class SslStreamCertificateContext
    {
        private const bool TrimRootCertificate = true;
        private const bool ChainBuildNeedsTrustedRoot = false;

        private SslStreamCertificateContext(X509Certificate2 target, ReadOnlyCollection<X509Certificate2> intermediates, SslCertificateTrust? trust)
        {
            IntermediateCertificates = intermediates;
            TargetCertificate = target;
            Trust = trust;
            throw new Exception("todo");
        }

        internal static SslStreamCertificateContext Create(X509Certificate2 target) =>
            Create(target, null, offline: false, trust: null, noOcspFetch: true);
    }
}
