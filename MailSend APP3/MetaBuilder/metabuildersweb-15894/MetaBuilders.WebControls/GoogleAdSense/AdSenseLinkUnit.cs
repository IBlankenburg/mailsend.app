using System;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Configuration;
using System.Web.Configuration;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A Google AdSense LinkUnit
	/// </summary>
	[
	Designer( typeof( MetaBuilders.WebControls.Design.AdSenseLinkUnitDesigner ) ),
	]
	public class AdSenseLinkUnit : AdSenseContentAd
	{

		#region Properties

		#region Appearance

		/// <summary>
		/// Gets or sets the format of the ad.
		/// </summary>
		[
		Category( "Appearance" ),
		DefaultValue( _FormatDefault ),
		Description( "Gets or sets the format of the ad." ),
		]
		public virtual LinkUnitFormat Format
		{
			get
			{
				Object state = ViewState["Format"];
				if ( state != null )
				{
					return (LinkUnitFormat)state;
				}
				return _FormatDefault;
			}
			set
			{
				ViewState["Format"] = value;
			}
		}
		private const LinkUnitFormat _FormatDefault = LinkUnitFormat.HorizontalLeaderboard4;


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

		#endregion

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

			writer.WriteLine( @"//-->" );
			writer.WriteLine( @"</script>" );
			writer.WriteLine( @"<script  type=""text/javascript"" src=""http://pagead2.googlesyndication.com/pagead/show_ads.js"">" );
			writer.WriteLine( @"</script>" );
		}

		private string GetFormatString()
		{
			String format = this.GetWidth() + "x" + this.GetHeight() + "_0ads_al";
			switch ( this.Format )
			{
				case LinkUnitFormat.HorizontalLeaderboard5:
				case LinkUnitFormat.HorizontalBanner5:
				case LinkUnitFormat.SquareVerySmall5:
				case LinkUnitFormat.SquareSmall5:
				case LinkUnitFormat.SquareMedium5:
				case LinkUnitFormat.SquareLarge5:
					format += "_s";
					break;
			}
			return format;
		}

		internal string GetHeight()
		{
			switch ( this.Format )
			{
				case LinkUnitFormat.HorizontalLeaderboard4:
				case LinkUnitFormat.HorizontalLeaderboard5:
				case LinkUnitFormat.HorizontalBanner4:
				case LinkUnitFormat.HorizontalBanner5:
					return "15";
				case LinkUnitFormat.SquareVerySmall4:
				case LinkUnitFormat.SquareVerySmall5:
				case LinkUnitFormat.SquareSmall4:
				case LinkUnitFormat.SquareSmall5:
				case LinkUnitFormat.SquareMedium4:
				case LinkUnitFormat.SquareMedium5:
				case LinkUnitFormat.SquareLarge4:
				case LinkUnitFormat.SquareLarge5:
					return "90";
				default:
					return "15";
			}
		}

		internal string GetWidth()
		{
			switch ( this.Format )
			{
				case LinkUnitFormat.HorizontalLeaderboard4:
				case LinkUnitFormat.HorizontalLeaderboard5:
					return "728";
				case LinkUnitFormat.HorizontalBanner4:
				case LinkUnitFormat.HorizontalBanner5:
					return "468";
				case LinkUnitFormat.SquareVerySmall4:
				case LinkUnitFormat.SquareVerySmall5:
					return "120";
				case LinkUnitFormat.SquareSmall4:
				case LinkUnitFormat.SquareSmall5:
					return "160";
				case LinkUnitFormat.SquareMedium4:
				case LinkUnitFormat.SquareMedium5:
					return "180";
				case LinkUnitFormat.SquareLarge4:
				case LinkUnitFormat.SquareLarge5:
					return "200";
				default:
					return "728";
			}
		}

		#endregion

	}
}
