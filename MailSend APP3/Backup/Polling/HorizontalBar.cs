using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Displays a graphical bar, such as in a graph.
	/// </summary>
	internal class HorizontalBar : System.Web.UI.WebControls.WebControl
	{

		/// <summary>
		/// Gets or sets the width of the bar, as a percentage.
		/// </summary>
		[
		Description( "Gets or sets the width of the bar, as a percentage." ),
		Category( "Appearance" ),
		DefaultValue( 0 ),
		Bindable( true ),
		]
		public virtual Double Percentage
		{
			get
			{
				Object savedState = this.ViewState["Percentage"];
				if ( savedState != null )
				{
					return (Double)savedState;
				}
				return 0;
			}
			set
			{
				this.ViewState["Percentage"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the url of the image used as the background of the bar.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Description( "Gets or sets the url of the image used as the background of the bar." ),
		Editor( typeof( ImageUrlEditor ), typeof( UITypeEditor ) ),
		]
		public virtual String BackImageUrl
		{
			get
			{
				if ( ControlStyleCreated )
				{
					return ( (BarStyle)ControlStyle ).BackImageUrl;
				}
				return String.Empty;
			}
			set
			{
				( (BarStyle)ControlStyle ).BackImageUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets the url of the image used as the foreground, or filled-in area, of the bar.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Description( "Gets or sets the url of the image used as the foreground, or filled-in area, of the bar." ),
		Editor( typeof( ImageUrlEditor ), typeof( UITypeEditor ) ),
		]
		public virtual String ForeImageUrl
		{
			get
			{
				if ( ControlStyleCreated )
				{
					return ( (BarStyle)ControlStyle ).ForeImageUrl;
				}
				return String.Empty;
			}
			set
			{
				( (BarStyle)ControlStyle ).ForeImageUrl = value;
			}
		}

		/// <summary>
		/// Creates a <see cref="BarStyle"/> for this control.
		/// </summary>
		protected override Style CreateControlStyle()
		{
			return new BarStyle( ViewState );
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		protected override void Render( System.Web.UI.HtmlTextWriter writer )
		{
			Table bar = new Table();
			bar.BorderColor = this.BorderColor;
			bar.BorderStyle = this.BorderStyle;
			bar.BorderWidth = this.BorderWidth;
			if ( this.Width.IsEmpty )
			{
				bar.Width = Unit.Percentage( 100 );
			}
			else
			{
				bar.Width = this.Width;
			}
			bar.Height = this.Height;
			bar.CellSpacing = 0;
			bar.CellPadding = 0;
			bar.BackImageUrl = this.BackImageUrl;

			TableRow barRow = new TableRow();
			barRow.BackColor = this.BackColor;
			bar.Rows.Add( barRow );

			TableCell barCell = new TableCell();
			barCell.BorderWidth = Unit.Pixel( 0 );
			barRow.Cells.Add( barCell );

			Boolean spaceRequired = false;
			Boolean divRequired = false;
			HttpContext context = HttpContext.Current;
			if ( context != null )
			{
				HttpBrowserCapabilities browser = context.Request.Browser;
				if ( browser.Browser.ToUpperInvariant() == "NETSCAPE" && browser.MajorVersion < 5 )
				{
					// death to netscape4 and it's disapearing-cell behavior.
					spaceRequired = true;
				}
				if ( browser.Browser.ToUpperInvariant() == "NETSCAPE" && browser.MajorVersion >= 5 )
				{
					divRequired = true;
				}
			}

			if ( Percentage != 0 )
			{

				Table fillTable = new Table();
				fillTable.Rows.Add( new TableRow() );
				fillTable.Rows[0].Cells.Add( new TableCell() );
				barCell.Controls.Add( fillTable );

				if ( spaceRequired )
				{
					fillTable.Rows[0].Cells[0].Controls.Add( new LiteralControl( "&nbsp;" ) );
				}

				fillTable.BorderWidth = Unit.Pixel( 0 );
				fillTable.Height = this.Height;
				fillTable.CellPadding = 0;
				fillTable.CellSpacing = 0;
				fillTable.Width = Unit.Percentage( this.Percentage );
				fillTable.BackColor = this.ForeColor;
				fillTable.BackImageUrl = this.ForeImageUrl;
			}
			else
			{
				if ( spaceRequired )
				{
					barCell.Controls.Add( new LiteralControl( "&nbsp;" ) );
				}
				if ( divRequired )
				{
					barCell.Controls.Add( new LiteralControl( "<div style='height:" + this.Height.ToString( System.Globalization.CultureInfo.InvariantCulture ) + ";' ></div>" ) );
				}
			}


			bar.RenderControl( writer );
		}


	}
}
