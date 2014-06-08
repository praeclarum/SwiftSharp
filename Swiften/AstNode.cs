using System;
using System.IO;
using System.Dynamic;
using System.Collections.Generic;

namespace Swiften
{
	public abstract class AstNode
	{
		public dynamic Tags = new AstNodeTags ();

		public abstract void AcceptVisitor (IAstVisitor visitor);

		public override string ToString ()
		{
			var sw = new StringWriter ();
			var w = new SwiftWriter (sw);
			this.AcceptVisitor (w);
			return sw.ToString ();
		}
	}

	public class AstNodeTags : DynamicObject
	{
		readonly Dictionary<string, object> expando = new Dictionary<string, object> ();
		public override bool TryGetMember (GetMemberBinder binder, out object result)
		{
			if (!expando.TryGetValue (binder.Name, out result)) {
				result = null;
			}
			return true;
		}
		public override bool TrySetMember (SetMemberBinder binder, object value)
		{
			expando [binder.Name] = value;
			return true;
		}
	}
}

