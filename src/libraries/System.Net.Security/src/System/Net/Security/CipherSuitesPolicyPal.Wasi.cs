// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Authentication;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security
{
    internal sealed class CipherSuitesPolicyPal
    {
        private readonly List<TlsCipherSuite> _tlsCipherSuites = new List<TlsCipherSuite>();

        internal IEnumerable<TlsCipherSuite> GetCipherSuites() => _tlsCipherSuites;

        internal CipherSuitesPolicyPal(IEnumerable<TlsCipherSuite> allowedCipherSuites)
        {
            foreach (TlsCipherSuite cs in allowedCipherSuites)
            {
                _tlsCipherSuites.Add(cs);
            }
        }
    }
}
