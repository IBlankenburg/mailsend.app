using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Allows the client to upload a file to the server.
	/// </summary>
	/// <remarks>
	/// Use the FileUpload server control to handle uploading binary or text files from a browser client to the server.
	/// </remarks>
	/// <example>
	/// The following is an example of using the FileUpload control.
	/// <code><![CDATA[
	/// <%@ Register tagprefix="mbf" namespace="MetaBuilders.WebControls" assembly="MetaBuilders.WebControls.FileUpload" %>
	/// <script language="C#" runat="server">
	/// protected void FileUp_FileReceived( Object sender, EventArgs e ) {
	/// 	if ( FileUp.FileName != "" ) {
	/// 		try {
	/// 			FileUp.PostedFile.SaveAs( System.IO.Path.Combine( Server.MapPath("/"), FileUp.FileName ) );
	/// 			Result.Text = "File Saved";
	/// 		} catch ( Exception ex ) {
	/// 			Result.Text = "File Not Saved: " + ex.Message;
	/// 		}
	/// 	} else {
	/// 		Result.Text = "No File Uploaded";
	/// 	}
	/// }
	/// </script>
	/// <html><body><form runat="server">
	/// <mbf:FileUpload runat="server" OnFileReceived="FileUp_FileReceived" id="FileUp" />
	/// <asp:Button runat="server" text="Smack" />
	/// <br />
	/// <asp:Label runat="server" id="Result" runat="server" EnableViewState="false" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	DefaultEvent( "FileReceived" ),
	ValidationProperty( "FileName" ),
	]
	public class FileUpload : System.Web.UI.WebControls.FileUpload, IPostBackDataHandler
	{

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether an automatic postback to the server will occur whenever the user chooses a file to upload.
		/// </summary>
		[
		Description( "Gets or sets a value indicating whether an automatic postback to the server will occur whenever the user chooses a file to upload." ),
		Bindable( true ),
		Category( "Behavior" ),
		DefaultValue( false ),
		]
		public virtual Boolean AutoPostBack
		{
			get
			{
				Object savedState = this.ViewState[ "AutoPostBack" ];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState[ "AutoPostBack" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum length of the file path for the file to upload from the client computer.
		/// </summary>
		[
		Description( "Gets or sets the maximum length of the file path for the file to upload from the client computer." ),
		Bindable( true ),
		Category( "Behavior" ),
		DefaultValue( -1 ),
		]
		public virtual Int32 MaxLength
		{
			get
			{
				Object savedState = this.ViewState[ "MaxLength" ];
				if ( savedState != null )
				{
					return (Int32)savedState;
				}
				return -1;
			}
			set
			{
				this.ViewState[ "MaxLength" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets a comma-separated list of MIME encodings used to constrain the file types the user can select.
		/// </summary>
		/// <remarks>Use this property to specify the file type that can be uploaded to the server.
		/// For example, to restrict the selection to images, set this property to "image/*".</remarks>
		[
		Description( "Gets or sets a comma-separated list of MIME encodings used to constrain the file types the user can select." ),
		Bindable( true ),
		Category( "Behavior" ),
		DefaultValue( "" ),
		]
		public virtual String Accept
		{
			get
			{
				Object savedState = this.ViewState[ "Accept" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return String.Empty;
			}
			set
			{
				this.ViewState[ "Accept" ] = value;
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// The event which is raised when the client uploads a file to the server.
		/// </summary>
		public event EventHandler FileReceived;

		/// <summary>
		/// Raises the FileReceived event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> for the event.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected virtual void OnFileReceived( EventArgs e )
		{
			if ( FileReceived != null )
			{
				FileReceived( this, e );
			}
		}

		#endregion

		#region Implementation of IPostBackDataHandler

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected virtual void RaisePostDataChangedEvent()
		{
			OnFileReceived( EventArgs.Empty );
		}

		Boolean IPostBackDataHandler.LoadPostData( string postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{
			return this.LoadPostData( postDataKey, postCollection );
		}

		/// <exclude />
		protected virtual Boolean LoadPostData( string postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{
			if ( this.HasFile && !this.postDataLoaded )
			{
				this.postDataLoaded = true;
				return true;
			}
			return false;
		}

		private Boolean postDataLoaded = false;

		#endregion

		#region Life Cycle

		/// <returns>A ControlCollection which does not allow child controls.</returns>
		/// <exclude/>
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection( this );
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnLoad( System.EventArgs e )
		{
			base.OnLoad( e );
			if ( this.Page != null )
			{
				Page.RegisterRequiresPostBack( this );
			}
		}

		/// <exclude />
		protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
		{

			if ( this.AutoPostBack && ( this.Page != null ) )
			{
				String onChangeAttribute = null;

				if ( base.HasAttributes )
				{
					onChangeAttribute = base.Attributes[ "onchange" ];
					if ( onChangeAttribute != null )
					{
						onChangeAttribute += ";";
						base.Attributes.Remove( "onchange" );
					}
				}

				PostBackOptions options = new PostBackOptions( this, string.Empty );
				if ( this.Page.Form != null )
				{
					options.AutoPostBack = true;
				}

				onChangeAttribute = ( onChangeAttribute ?? "" ) + this.Page.ClientScript.GetPostBackEventReference( options, true );

				writer.AddAttribute( HtmlTextWriterAttribute.Onchange, onChangeAttribute );
			}

			if ( this.MaxLength != -1 )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Maxlength, this.MaxLength.ToString(CultureInfo.InstalledUICulture) );
			}
			if ( !String.IsNullOrEmpty( this.Accept ) )
			{
				writer.AddAttribute( "accept", this.Accept );
			}

			base.AddAttributesToRender( writer );


		}

		#endregion

	}
}
