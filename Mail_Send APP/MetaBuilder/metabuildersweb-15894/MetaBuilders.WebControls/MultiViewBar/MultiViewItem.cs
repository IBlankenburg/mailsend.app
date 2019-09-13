using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {
	
	/// <summary>
	/// Represents a content item in the <see cref="MultiViewBar"/> control.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The <see cref="MultiViewItem"/> control contains the controls shown for each item within the
	/// <see cref="ContentTemplate"/> property. The controls you add to this template are only 
	/// rendered once, unlike the databound controls such as <see cref="DataGrid"/>. However, 
	/// much like the controls within other templates, the item's controls are not accessible directly
	/// from the codebehind for the page. Use FindControl to get access to your controls.
	/// The following shows an example of declaring a simple <see cref="MultiViewBar"/> control and
	/// accessing the controls withing the <see cref="ContentTemplate"/> area of the <see cref="MultiViewItem"/>.
	/// </p>
	/// <example><code>
	/// <![CDATA[
	/// <%@ Register TagPrefix="mb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.MultiViewBar" %>
	/// <script runat="server" language="c#" >
	/// protected void MyButton_Click( Object sender, EventArgs e ) {
	///  Label MyLabel = FindControl( "MyLabel" ) as Label;
	///  MyLabel.Text = "Lorem Ipsum";
	/// }
	/// </script>
	/// <html><body><form runat="server">
	///  <mb:MultiViewBar runat="server" id="MultiViewBar1" >
	///   <Items>
	///    <mb:MultiViewItem Title="Item One" >
	///     <ContentTemplate>
	///      <asp:Button runat="server" Text="Click Me" OnClick="MyButton_Click" />
	///      <asp:Label runat="server" Id="MyLabel" />
	///     </ContentTemplate>
	///    </mb:MultiViewItem>
	///   </Items>
	/// </mb:MultiViewBar>
	/// </form></body></html>
	/// ]]></code></example>
	/// <p>
	/// The <see cref="Title"/> you set is used both in the <see cref="MultiViewBar.CurrentItem"/>
	/// property of the parent <see cref="MultiViewBar"/>, and is displayed in the button which selects
	/// the item.
	/// </p>
	/// <p>The image chosen with the <see cref="ImageUrl"/> property is displayed
	/// next to the title in the item selection button.</p>
	/// </remarks>
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), 
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" ), 
	ParseChildren( true ),
	PersistChildren( false ),
	DefaultProperty( "Title" ),
	ToolboxItem( false ),
	Themeable(true),
	]
	public sealed class MultiViewItem : Control, IPostBackDataHandler {

		#region Properties

		/// <summary>
		/// Gets or sets the title of the item.
		/// </summary>
		/// <remarks>
		/// The Title you set is used both in the <see cref="MultiViewBar.CurrentItem"/>
		/// property of the parent <see cref="MultiViewBar"/>, and is displayed in the button
		/// which selects the item.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the title of the item." ),
		DefaultValue( "" ),
		]
		public String Title {
			get {
				Object savedState = this.ViewState[ "Title" ];
				if ( savedState != null ) {
					return (String)savedState;
				}
				if ( this.ID != null ) {
					return this.ID;
				}
				return this.UniqueID;
			}
			set {
				MultiViewBar parent  = this.Parent as MultiViewBar;
				if ( parent != null ) {
					String currentTitle = this.Title;
					if ( parent.CurrentItem == currentTitle ) {
						parent.CurrentItem = value;
					}
				}
				this.ViewState[ "Title" ] = value;

			}
		}

		/// <summary>
		/// Gets or sets the url of the image displayed on the selection button for the item.
		/// </summary>
		/// <remarks>
		/// The image chosen with the <see cref="ImageUrl"/> property is displayed
		/// next to the title in the item selection button.
		/// </remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Category( "Appearance" ),
		Description( "Gets or sets the url of the image displayed on the selection button for the item." ),
		DefaultValue( "" ),
		System.ComponentModel.Editor( typeof( System.Web.UI.Design.UrlEditor ), typeof( System.Drawing.Design.UITypeEditor ) ),
		]
		public String ImageUrl {
			get {
				Object savedState = this.ViewState[ "ImageUrl" ];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				this.ViewState[ "ImageUrl" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the template displayed for the item.
		/// </summary>
		/// <remarks>
		/// The <see cref="MultiViewItem"/> control contains the controls shown for each item within the
		/// <see cref="ContentTemplate"/> property. The controls you add to this template are only 
		/// rendered once, unlike the databound controls such as <see cref="DataGrid"/>. However, 
		/// much like the controls within other templates, the item's controls are not accessible directly
		/// from the codebehind for the page. Using FindControl from the
		/// <see cref="MultiViewBar"/> or the containing Page will give you access to the controls.
		/// The following shows an example of declaring a simple <see cref="MultiViewBar"/> control and
		/// accessing the controls withing the <see cref="ContentTemplate"/> area of the <see cref="MultiViewItem"/>.
		/// </remarks>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "" ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		]
		public ITemplate ContentTemplate {
			get {
				return contentTemplate;
			}
			set {
				contentTemplate = value;
				this.ChildControlsCreated = false;
				this.EnsureChildControls();
			}
		}
		private ITemplate contentTemplate;

		/// <summary>
		/// Gets the control collection for the content area of the control.
		/// </summary>
		/// <remarks>
		/// Use this control collection to dynamiclly add and remove controls from the same area which
		/// the ContentTemplate applies to.
		/// </remarks>
		[
		Browsable( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ),
		]
		public ControlCollection ContentArea {
			get {
				this.EnsureChildControls();
				return this.contentPanel.Controls;
			}
		}

		/// <summary>
		/// Do not use this control collection to programmatically edit the child controls
		/// of this <see cref="MultiViewItem"/>. Use the <see cref="ContentArea"/>
		/// collection instead.
		/// </summary>
		[
		EditorBrowsable( EditorBrowsableState.Never ),
		]
		public override ControlCollection Controls {
			get {
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		#endregion

		#region Lifecycle

		/// <exclude />
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			contentPanel = new System.Web.UI.WebControls.Panel();
			this.Controls.Add( contentPanel );
			if ( this.contentTemplate != null ) {
				contentTemplate.InstantiateIn( contentPanel );
			}
		}

		/// <exclude />
		public override void DataBind() {
			CreateChildControls();
			ChildControlsCreated = true;
			base.DataBind();
		}

		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			if ( this.EnableScrollTracking ) {
				this.RegisterScrollScript();
				this.Page.RegisterRequiresPostBack( this );
			}
		}

		/// <exclude />
		protected override void Render( HtmlTextWriter writer )
		{
			this.EnsureChildControls();
			if ( this.EnableScrollTracking ) {
				this.contentPanel.Attributes[ "id" ] = this.ClientID + "_ScrollArea";
				this.contentPanel.Style[ "overflow" ] = "auto";
				this.contentPanel.Style[ "height" ] = "100%";
			}
			this.RenderChildren( writer );
		}

		
		private Boolean EnableScrollTracking {
			get {
				MultiViewBar parent = this.Parent as MultiViewBar;
				if ( parent != null && !parent.EnableScrollTracking ) {
					return false;
				}
				
				HttpContext context = HttpContext.Current;
				if ( context != null ) {
					HttpBrowserCapabilities browser = context.Request.Browser;
					return browser.TagWriter.Equals( typeof( HtmlTextWriter ) );
				}

				return false;
			}
		}
		#endregion
		
		#region IPostBackDataHandler
		void IPostBackDataHandler.RaisePostDataChangedEvent() {
			// no event
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) {
			LoadScrollData( postDataKey, postCollection );
			return false;
		}
		#endregion

		#region Scroll Tracking

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "postDataKey" )]
		private void LoadScrollData( string postDataKey, System.Collections.Specialized.NameValueCollection postCollection ) {
			String postedScrollX = postCollection[this.ScrollXName];
			String postedScrollY = postCollection[this.ScrollYName];
			this.scrollX = postedScrollX == null ? "0" : postedScrollX;
			this.scrollY = postedScrollY == null ? "0" : postedScrollY;
		}

		private void RegisterScrollScript() {
			if ( this.Page == null ) {
				return;
			}
			MultiViewBar parent = this.Parent as MultiViewBar;
			if ( parent == null || !parent.EnableClientScript ) {
				return;
			}

			ClientScriptManager script = this.Page.ClientScript;

			script.RegisterHiddenField( this.ScrollXName, this.scrollX );
			script.RegisterHiddenField( this.ScrollYName, this.scrollY );

			script.RegisterClientScriptResource( typeof( MultiViewItem ), "MetaBuilders.WebControls.Embedded.MultiViewItemScript.js" );

			script.RegisterStartupScript( typeof( MultiViewItem ), "startup", "MetaBuilders_MultiViewItem_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_MultiViewItem_Init" ), true );
			script.RegisterArrayDeclaration( "MetaBuilders_MultiViewItems", "'" + this.ClientID + "_ScrollArea'" );
		}


		private String ScrollXName {
			get {
				return this.ClientID + "_ScrollArea_ScrollX";
			}
		}

		private String ScrollYName {
			get {
				return this.ClientID + "_ScrollArea_ScrollY";
			}
		}

		private String scrollX = "0";
		private String scrollY = "0";


		#endregion
	
		private System.Web.UI.WebControls.Panel contentPanel;
	}
}
