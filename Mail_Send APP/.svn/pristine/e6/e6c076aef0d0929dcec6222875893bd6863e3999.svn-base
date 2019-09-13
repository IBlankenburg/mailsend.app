using System;
using System.Configuration.Provider;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Defines the contract to provide polling services using custom polling providers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// MetaBuilders Polling is designed to enable you to easily use a number of different polling providers for your ASP.NET applications. 
	/// You can use the supplied polling providers that are included in this package, or you can implement your own provider.
	/// </para>
	/// <para>
	/// When implementing a custom polling provider, you are required to inherit the <b>PollingProvider</b> abstract class.
	/// For more information, see "Implementing a Polling Provider".
	/// </para>
	/// </remarks>
	public abstract class PollingProvider : ProviderBase
	{

		#region Polling Provider Configuration Properties

		/// <summary>
		/// Gets a value indicating whether the polling provider is configured to use cookies to track when a user votes on a poll.
		/// </summary>
		public abstract Boolean EnableCookieVoteTracking
		{
			get;
		}

		#endregion

		#region Voting

		/// <summary>
		/// Logs a vote for the given poll and option.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="optionID">The option being voted for.</param>
		protected internal abstract void AddVote( String member, String ipAddress, Int32 pollID, Int32 optionID );

		/// <summary>
		/// Logs a vote for the given poll and option.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="optionID">The option being voted for.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		protected internal abstract void AddVote( String member, String ipAddress, Int32 pollID, Int32 optionID, Int32 voteRating );

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="writeInText">The text written in by the user.</param>
		protected internal abstract void AddWriteInVote( String member, String ipAddress, Int32 pollID, String writeInText );

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="writeInText">The text written in by the user.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		protected internal abstract void AddWriteInVote( String member, String ipAddress, Int32 pollID, String writeInText, Int32 voteRating );

		#endregion

		#region User Tracking

		/// <summary>
		/// Determines if the given member has voted on the given poll.
		/// </summary>
		protected internal abstract Boolean UserHasVoted( String member, Int32 pollID );

		#endregion

		#region Management

		/// <summary>
		/// Adds a new poll with the given properties.
		/// </summary>
		/// <param name="text">The text of the question of the poll.</param>
		/// <param name="startDate">The date and time on which the poll will start.</param>
		/// <param name="voteMode">The selection mode of the poll, single or multiple.</param>
		/// <param name="enabled">Whether the poll is enabled or not.</param>
		/// <param name="allowWriteIns">Whether the poll allows write-in votes.</param>
		/// <param name="category">The category of the poll.</param>
		protected internal abstract void InsertPoll( String text, DateTime startDate, VoteSelectionMode voteMode, Boolean enabled, Boolean allowWriteIns, String category );

		/// <summary>
		/// Updates the poll specified by pollID.
		/// </summary>
		/// <param name="pollID">The ID of the poll to update.</param>
		/// <param name="text">The text of the question of the poll.</param>
		/// <param name="startDate">The date and time on which the poll will start.</param>
		/// <param name="voteMode">The selection mode of the poll, single or multiple.</param>
		/// <param name="enabled">Whether the poll is enabled or not.</param>
		/// <param name="allowWriteIns">Whether the poll allows write-in votes.</param>
		/// <param name="category">The category of the poll</param>
		protected internal abstract void UpdatePoll( Int32 pollID, String text, DateTime startDate, VoteSelectionMode voteMode, Boolean enabled, Boolean allowWriteIns, String category );

		/// <summary>
		/// Deletes the given poll.
		/// </summary>
		/// <param name="pollID">The ID of the poll to delete.</param>
		protected internal abstract void DeletePoll( Int32 pollID );

		/// <summary>
		/// Adds a new option to a poll.
		/// </summary>
		/// <param name="pollID">The poll to add the option to.</param>
		/// <param name="text">The text of the option.</param>
		/// <param name="displayOrder">The order in which the option will be displayed.</param>
		protected internal abstract void InsertOption( Int32 pollID, String text, Int32 displayOrder );

		/// <summary>
		/// Updates the option specified by optionID.
		/// </summary>
		/// <param name="optionID">The ID of the option to update.</param>
		/// <param name="pollID">The poll associated to the option.</param>
		/// <param name="text">The text of the option.</param>
		/// <param name="displayOrder">The order in which the option will be displayed.</param>
		protected internal abstract void UpdateOption( Int32 optionID, Int32 pollID, String text, Int32 displayOrder );

		/// <summary>
		/// Deletes the given option.
		/// </summary>
		/// <param name="optionID">The ID of the option to delete.</param>
		protected internal abstract void DeleteOption( Int32 optionID );

		/// <summary>
		/// Updates the write-in vote specified by voteID.
		/// </summary>
		/// <param name="voteID">The ID of the write-in vote to update.</param>
		/// <param name="text">The text of the answer.</param>
		protected internal abstract void UpdateWriteInVote( System.Guid voteID, String text );

		/// <summary>
		/// Deletes a vote specified by ID.
		/// </summary>
		/// <param name="voteID">The ID of the vote to delete.</param>
		protected internal abstract void DeleteVote( System.Guid voteID );

		/// <summary>
		/// Resets the given poll to no votes.
		/// </summary>
		/// <param name="pollID">The ID of the poll to reset.</param>
		protected internal abstract void ResetPoll( Int32 pollID );

		#endregion

		#region Selection

		/// <summary>
		/// Gets the current poll data and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <remarks>
		/// <p>Which poll is deemed "current" is up to the provider, but the built in providers use the current date as the determining factor.</p>
		/// </remarks>
		/// <returns>A <see cref="PollDataSet"/> containing the current poll.</returns>
		protected internal abstract PollDataSet GetCurrentPoll();

		/// <summary>
		/// Gets the current poll data and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <remarks>
		/// <p>Which poll is deemed "current" is up to the provider, but the built in providers use the current date and given category as the determining factor.</p>
		/// </remarks>
		/// <returns>A <see cref="PollDataSet"/> containing the current poll.</returns>
		protected internal abstract PollDataSet GetCurrentPoll( String pollCategory );

		/// <summary>
		/// Gets the poll data specified by the pollID, and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <param name="pollID">The ID of the poll to retrieve.</param>
		/// <returns>A <see cref="PollDataSet"/> containing the specified poll data.</returns>
		protected internal abstract PollDataSet GetPoll( Int32 pollID );

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
		protected internal abstract PollDataSet GetAllPolls();

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls with the given filter.
		/// </summary>
		protected internal abstract PollDataSet GetPollsByFilter( PollSelectionFilter filter );

		#endregion
	}
}
