using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Provides an interface similar to the Windows.Forms.MessageBox.
	/// Call <see cref="DialogWindowBase.Open"/> to display the MessageBox dialog window, or point a <see cref="DialogOpenButton"/> at it.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The DialogMessageBox control requires javascript support in order to function.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following example shows you how to use the <see cref="DialogMessageBox"/> on your page.
	/// <code>
	/// <![CDATA[
	/// <%@ Register TagPrefix="mbdw" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DialogWindow" %>
	/// <script runat="server" language="c#" >
	/// protected void MyDialogWindow_DialogClosed( Object sender, DialogResultEventArgs e ) {
	///		Entry.Text = e.Results;
	/// }
	/// </script>
	/// <form runat="server">
	///		<asp:TextBox runat="server" Id="Entry" />
	///		<mbdw:DialogMessageBox runat="server" Id="MyMessageBox" 
	///			OnDialogClosed="MyDialogWindow_DialogClosed" 
	///			Prompt="What Is Your Answer?" />
	///		<mbdw:DialogOpenButton runat="server" 
	///			DialogToOpen="MyMessageBox" 
	///			Text="Open A MessageBox" />
	/// </form>
	/// ]]>
	/// </code>
	/// </example>
	public class DialogMessageBox : DialogWindowBase
	{

		#region Properties

		/// <summary>
		/// Gets or sets the main text of the messagebox.
		/// </summary>
		[
		Description( "Gets or sets the main text of the messagebox." ),
		MbwcCategory( "Dialog" ),
		DefaultValue( "" ),
		Bindable( true ),
		]
		public virtual String Prompt
		{
			get
			{
				Object savedState = this.ViewState[ "Prompt" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState[ "Prompt" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the title of the messagebox window.
		/// </summary>
		[
		Description( "Gets or sets the title of the messagebox window." ),
		MbwcCategory( "Dialog" ),
		DefaultValue( "" ),
		Bindable( true ),
		]
		public virtual String Title
		{
			get
			{
				Object savedState = this.ViewState[ "Title" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState[ "Title" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the buttons of the messagebox.
		/// </summary>
		[
		Description( "Gets or sets the buttons of the messagebox." ),
		MbwcCategory( "Dialog" ),
		DefaultValue( DialogMessageBoxButtons.OKCancel ),
		Bindable( true ),
		]
		public virtual DialogMessageBoxButtons Buttons
		{
			get
			{
				Object savedState = this.ViewState[ "Buttons" ];
				if ( savedState != null )
				{
					return (DialogMessageBoxButtons)savedState;
				}
				return DialogMessageBoxButtons.OKCancel;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( DialogMessageBoxButtons ), value ) )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}
				this.ViewState[ "Buttons" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the buttons of the messagebox.
		/// </summary>
		[
		Description( "Gets or sets the buttons of the messagebox." ),
		MbwcCategory( "Dialog" ),
		DefaultValue( DialogMessageBoxIcon.None ),
		Bindable( true ),
		]
		public virtual DialogMessageBoxIcon Icon
		{
			get
			{
				Object savedState = this.ViewState[ "Icon" ];
				if ( savedState != null )
				{
					return (DialogMessageBoxIcon)savedState;
				}
				return DialogMessageBoxIcon.None;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( DialogMessageBoxIcon ), value ) )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}
				this.ViewState[ "Icon" ] = value;
			}
		}
		#endregion

		/// <summary>
		/// Generates the script neccessary to open the current messagebox.
		/// </summary>
		/// <remarks>
		/// <p>Calling <see cref="DialogWindowBase.Open"/> will cause this script to be sent to the client browser.</p>
		/// <p>This method can also be used to open the messagebox on the client without postingback.</p>
		/// <p>This method is only public for developers who want to customize how and when the window is opened via code or a custom server control.</p>
		/// </remarks>
		public override String GetDialogOpenScript()
		{

			DialogHandlerFactory.EnsureHandlerFactory();
			StringBuilder url = new StringBuilder();
			url.Append( "~/" );
			url.Append( DialogHandlerFactory.HandlerName );
			url.Append( "?" );
			url.Append( DialogHandlerFactory.HandlerNameKey );
			url.Append( "=DialogMessageBoxPage" );
			url.Append( "&prompt=" );
			url.Append( HttpUtility.UrlEncode( this.Prompt ) );
			if ( this.Title != null && this.Title.Length > 0 )
			{
				url.Append( "&title=" );
				url.Append( HttpUtility.UrlEncode( this.Title ) );
			}
			url.Append( "&icon=" );
			url.Append( this.Icon.ToString() );
			url.Append( "&buttons=" );
			url.Append( this.Buttons.ToString() );
			String finalUrl = this.ResolveUrl( url.ToString() );

			NameValueCollection features = this.GetStandardFeatures();
			Int32 windowHeight = 125;
			if ( this.Prompt.Length > 80 )
			{
				Int32 overLength = this.Prompt.Length - 80;
				windowHeight += ( ( ( overLength / 40 ) + 1 ) * 25 );
			}
			features[ "height" ] = windowHeight.ToString(CultureInfo.InvariantCulture);
			features[ "innerHeight" ] = windowHeight.ToString( CultureInfo.InvariantCulture );
			features[ "width" ] = "300";
			features[ "innerWidth" ] = "300";

			return this.GetDialogOpenScript( finalUrl, features );
		}


	}

}


