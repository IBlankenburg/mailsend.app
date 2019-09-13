using System;
using System.Web.UI.Design;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Provides designer services for the <see cref="ParsingContainer"/> control.
	/// </summary>
	public class ParsingContainerDesigner : System.Web.UI.Design.ControlDesigner {
		
		/// <summary>
		/// Overrides <see cref="ControlDesigner.AllowResize"/>.
		/// </summary>
		public override bool AllowResize {
			get {
				return false;
			}
		}

		/// <exclude />
		public override string GetDesignTimeHtml() {
			return this.CreatePlaceHolderDesignTimeHtml("The Content property will define the child controls at runtime.");
		}

	}
}
