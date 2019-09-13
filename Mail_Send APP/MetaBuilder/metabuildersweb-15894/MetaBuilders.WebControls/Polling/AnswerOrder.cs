using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Specifies the order of poll options in the <see cref="PollView"/> control.
	/// </summary>
	/// <remarks>
	/// The <see cref="PollView"/> control displays the voting results for a poll in the order
	/// specified by this enum.
	/// </remarks>
	public enum OptionOrder
	{

		/// <summary>
		/// The options will be displayed in the order specified by the DisplayOrder property of the option.
		/// </summary>
		/// <remarks>
		/// This value will result in the options being displayed in the same order as they apear when in "voting" mode.
		/// </remarks>
		Auto,

		/// <summary>
		/// The options will be displayed in descending order of vote count.
		/// </summary>
		VoteCountDescending,

		/// <summary>
		/// The options will be displayed in ascending order of vote count.
		/// </summary>
		VoteCountAscending
	}
}
