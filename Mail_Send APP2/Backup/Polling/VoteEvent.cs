using System;
using System.ComponentModel;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Provides the data for the <see cref="PollView.VoteReceived"/> event of the <see cref="PollView"/> control.
	/// </summary>
	public class VoteEventArgs : CancelEventArgs
	{

		/// <summary>
		/// Creates a new instance of the <see cref="VoteEventArgs"/> class
		/// </summary>
		/// <param name="pendingVotes">The votes about to be recorded.</param>
		public VoteEventArgs( PollDataSet.VotesDataTable pendingVotes )
			: base()
		{
			this._pendingVotes = pendingVotes;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="VoteEventArgs"/> class
		/// </summary>
		/// <param name="pendingVotes">The votes about to be recorded.</param>
		/// <param name="cancel">Determines if the vote should be canceled</param>
		public VoteEventArgs( PollDataSet.VotesDataTable pendingVotes, Boolean cancel )
			: base( cancel )
		{
			this._pendingVotes = pendingVotes;
		}

		/// <summary>
		/// Gets the votes about to be recorded.
		/// </summary>
		public PollDataSet.VotesDataTable PendingVotes
		{
			get
			{
				return _pendingVotes;
			}
		}
		PollDataSet.VotesDataTable _pendingVotes;

	}

}
