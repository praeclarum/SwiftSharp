using System;
using IKVM.Reflection.Emit;
using IKVM.Reflection;

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

		public override void VisitExpressionStatement (ExpressionStatement stmt)
		{
			if (stmt.Value == null)
				return;

			stmt.Value.AcceptVisitor (this);
		}

		public override void VisitStringLiteral (StringLiteral lit)
		{
			il.Emit (OpCodes.Ldstr, lit.Value ?? "");
		}

		public override void VisitFunctionCallExpression (FunctionCallExpression expr)
		{
			foreach (var a in expr.Arguments) {
				a.Value.AcceptVisitor (this);
			}
			expr.Value.AcceptVisitor (this);
			il.EmitCalli (
				OpCodes.Calli, 
				System.Runtime.InteropServices.CallingConvention.StdCall, 
				type.Assembly.StringType,
				IKVM.Reflection.Type.EmptyTypes);
		}
	}
}

