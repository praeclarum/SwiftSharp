using System;
using System.IO;

namespace Swiften.Compiler
{
	class CompilerApp
	{
		public static void Main (string[] args)
		{
			foreach (var path in args) {
				new SwiftParser ().Parse (File.ReadAllText (path));
			}
		}
	}
}
