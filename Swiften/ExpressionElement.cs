using System;

namespace Swiften
{
	public class ExpressionElement : AstNode
	{
		public string Name;
		public Expression Value;

		public ExpressionElement (string name, Expression value)
		{
			Name = name;
			Value = value;
		}

		public ExpressionElement (Expression value)
		{
			Name = null;
			Value = value;
		}

		public override void AcceptVisitor (IAstVisitor visitor)
		{
			visitor.VisitExpressionElement (this);
		}
	}
}

