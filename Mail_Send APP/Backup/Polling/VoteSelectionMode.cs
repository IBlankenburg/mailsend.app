using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Determines how a poll will be voted on.
	/// </summary>
	public enum VoteSelectionMode
	{

		/// <summary>
		/// Only one option can be voted on.
		/// </summary>
		Single,

		/// <summary>
		/// Multiple options can be voted on.
		/// </summary>
		Multiple,

		/// <summary>
		/// Each option can be given a rating to order their importance.
		/// </summary>
		Rating
	}
}
