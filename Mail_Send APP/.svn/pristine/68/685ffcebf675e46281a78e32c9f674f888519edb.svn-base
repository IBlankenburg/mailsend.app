using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Globalization;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The MultiViewBar control provides a vertical list of templated content items, 
	/// where only one item is visible at a time. Buttons are provided to switch
	/// which item is the visible one.
	/// </summary>
	/// <remarks>
	/// <p>
	/// This UI is similar to the navigation panel found in various versions of Microsoft Outlook.
	/// Use the <see cref="ButtonPlacement"/> property to adjust the layout of the buttons on the bar.
	/// </p>
	/// <p>
	/// In the designer, use the <see cref="Items"/> collection property to add <see cref="MultiViewItem"/> controls.
	/// To add controls to an item, right click on the <see cref="MultiViewBar"/> control,
	/// choose "Edit Templates", and then choose the title of the item you want to edit. Now you will
	/// be able to drag controls from the toolbox into the template editing area for that item.
	/// </p>
	/// <p>
	/// The <see cref="MultiViewItem"/> control contains the controls shown for each item within the
	/// <see cref="MultiViewItem.ContentTemplate"/> property. The controls you add to this template are only 
	/// rendered once, unlike the databound controls such as <see cref="DataGrid"/>. However, 
	/// much like the controls within other templates, the item's controls are not accessible directly
	/// from the codebehind for the page. Use FindControl to get access to your controls.
	/// </p>
	/// </remarks>
	/// <example>
	/// The following is an example <see cref="MultiViewBar"/> where the content needs to be accessed.
	/// <code><![CDATA[
	/// <%@ Register TagPrefix="mb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.MultiViewBar" %>
	/// <script runat="server" language="c#" >
	/// protected void MyButton_Click( Object sender, EventArgs e ) {
	///  Label MyLabel = FindControl( "MyLabel" ) as Label;
	///  MyLabel.Text = "You Clicked The Button";
	/// }
	/// </script>
	/// <html><body><form runat="server">
	///  <mb:MultiViewBar runat="server" id="MultiViewBar1"
	///   BorderWidth="1px" 
	///   BorderColor="Black" 
	///   Font-Name="Arial"
	///   >
	///   <ButtonStyle
	///    BackColor="#708CA9"
	///    ForeColor="Black"
	///   />
	///   <CurrentButtonStyle
	///    BackColor="#FFC080"
	///    Font-Bold="True"
	///   />
	///   <Items>
	///    <mb:MultiViewItem Title="Item One" >
	///     <ContentTemplate>
	///      <asp:Button runat="server" Text="Click Me" OnClick="MyButton_Click" />
	///      <asp:Label runat="server" Id="MyLabel" />
	///     </ContentTemplate>
	///    </mb:MultiViewItem>
	///    <mb:MultiViewItem Title="Item Two" >
	///     <ContentTemplate>
	///      Lorem ipsum dolor sit amet, consectetuer adipiscing elit. 
	///      Nullam rhoncus cursus lorem. 
	///      Class aptent taciti sociosqu ad litora torquent per conubia nostra, 
	///      per inceptos hymenaeos. Sed eu libero eget erat volutpat tempor. 
	///      Pellentesque varius felis ac libero.
	///     </ContentTemplate>
	///    </mb:MultiViewItem>
	///   </Items>
	/// </mb:MultiViewBar>
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), 
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" ), 
	Designer( typeof( Design.MultiViewBarDesigner ) ),
	DefaultProperty( "CurrentItem" ),
	ParseChildren( true ),
	PersistChildren( false ),
	]
	public class MultiViewBar : WebControl, IPostBackEventHandler, IPostBackDataHandler {

		/// <summary>
		/// Creates a new instance of the MultiViewBar.
		/// </summary>
		public MultiViewBar() : base() {
		}

		#region Properties

		/// <exclude />
		protected override HtmlTextWriterTag TagKey
		{
			get {
				return HtmlTextWriterTag.Table;
			}
		}


		/// <summary>
		/// Gets or sets the direction in which the bar renders.
		/// </summary>
		/// <remarks>
		/// <p>
		/// The default value of <see cref="MultiViewLayoutDirection.Vertical"/>
		/// will result in the bar rendering vertically.
		/// </p>
		/// <p>
		/// The value of <see cref="MultiViewLayoutDirection.Horizontal" /> will cause the bar
		/// to render horizontally.
		/// </p>
		/// </remarks>
		[
		Category("Appearance"),
		Description("Gets or sets the direction in which the bar renders."),
		DefaultValue(MultiViewLayoutDirection.Vertical),
		]
		public MultiViewLayoutDirection LayoutDirection {
			get {
				Object state = ViewState["LayoutDirection"];
				if ( state != null ) {
					return (MultiViewLayoutDirection)state;
				}
				return MultiViewLayoutDirection.Vertical;
			}
			set {
				if ( !Enum.IsDefined( typeof( MultiViewLayoutDirection ), value ) ) {
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( MultiViewLayoutDirection ) );
				}
				ViewState["LayoutDirection"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the placement of the item selection buttons on the bar.
		/// </summary>
		/// <remarks>
		/// <p>
		/// The default value of <see cref="MultiViewButtonPlacement.Surrounding"/>
		/// will result in the buttons wrapping around the content,
		/// with the current item's button being directly above the content.
		/// </p>
		/// <p>
		/// The value of <see cref="MultiViewButtonPlacement.Bottom" /> will cause the buttons
		/// to always be at the bottom of the bar, and a header will be displayed at the top
		/// of the bar.
		/// </p>
		/// <p>
		/// The value of <see cref="MultiViewButtonPlacement.Top" /> will cause the buttons
		/// to always be at the top of the bar.
		/// </p>
		/// </remarks>
		[
		Description( "Gets or sets the placement of the buttons on the bar." ),
		Category( "Appearance" ),
		DefaultValue( MultiViewButtonPlacement.Surrounding ),
		]
		public virtual MultiViewButtonPlacement ButtonPlacement {
			get {
				Object state = this.ViewState[ "ButtonPlacement" ];
				if ( state != null ) {
					return (MultiViewButtonPlacement)state;
				}
				return MultiViewButtonPlacement.Surrounding;
			}
			set {
				if ( !Enum.IsDefined( typeof( MultiViewButtonPlacement ), value ) ) {
					throw new ArgumentException( Resources.MultiViewBar_ButtonPlacement_Invalid, "value" );
				}
				this.ViewState[ "ButtonPlacement" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the current <see cref="MultiViewItem"/> by the item's Title.
		/// </summary>
		/// <remarks>If not set, then the first item will become the current item.</remarks>
		[
		Description( "Gets or sets the current MultiViewItem by the item's Title" ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		TypeConverter( typeof( Design.MultiViewBarCurrentItemConverter ) ),
		MergableProperty( false ),
		]
		public virtual String CurrentItem {
			get {
				Object savedState = this.ViewState["CurrentItem"];
				if ( savedState != null  ) {
					return (String)savedState;
				}
				return String.Empty;
			}
			set {
				this.ViewState["CurrentItem"] = value;
			}
		}
		
		/// <summary>
		/// Gets the collection of <see cref="MultiViewItem"/> controls contained by the control.
		/// </summary>
		[
		Description("Gets the collection of MultiViewItems contained by the control."),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		NotifyParentProperty( true ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		MergableProperty( false ),
		]
		public virtual MultiViewItemCollection Items {
			get {
				if ( items == null ) {
					items = new MultiViewItemCollection( this );
				}
				return items;
			}
		}
		private MultiViewItemCollection items;

		/// <summary>
		/// Gets the <see cref="MultiViewItemButtonStyle"/> applied to the button which shows the title of the current <see cref="MultiViewItem"/>
		/// </summary>
		[
		Description("Gets the MultiViewItemButtonStyle applied to the button representing the current MultiViewItem"),
		Category("Style"),
		NotifyParentProperty(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty),
		]
		public virtual MultiViewItemButtonStyle CurrentButtonStyle {
			get {
				if ( currentButtonStyle == null ) {
					currentButtonStyle = new MultiViewItemButtonStyle();
					if ( this.IsTrackingViewState ) {
						((IStateManager)currentButtonStyle).TrackViewState();
					}
				}
				return currentButtonStyle;
			}
		}
		private MultiViewItemButtonStyle currentButtonStyle;

		/// <summary>
		/// Gets the <see cref="MultiViewItemButtonStyle"/> applied to buttons which select the contained <see cref="MultiViewItem"/> controls.
		/// </summary>
		[
		Description("Gets the MultiViewItemButtonStyle applied to buttons representing the contained MultiViewItems"),
		Category("Style"),
		NotifyParentProperty(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty),
		]
		public virtual MultiViewItemButtonStyle ButtonStyle {
			get {
				if ( buttonStyle == null ) {
					buttonStyle = new MultiViewItemButtonStyle();
					if ( this.IsTrackingViewState ) {
						((IStateManager)buttonStyle).TrackViewState();
					}
				}
				return buttonStyle;
			}
		}
		private MultiViewItemButtonStyle buttonStyle;

		/// <summary>
		/// Gets the <see cref="MultiViewItemButtonStyle"/> applied to the header of the <see cref="MultiViewBar"/>.
		/// </summary>
		/// <remarks>
		/// The header is only displayed when the <see cref="ButtonPlacement"/> property
		/// is set to <see cref="MultiViewButtonPlacement.Bottom"/>.
		/// </remarks>
		[
		Description("Gets the MultiViewItemButtonStyle applied to the Header of the MultiViewBar."),
		Category("Style"),
		NotifyParentProperty(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerProperty),
		]
		public virtual MultiViewItemButtonStyle HeaderStyle {
			get {
				if ( headerStyle == null ) {
					headerStyle = new MultiViewItemButtonStyle();
					if ( this.IsTrackingViewState ) {
						((IStateManager)headerStyle).TrackViewState();
					}
				}
				return headerStyle;
			}
		}
		private MultiViewItemButtonStyle headerStyle;

		/// <summary>
		/// Gets or sets if <see cref="MultiViewBar"/> uses clientside script to enhance the control.
		/// </summary>
		[
		Description("Gets or sets if MultiViewBar uses clientside script to enhance the control."),
		Category( "Behavior" ),
		DefaultValue( true ),
		]
		public virtual Boolean EnableClientScript {
			get {
				Object savedState = this.ViewState[ "EnableClientScript" ];
				if ( savedState != null  ) {
					return (Boolean)savedState;
				}
				return true;
			}
			set {
				this.ViewState[ "EnableClientScript" ] = value;
			}
		}

		private String ClientSideCurrentItemTrackerID {
			get {
				return this.UniqueID + ":CurrentItemTracker";
			}
		}

		
		/// <summary>
		/// Gets or sets if MultiViewBar uses clientside script to track the scroll position of items.
		/// </summary>
		/// <remarks>
		/// This property only has an effect if <see cref="EnableClientScript"/> is set to <c>false</c>.
		/// Scrollbars will only appear when the <see cref="WebControl.Height"/> property is set
		/// and browser conditions support it with the current html structure.
		/// </remarks>
		[
		Description("Gets or sets if MultiViewBar uses clientside script to track the scroll position of items."),
		Category( "Behavior" ),
		DefaultValue( false ),
		]
		public virtual Boolean EnableScrollTracking {
			get {
				Object savedState = this.ViewState[ "EnableScrollTracking" ];
				if ( savedState != null  ) {
					return (Boolean)savedState;
				}
				return false;
			}
			set {
				this.ViewState[ "EnableScrollTracking" ] = value;
			}
		}

		#endregion

		#region Events
		
		private static readonly object eventItemsChanged = new Object();
		
		/// <summary>
		/// The event raised when the collection of MultiViewItems has changed.
		/// </summary>
		[
		Description("The event raised when the collection of MultiViewItems has changed."),
		Category("Action"),
		]
		public event EventHandler ItemsChanged {
			add {
				Events.AddHandler( eventItemsChanged, value );
			}
			remove {
				Events.RemoveHandler( eventItemsChanged, value );
			}
		}

		/// <summary>
		/// Raises the ItemsChanged event
		/// </summary>
		protected internal void OnItemsChanged( EventArgs e ) {
			EventHandler itemsChanged = (EventHandler)Events[ eventItemsChanged ];
			if ( itemsChanged != null ) {
				itemsChanged( this, e );
			}
		}

		#endregion

		#region ViewState

		/// <exclude />
		protected override void LoadViewState( object savedState )
		{
			if ( savedState == null ) {
				base.LoadViewState( null );
				return;
			}

			Object[] state = savedState as Object[];
			if ( state == null || state.Length != 4 ) {
				throw new ArgumentException( Resources.InvalidViewState );
			} else {
				base.LoadViewState( state[0] );
				if ( state[1] != null ) {
					((IStateManager)ButtonStyle).LoadViewState( state[1] );
				}
				if ( state[2] != null ) {
					((IStateManager)CurrentButtonStyle).LoadViewState( state[2] );
				}
				if ( state[3] != null ) {
					((IStateManager)HeaderStyle).LoadViewState( state[3] );
				}
			}
		}

		/// <exclude />
		protected override object SaveViewState()
		{
			Object[] state = new Object[4];
			state[0] = base.SaveViewState();
			state[1] = buttonStyle == null ? null : ((IStateManager)buttonStyle).SaveViewState();
			state[2] = currentButtonStyle == null ? null : ((IStateManager)currentButtonStyle).SaveViewState();
			state[2] = this.headerStyle == null ? null : ((IStateManager)headerStyle).SaveViewState();
			for( Int32 i = 0; i < state.Length; i++ ) {
				if ( state[i] != null ) {
					return state;
				}
			}
			return null;
		}

		/// <exclude />
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ( buttonStyle != null ) {
				((IStateManager)buttonStyle).TrackViewState();
			}
			if ( currentButtonStyle != null ) {
				((IStateManager)currentButtonStyle).TrackViewState();
			}
			if ( headerStyle != null ) {
				((IStateManager)headerStyle).TrackViewState();
			}
		}

		#endregion

		#region Lifecycle

		/// <exclude />
		protected override ControlCollection CreateControlCollection()
		{
			return new MultiViewBarControlCollection( this );
		}

		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			if ( this.Page != null ) {
				this.Page.RegisterRequiresPostBack( this );
				if ( this.RenderUplevel ) {
					this.RegisterClientScript();
				}
			}
		}

		/// <exclude />
		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{
			writer.AddAttribute( "cellspacing", "0" );
			writer.AddAttribute( "cellpadding", "0" );
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse");

			if ( !this.BorderWidth.IsEmpty ) {
				writer.AddAttribute( "border", this.BorderWidth.Value.ToString( CultureInfo.InvariantCulture ) );
				writer.AddAttribute( "bordercolor", System.Drawing.ColorTranslator.ToHtml( this.BorderColor ) );
			} else {
				writer.AddAttribute( "border", "0" );
			}
			base.AddAttributesToRender( writer );
		}

		/// <exclude />
		protected override void Render( HtmlTextWriter writer )
		{
			if ( this.Page != null ) {
				this.Page.VerifyRenderingInServerForm( this );
			}
			base.Render( writer );
		}

		/// <exclude />
		public override void RenderBeginTag( HtmlTextWriter writer )
		{
			String currentItemIndex = "0";
			for ( Int32 i = 0; i < this.Items.Count; i++ )
			{
				MultiViewItem item = this.Items[i];
				if ( item.Title == this.CurrentItem )
				{
					currentItemIndex = i.ToString( CultureInfo.InvariantCulture );
					break;
				}
			}

			writer.WriteBeginTag( "input" );
			writer.WriteAttribute( "type", "hidden" );
			writer.WriteAttribute( "value", currentItemIndex );
			writer.WriteAttribute( "id", ClientSideCurrentItemTrackerID );
			writer.WriteAttribute( "name", ClientSideCurrentItemTrackerID );
			writer.Write( " />" );

			base.RenderBeginTag( writer );
		}

		/// <exclude />
		protected override void RenderContents( HtmlTextWriter writer )
		{
			MultiViewContentsRenderer renderer = null;
			switch( this.ButtonPlacement ) {
				case MultiViewButtonPlacement.Surrounding:
					if ( this.RenderUplevel ) {
						renderer = new SurroundButtonUpLevelRenderer( this );
					} else {
						renderer = new SurroundButtonDownLevelRenderer( this );
					}
					break;
				case MultiViewButtonPlacement.Bottom:
					if ( this.RenderUplevel ) {
						renderer = new BottomButtonUpLevelRenderer( this );
					} else {
						renderer = new BottomButtonDownLevelRenderer( this );
					}
					break;
				case MultiViewButtonPlacement.Top:
					if ( this.RenderUplevel ) {
						renderer = new TopButtonUpLevelRenderer( this );
					} else {
						renderer = new TopButtonDownLevelRenderer( this );
					}
					break;
			}
			if ( renderer != null ) {
				renderer.Render( writer );
			}
		}

		private Boolean RenderUplevel {
			get {
				return this.Page != null && this.EnableClientScript && this.UplevelBrowser;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		private Boolean UplevelBrowser {
			get {
				if ( HttpContext.Current == null ) { return true; }

				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return !( browser.TagWriter.Equals( typeof( Html32TextWriter ) ) );
			}
		}


		#endregion

		#region ClientScript

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate" )]
		private static readonly String arrayName = "Page_MetaBuilders_MultiViewBars";

		private void RegisterClientScript() {
			if ( Page == null ) { return; }

			String currentItemIndex = "0";
			for( Int32 i = 0; i < this.Items.Count; i++ ) {
				MultiViewItem item = this.Items[ i ];
				if ( item.Title == this.CurrentItem ) {
					currentItemIndex = i.ToString( CultureInfo.InvariantCulture );
					break;
				}
			}

			ClientScriptManager script = this.Page.ClientScript;
			//script.RegisterHiddenField( ClientSideCurrentItemTrackerID, currentItemIndex );
			script.RegisterClientScriptResource( typeof( MultiViewBar ), "MetaBuilders.WebControls.Embedded.MultiViewBarScript.js" );
			script.RegisterArrayDeclaration( arrayName, "{ ID:'" + this.ClientID + "', Placement:'" + this.ButtonPlacement.ToString() + "', Layout:'" + this.LayoutDirection.ToString() + "', HiddenID:'" + ClientSideCurrentItemTrackerID + "', ItemCount:'" + this.Items.Count.ToString( CultureInfo.InvariantCulture ) + "' }" );
			script.RegisterStartupScript( typeof( MultiViewBar ), "startup", "MetaBuilders_MultiViewBar_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_MultiViewBar_Init" ), true );
		}

		#endregion

		#region IPostBackEventHandler
		
		#region Explicit IPostBackEventHandler

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		void IPostBackEventHandler.RaisePostBackEvent( string eventArgument ) {
			this.RaisePostBackEvent( eventArgument );
		}
		
		#endregion

		#region Overridable IPostBackEventHandler

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		protected virtual void RaisePostBackEvent( String eventArgument )
		{
			this.CurrentItem = eventArgument;
		}
		
		#endregion

		#endregion

		#region IPostBackDataHandler

		#region Explicit IPostBackDataHandler

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		void IPostBackDataHandler.RaisePostDataChangedEvent() {
			this.RaisePostDataChangedEvent();
		}

		Boolean IPostBackDataHandler.LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) {
			return LoadPostData( postDataKey, postCollection );
		}
		
		#endregion

		#region Overridable IPostBackDataHandler

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		protected virtual void RaisePostDataChangedEvent()
		{
			// no event
		}

		/// <exclude />
		protected virtual Boolean LoadPostData( string postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{

			String hiddenInputValue = postCollection[ this.ClientSideCurrentItemTrackerID ];
			if ( hiddenInputValue != null ) {
				MultiViewItem postedItem = this.ItemByIndex( hiddenInputValue );
				if ( postedItem != null && this.CurrentItem != postedItem.Title ) {
					this.CurrentItem = postedItem.Title;
					return true;
				}
			}

			foreach( MultiViewItem item in this.Items ) {
				if ( postCollection[ postDataKey + ":" + item.Title ] != null || postCollection[ postDataKey + ":" + item.Title + "_Image.x" ] != null ) {
					this.CurrentItem = item.Title;
					return true;
				}
			}

			return false;
		}

		private MultiViewItem ItemByIndex( String itemIndex ) {
			try {
				Int32 index = Int32.Parse( itemIndex, CultureInfo.InvariantCulture );
				if ( 0 <= index && index < this.Items.Count ) {
					return this.Items[ index ];
				}
			} catch( System.FormatException ex ) {
				System.Diagnostics.Debug.WriteLine( ex );
			}
			return null;
		}


		#endregion

		#endregion

	}
}
