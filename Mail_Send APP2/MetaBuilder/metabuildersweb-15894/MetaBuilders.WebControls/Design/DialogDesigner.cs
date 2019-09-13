using System;
using System.Web.UI.Design;
using System.Configuration;
using System.Diagnostics;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>
	/// The designer for DialogWindow controls.
	/// </summary>
	/// <exclude/>
	public class DialogDesigner : System.Web.UI.Design.ControlDesigner
	{

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		public override void Initialize( System.ComponentModel.IComponent component )
		{
			base.Initialize( component );

			try
			{
				IWebApplication webApp = (IWebApplication)Component.Site.GetService( typeof( IWebApplication ) );
				Configuration config = webApp.OpenWebConfiguration( false );
				DialogHandlerFactory.WriteHandlerToConfiguration( config );
			}
			catch ( Exception ex )
			{
				// Gulp
				Debug.Write( ex.ToString() );
			}
		}

		/// <exclude/>
		public override string GetDesignTimeHtml()
		{
			return this.CreatePlaceHolderDesignTimeHtml();
		}

	}
}
