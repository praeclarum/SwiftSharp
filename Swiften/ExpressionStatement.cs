using System;

namespace Swiften
{
	public class ExpressionStatement : Statement
	{
		public Expression Value;

		public ExpressionStatement (Expression value)
		{
			Value = value;
		}

		public override void AcceptVisitor (IAstVisitor visitor)
		{
			visitor.VisitExpressionStatement (this);
		}
	}
}

