using System;
using System.Web.UI;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Customizes the building of the <see cref="ParsingContainer"/> control.
	/// </summary>
	public class ParsingContainerControlBuilder : System.Web.UI.ControlBuilder {
		
		/// <summary>
		/// Overrides <see cref="ControlBuilder.AllowWhitespaceLiterals"/>
		/// </summary>
		/// <returns>Returns false.</returns>
		public override bool AllowWhitespaceLiterals() {
			return false;
		}

		/// <summary>
		/// Overrides <see cref="ControlBuilder.HasBody"/>
		/// </summary>
		/// <returns>Returns false.</returns>
		public override bool HasBody() {
			return false;
		}

		/// <summary>
		/// Overrides <see cref="ControlBuilder.HtmlDecodeLiterals"/>
		/// </summary>
		/// <returns>Returns true.</returns>
		public override bool HtmlDecodeLiterals() {
			return true;
		}

	}
}
