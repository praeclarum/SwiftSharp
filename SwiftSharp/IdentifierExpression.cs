using System;
using System.IO;

namespace Swiften
{
	public class IdentifierExpression : Expression
	{
		public string Identifier;

		public IdentifierExpression (string identifier)
		{
			Identifier = identifier;
		}

		public override void AcceptVisitor (IAstVisitor visitor)
		{
			visitor.VisitIdentifierExpression (this);
		}
	}
}

