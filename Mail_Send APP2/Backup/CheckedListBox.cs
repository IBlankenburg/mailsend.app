using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Represents a list box control that allows single or multiple item selection via CheckBoxes.
	/// </summary>
	/// <remarks>
	/// Use the CheckedListBox control to create a list control that allows single or multiple item selection via CheckBoxes.
	/// The Height property may not have an effect in all browers.
	/// </remarks>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <%@ Page Language="C#" trace="true" Debug="true" %>
	/// <%@ Register TagPrefix="mb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls"%>
	/// <script runat="server">
	///     protected void checkedList_Changed( Object sender, EventArgs e ) {
	///         Result.Text = System.DateTime.Now.ToString();
	///         foreach( ListItem item in MyList.Items ) {
	///             if ( item.Selected ) {
	///                 Result.Text += "<br>" + item.Text;
	///             }
	///         }
	///     }
	///     
	/// </script>
	/// <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
	/// <html>
	///     <head>
	///     </head>
	/// 	<body>
	/// 	<form runat="server">
	/// 
	///         <mb:CheckedListBox runat="server" ToolTip="Pick A Saying" id="MyList" onselectedindexchanged="checkedList_Changed" >
	///             <asp:ListItem value="The quick brown fox" />
	///             <asp:ListItem value="jumped over the lazy dog." />
	///             <asp:ListItem value="The quick brown fox jumped over the lazy dog." />
	///             <asp:ListItem value="The quick brown fox" />
	///             <asp:ListItem value="jumped over the lazy dog." />
	///             <asp:ListItem value="The quick brown fox" />
	///             <asp:ListItem value="jumped over the lazy dog." />
	///         </mb:CheckedListBox>
	///        
	///         <asp:Button runat="server" text="Click Me" />
	/// 		<br />
	/// 		<asp:Label runat="server" id="Result" />
	///         
	/// 	</form>
	/// 	
	/// 	</body>
	/// </html>
	/// ]]>
	/// </code>
	/// </example>
	[
	Designer( typeof( CheckedListBoxDesigner ) ),
	]
	public class CheckedListBox : System.Web.UI.WebControls.ListControl, INamingContainer, IPostBackDataHandler
	{

		/// <summary>
		/// Creates a new instance of the <see cref="CheckedListBox"/> class.
		/// </summary>
		public CheckedListBox()
			: base()
		{
			this.controlToRepeat = new CheckBox();
			this.controlToRepeat.ID = "0";
			this.controlToRepeat.EnableViewState = false;
			this.Controls.Add( this.controlToRepeat );
			this.BorderWidth = Unit.Parse( "2px", CultureInfo.InvariantCulture );
			this.BorderStyle = BorderStyle.Inset;
		}


		#region Properties

		/// <summary>
		/// Gets the Style applied to the checkboxes for the items.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the Style applied to the checkboxes for the items." ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.Attribute ),
		]
		public Style CheckBoxStyle
		{
			get
			{
				if ( _checkBoxStyle == null )
				{
					_checkBoxStyle = new Style();
					if ( IsTrackingViewState )
					{
						( (IStateManager)_checkBoxStyle ).TrackViewState();
					}
				}
				return _checkBoxStyle;
			}
		}
		private Style _checkBoxStyle;

		/// <summary>
		/// Overrides <see cref="System.Web.UI.WebControls.WebControl.TagKey"/>.
		/// </summary>
		/// <exclude/>
		protected override System.Web.UI.HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Div;
			}
		}

		#endregion

		#region ViewState

		/// <exclude/>
		protected override object SaveViewState()
		{
			Object[] state = new Object[ 2 ];
			state[ 0 ] = base.SaveViewState();
			if ( _checkBoxStyle != null )
			{
				state[ 1 ] = ( (IStateManager)_checkBoxStyle ).SaveViewState();
			}
			for ( int i = 0; i < state.Length; i++ )
			{
				if ( state[ i ] != null )
				{
					return state;
				}
			}
			return null;
		}

		/// <exclude/>
		protected override void LoadViewState( object savedState )
		{
			Object[] state = savedState as Object[];
			if ( state == null )
			{
				base.LoadViewState( null );
				return;
			}
			base.LoadViewState( state[ 0 ] );
			if ( state[ 1 ] != null )
			{
				( (IStateManager)this.CheckBoxStyle ).LoadViewState( state[ 1 ] );
			}
		}

		/// <exclude/>
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ( this._checkBoxStyle != null )
			{
				( (IStateManager)this._checkBoxStyle ).TrackViewState();
			}
		}

		#endregion

		#region Implementation of IPostBackDataHandler

		void System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		bool System.Web.UI.IPostBackDataHandler.LoadPostData( string postDataKey, NameValueCollection postCollection )
		{
			return this.LoadPostData( postDataKey, postCollection );
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected virtual void RaisePostDataChangedEvent()
		{
			this.OnSelectedIndexChanged( EventArgs.Empty );
		}

		/// <exclude/>
		protected virtual Boolean LoadPostData( String postDataKey, NameValueCollection postCollection )
		{
			string itemIndexString;
			int itemIndex;
			bool itemSelected;

			itemIndexString = postDataKey.Substring( this.UniqueID.Length + 1 );
			itemIndex = Int32.Parse( itemIndexString, CultureInfo.InvariantCulture );
			if ( itemIndex >= 0 && itemIndex < this.Items.Count )
			{
				itemSelected = ( postCollection[ postDataKey ] != null );
				if ( this.Items[ itemIndex ].Selected != itemSelected )
				{
					this.Items[ itemIndex ].Selected = itemSelected;
					if ( !( this.hasNotifiedOfChange ) )
					{
						this.hasNotifiedOfChange = true;
						return true;
					}
				}
			}
			return false;
		}

		#endregion

		#region Rendering

		/// <summary>
		/// Overrides <see cref="System.Web.UI.Control.OnPreRender"/>
		/// </summary>
		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnPreRender( EventArgs e )
		{
			if ( this.Page != null )
			{
				for ( Int32 itemIndex = 0; itemIndex < this.Items.Count; itemIndex++ )
				{
					if ( this.Items[ itemIndex ].Selected )
					{
						this.controlToRepeat.ID = itemIndex.ToString( NumberFormatInfo.InvariantInfo );
						this.Page.RegisterRequiresPostBack( this.controlToRepeat );
					}
				}
				this.RegisterClientScript();
			}
		}

		/// <summary>
		/// Register the client script for the control with the page.
		/// </summary>
		/// <exclude/>
		protected virtual void RegisterClientScript()
		{

			ClientScriptManager script = this.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( CheckedListBox ), "MetaBuilders.WebControls.Embedded.CheckedListBoxScript.js" );
			script.RegisterArrayDeclaration( "MetaBuilders_CheckedListBoxes", "{ ID:'" + this.ClientID + "', ContainerID:'" + "" + this.ContainerID + "'}" );
			script.RegisterStartupScript( typeof( CheckedListBox ), "CheckedListBox Init", "MetaBuilders_CheckedListBox_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_CheckedListBox_Init" ), true );
		}

		/// <summary>
		/// Overrides <see cref="System.Web.UI.Control.Render"/>
		/// </summary>
		/// <exclude/>
		protected override void Render( HtmlTextWriter writer )
		{
			this.Style[ "overflow" ] = "auto";

			// I move the current font info from this control style
			// to be applied to the inner table because of wacky cascade rules for font styling
			innerTableStyle = new Style();
			innerTableStyle.Font.CopyFrom( this.ControlStyle.Font );

			#region There's no "Reset" method for the FontInfo class, d'oh
			this.ControlStyle.Font.Bold = false;
			this.ControlStyle.Font.Italic = false;
			this.ControlStyle.Font.Name = "";
			this.ControlStyle.Font.Overline = false;
			this.ControlStyle.Font.Size = FontUnit.Empty;
			this.ControlStyle.Font.Strikeout = false;
			this.ControlStyle.Font.Underline = false;
			this.ControlStyle.Font.ClearDefaults();
			#endregion

			short originalTabIndex = this.TabIndex;
			bool tabIndexChanged = false;
			this.controlToRepeat.TabIndex = originalTabIndex;
			if ( originalTabIndex != 0 )
			{
				if ( !( this.ViewState.IsItemDirty( "TabIndex" ) ) )
				{
					tabIndexChanged = true;
				}
				this.TabIndex = 0;
			}

			RenderBeginTag( writer );
			RenderContents( writer );
			RenderEndTag( writer );

			if ( originalTabIndex != 0 )
			{
				this.TabIndex = originalTabIndex;
			}
			if ( tabIndexChanged )
			{
				this.ViewState.SetItemDirty( "TabIndex", false );
			}
		}

		/// <summary>
		/// Overridden to give w3c dom compliant browsers a html4 writer
		/// </summary>
		/// <exclude/>
		public override void RenderBeginTag( System.Web.UI.HtmlTextWriter writer )
		{
			this.tagWriter = null;
			base.RenderBeginTag( getCorrectTagWriter( writer ) );
		}

		/// <summary>
		/// Overrides <see cref="System.Web.UI.WebControls.WebControl.RenderContents"/>
		/// </summary>
		/// <exclude/>
		protected override void RenderContents( System.Web.UI.HtmlTextWriter writer )
		{
			innerTableStyle.AddAttributesToRender( writer );
			writer.AddAttribute( HtmlTextWriterAttribute.Id, this.ContainerID );
			writer.AddAttribute( HtmlTextWriterAttribute.Cellpadding, "0" );
			writer.AddAttribute( HtmlTextWriterAttribute.Cellspacing, "0" );
			writer.AddAttribute( HtmlTextWriterAttribute.Border, "0" );
			writer.RenderBeginTag( HtmlTextWriterTag.Table );

			this.controlToRepeat.Enabled = this.Enabled;
			this.controlToRepeat.AutoPostBack = this.AutoPostBack;

			for ( int i = 0; i < this.Items.Count; i++ )
			{
				writer.RenderBeginTag( "tr" );
				writer.AddAttribute( HtmlTextWriterAttribute.Nowrap, "true" );
				writer.RenderBeginTag( "td" );

				RenderItem( i, writer );

				writer.RenderEndTag();
				writer.RenderEndTag();
			}

			writer.RenderEndTag();
		}

		/// <summary>
		/// Overridden to give w3c dom compliant browsers a html4 writer
		/// </summary>
		/// <exclude/>
		public override void RenderEndTag( System.Web.UI.HtmlTextWriter writer )
		{
			base.RenderEndTag( getCorrectTagWriter( writer ) );
		}

		/// <summary>
		/// Renders one ListItem
		/// </summary>
		/// <param name="repeatIndex">The index of the ListItem to render.</param>
		/// <param name="writer">The <see cref="System.Web.UI.HtmlTextWriter"/> to write to.</param>
		/// <exclude/>
		protected virtual void RenderItem( int repeatIndex, System.Web.UI.HtmlTextWriter writer )
		{
			this.controlToRepeat.ID = repeatIndex.ToString( NumberFormatInfo.InvariantInfo );
			this.controlToRepeat.Text = this.Items[ repeatIndex ].Text;
			this.controlToRepeat.Checked = this.Items[ repeatIndex ].Selected;

			this.CheckBoxStyle.AddAttributesToRender( writer );
			this.controlToRepeat.RenderControl( writer );
		}

		#endregion

		#region Overrides

		/// <exclude/>
		protected override Control FindControl( string id, int pathOffset )
		{
			return this;
		}

		/// <exclude/>
		public override void Focus()
		{
			this.Page.SetFocus( this.ClientID + this.ClientIDSeparator + "0" );
		}

		#endregion

		#region Private
		private CheckBox controlToRepeat;
		private Boolean hasNotifiedOfChange;

		// this is a nice little utility function which lets me use my specified writer for only this control's tag
		private HtmlTextWriter tagWriter;
		private HtmlTextWriter getCorrectTagWriter( HtmlTextWriter writer )
		{
			if ( this.Site != null && this.Site.DesignMode )
			{
				return writer;
			}

			if ( this.tagWriter != null )
			{
				return this.tagWriter;
			}

			this.tagWriter = writer;

			if ( this.Page != null && this.Page.Request != null && writer is System.Web.UI.Html32TextWriter )
			{
				System.Web.HttpBrowserCapabilities browser = this.Page.Request.Browser;
				if ( browser.W3CDomVersion.Major > 0 || ( browser.Browser == "Netscape" && browser.MajorVersion >= 5 ) )
				{
					if ( !( browser.Browser == "Opera" && browser.MajorVersion < 7 ) )
					{
						this.tagWriter = new HtmlTextWriter( writer.InnerWriter );
					}
				}
			}

			return this.tagWriter;
		}

		private String ContainerID
		{
			get
			{
				return this.ClientID + "_Container";
			}
		}

		private Style innerTableStyle;

		#endregion
	}
}
