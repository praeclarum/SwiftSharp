using System;

namespace Swiften
{
	public abstract class BaseAstVisitor : IAstVisitor
	{
		public virtual void VisitExpressionElement (ExpressionElement exprElem)
		{
		}

		public virtual void VisitExpressionStatement (ExpressionStatement stmt)
		{
		}

		public virtual void VisitFunctionCallExpression (FunctionCallExpression expr)
		{
		}

		public virtual void VisitIdentifierExpression (IdentifierExpression expr)
		{
		}

		public virtual void VisitStringLiteral (StringLiteral lit)
		{
		}
	}
}

