using System;
using System.Resources;
using System.Web.UI;
using System.Globalization;

namespace MetaBuilders.WebControls {
	
	/// <summary>
	/// The utility class which deals with the clientscript used by the ExpandingButton controls.
	/// </summary>
	/// <remarks>
	/// This class is not neccessary for use of the controls.
	/// It is public in case a developer wants to create their own expanding button that does not derive from the built in ones.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Util" )]
	public static class ExpandingButtonScriptUtil {
		
		/// <summary>
		/// Does the actual regististration of the script with the page.
		/// </summary>
		/// <param name="expander">The control to be registered as an expanding button.</param>
		/// <param name="target">The control to be targeted by the button.</param>
		/// <param name="tracker">The control which tracks, on the clientside, the state of the target control.</param>
		/// <param name="expandedValue">The value of the control in its expanded state.</param>
		/// <param name="contractedValue">The value of the control in its contracted state.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public static void RegisterScriptForControl(Control expander, Control target, Control tracker, String expandedValue, String contractedValue ) {
			if ( expander == null || expander.Page == null || target == null ) {
				return;
			}

			String type = "u";
			if ( expander is System.Web.UI.WebControls.Button ) 
			{
				type = "b";
			} 
			else if ( expander is System.Web.UI.WebControls.LinkButton ) 
			{
				type = "l";
			} 
			else if ( expander is System.Web.UI.WebControls.ImageButton ) 
			{
				type = "i";
			} 
			else if ( expander is System.Web.UI.WebControls.CheckBox ) 
			{
				type = "c";
			}

			String exValue = expandedValue;
			String ctValue = contractedValue;
			if ( !String.IsNullOrEmpty( exValue ) && String.IsNullOrEmpty( ctValue ) ) {
				ctValue = exValue;
			} else if ( !String.IsNullOrEmpty( ctValue ) && String.IsNullOrEmpty( exValue ) ) {
				exValue = ctValue;
			}

			if ( !exValue.StartsWith( "{",	StringComparison.Ordinal ) )
			{
				exValue = "'" + exValue + "'";
			}

			if ( !ctValue.StartsWith( "{", StringComparison.Ordinal ) )
			{
				ctValue = "'" + ctValue + "'";
			}


			String trackerID = "";
			if ( tracker != null ) {
				trackerID = tracker.ClientID;
			}

			ClientScriptManager script = expander.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( ExpandingButtonScriptUtil ), "MetaBuilders.WebControls.Embedded.ExpandingButtonScriptUtil.js" );
			script.RegisterStartupScript( typeof( ExpandingButtonScriptUtil ), scriptKey, "ExpandingButtons_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "ExpandingButtons_Init" ), true );
			script.RegisterArrayDeclaration( arrayName, 
				String.Format( CultureInfo.InvariantCulture, "{{ ID:'{0}', TargetID:'{1}', TrackerID:'{2}', ExValue:{3}, CtValue:{4}, Type:'{5}' }}"
					,expander.ClientID
					,target.ClientID
					,trackerID
					,System.Web.HttpUtility.HtmlEncode(exValue)
					,System.Web.HttpUtility.HtmlEncode(ctValue)
					,type
				));

		}

		private static String scriptKey = typeof(ExpandingButtonScriptUtil).FullName;
		private static String arrayName = "MetaBuilders_WebControls_ExpandingButtons";
		
	}
}
