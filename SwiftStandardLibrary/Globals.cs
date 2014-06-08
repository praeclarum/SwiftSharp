using System;

namespace SwiftStandardLibrary
{
	public static class Globals
	{
		public static void print<T> (T @object)
		{
			Console.Write (@object);
		}

		public static void println ()
		{
			Console.WriteLine ();
		}

		public static void println<T> (T @object)
		{
			Console.WriteLine (@object);
		}

		public static void Main (string[] args)
		{
		}
	}
}

