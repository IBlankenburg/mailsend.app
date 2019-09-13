using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The possible values for the format of an AdSense LinkUnit ad.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue" )]
	public enum LinkUnitFormat
	{
		/// <summary>
		/// A 728x15 banner ad with 4 links
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Leaderboard" )]
		HorizontalLeaderboard4 = 1,

		/// <summary>
		/// A 728x15 banner ad with 5 links
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Leaderboard" )]
		HorizontalLeaderboard5 = 2,
		
		/// <summary>
		/// A 468x15 banner ad with 4 links
		/// </summary>
		HorizontalBanner4 = 3,

		/// <summary>
		/// A 468x15 banner ad with 5 links
		/// </summary>
		HorizontalBanner5 = 4,
		
		/// <summary>
		/// A 120x90 square ad with 4 links
		/// </summary>
		SquareVerySmall4 = 5,

		/// <summary>
		/// A 120x90 square ad with 5 links
		/// </summary>
		SquareVerySmall5 = 6,
		
		/// <summary>
		/// A 160x90 square ad with 4 links
		/// </summary>
		SquareSmall4 = 7,

		/// <summary>
		/// A 160x90 square ad with 5 links
		/// </summary>
		SquareSmall5 = 8,
		
		/// <summary>
		/// A 180x90 square ad with 4 links
		/// </summary>
		SquareMedium4 = 9,

		/// <summary>
		/// A 180x90 square ad with 5 links
		/// </summary>
		SquareMedium5 = 10,
		
		/// <summary>
		/// A 200x90 square ad with 4 links
		/// </summary>
		SquareLarge4 = 11,

		/// <summary>
		/// A 200x90 square ad with 5 links
		/// </summary>
		SquareLarge5 = 12

	}
}
