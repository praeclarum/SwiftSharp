using System;
using System.IO;
using System.Collections.Generic;

namespace Swiften.Compiler
{
	class CompilerApp
	{
		public static void Main (string[] args)
		{
			var c = new AssemblyCompiler ();

			//
			// Parse the command line
			//
			foreach (var path in args) {
				if (path.StartsWith ("-", StringComparison.Ordinal)) {
				} else {
					c.AddFile (path);
				}
			}

			//
			// Build it
			//
			c.Compile ();
		}
	}
}
