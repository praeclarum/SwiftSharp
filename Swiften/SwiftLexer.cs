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
		
		public int token ()
		{
			return tok;
		}

		public object value ()
		{
			return val;
		}

		public bool advance ()
		{
			if (p >= input.Length)
				return false;

			SkipWhiteSpace ();

			if (p >= input.Length)
				return false;

			var ch = input[p];

			switch (ch) {
			case '(':
			case ')':
			case '[':
			case ']':
			case '{':
			case '}':
			case ':':
			case ';':
				tok = (int)ch;
				val = null;
				p++;
				break;
			case '\"':
				LexString ();
				break;
			default:
				if (char.IsDigit (ch)) {
					LexNumber ();
				} else if (IsIdentifierHead (ch)) {
					LexIdentifier ();
				} else {
					throw new LexException ("Unrecognized character '" + ch + "' (" + (int)ch + ")");
				}
				break;
			}

			Console.WriteLine ("lex:");
			Console.WriteLine ("  tok: " + SwiftParser.GetTokenName (tok));
			Console.WriteLine ("  val: " + val);

			return true;
		}

		#endregion

		void SkipWhiteSpace ()
		{
			while (p < input.Length && char.IsWhiteSpace (input[p])) {
				p++;
			}
		}

		static bool IsIdentifierHead (char ch)
		{
			return ch == '_' || char.IsLetter (ch);
		}

		static bool IsIdentifierChar (char ch)
		{
			return ch == '_' || char.IsLetterOrDigit (ch);
		}

		void LexString ()
		{
//			var hasQuotedItem = false;

			p++; // Skip "
			var startIndex = p;

			while (p < input.Length && input [p] != '"') {
				var ch = input [p];
				if (ch == '\\') {
//					hasQuotedItem = (p + 1 < input.Length && input [p + 1] == '(');
					p += 2;
				} else {
					p++;
				}
			}
			var length = p - startIndex;
			if (p < input.Length) {
				p++; // Consume trailing "
			}

			tok = Token.STRING_LITERAL;
			val = input.Substring (startIndex, length);
		}

		void LexNumber ()
		{
			throw new NotImplementedException ();
		}

		void LexIdentifier ()
		{
			var startIndex = p;

			while (p < input.Length && IsIdentifierChar (input [p])) {
				p++;
			}

			var length = p - startIndex;

			if (!LexKeyword (startIndex, length)) {
				tok = Token.IDENTIFIER;
				val = input.Substring (startIndex, length);
			}
		}

		bool LexKeyword (int startIndex, int length)
		{
			return false;
		}
	}

	public class LexException : Exception
	{
		public LexException (string message)
			: base (message)
		{
		}
	}
}

