using System;
using System.IO;
using IKVM.Reflection.Emit;
using IKVM.Reflection;
using System.Collections.Generic;

namespace Swiften.Compiler
{
	public class TypeCompiler
	{
		public TypeBuilder Builder;

		public AssemblyCompiler Assembly;

		public TypeCompiler (AssemblyCompiler asm, TypeBuilder typeBuilder)
		{
			Assembly = asm;
			Builder = typeBuilder;
		}
	}


}
