using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A variation on the DropDownList control with the added ability to type directly into the text portion of the control.
	/// </summary>
	/// <remarks>
	/// <p>On modern browsers that support javascript and css properly, the ComboBox has a similar appearance to the standard DropDownList,
	/// except that the top area accepts direct text entry.
	/// On older browsers, or those that don't support modern standards, 
	/// the ComboBox will appear as two seperate controls, a TextBox, followed by a DropDownList.
	/// </p>
	/// <p>
	/// If you are using a visual designer with the ComboBox, be aware that Grid Mode layout support,
	/// while existing, is limited by the same constraints as the DropDownList control.
	/// Most 'alternate' browsers will not display the drop-down portion of the control correctly.
	/// </p>
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to use the combobox on a page.
	/// <code>
	/// <![CDATA[
	/// <%@ Register TagPrefix="mbcbb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.ComboBox" %>
	/// <html><body><form runat="server">
	///  <mbcbb:ComboBox id="ComboBox1" runat="server" >
	///	  <Items>
	///    <asp:ListItem Value="1">Item 1</asp:ListItem>
	///    <asp:ListItem Value="2">Item 2</asp:ListItem>
	///    <asp:ListItem Value="3">Item 3</asp:ListItem>
	///   </Items>
	///  </mbcbb:ComboBox>
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	[
	Designer( typeof( MetaBuilders.WebControls.Design.ComboBoxDesigner ) ),
	DefaultProperty( "Text" ),
	ValidationProperty( "Text" ),
	ControlValueProperty( "Text" ),
	DefaultEvent( "TextChanged" ),
	]
	public partial class ComboBox : ListControl, INamingContainer, ICompositeControlDesignerAccessor, IPostBackDataHandler
	{

		#region Properties

		/// <exclude />
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Span;
			}
		}

		/// <summary>
		/// Gets or sets the number of rows displayed in the dropdown portion of the <see cref="ComboBox"/> control.
		/// </summary>
		/// <value>
		/// The number of rows displayed in the <see cref="ComboBox"/> control. The default value is 4.
		/// </value>
		[
		Description( "Gets or sets the number of rows of the dropdown portion of the ComboBox control." ),
		Category( "Appearance" ),
		DefaultValue( 8 ),
		Bindable( true ),
		]
		public virtual Int32 Rows
		{
			get
			{
				Object state = this.ViewState[ "Rows" ];
				if ( state != null )
					return (Int32)state;
				return 8;
			}
			set
			{
				if ( value < 1 )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}
				this.ViewState[ "Rows" ] = value;
			}

		}

		/// <summary>
		/// Gets a boolean value indicating whether the string in the text portion of the <see cref="ComboBox"/>
		/// can be found in the text property of any of the ListItems in the Items collection.
		/// </summary>
		[
		Description( "Gets a boolean value indicating whether the string in the text portion of the ComboBox can be found in the text property of any of the ListItems in the Items collection." ),
		Browsable( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )
		]
		public virtual bool TextIsInList
		{
			get
			{
				this.EnsureChildControls();
				return ( Items.FindByText( text.Text ) != null );
			}
		}

		/// <summary>
		/// Gets or sets the template displayed as the button of the combobox.
		/// </summary>
		/// <remarks>
		/// If the ButtonTemplate is not set, a default template is used which
		/// resembles the standard dropdown button in windows xp.
		/// </remarks>
		[
		Description( "Gets or sets the template displayed as the button of the combobox." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		Browsable( false ),
		TemplateContainer( typeof( ComboBox ) ),
		]
		public ITemplate ButtonTemplate
		{
			get
			{
				return buttonTemplate;
			}
			set
			{
				buttonTemplate = value;
				base.ChildControlsCreated = false;
			}
		}
		private ITemplate buttonTemplate;

		/// <exclude />
		[
		DefaultValue( typeof( Unit ), "1px" ),
		]
		public override Unit BorderWidth
		{
			get
			{
				if ( !this.ControlStyleCreated )
				{
					return Unit.Pixel( 1 );
				}
				return this.ControlStyle.BorderWidth;
			}
			set
			{
				this.ControlStyle.BorderWidth = value;
			}
		}

		/// <exclude />
		[
		DefaultValue( typeof( Color ), "ControlDark" ),
		]
		public override Color BorderColor
		{
			get
			{
				if ( !this.ControlStyleCreated )
				{
					return SystemColors.ControlDark;
				}
				return this.ControlStyle.BorderColor;
			}
			set
			{
				this.ControlStyle.BorderColor = value;
			}
		}

		#endregion

		#region Delegated Properties

		/// <summary>
		/// Gets or sets the text content of the text box portion of the <see cref="ComboBox"/> control.
		/// </summary>
		[
		Description( "Gets or sets the text content of the text box portion of the ComboBox control." ),
		Category( "Appearance" ),
		Bindable( true ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Visible ),
		Browsable( true ),
		]
		public override String Text
		{
			get
			{
				Object state = ViewState[ "Text" ];
				if ( state != null )
				{
					return (String)state;
				}
				return "";
			}
			set
			{
				ViewState[ "Text" ] = value;
			}
		}

		/// <exclude/>
		public override Boolean CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
				this.TextControl.CausesValidation = value;
			}
		}

		/// <exclude/>
		[PersistenceMode( PersistenceMode.InnerProperty )]
		public override ListItemCollection Items
		{
			get
			{
				return base.Items;
			}
		}

		/// <exclude/>
		public override int SelectedIndex
		{
			get
			{
				return base.SelectedIndex;
			}
			set
			{
				base.SelectedIndex = value;
				if ( this.isLoaded )
				{
					if ( this.SelectControl.SelectedItem != null )
					{
						this.Text = this.SelectControl.SelectedItem.Text;
					}
					else
					{
						this.Text = String.Empty;
					}
				}
			}
		}

		/// <exclude/>
		public override string SelectedValue
		{
			get
			{
				return base.SelectedValue;
			}
			set
			{
				base.SelectedValue = value;
				if ( this.isLoaded )
				{
					if ( this.SelectControl.SelectedItem != null )
					{
						this.Text = this.SelectControl.SelectedItem.Text;
					}
					else
					{
						this.Text = String.Empty;
					}
				}
			}
		}

		/// <exclude/>
		public override string ValidationGroup
		{
			get
			{
				return base.ValidationGroup;
			}
			set
			{
				base.ValidationGroup = value;
				this.TextControl.ValidationGroup = value;
			}
		}

		#endregion

		#region Hidden Base Properties

		/// <summary>
		/// Hides the Height property, as it not relevant on this control.
		/// </summary>
		[
		EditorBrowsableAttribute( EditorBrowsableState.Never ),
		BindableAttribute( BindableSupport.No ),
		BrowsableAttribute( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )
		]
		public override Unit Height
		{
			get
			{
				return Unit.Empty;
			}
			set
			{
				base.Height = Unit.Empty;
			}
		}

		#endregion

		#region Life Cycle

		/// <exclude/>
		protected override void CreateChildControls()
		{
			this.Controls.Clear();

			base.CreateChildControls();

			container = new WebControl( HtmlTextWriterTag.Span );
			container.EnableViewState = false;
			container.ID = "Container";
			container.Style[ HtmlTextWriterStyle.Position ] = "relative";
			this.Controls.Add( container );

			text = new TextBox();
			text.EnableViewState = false;
			text.ID = "Text";
			text.Attributes[ "autocomplete" ] = "off";
			container.Controls.Add( text );
			text.TextChanged += new EventHandler( this.raiseTextChanged );

			button = new WebControl( HtmlTextWriterTag.Span );
			button.EnableViewState = false;
			button.ID = "Button";
			button.Style[ HtmlTextWriterStyle.Display ] = "none";
			button.Style[ HtmlTextWriterStyle.Cursor ] = "default";
			container.Controls.Add( button );

			if ( this.buttonTemplate != null )
			{
				this.buttonTemplate.InstantiateIn( button );
			}

			select = new ListBox();
			select.EnableViewState = false;
			select.SelectionMode = ListSelectionMode.Single;
			select.ID = "List";
			container.Controls.Add( select );
			select.SelectedIndexChanged += new EventHandler( select_SelectedIndexChanged );
		}

		/// <exclude/>
		protected override Style CreateControlStyle()
		{
			Style baseStyle = base.CreateControlStyle();
			baseStyle.BorderWidth = Unit.Pixel( 1 );
			baseStyle.BorderColor = SystemColors.ControlDark;
			return baseStyle;
		}

		/// <exclude/>
		protected override void LoadViewState( object savedState )
		{
			base.LoadViewState( savedState );

			foreach ( ListItem item in this.Items )
			{
				this.SelectControl.Items.Add( item );
			}
			this.TextControl.Text = this.Text;
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnLoad( System.EventArgs e )
		{
			this.isLoaded = true;
			base.OnLoad( e );
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnSelectedIndexChanged( EventArgs e )
		{
			if ( !this.selectedIndexFired )
			{
				this.fireTextChanged = false;
				base.OnSelectedIndexChanged( e );
				this.fireTextChanged = true;
				this.selectedIndexFired = true;
			}
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnTextChanged( EventArgs e )
		{
			if ( fireTextChanged )
			{
				base.OnTextChanged( e );
			}
		}

		/// <exclude/>
		public override void Focus()
		{
			this.TextControl.Focus();
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnPreRender( System.EventArgs e )
		{

			base.OnPreRender( e );

			if ( this.AutoPostBack )
			{
				this.text.AutoPostBack = true;
			}

			RegisterScript();

			this.Page.RegisterRequiresPostBack( this );
		}

		/// <exclude/>
		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{

			if ( this.Page != null )
			{
				this.Page.VerifyRenderingInServerForm( this );
			}

			base.AddAttributesToRender( writer );
		}

		/// <exclude/>
		protected override void Render( HtmlTextWriter writer )
		{
			this.EnsureChildControls();
			this.SetChildProperties();

			RenderBeginTag( writer );
			RenderChildren( writer );
			RenderEndTag( writer );
		}

		private void SetChildProperties()
		{
			this.container.BackColor = this.BackColor;

			this.text.BackColor = this.BackColor;
			this.text.ForeColor = this.ForeColor;
			this.text.Font.CopyFrom( this.Font );
			this.text.TabIndex = this.TabIndex;
			this.text.Text = this.Text;
			this.text.ValidationGroup = this.ValidationGroup;
			this.text.CausesValidation = this.CausesValidation;
			this.TabIndex = 0;

			this.select.BackColor = this.BackColor;
			this.select.ForeColor = this.ForeColor;
			this.select.BorderColor = this.BorderColor;
			this.select.BorderStyle = this.BorderStyle;
			this.select.BorderWidth = this.BorderWidth;
			this.select.Font.CopyFrom( this.Font );
			this.select.Attributes[ "onchange" ] = "ComboBox_SimpleAttach(this, this.form['" + this.text.UniqueID + "']); ";

			if ( this.button.Controls.Count == 0 )
			{
				DefaultButtonTemplate defaultTemplate = new DefaultButtonTemplate();
				defaultTemplate.InstantiateIn( this.button );
				defaultTemplate.SetImageUrl();
			}

			if ( this.DesignMode )
			{
				this.button.Style[ HtmlTextWriterStyle.Position ] = "relative";
				this.button.Style[ HtmlTextWriterStyle.Top ] = "auto";
				this.button.Style[ "bottom" ] = "1px";
				this.text.BorderWidth = Unit.Pixel( 0 );
				this.text.Style[ HtmlTextWriterStyle.BorderWidth ] = "0px";
			}
			else
			{
				this.select.Items.Clear();
				foreach ( ListItem item in this.Items )
				{
					this.select.Items.Add( item );
				}
			}

		}

		#endregion

		#region ClientScript

		private void RegisterScript()
		{
			String scriptKey = "MetaBuilders.WebControls.ComboBox";
			String arrayDeclaration = @"{ " +
				" ID:'" + this.ClientID + "'" +
				", ContainerID:'" + this.ContainerControl.ClientID + "'" +
				", EntryID:'" + this.TextControl.ClientID + "'" +
				", ListID:'" + this.SelectControl.ClientID + "'" +
				", ButtonID:'" + this.ButtonControl.ClientID + "'" +
				", ListSize:" + this.Rows +
			" }";

			ClientScriptManager script = this.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( ComboBox ), "MetaBuilders.WebControls.Embedded.ComboBoxScript.js" );
			script.RegisterStartupScript( typeof( ComboBox ), scriptKey, "MetaBuilders_ComboBox_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_ComboBox_Init" ), true );
			script.RegisterArrayDeclaration( "MetaBuilders_ComboBoxes", arrayDeclaration );

		}

		#endregion

		#region Private

		private HtmlTextWriter getCorrectTagWriter( HtmlTextWriter writer )
		{

			HtmlTextWriter tagWriter = writer;

			if ( writer is System.Web.UI.Html32TextWriter )
			{
				HttpBrowserCapabilities browser = this.Page.Request.Browser;
				if ( browser.W3CDomVersion.Major > 0 )
				{
					tagWriter = new HtmlTextWriter( writer.InnerWriter );
				}
				else if ( String.Compare( browser.Browser, "netscape", StringComparison.OrdinalIgnoreCase ) == 0 )
				{
					if ( browser.MajorVersion >= 5 )
					{
						tagWriter = new HtmlTextWriter( writer.InnerWriter );
					}
				}
			}

			return tagWriter;
		}

		void select_SelectedIndexChanged( object sender, EventArgs e )
		{
			base.SelectedIndex = this.select.SelectedIndex;
			this.OnSelectedIndexChanged( e );
		}

		private void raiseTextChanged( Object sender, EventArgs e )
		{
			this.Text = this.text.Text;
			OnTextChanged( e );
		}

		/// <summary>
		/// The container control of ComboBox's controls.
		/// </summary>
		/// <remarks>
		/// This is used by the designer.
		/// </remarks>
		protected internal WebControl ContainerControl
		{
			get
			{
				EnsureChildControls();
				return container;
			}
		}

		/// <summary>
		/// The button which activates the dropdownlist portion of the ComboBox.
		/// </summary>
		/// <remarks>This is used by the designer.</remarks>
		protected internal WebControl ButtonControl
		{
			get
			{
				EnsureChildControls();
				return button;
			}
		}

		/// <summary>
		/// The text area of the ComboBox.
		/// </summary>
		/// <remarks>This is used by the designer.</remarks>
		protected internal TextBox TextControl
		{
			get
			{
				EnsureChildControls();
				return text;
			}
		}

		/// <summary>
		/// The select area of the ComboBox.
		/// </summary>
		/// <remarks>This is used by the designer.</remarks>
		protected internal ListBox SelectControl
		{
			get
			{
				this.EnsureChildControls();
				return this.select;
			}
		}

		private WebControl container;
		private WebControl button;
		private TextBox text;
		private ListBox select;

		private Boolean isLoaded = false;
		private Boolean selectedIndexFired = false;
		private Boolean fireTextChanged = true;

		#endregion

		#region Composite Control

		/// <exclude />
		public override void DataBind()
		{
			this.OnDataBinding( EventArgs.Empty );
			this.EnsureChildControls();
			this.DataBindChildren();
		}

		/// <exclude />
		protected virtual void RecreateChildControls()
		{
			base.ChildControlsCreated = false;
			this.EnsureChildControls();
		}

		/// <exclude />
		void ICompositeControlDesignerAccessor.RecreateChildControls()
		{
			this.RecreateChildControls();
		}

		/// <exclude />
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		#endregion

		#region IPostBackDataHandler Members

		bool IPostBackDataHandler.LoadPostData( string postDataKey, NameValueCollection postCollection )
		{
			return this.LoadPostData( postDataKey, postCollection );
		}

		/// <exclude />
		protected virtual Boolean LoadPostData( String postDataKey, NameValueCollection postCollection )
		{
			bool listIndexChanged = false;

			if ( !TextIsInList )
			{
				if ( this.SelectedIndex != -1 )
				{
					listIndexChanged = true;
					this.SelectedIndex = -1;
				}
			}

			isLoaded = true;
			return listIndexChanged;
		}

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected virtual void RaisePostDataChangedEvent()
		{
			this.OnSelectedIndexChanged( EventArgs.Empty );
		}

		#endregion
	}
}
