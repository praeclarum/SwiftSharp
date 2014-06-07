using System;
using System.IO;

namespace Swiften.Compiler
{
	class CompilerApp
	{
		public static void Main (string[] args)
		{
			foreach (var path in args) {
				var lex = new SwiftLexer (File.ReadAllText (path));
				new SwiftParser ().yyparse (lex, new yydebug.yyDebugSimple ());
			}
		}
	}
}
