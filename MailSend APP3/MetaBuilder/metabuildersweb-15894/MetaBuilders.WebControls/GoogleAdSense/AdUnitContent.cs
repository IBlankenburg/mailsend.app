using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The possible values which determine what kind of content to show in an AdSense ad.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue" )]
	public enum AdUnitContent
	{
		/// <summary>
		/// Both text and images are possible to be shown in the ad.
		/// </summary>
		TextAndImages = 1,

		/// <summary>
		/// Only text can be shown in the ad.
		/// </summary>
		Text = 2,

		/// <summary>
		/// Only images can be shown in the ad.
		/// </summary>
		Images = 3
	}
}
