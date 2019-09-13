using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Specifies which interface the <see cref="PollView"/> control displays.
	/// </summary>
	public enum PollViewState
	{
		/// <summary>
		/// The PollView control will determine the appropriate interface.
		/// </summary>
		Auto,
		/// <summary>
		/// The PollView control will show the interface for voting on options.
		/// </summary>
		Input,
		/// <summary>
		/// The PollView control will show the interface for viewing the results of the poll.
		/// </summary>
		Result
	}
}
