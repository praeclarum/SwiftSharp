using System;
using System.Collections.Generic;

namespace Swiften
{
	public partial class SwiftParser
	{
		const int yacc_verbose_flag = 1;

		public static string GetTokenName (int token) { return yyNames[token]; }

		public void Parse (string input)
		{
			var lex = new SwiftLexer (input);

			try {
				var topLevels = (List<Statement>)yyparse (lex, new yydebug.yyDebugSimple ());

				foreach (var t in topLevels) {
					Console.WriteLine (t);
				}

			} catch (InvalidCastException ex) {
				Console.WriteLine (ex);
				throw;
			}
		}

		static List<T> MakeList<T> ()
		{
			return new List<T> ();
		}

		static List<T> MakeList<T> (T firstValue)
		{
			var l = new List<T> ();
			l.Add (firstValue);
			return l;
		}

		static List<T> AddToList<T> (T lastValue, object listObject)
		{
			var l = (List<T>)listObject;
			l.Add (lastValue);
			return l;
		}
	}
}

