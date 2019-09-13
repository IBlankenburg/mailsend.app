using System;
using System.Web.UI;
using System.Drawing;
using System.Web;
using System.ComponentModel;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Base class for Google AdSense ads
	/// </summary>
	[
	Themeable(true),
	]
	public abstract class AdSenseContentAd : Control
	{

		#region Google Properties

		/// <summary>
		/// Gets or sets the PublisherId for the ad.
		/// </summary>
		[
		MbwcCategory( "Google" ),
		DefaultValue( _PublisherIdDefault ),
		Description( "Gets or sets the PublisherId for the ad." ),
		]
		public virtual String PublisherId
		{
			get
			{
				Object state = ViewState["PublisherId"];
				if ( state != null )
				{
					return (String)state;
				}
				return _PublisherIdDefault;
			}
			set
			{
				ViewState["PublisherId"] = value;
			}
		}
		private const String _PublisherIdDefault = "";


		/// <summary>
		/// Gets or sets the content to use if no relevant ads are available.
		/// </summary>
		[
		MbwcCategory( "Google" ),
		DefaultValue( _AlternateContentDefault ),
		Description( "Gets or sets the content to use if no relevant ads are available." ),
		]
		public virtual AdSenseAlternateContent AlternateContent
		{
			get
			{
				Object state = ViewState["AlternateContent"];
				if ( state != null )
				{
					return (AdSenseAlternateContent)state;
				}
				return _AlternateContentDefault;
			}
			set
			{
				ViewState["AlternateContent"] = value;
			}
		}
		private const AdSenseAlternateContent _AlternateContentDefault = AdSenseAlternateContent.PublicServiceAds;


		/// <summary>
		/// Gets or sets the url to get ads from if no relevant ads are available.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ),
		MbwcCategory( "Google" ),
		DefaultValue( _AlternateAdUrlDefault ),
		Description( "Gets or sets the url to get ads from if no relevant ads are available." ),
		]
		public virtual String AlternateAdUrl
		{
			get
			{
				Object state = ViewState["AlternateAdUrl"];
				if ( state != null )
				{
					return (String)state;
				}
				return _AlternateAdUrlDefault;
			}
			set
			{
				ViewState["AlternateAdUrl"] = value;
			}
		}
		private const String _AlternateAdUrlDefault = "";


		/// <summary>
		/// Gets or sets the color to fill the content area with if no relevant ads are available.
		/// </summary>
		[
		MbwcCategory( "Google" ),
		DefaultValue( typeof( Color ), "Empty" ),
		Description( "Gets or sets the color to fill the content area with if no relevant ads are available." ),
		]
		public virtual Color AlternateBackColor
		{
			get
			{
				Object state = ViewState["AlternateBackColor"];
				if ( state != null )
				{
					return (Color)state;
				}
				return _AlternateBackColorDefault;
			}
			set
			{
				ViewState["AlternateBackColor"] = value;
			}
		}
		private Color _AlternateBackColorDefault = Color.Empty;


		/// <summary>
		/// Gets or sets the channel this ad is associated with.
		/// </summary>
		[
		MbwcCategory( "Google" ),
		DefaultValue( _ChannelDefault ),
		Description( "Gets or sets the channel this ad is associated with." ),
		]
		public virtual String Channel
		{
			get
			{
				Object state = ViewState["Channel"];
				if ( state != null )
				{
					return (String)state;
				}
				return _ChannelDefault;
			}
			set
			{
				ViewState["Channel"] = value;
			}
		}
		private const String _ChannelDefault = "";


		#endregion

		#region Life Cycle

		/// <exclude />
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection( this );
		}

		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			if ( Page != null && Page.ClientScript != null && !IsLocalRequest && String.IsNullOrEmpty( this.PublisherId ) )
			{
				Page.ClientScript.RegisterStartupScript( typeof( AdSenseContentAd ), "PublisherID Warning", "alert( 'AdSense Is Missing The PublisherID' );", true );
			}
		}

		/// <summary>
		/// Determines if the current request is local
		/// </summary>
		protected static Boolean IsLocalRequest
		{
			get
			{
				if ( HttpContext.Current == null || HttpContext.Current.Request == null )
				{
					return true;
				}
				return HttpContext.Current.Request.IsLocal;
			}
		}

		/// <summary>
		/// Converts the given Color to the web-hex equivalent
		/// </summary>
		protected static string ColorToHexString( Color color )
		{
			byte[] bytes = new byte[3];
			bytes[0] = color.R;
			bytes[1] = color.G;
			bytes[2] = color.B;
			char[] chars = new char[bytes.Length * 2];
			for ( int i = 0; i < bytes.Length; i++ )
			{
				int b = bytes[i];
				chars[i * 2] = hexDigits[b >> 4];
				chars[i * 2 + 1] = hexDigits[b & 0xF];
			}
			return new string( chars );
		}

		private static char[] hexDigits = 
		{
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
		};

		#endregion

	}
}
