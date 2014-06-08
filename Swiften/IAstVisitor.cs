using System;

namespace Swiften
{
	public interface IAstVisitor
	{
		void VisitExpressionElement (ExpressionElement elem);
		void VisitExpressionStatement (ExpressionStatement stmt);
		void VisitFunctionCallExpression (FunctionCallExpression expr);
		void VisitIdentifierExpression (IdentifierExpression expr);
		void VisitStringLiteral (StringLiteral lit);
	}
}

