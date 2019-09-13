using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using System.Web.UI.Design;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Provides a numeric up-down control.
	/// </summary>
	/// <remarks>
	/// This control is generally combined with a compare validator used to ensure only numbers are entered.
	/// </remarks>
	/// <example>
	/// Here is a simple example using the UpDown control.
	/// <code><![CDATA[
	/// <%@ Register TagPrefix="mbud" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.UpDown" %>
	/// <html><body><form runat="server">
	/// <p>
	/// <asp:CompareValidator runat="server" ControlToValidate="UpDown1" Type="Double" Operator="DataTypeCheck" ErrorMessage="Number Required" Display="Static" />
	/// <mbud:UpDown id="UpDown1" runat="server" Increment="1" UpImageUrl="up.gif" DownImageUrl="down.gif" />
	/// <asp:Button runat="server" text="smack" />
	/// </p>
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	DefaultEvent( "TextChanged" ),
	DefaultProperty( "Text" ),
	ValidationPropertyAttribute( "Text" )
	]
	public class UpDown : System.Web.UI.WebControls.WebControl, INamingContainer
	{

		/// <summary>
		/// Creates a new instance of the <see cref="UpDown"/> control.
		/// </summary>
		public UpDown()
			: base()
		{
		}

		#region Style transference
		/// <summary>
		/// Applies <see cref="WebControl.BackColor"/> to the Textbox area of the control.
		/// </summary>
		public override System.Drawing.Color BackColor
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.BackColor;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.BackColor = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.BorderColor"/> to the Textbox area of the control.
		/// </summary>
		public override System.Drawing.Color BorderColor
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.BorderColor;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.BorderColor = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.BorderStyle"/> to the Textbox area of the control.
		/// </summary>
		public override System.Web.UI.WebControls.BorderStyle BorderStyle
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.BorderStyle;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.BorderStyle = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.BorderWidth"/> to the Textbox area of the control.
		/// </summary>
		public override System.Web.UI.WebControls.Unit BorderWidth
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.BorderWidth;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.BorderWidth = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.CssClass"/> to the Textbox area of the control.
		/// </summary>
		public override string CssClass
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.CssClass;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.CssClass = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.Font"/> to the Textbox area of the control.
		/// </summary>
		public override System.Web.UI.WebControls.FontInfo Font
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.Font;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.ForeColor"/> to the Textbox area of the control.
		/// </summary>
		public override System.Drawing.Color ForeColor
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.ForeColor;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.ForeColor = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.Height"/> to the Textbox area of the control.
		/// </summary>
		public override System.Web.UI.WebControls.Unit Height
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.Height;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.Height = value;
			}
		}


		/// <summary>
		/// Applies <see cref="WebControl.TabIndex"/> to the Textbox area of the control.
		/// </summary>
		public override short TabIndex
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.TabIndex;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.TabIndex = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.ToolTip"/> to the Textbox area of the control.
		/// </summary>
		public override string ToolTip
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.ToolTip;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.ToolTip = value;
			}
		}

		/// <summary>
		/// Applies <see cref="WebControl.Width"/> to the Textbox area of the control.
		/// </summary>
		public override System.Web.UI.WebControls.Unit Width
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.Width;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.Width = value;
			}
		}
		#endregion

		/// <summary>
		/// Gets or sets the image to display for the "Up" button.
		/// </summary>
		/// <remarks>Clicking on this image will increment the value.</remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), Editor( "System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor" ),
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" )
		]
		public virtual String UpImageUrl
		{
			get
			{
				this.EnsureChildControls();
				return this.up.ImageUrl;
			}
			set
			{
				this.EnsureChildControls();
				this.up.ImageUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets the image to display for the "Down" button.
		/// </summary>
		/// <remarks>Clicking on this image will decrement the value.</remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), Editor( "System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor" ),
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" )
		]
		public virtual String DownImageUrl
		{
			get
			{
				this.EnsureChildControls();
				return this.down.ImageUrl;
			}
			set
			{
				this.EnsureChildControls();
				this.down.ImageUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets the displayed value of the control.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" )
		]
		public virtual String Text
		{
			get
			{
				this.EnsureChildControls();
				return this.textBox.Text;
			}
			set
			{
				this.EnsureChildControls();
				this.textBox.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the amount which the up or down buttons will change the value.
		/// </summary>
		[
		Bindable( true ),
		Category( "Behavior" ),
		DefaultValue( 1 )
		]
		public virtual Decimal Increment
		{
			get
			{
				Object savedState = this.ViewState["Increment"];
				if ( savedState != null )
				{
					return (Decimal)savedState;
				}
				return 1;
			}
			set
			{
				this.ViewState["Increment"] = value;
			}
		}

		/// <summary>
		/// Occurs when the <see cref="UpDown.Text"/> property has changed.
		/// </summary>
		public event EventHandler TextChanged;

		/// <summary>
		/// Raises the <see cref="UpDown.TextChanged"/> event.
		/// </summary>
		protected virtual void OnTextChanged( EventArgs e )
		{
			if ( this.TextChanged != null )
			{
				this.TextChanged( this, e );
			}
		}

		private void bubbleTextChanged( Object sender, EventArgs e )
		{
			this.OnTextChanged( e );
		}

		private Table layout;
		private TextBox textBox;
		private System.Web.UI.WebControls.HyperLink up;
		private System.Web.UI.WebControls.HyperLink down;

		/// <summary>
		/// Overrides <see cref="Control.CreateChildControls"/>
		/// </summary>
		protected override void CreateChildControls()
		{
			layout = new Table();
			textBox = new TextBox();
			up = new HyperLink();
			down = new HyperLink();

			this.textBox.TextChanged += new EventHandler( bubbleTextChanged );
			this.textBox.Style["text-align"] = "right";
			this.textBox.ID = "textBox";
			this.textBox.Style["margin"] = "0px";

			this.up.Text = Resources.Standard_Up;
			this.down.Text = Resources.Standard_Down;


			layout.CellPadding = 0;
			layout.CellSpacing = 0;
			layout.BorderWidth = 0;
			layout.Rows.Add( new TableRow() );
			layout.Rows.Add( new TableRow() );
			layout.Rows[0].Cells.Add( new TableCell() );
			layout.Rows[0].Cells[0].RowSpan = 2;
			layout.Rows[0].Cells[0].Attributes["valign"] = "middle";
			layout.Rows[0].Cells.Add( new TableCell() );
			layout.Rows[1].Cells.Add( new TableCell() );

			layout.Rows[0].Cells[0].Controls.Add( textBox );
			layout.Rows[0].Cells[1].Controls.Add( up );
			layout.Rows[1].Cells[layout.Rows[1].Cells.Count - 1].Controls.Add( down );

			this.Controls.Add( layout );
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>
		/// </summary>
		protected override void OnPreRender( System.EventArgs e )
		{
			base.OnPreRender( e );
			this.RegisterClientScript();
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>
		/// </summary>
		protected override void Render( HtmlTextWriter writer )
		{
			this.EnsureChildControls();
			if ( String.IsNullOrEmpty( this.up.ImageUrl ) )
			{
				this.up.ImageUrl = this.Page.ClientScript.GetWebResourceUrl( typeof( UpDown ), "MetaBuilders.WebControls.Embedded.up.gif" );
			}
			if ( String.IsNullOrEmpty( this.down.ImageUrl ) )
			{
				this.down.ImageUrl = this.Page.ClientScript.GetWebResourceUrl( typeof( UpDown ), "MetaBuilders.WebControls.Embedded.down.gif" );
			}
			this.layout.Height = this.Height;
			this.textBox.Enabled = this.Enabled;
			this.up.Enabled = this.Enabled;
			this.down.Enabled = this.Enabled;
			base.Render( writer );
		}

		/// <summary>
		/// Registers ClientScript with the Page.
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			ClientScriptManager script = Page.ClientScript;
			script.RegisterClientScriptResource( typeof( UpDown ), "MetaBuilders.WebControls.Embedded.UpDownScript.js" );

			if ( this.Enabled )
			{
				this.up.NavigateUrl = "javascript: UpDown_Increment( '" + this.textBox.UniqueID + "', " + this.Increment.ToString( CultureInfo.InvariantCulture ) + " );";
				this.down.NavigateUrl = "javascript: UpDown_Decrement( '" + this.textBox.UniqueID + "', " + this.Increment.ToString( CultureInfo.InvariantCulture ) + " );";
			}
		}



	}
}
