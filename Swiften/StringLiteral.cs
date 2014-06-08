using System;

namespace Swiften
{
	public class StringLiteral : Literal
	{
		public string Value;

		public StringLiteral (string value)
		{
			Value = value;
		}

		public override void AcceptVisitor (IAstVisitor visitor)
		{
			visitor.VisitStringLiteral (this);
		}
	}
}

