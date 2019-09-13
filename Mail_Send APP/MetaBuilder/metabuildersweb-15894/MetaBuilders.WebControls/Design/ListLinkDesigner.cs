using System;

namespace MetaBuilders.WebControls.Design
{
	/// <summary>
	/// The Designer for the <see cref="ListLink"/> control.
	/// </summary>
	internal class ListLinkDesigner : System.Web.UI.Design.ControlDesigner
	{


		/// <summary>
		/// Overridden to create a placeholder.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml()
		{
			return this.CreatePlaceHolderDesignTimeHtml();
		}

		/// <summary>
		/// Overridden to disable resizing.
		/// </summary>
		public override bool AllowResize
		{
			get
			{
				return false;
			}
		}

	}
}