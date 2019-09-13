using System;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Indicates the placement of the selection buttons for <see cref="MultiViewItem"/> controls on the
	/// <see cref="MultiViewBar"/> control.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The default value of <see cref="MultiViewButtonPlacement.Surrounding"/>
	/// will result in the buttons wrapping around the content,
	/// with the current item's button being directly above the content.
	/// </p>
	/// <p>
	/// The value of <see cref="MultiViewButtonPlacement.Top" /> will cause the buttons
	/// to always be at the top of the bar.
	/// </p>
	/// <p>
	/// The value of <see cref="MultiViewButtonPlacement.Bottom" /> will cause the buttons
	/// to always be at the bottom of the bar, and a header will be displayed at the top
	/// of the bar.
	/// </p>
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue" ), 
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" )]
	public enum MultiViewButtonPlacement {
		/// <summary>
		/// The default value of <see cref="MultiViewButtonPlacement.Surrounding"/>
		/// will result in the buttons wrapping around the content,
		/// with the current item's button being directly above the content.
		/// </summary>
		Surrounding = 1,
		/// <summary>
		/// The value of <see cref="MultiViewButtonPlacement.Bottom" /> will cause the buttons
		/// to always be at the bottom of the bar, and a header will be displayed at the top
		/// of the bar.
		/// </summary>
		Bottom = 2,
		/// <summary>
		/// The value of <see cref="MultiViewButtonPlacement.Top" /> will cause the buttons
		/// to always be at the top of the bar.
		/// </summary>
		Top = 3
	}
}
