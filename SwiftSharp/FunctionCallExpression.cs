using System;
using System.Collections.Generic;
using System.Linq;

namespace Swiften
{
	public class FunctionCallExpression : Expression
	{
		public Expression Value;

		public List<ExpressionElement> Arguments;

		public FunctionCallExpression (Expression value, IEnumerable<ExpressionElement> arguments)
		{
			Value = value;
			Arguments = arguments.ToList ();
		}

		public override void AcceptVisitor (IAstVisitor visitor)
		{
			visitor.VisitFunctionCallExpression (this);
		}
	}
}

