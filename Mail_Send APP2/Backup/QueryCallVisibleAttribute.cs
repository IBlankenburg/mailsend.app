using System;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Methods to be called by the QueryCall component must be adornded with this Attribute.
	/// </summary>
	[
	AttributeUsage(AttributeTargets.Method)
	]
	public sealed class QueryCallVisibleAttribute : System.Attribute {
		/// <summary>
		/// Creates a new instance of the QueryCallVisibleAttribute
		/// </summary>
		public QueryCallVisibleAttribute() {}
	}
}
