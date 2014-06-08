using System;
using System.IO;
using IKVM.Reflection.Emit;
using IKVM.Reflection;
using System.Collections.Generic;

using Type = IKVM.Reflection.Type;
using System.Linq;

namespace Swiften.Compiler
{
	public class AssemblyCompiler : BaseAstVisitor
	{
		readonly Universe universe = new Universe ();
		AssemblyBuilder asm;

		ModuleBuilder module;

		TypeCompiler mainType;

		MethodCompiler mainMethod;

		public TextWriter ErrorOutput = Console.Out;
		public string OutputPath;

		List<string> files = new List<string> ();

		public void AddFile (string rawPath)
		{
			var path = Path.GetFullPath (rawPath);
			files.Add (path);
			if (OutputPath == null) {
				OutputPath = Path.ChangeExtension (path, ".exe");
			}
		}

		bool VerifyConfiguration ()
		{
			if (files.Count == 0) {
				ErrorOutput.WriteLine ("No files specified");
				return false;
			}
			if (string.IsNullOrEmpty (OutputPath)) {
				ErrorOutput.WriteLine ("No output path specified");
				return false;
			}
			return true;
		}

		public IKVM.Reflection.Type StringType {
			get;
			private set;
		}

		public IKVM.Reflection.Type VoidType {
			get;
			private set;
		}

		public IKVM.Reflection.Type SwiftLibType {
			get;
			private set;
		}

		public MethodInfo LookupGlobal (string name, Type[] types)
		{
			var m = SwiftLibType.GetMethod (name, types);

			if (m == null) {

				var ms = SwiftLibType.GetMethods (BindingFlags.Public | BindingFlags.Static)
					.Where (x => x.Name == name);

				foreach (var me in ms) {
					var ps = me.GetParameters ();
					if (ps.Length != types.Length)
						continue;
					var compat = true;
					Dictionary<string, Type> gtypes = null;
					for (int i = 0; compat && i < ps.Length; i++) {
						var p = ps [i].ParameterType;
						if (p.IsGenericParameter) {
							if (gtypes == null)
								gtypes = new Dictionary<string, Type> ();
							if (gtypes.ContainsKey (p.Name)) {
								p = gtypes [p.Name];
							} else {
								gtypes [p.Name] = types [i];
								p = types [i];
							}
						}
						compat = p.IsAssignableFrom (types [i]);
					}
					if (compat) {
						if (gtypes != null) {

							var gargs = me.GetGenericArguments ();
							if (gtypes.Count != gargs.Length) {
								throw new Exception ("Some generic arguments couldn't be inferred");
							}

							return me.MakeGenericMethod (gargs.Select (x => gtypes[x.Name]).ToArray ());
						}

						return me;
					}
				}

				throw new Exception ("Could not find global function " + name + " for arg types " + string.Join (",", types.Select (x => x.ToString ())));
			}

			return m;
		}

		public void Compile ()
		{
			if (!VerifyConfiguration ()) {
				return;
			}

			//
			// Load some assemblies
			//
			var mscorlib = universe.Load ("mscorlib");
			StringType = mscorlib.GetType ("System.String");
			VoidType = mscorlib.GetType ("System.Void");

			var swiftGlobalsType = typeof(SwiftStandardLibrary.Globals);
			var swiftlib = universe.LoadFile (swiftGlobalsType.Assembly.Location);
			SwiftLibType = swiftlib.GetType (swiftGlobalsType.FullName);

			//
			// Create the assembly and the globals class
			//
//			var ext = Path.GetExtension (OutputPath).ToLowerInvariant ();
			var dir = Path.GetDirectoryName (OutputPath);
			var name = new AssemblyName (Path.GetFileNameWithoutExtension (OutputPath));

			asm = universe.DefineDynamicAssembly (name, AssemblyBuilderAccess.Save, dir);
			module = asm.DefineDynamicModule (name.Name, OutputPath);

			mainType = new TypeCompiler (this, module.DefineType (name.Name + ".Globals", TypeAttributes.Public));
			mainMethod = new MethodCompiler (mainType, "Main", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);

			foreach (var i in files) {
				CompileFile (i);
			}

			mainMethod.EndCompilation ();
			mainType.Builder.CreateType ();

			asm.SetEntryPoint (mainMethod.Builder, PEFileKinds.ConsoleApplication);
			asm.Save (OutputPath);
		}

		void CompileFile (string path)
		{
			var topLevels = new SwiftParser ().Parse (File.ReadAllText (path));
			foreach (var s in topLevels) {
				s.AcceptVisitor (this);
			}
		}

		public override void VisitExpressionStatement (ExpressionStatement stmt)
		{
			stmt.AcceptVisitor (mainMethod);
		}
	}

}
