using System;
using IKVM.Reflection.Emit;
using IKVM.Reflection;
using System.Linq;

using Type = IKVM.Reflection.Type;

namespace Swiften.Compiler
{
	public class MethodCompiler : BaseAstVisitor
	{
		TypeCompiler type;

		readonly MethodBuilder builder;
		readonly ILGenerator il;

		public MethodBuilder Builder {
			get {
				return builder;
			}
		}

		public MethodCompiler (TypeCompiler type, string name, MethodAttributes attribs)
		{
			this.type = type;
			this.builder = type.Builder.DefineMethod (
				name, 
				attribs, 
				type.Assembly.VoidType, new[] {type.Assembly.StringType.MakeArrayType ()});
			this.builder.DefineParameter (1, ParameterAttributes.None, "args");
			this.il = builder.GetILGenerator ();
		}

		public void EndCompilation ()
		{
			if (builder.ReturnType == type.Assembly.VoidType) {
				il.Emit (OpCodes.Ret);
			}
		}

		public override void VisitExpressionStatement (ExpressionStatement stmt)
		{
			if (stmt.Value == null)
				return;


			stmt.Value.AcceptVisitor (this);

			if (GetExpressionType (stmt.Value) != type.Assembly.VoidType) {
				il.Emit (OpCodes.Pop);
			}
		}

		class ExpressionTypeGetter : BaseAstVisitor
		{
			public MethodCompiler M;
			public IKVM.Reflection.Type ExpressionType = null;
			public override void VisitStringLiteral (StringLiteral lit)
			{
				ExpressionType = M.type.Assembly.StringType;
			}
			public override void VisitFunctionCallExpression (FunctionCallExpression expr)
			{
				ExpressionType = M.LookupMethod (expr).ReturnType;
			}
		}

		Type GetExpressionType (Expression expr)
		{
			IKVM.Reflection.Type ty = expr.Tags.Type;
			if (ty == null) {
				var g = new ExpressionTypeGetter {
					M = this,
				};
				expr.AcceptVisitor (g);
				ty = g.ExpressionType;
				expr.Tags.Type = ty;
			}
			return ty;
		}

		public override void VisitStringLiteral (StringLiteral lit)
		{
			il.Emit (OpCodes.Ldstr, lit.Value ?? "");
		}

		public override void VisitFunctionCallExpression (FunctionCallExpression expr)
		{
			MethodInfo m = LookupMethod (expr);

			if (m.GetParameters ().Length != expr.Arguments.Count)
				throw new Exception ("Argument count mismatch " + expr.Value);

			foreach (var a in expr.Arguments) {
				a.Value.AcceptVisitor (this);
			}

			il.EmitCall (OpCodes.Call, m, IKVM.Reflection.Type.EmptyTypes);
		}

		MethodInfo LookupMethod (FunctionCallExpression expr)
		{
			MethodInfo m = expr.Tags.Method;
			if (m != null)
				return m;

			var globalVar = expr.Value as IdentifierExpression;
			if (globalVar != null) {
				var argTypes = expr.Arguments.Select (x => GetExpressionType (x.Value)).ToArray ();
				m = type.Assembly.LookupGlobal (
					globalVar.Identifier,
					argTypes);
				expr.Tags.Method = m;
				return m;
			}
			throw new Exception ("Don't know how to call " + expr.Value);
		}
	}
}

