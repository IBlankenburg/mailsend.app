using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web;
using System.Collections.Generic;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Interface for allowing the user to upload multiple files at once.
	/// </summary>
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), 
	Designer( typeof( MultiFileUploadDesigner ) ),
	DefaultEvent( "FilesReceived" ),
	]
	public class MultiFileUpload : CompositeControl
	{

		#region Properties

		/// <summary>
		/// Gets or sets the maximum number of files allowed to be uploaded at once.
		/// </summary>
		[
		Bindable( true ),
		Category( "Behavior" ),
		DefaultValue( _MaxFilesDefault ),
		Description( "Gets or sets the maximum number of files allowed to be uploaded at once." ),
		]
		public virtual Int32 MaxFiles
		{
			get
			{
				Object state = ViewState[ "MaxFiles" ];
				if ( state != null )
				{
					return (Int32)state;
				}
				return _MaxFilesDefault;
			}
			set
			{
				if ( value < 1 )
				{
					throw new ArgumentOutOfRangeException( "value", Resources.MultiFileUpload_MaxFilesGreaterThanOne );
				}
				ViewState[ "MaxFiles" ] = value;
				this.ChildControlsCreated = false;
			}
		}
		private const Int32 _MaxFilesDefault = 5;

		/// <summary>
		/// Gets the files posted by the user with the MultiFileUpload control.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays" ), Browsable( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ),
		]
		public HttpPostedFile[] PostedFiles
		{
			get
			{
				List<HttpPostedFile> files = new List<HttpPostedFile>();
				foreach ( FileUpload upload in this.uploaders )
				{
					if ( upload.HasFile )
					{
						files.Add( upload.PostedFile );
					}
				}
				HttpPostedFile[] result = new HttpPostedFile[ files.Count ];
				files.CopyTo( result );
				return result;
			}
		}

		/// <exclude/>
		[
		Browsable(false),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ),
		]
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Div;
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// The event which is raised when the client uploads files to the server.
		/// </summary>
		public event EventHandler FilesReceived;

		/// <summary>
		/// Raises the FilesReceived event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> for the event.</param>
		protected virtual void OnFilesReceived( EventArgs e )
		{
			if ( FilesReceived != null )
			{
				FilesReceived( this, e );
			}
		}


		#endregion

		#region Life Cycle

		/// <exclude/>
		protected override void CreateChildControls()
		{
			this.Controls.Clear();

			designTable = new Table();
			topRow = new TableRow();
			topCell = new TableCell();
			bottomRow = new TableRow();
			blCell = new TableCell();
			brCell = new TableCell();

			uploaders = new FileUpload[ this.MaxFiles ];
			for ( int i = 0; i < uploaders.Length; i++ )
			{
				FileUpload uploader = new FileUpload();
				uploader.ID = "Uploader" + i.ToString( CultureInfo.InstalledUICulture );
				if ( i > 0 )
				{
					uploader.Style[ HtmlTextWriterStyle.Display ] = "none";
				}
				uploaders[ i ] = uploader;
				uploader.FileReceived += new EventHandler( uploader_FileReceived );
			}

			remover = new Label();
			remover.ID = "RemoveFileUpload";
			remover.Text = Resources.MultiFileUpload_Remove;
			remover.Style[ HtmlTextWriterStyle.TextDecoration ] = "underline";
			remover.Style[ HtmlTextWriterStyle.Cursor ] = "pointer";

			selector = new ListBox();
			selector.ID = "FileSelector";
			selector.Items.Add( Resources.MultiFileUpload_UploadedFilesHeader );

			this.Controls.Add( designTable );
			designTable.Rows.Add( topRow );
			topRow.Cells.Add( topCell );
			foreach ( FileUpload uploader in uploaders )
			{
				topCell.Controls.Add( uploader );
			}
			designTable.Rows.Add( bottomRow );
			bottomRow.Cells.Add( blCell );
			blCell.Controls.Add( selector );
			bottomRow.Cells.Add( brCell );
			brCell.Controls.Add( remover );

			topCell.ColumnSpan = 2;
			brCell.HorizontalAlign = HorizontalAlign.Right;
			brCell.VerticalAlign = VerticalAlign.Top;

		}

		void uploader_FileReceived( object sender, EventArgs e )
		{
			if ( !hasNotifiedOfFilesRecieved )
			{
				this.OnFilesReceived( EventArgs.Empty );
				hasNotifiedOfFilesRecieved = true;
			}
		}
		private Boolean hasNotifiedOfFilesRecieved;

		/// <exclude/>
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );

			if ( Page != null )
			{
				this.EnsureChildControls();
				RegisterClientScript();
			}
		}

		/// <summary>
		/// Registers the clientscript used by the control.
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			StringBuilder arrayDeclaration = new StringBuilder();
			arrayDeclaration.Append( "{ SelectorID:'" );
			arrayDeclaration.Append( this.selector.ClientID );
			arrayDeclaration.Append( "', RemoverID:'" );
			arrayDeclaration.Append( this.remover.ClientID );
			arrayDeclaration.Append( "', Uploaders:[" );
			for ( int i = 0; i < this.uploaders.Length; i++ )
			{
				FileUpload uploader = this.uploaders[ i ];
				if ( i != 0 )
				{
					arrayDeclaration.Append( "," );
				}
				arrayDeclaration.Append( " '" );
				arrayDeclaration.Append( uploader.ClientID );
				arrayDeclaration.Append( "'" );
			}
			arrayDeclaration.Append( "] } " );

			ClientScriptManager script = this.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( MultiFileUpload ), "MetaBuilders.WebControls.Embedded.MultiFileUploadScript.js" );
			script.RegisterArrayDeclaration( "MetaBuilders_MultiFileUploads", arrayDeclaration.ToString() );
			script.RegisterStartupScript( typeof( MultiFileUpload ), "MetaBuilders MultiFileUpload Startup", "MetaBuilders_MultiFileUpload_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_MultiFileUpload_Init" ), true );
		}

		/// <exclude/>
		protected override void Render( HtmlTextWriter writer )
		{
			this.EnsureChildControls();
			this.selector.Rows = ( this.MaxFiles < 10 ) ? this.MaxFiles + 1 : 11;
			if ( this.Width != Unit.Empty )
			{
				this.designTable.Width = Unit.Percentage( 100 );
				this.blCell.Width = Unit.Percentage( 100 );
				foreach ( FileUpload uploader in this.uploaders )
				{
					uploader.Width = Unit.Percentage( 100 );
				}
				this.selector.Width = Unit.Percentage( 100 );
			}
			base.Render( writer );
		}

		#endregion

		internal FileUpload[] uploaders;
		private Label remover;
		private ListBox selector;

		private Table designTable;
		private TableRow topRow;
		private TableCell topCell;
		private TableRow bottomRow;
		private TableCell blCell;
		private TableCell brCell;

	}
}
