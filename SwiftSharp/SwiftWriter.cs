using System;
using System.Collections.Generic;
using System.IO;

namespace Swiften
{
	public class SwiftWriter : BaseAstVisitor
	{
		TextWriter w;

		public SwiftWriter (TextWriter writer)
		{
			w = writer;
		}

		public override void VisitStringLiteral (StringLiteral lit)
		{
			w.Write ('"');
			w.Write (lit.Value);
			w.Write ('"');
		}

		public override void VisitFunctionCallExpression (FunctionCallExpression expr)
		{
			if (expr.Value != null) {
				expr.Value.AcceptVisitor (this);
			}
			WriteList (expr.Arguments, "(", ")");
		}

		public override void VisitExpressionElement (ExpressionElement elem)
		{
			if (!string.IsNullOrEmpty (elem.Name)) {
				w.Write (elem);
				w.Write (": ");
			}
			if (elem.Value != null) {
				elem.Value.AcceptVisitor (this);
			}
		}

		public override void VisitExpressionStatement (ExpressionStatement stmt)
		{
			if (stmt.Value != null)
				stmt.Value.AcceptVisitor (this);
		}

		public override void VisitIdentifierExpression (IdentifierExpression expr)
		{
			w.Write (expr.Identifier);
		}

		void WriteList<T> (List<T> nodes, string prefix = null, string postfix = null)
			where T : AstNode
		{
			if (prefix != null)
				w.Write (prefix);

			if (nodes != null) {
				var head = "";
				foreach (var n in nodes) {
					w.Write (head);
					n.AcceptVisitor (this);
					head = ", ";
				}
			}

			if (postfix != null)
				w.Write (postfix);
		}
	}
}

