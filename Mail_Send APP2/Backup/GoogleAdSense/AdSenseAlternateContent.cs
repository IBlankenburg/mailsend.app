using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The possible values which determine what to display in an AdSense ad when relevant ads are unavailable.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue" )]
	public enum AdSenseAlternateContent
	{
		/// <summary>
		/// Standard public service ads will be shown
		/// </summary>
		PublicServiceAds = 1,

		/// <summary>
		/// Ads from a different provider will be shown
		/// </summary>
		AlternateUrlAds = 2,

		/// <summary>
		/// A solid color will be shown
		/// </summary>
		ColorFill = 3
	}
}
