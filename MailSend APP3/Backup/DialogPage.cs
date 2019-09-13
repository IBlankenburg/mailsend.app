using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections.Specialized;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// DialogPage is the base class for pages which are functioning as dialogs.
	/// </summary>
	/// <remarks>
	/// When using the <see cref="DialogWindow"/> control, 
	/// the <see cref="DialogWindow.TargetUrl"/> should point to a page which derives,
	/// directly or indirectly, from <see cref="DialogPage"/>.
	/// </remarks>
	/// <example>
	/// The following example shows you a typical <see cref="DialogPage"/>.
	/// <code>
	/// <![CDATA[
	/// <%@ Page Language="C#" Inherits="MetaBuilders.WebControls.DialogPage"  %>
	/// <script runat="server">
	/// protected void Smack_Click(Object sender, EventArgs e ) {
	///		this.Close(Results.Text);
	/// }
	/// </script>
	/// <html><body><form runat="server">
	///		<asp:TextBox runat="server" Id="Results" />
	///		<asp:Button runat="server" Id="Smack" Text="Smack" OnClick="Smack_Click" />
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	public class DialogPage : Page
	{

		/// <exclude/>
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			RegisterScript();
		}

		/// <summary>
		/// Registers the neccessary script for the dialog to function.
		/// </summary>
		protected virtual void RegisterScript()
		{
			Page.ClientScript.RegisterClientScriptBlock( typeof( DialogPage ), scriptKey, @"
function MetaBuilders_DialogWindow_Close(results) {
	if ( window.opener != null && typeof( window.opener.MetaBuilders_DialogWindow_DoDialogPostBack) != 'undefined' ) {
		window.opener.MetaBuilders_DialogWindow_DoDialogPostBack(results);
	} else {
		window.alert('The page that this dialog belonged to is no longer available. The results may not be what was expected');
	}
	window.close();
}", true );
		}

		/// <summary>
		/// Gets the script required to close the dialog with the given results.
		/// </summary>
		/// <remarks>
		/// Simply call <see cref="DialogPage.Close(String)"/> to close the dialog from the server code.
		/// Only call this method to get the script neccessary to close the dialog from a client script event handler.
		/// </remarks>
		/// <param name="results">The results which will be available in the <see cref="DialogWindowBase.DialogClosed"/> event.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		public String GetDialogCloseScript( String results )
		{
			results = results ?? "";
			return @"MetaBuilders_DialogWindow_Close('" + results.Replace( @"\", @"\\\\" ).Replace( "\r", "\\\\r" ).Replace( "\n", "\\\\n" ).Replace( "'", @"\'" ) + @"');";
		}

		/// <summary>
		/// Gets the script required to close the dialog with default results.
		/// </summary>
		/// <remarks>
		/// Simply call <see cref="DialogPage.Close(String)"/> to close the dialog from the server code.
		/// Only call this method to get the script neccessary to close the dialog from a client script event handler.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		public String GetDialogCloseScript()
		{
			return @"MetaBuilders_DialogWindow_Close(null);";
		}

		/// <summary>
		/// Closes the dialog, and returns the given results for the <see cref="DialogWindowBase.DialogClosed"/> event.
		/// </summary>
		/// <param name="results">The results which will be available in the <see cref="DialogWindowBase.DialogClosed"/> event.</param>
		public void Close( String results )
		{
			String script = this.GetDialogCloseScript( results );
			Page.ClientScript.RegisterStartupScript( typeof( DialogPage ), scriptKey, script, true );
		}

		/// <summary>
		/// Closes the dialog, and returns the default results.
		/// </summary>
		public void Close()
		{
			Page.ClientScript.RegisterStartupScript( typeof( DialogPage ), scriptKey, GetDialogCloseScript(), true );
		}

		private String scriptKey = "MetaBuilders.WebControls.DialogPage";

	}
}
