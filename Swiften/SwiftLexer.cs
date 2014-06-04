using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Swiften
{
	public class SwiftLexer : yyParser.yyInput
	{
		string input;

		int p;
		int tok;
		object val;

		#region yyInput implementation

		public SwiftLexer (string input)
		{
			this.input = input;
		}
		
		public bool advance ()
		{
			throw new NotImplementedException ();
		}
		
		public int token ()
		{
			return tok;
		}
		
		public object value ()
		{
			return val;
		}
		
		#endregion
	}

	public class LexException : Exception
	{
		public LexException (string message)
			: base (message)
		{
		}
	}
}

