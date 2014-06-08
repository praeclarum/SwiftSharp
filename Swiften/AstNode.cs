using System;
using System.IO;

namespace Swiften
{
	public abstract class AstNode
	{
		public AstNode ()
		{
		}

		public abstract void AcceptVisitor (IAstVisitor visitor);

		public override string ToString ()
		{
			var sw = new StringWriter ();
			var w = new SwiftWriter (sw);
			this.AcceptVisitor (w);
			return sw.ToString ();
		}
	}
}

