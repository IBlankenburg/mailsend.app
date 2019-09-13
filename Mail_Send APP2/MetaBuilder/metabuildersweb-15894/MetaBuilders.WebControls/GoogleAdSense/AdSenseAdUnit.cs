using System;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web.Configuration;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A Google AdSense AdUnit
	/// </summary>
	[
	Designer( typeof( MetaBuilders.WebControls.Design.AdSenseAdUnitDesigner ) ),
	]
	public class AdSenseAdUnit : AdSenseContentAd
	{

		#region Appearance Properties

		/// <summary>
		/// Gets or sets the format of the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( _FormatDefault ),
		Description( "Gets or sets the format of the ad." ),
		]
		public virtual AdUnitFormat Format
		{
			get
			{
				Object state = ViewState["Format"];
				if ( state != null )
				{
					return (AdUnitFormat)state;
				}
				return _FormatDefault;
			}
			set
			{
				ViewState["Format"] = value;
			}
		}
		private const AdUnitFormat _FormatDefault = AdUnitFormat.HorizontalLeaderboard;


		/// <summary>
		/// Gets or sets the type of content displayed within the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( _ContentDefault ),
		Description( "Gets or sets the type of content displayed within the ad." ),
		]
		public virtual AdUnitContent Content
		{
			get
			{
				Object state = ViewState["Content"];
				if ( state != null )
				{
					return (AdUnitContent)state;
				}
				return _ContentDefault;
			}
			set
			{
				ViewState["Content"] = value;
			}
		}
		private const AdUnitContent _ContentDefault = AdUnitContent.TextAndImages;

		/// <summary>
		/// Gets or sets the color of the border around the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( typeof( Color ), "Empty" ),
		Description( "Gets or sets the color of the border." ),
		]
		public virtual Color BorderColor
		{
			get
			{
				Object state = ViewState["BorderColor"];
				if ( state != null )
				{
					return (Color)state;
				}
				return _BorderColorDefault;
			}
			set
			{
				ViewState["BorderColor"] = value;
			}
		}
		private Color _BorderColorDefault = Color.Empty;


		/// <summary>
		/// Gets or sets the color of the title of the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( typeof( Color ), "Empty" ),
		Description( "Gets or sets the color of the title of the ad." ),
		]
		public virtual Color TitleColor
		{
			get
			{
				Object state = ViewState["TitleColor"];
				if ( state != null )
				{
					return (Color)state;
				}
				return _TitleColorDefault;
			}
			set
			{
				ViewState["TitleColor"] = value;
			}
		}
		private Color _TitleColorDefault = Color.Empty;


		/// <summary>
		/// Gets or sets the background color of the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( typeof( Color ), "Empty" ),
		Description( "Gets or sets the background color of the ad." ),
		]
		public virtual Color BackColor
		{
			get
			{
				Object state = ViewState["BackColor"];
				if ( state != null )
				{
					return (Color)state;
				}
				return _BackColorDefault;
			}
			set
			{
				ViewState["BackColor"] = value;
			}
		}
		private Color _BackColorDefault = Color.Empty;


		/// <summary>
		/// Gets or sets the color of the text in the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( typeof( Color ), "Empty" ),
		Description( "Gets or sets the color of the text in the ad." ),
		]
		public virtual Color TextColor
		{
			get
			{
				Object state = ViewState["TextColor"];
				if ( state != null )
				{
					return (Color)state;
				}
				return _TextColorDefault;
			}
			set
			{
				ViewState["TextColor"] = value;
			}
		}
		private Color _TextColorDefault = Color.Empty;


		/// <summary>
		/// Gets or sets the color of urls in the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( typeof( Color ), "Empty" ),
		Description( "Gets or sets the color of urls in the ad." ),
		]
		public virtual Color UrlColor
		{
			get
			{
				Object state = ViewState["UrlColor"];
				if ( state != null )
				{
					return (Color)state;
				}
				return _UrlColorDefault;
			}
			set
			{
				ViewState["UrlColor"] = value;
			}
		}
		private Color _UrlColorDefault = Color.Empty;


		/// <summary>
		/// Gets or sets the style of the corners of the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( _CornerStyleDefault ),
		Description( "Gets or sets the style of the corners of the ad." ),
		]
		public virtual AdUnitCornerStyle CornerStyle
		{
			get
			{
				Object state = ViewState["CornerStyle"];
				if ( state != null )
				{
					return (AdUnitCornerStyle)state;
				}
				return _CornerStyleDefault;
			}
			set
			{
				ViewState["CornerStyle"] = value;
			}
		}
		private const AdUnitCornerStyle _CornerStyleDefault = AdUnitCornerStyle.Square;

		#endregion

		#region Lifecycle

		/// <exclude />
		protected override void Render( HtmlTextWriter writer )
		{

			writer.WriteLine( @"<script type=""text/javascript""><!--" );

			if ( !String.IsNullOrEmpty( this.PublisherId ) && !IsLocalRequest )
			{
				writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ad_client = ""{0}"";", this.PublisherId ) );
			}
			else
			{
				writer.WriteLine( @"google_ad_client = ""ca_test"";" );
			}
			if ( this.AlternateContent != AdSenseAlternateContent.PublicServiceAds )
			{
				if ( this.AlternateContent == AdSenseAlternateContent.ColorFill && this.AlternateBackColor != Color.Empty )
				{
					writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_alternate_color = ""{0}"";", ColorToHexString( this.AlternateBackColor ) ) );
				}
				else if ( this.AlternateContent == AdSenseAlternateContent.AlternateUrlAds && !String.IsNullOrEmpty( this.AlternateAdUrl ) )
				{
					writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_alternate_ad_url = ""{0}"";", this.AlternateAdUrl ) );
				}
			}

			writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ad_width = {0};", this.GetWidth() ) );
			writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ad_height = {0};", this.GetHeight() ) );
			writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ad_format = ""{0}"";", this.GetFormatString() ) );
			writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ad_type = ""{0}"";", this.GetContentType() ) );
			writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ad_channel = ""{0}"";", this.Channel ?? "" ) );
			if ( this.BorderColor != Color.Empty )
			{
				writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_color_border = ""{0}"";", ColorToHexString( this.BorderColor ) ) );
			}
			if ( this.BackColor != Color.Empty )
			{
				writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_color_bg = ""{0}"";", ColorToHexString( this.BackColor ) ) );
			}
			if ( this.TitleColor != Color.Empty )
			{
				writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_color_link = ""{0}"";", ColorToHexString( this.TitleColor ) ) );
			}
			if ( this.TextColor != Color.Empty )
			{
				writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_color_text = ""{0}"";", ColorToHexString( this.TextColor ) ) );
			}
			if ( this.UrlColor != Color.Empty )
			{
				writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_color_url = ""{0}"";", ColorToHexString( this.UrlColor ) ) );
			}
			writer.WriteLine( String.Format( CultureInfo.InvariantCulture, @"google_ui_features = ""{0}"";", this.GetCornerStyle() ) );

			writer.WriteLine( @"//-->" );
			writer.WriteLine( @"</script>" );
			writer.WriteLine( @"<script  type=""text/javascript"" src=""http://pagead2.googlesyndication.com/pagead/show_ads.js"">" );
			writer.WriteLine( @"</script>" );
		}

		private string GetCornerStyle()
		{
			switch ( this.CornerStyle )
			{
				case AdUnitCornerStyle.Square:
					return "rc:0";
				case AdUnitCornerStyle.SlightlyRounded:
					return "rc:6";
				case AdUnitCornerStyle.VeryRounded:
					return "rc:10";
				default:
					return "";
			}
		}

		private string GetContentType()
		{
			switch ( this.Content )
			{
				case AdUnitContent.TextAndImages:
					return "text_image";
				case AdUnitContent.Text:
					return "text";
				case AdUnitContent.Images:
					return "image";
				default:
					return "text_image";
			}
		}

		private string GetFormatString()
		{
			return this.GetWidth() + "x" + this.GetHeight() + "_as";
		}

		internal string GetHeight()
		{
			switch ( this.Format )
			{
				case AdUnitFormat.HorizontalLeaderboard:
					return "90";
				case AdUnitFormat.HorizontalBanner:
					return "60";
				case AdUnitFormat.HorizontalHalfBanner:
					return "60";
				case AdUnitFormat.VerticalSkyscraper:
					return "600";
				case AdUnitFormat.VerticalWideSkyscraper:
					return "600";
				case AdUnitFormat.VerticalBanner:
					return "240";
				case AdUnitFormat.SquareButton:
					return "125";
				case AdUnitFormat.SquareSmallSquare:
					return "200";
				case AdUnitFormat.SquareMediumSquare:
					return "250";
				case AdUnitFormat.SquareSmallRectangle:
					return "150";
				case AdUnitFormat.SquareMediumRectangle:
					return "250";
				case AdUnitFormat.SquareLargeRectangle:
					return "280";
				default:
					return "90";
			}
		}

		internal string GetWidth()
		{
			switch ( this.Format )
			{
				case AdUnitFormat.HorizontalLeaderboard:
					return "728";
				case AdUnitFormat.HorizontalBanner:
					return "468";
				case AdUnitFormat.HorizontalHalfBanner:
					return "234";
				case AdUnitFormat.VerticalSkyscraper:
					return "120";
				case AdUnitFormat.VerticalWideSkyscraper:
					return "160";
				case AdUnitFormat.VerticalBanner:
					return "120";
				case AdUnitFormat.SquareButton:
					return "125";
				case AdUnitFormat.SquareSmallSquare:
					return "200";
				case AdUnitFormat.SquareMediumSquare:
					return "250";
				case AdUnitFormat.SquareSmallRectangle:
					return "180";
				case AdUnitFormat.SquareMediumRectangle:
					return "300";
				case AdUnitFormat.SquareLargeRectangle:
					return "336";
				default:
					return "728";
			}
		}


		#endregion

	}
}
