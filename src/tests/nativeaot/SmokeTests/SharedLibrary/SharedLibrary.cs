// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharedLibrary
{
    public class ClassLibrary
    {
        [UnmanagedCallersOnly(EntryPoint = "ReturnsPrimitiveInt", CallConvs = new Type[] { typeof(CallConvStdcall) })]
        public static int ReturnsPrimitiveInt()
        {
            return 10;
        }

        [UnmanagedCallersOnly(EntryPoint = "ReturnsPrimitiveBool", CallConvs = new Type[] { typeof(CallConvStdcall) })]
        public static bool ReturnsPrimitiveBool()
        {
            return true;
        }

        [UnmanagedCallersOnly(EntryPoint = "ReturnsPrimitiveChar", CallConvs = new Type[] { typeof(CallConvStdcall) })]
        public static char ReturnsPrimitiveChar()
        {
            return 'a';
        }

        [UnmanagedCallersOnly(EntryPoint = "EnsureManagedClassLoaders", CallConvs = new Type[] { typeof(CallConvStdcall) })]
        public static void EnsureManagedClassLoaders()
        {
            Random random = new Random();
            random.Next();
        }

        [UnmanagedCallersOnly(EntryPoint = "CheckSimpleExceptionHandling", CallConvs = new Type[] { typeof(CallConvStdcall) })]
        public static int CheckSimpleExceptionHandling()
        {
            return DoCheckSimpleExceptionHandling();
        }

        public static int DoCheckSimpleExceptionHandling()
        {
            int result = 10;

            try
            {
                Console.WriteLine("Throwing exception");
                throw new Exception();
            }
            catch when (result == 10)
            {
                result += 20;
            }
            finally
            {
                result += 70;
            }

            return result;
        }

        private static bool s_collected;

        class ClassWithFinalizer
        {
            ~ClassWithFinalizer() { s_collected = true; }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void MakeGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            object[] arr = new object[1024 * 1024];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new object();

            new ClassWithFinalizer();
        }

        [UnmanagedCallersOnly(EntryPoint = "CheckSimpleGCCollect", CallConvs = new Type[] { typeof(CallConvStdcall) })]
        public static int CheckSimpleGCCollect()
        {
            return DoCheckSimpleGCCollect();
        }

        public static int DoCheckSimpleGCCollect()
        {
            string myString = string.Format("Hello {0}", "world");

            MakeGarbage();

            Console.WriteLine("Triggering GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return s_collected ? (myString == "Hello world" ? 100 : 1) : 2;
        }
    }
}

// Implements the component model interface defined in wit/world.wit
namespace LibraryWorld {
    public class LibraryWorldImpl : ILibraryWorld {
        public static int ReturnsPrimitiveInt()
        {
            return 10;
        }

        public static bool ReturnsPrimitiveBool()
        {
            return true;
        }

        public static uint ReturnsPrimitiveChar()
        {
            return (uint) 'a';
        }

        public static void EnsureManagedClassLoaders()
        {
            Random random = new Random();
            random.Next();
        }

        public static int CheckSimpleExceptionHandling()
        {
            return SharedLibrary.ClassLibrary.DoCheckSimpleExceptionHandling();
        }

        public static int CheckSimpleGcCollect()
        {
            return SharedLibrary.ClassLibrary.DoCheckSimpleGCCollect();
        }
    }
}
