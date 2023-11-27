// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//////////////////////////////////////////////////////////
// L-1-5-1.cs - Beta1 Layout Test - RDawson
//
// Tests layout of classes using 1-deep nesting in
// the same assembly and module (checking access from an
// unrelated class).
//
// See ReadMe.txt in the same project as this source for
// further details about these tests.
//

using System;
using Xunit;

public class Test_L_2_5_1{
	[Fact]
	public static int TestEntryPoint(){
		int mi_RetCode;
		mi_RetCode = B.Test_L_2_5_1();
		
		if(mi_RetCode == 100)
			Console.WriteLine("Pass");
		else
			Console.WriteLine("FAIL");
		
		return mi_RetCode;
	}
}

struct B{
	public static int Test_L_2_5_1(){
		int mi_RetCode = 100;
		
		A.Cls ac = new A.Cls();
		A a = new A();
		
		if(Test_Nested(ac) != 100)
			mi_RetCode = 0;
		
		//@csharp - C# simply won't compile non-related private/family/protected access
		
		if(Test_Nested(a.ClsPubInst) != 100)
			mi_RetCode = 0;
		
		if(Test_Nested(a.ClsAsmInst) != 100)
			mi_RetCode = 0;

		
		return mi_RetCode;
	}
	
	public static int Test_Nested(A.Cls ac){
		int mi_RetCode = 100;
		
		/////////////////////////////////
		// Test instance field access
		ac.NestFldPubInst = 100;
		if(ac.NestFldPubInst != 100)
			mi_RetCode = 0;
		
		ac.NestFldAsmInst = 100;
		if(ac.NestFldAsmInst != 100)
			mi_RetCode = 0;
		
		/////////////////////////////////
		// Test static field access
		A.Cls.NestFldPubStat = 100;
		if(A.Cls.NestFldPubStat != 100)
			mi_RetCode = 0;
		
		A.Cls.NestFldAsmStat = 100;
		if(A.Cls.NestFldAsmStat != 100)
			mi_RetCode = 0;
		
		/////////////////////////////////
		// Test instance method access  
		if(ac.NestMethPubInst() != 100)
			mi_RetCode = 0;
		
		if(ac.NestMethAsmInst() != 100)
			mi_RetCode = 0;
		
		/////////////////////////////////
		// Test static method access
		if(A.Cls.NestMethPubStat() != 100)
			mi_RetCode = 0;
		
		if(A.Cls.NestMethAsmStat() != 100)
			mi_RetCode = 0;
		
		////////////////////////////////////////////
		// Test access from within the nested class
		if(ac.Test_L_2_5_1() != 100)
			mi_RetCode = 0;
		
		return mi_RetCode;
	}
}

struct A{
	//////////////////////////////
	// Instance Fields
	public int FldPubInst;
	private int FldPrivInst;
	internal int FldAsmInst;           //Translates to "assembly"
	
	//////////////////////////////
	// Static Fields
	public static int FldPubStat;
	private static int FldPrivStat;
	internal static int FldAsmStat;    //assembly
	
	//////////////////////////////////////
	// Instance fields for nested classes
	public Cls ClsPubInst;
	private Cls ClsPrivInst;
	internal Cls ClsAsmInst;
	
	/////////////////////////////////////
	// Static fields of nested classes
	public static Cls ClsPubStat = new Cls();
	private static Cls ClsPrivStat = new Cls();
	
	//////////////////////////////
	// Instance Methods
	public int MethPubInst(){
		Console.WriteLine("A::MethPubInst()");
		return 100;
	}
	
	private int MethPrivInst(){
		Console.WriteLine("A::MethPrivInst()");
		return 100;
	}
	
	internal int MethAsmInst(){
		Console.WriteLine("A::MethAsmInst()");
		return 100;
	}
	
	//////////////////////////////
	// Static Methods
	public static int MethPubStat(){
		Console.WriteLine("A::MethPubStat()");
		return 100;
	}
	
	private static int MethPrivStat(){
		Console.WriteLine("A::MethPrivStat()");
		return 100;
	}
	
	internal static int MethAsmStat(){
		Console.WriteLine("A::MethAsmStat()");
		return 100;
	}
		
	public struct Cls{
		public int Test_L_2_5_1(){
			int mi_RetCode = 100;
			
			/////////////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////////////
			// ACCESS ENCLOSING FIELDS/MEMBERS
			/////////////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////////////
			
			//@csharp - C# will not allow nested classes to access non-static members of their enclosing classes
			
			/////////////////////////////////
			// Test static field access
			FldPubStat = 100;
			if(FldPubStat != 100)
				mi_RetCode = 0;
			
			FldAsmStat = 100;
			if(FldAsmStat != 100)
				mi_RetCode = 0;
			
			/////////////////////////////////
			// Test static method access
			if(MethPubStat() != 100)
				mi_RetCode = 0;
			
			if(MethAsmStat() != 100)
				mi_RetCode = 0;

			// to get rid of compiler warning 
			// warning CS0414: The private field 'A.ClsPrivStat' is assigned but its value is never used
			A.ClsPubStat.ToString();
			A.ClsPrivStat.ToString();
				
			////////////////////////////////////////////
			// Test access from within the nested class
			//@todo - Look at testing accessing one nested class from another, @bugug - NEED TO ADD SUCH TESTING, access the public nested class fields from here, etc...
			
			return mi_RetCode;
		}
		
		//////////////////////////////
		// Instance Fields
		public int NestFldPubInst;
		private int NestFldPrivInst;
		internal int NestFldAsmInst;           //Translates to "assembly"
		
		//////////////////////////////
		// Static Fields
		public static int NestFldPubStat;
		private static int NestFldPrivStat;
		internal static int NestFldAsmStat;    //assembly
		
		//////////////////////////////
		// Instance NestMethods
		public int NestMethPubInst(){
			Console.WriteLine("A::NestMethPubInst()");
			return 100;
		}
		
		private int NestMethPrivInst(){
			Console.WriteLine("A::NestMethPrivInst()");
			return 100;
		}
		
		internal int NestMethAsmInst(){
			Console.WriteLine("A::NestMethAsmInst()");
			return 100;
		}
		
		//////////////////////////////
		// Static NestMethods
		public static int NestMethPubStat(){
			Console.WriteLine("A::NestMethPubStat()");
			return 100;
		}
		
		private static int NestMethPrivStat(){
			Console.WriteLine("A::NestMethPrivStat()");
			return 100;
		}
		
		internal static int NestMethAsmStat(){
			Console.WriteLine("A::NestMethAsmStat()");
			return 100;
		}
		
	}
}








