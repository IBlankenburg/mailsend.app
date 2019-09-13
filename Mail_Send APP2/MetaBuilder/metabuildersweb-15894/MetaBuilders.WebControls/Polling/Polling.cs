using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Configuration.Provider;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Manages and selects polling data. This class cannot be inherited.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="Polling"/> class is used to both vote on polls and manage the poll data.
	/// The Polling class can be used directly to perform these actions, however the built-in
	/// <see cref="PollView"/> and <see cref="PollManager"/> controls wrap the Polling class
	/// to provide a more convenient experience.
	/// </para>
	/// <para>
	/// The <see cref="Polling"/> class provides facilities for:
	/// <list type="bullet">
	/// <item>Voting on poll options</item>
	/// <item>Tracking which users or members have voted on which polls</item>
	/// <item>Poll information management: adding/modifying/deleting polls and their associated options</item>
	/// <item>Selection of poll information and current results</item>
	/// <item>Storing all this data in Microsoft SQL Server or Microsoft Access.
	/// This list of targets is extensible for custom storage.
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// The <see cref="Polling"/> class relies on polling providers to communicate with a data source.
	/// This package includes a <see cref="SqlServerPollingProvider"/>, which stores polling information in a Microsoft SQL Server database,
	/// and an <see cref="AccessPollingProvider"/>, which stores polling information in a Microsoft Access database.
	/// You can also implement a custom polling provider to communicate with an alternative data source that can be utilized by the <see cref="Polling"/> class.
	/// Custom membership providers inherit the <see cref="PollingProvider"/> abstract class.
	/// For more information, see "Implementing a Polling Provider".
	/// </para>
	/// <para>
	/// Without any configuration, <see cref="Polling"/> will create and use the <see cref="AccessPollingProvider"/>.
	/// This default configuration will attempt to use the "App_Data" folder in your application root, and place an empty polling
	/// Access database within it. If the asp.net process does not have sufficient permissions to accomplish this, 
	/// then a configuration exception will be thrown, and you will have to manually configure you application to use the polling feature.
	/// </para>
	/// <para>To manually configure <see cref="Polling"/>, the following steps must be taken.
	/// <list type="number" >
	/// <item>Determine which provider you want to use... <see cref="SqlServerPollingProvider"/>, <see cref="AccessPollingProvider"/>, or a custom provider.</item>
	/// <item>Setup the datasource as required for your chosen provider.</item>
	/// <item>Modify web.config to declare your choice of polling provider.</item>
	/// <item>Optionally, determine if you want to enable cookie tracking of votes.</item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <example>
	/// The following web.config snippet shows a configuration file which chooses the Sql Server provider.
	/// <code>
	/// &lt;configuration&gt;
	/// 
	///   &lt;configSections&gt;
	///     &lt;sectionGroup name="metabuilders"&gt;
	///       &lt;section name="polling" type="MetaBuilders.WebControls.PollingConfig, MetaBuilders.WebControls" 
	///         allowDefinition="MachineToApplication" restartOnExternalChanges="true" requirePermission="false"
	///	      /&gt;
	///     &lt;/sectionGroup&gt;
	///   &lt;/configSections&gt;
	///   
	///   &lt;metabuilders&gt;
	///     &lt;polling defaultProvider="SqlServerPollingProvider" &gt;
	///       &lt;providers&gt;
	///         &lt;add
	///           name="SqlServerPollingProvider"
	///           type="MetaBuilders.WebControls.SqlServerPollingProvider"
	///           connectionStringName="SqlPollingConnection"
	///         /&gt;
	///       &lt;/providers&gt;
	///     &lt;/polling&gt;
	///   &lt;/metabuilders&gt;
	/// 
	///   &lt;connectionStrings&gt;
	///     &lt;add name="SqlPollingConnection" connectionString="Data Source=localhost;Initial Catalog=Polling;Integrated Security=True" providerName="System.Data.SqlClient" /&gt;
	///   &lt;/connectionStrings&gt;
	/// 
	/// &lt;/configuration&gt;
	/// </code>
	/// </example>
	/// <seealso cref="AccessPollingProvider" />
	/// <seealso cref="SqlServerPollingProvider" />
	/// <seealso cref="PollView" />
	/// <seealso cref="PollManager" />
	public sealed class Polling
	{

		#region Initialization and Configuration

		// Private so that this static class can't be instantiated
		private Polling()
		{
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline" )]
		static Polling()
		{
			voteTrackCookieName = "MetaBuildersPoll";
			syncLock = new Object();
			cacheSyncObject = new Object();
		}

		/// <summary>
		/// Loads the configuration specified in web.config
		/// </summary>
		private static void Initialize()
		{
			// Avoid claiming lock if providers are already loaded
			if ( provider == null )
			{
				lock ( syncLock )
				{
					// Do this again to make sure _provider is still null
					if ( provider == null )
					{
						// Get a reference to the <metabuilders/polling> section
						PollingConfig section = (PollingConfig)WebConfigurationManager.GetSection( "metabuilders/polling" );

						if ( section != null )
						{
							// Load registered providers and point provider to the default provider
							providers = new PollingProviderCollection();
							ProvidersHelper.InstantiateProviders( section.Providers, providers, typeof( PollingProvider ) );
							provider = providers[section.DefaultProvider];
						}
						else
						{

							provider = new AccessPollingProvider();
							NameValueCollection settings = new NameValueCollection();
							settings.Add( "connectionString", "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=|DataDirectory|MetaBuildersPolling.mdb" );
							provider.Initialize( "AccessPollingProvider", settings );

							providers = new PollingProviderCollection();
							providers.Add( provider );

								
						}

						if ( provider == null )
						{
							throw new ProviderException( "Unable to load default PollingProvider" );
						}
					}
				}
			}
		}


		/// <summary>
		/// Gets the <see cref="PollingProvider"/> that is acting as the provider implementation for <see cref="Polling"/>.
		/// </summary>
		public static PollingProvider Provider
		{
			get
			{
				Initialize();
				return Polling.provider;
			}
		}


		/// <summary>
		/// Gets the collection of polling providers registered with <see cref="Polling"/>.
		/// </summary>
		public static PollingProviderCollection Providers
		{
			get
			{
				Polling.Initialize();
				return providers;
			}
		}

		#endregion

		#region Voting

		/// <summary>
		/// Logs a vote for the given poll and option.
		/// </summary>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="optionID">The option being voted for.</param>
		public static void AddVote( Int32 pollID, Int32 optionID )
		{
			Polling.Provider.AddVote( Polling.MemberName, Polling.IPAddress, pollID, optionID );
			Polling.SetUserVotedCookie( pollID );
			Polling.ClearCache();
		}

		/// <summary>
		/// Logs a vote for the given poll and option.
		/// </summary>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="optionID">The option being voted for.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		public static void AddVote( Int32 pollID, Int32 optionID, Int32 voteRating )
		{
			Polling.Provider.AddVote( Polling.MemberName, Polling.IPAddress, pollID, optionID, voteRating );
			Polling.SetUserVotedCookie( pollID );
			Polling.ClearCache();
		}

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="writeInText">The text written in by the user.</param>
		public static void AddWriteInVote( Int32 pollID, String writeInText )
		{
			if ( writeInText == null )
			{
				throw new ArgumentNullException( "writeInText" );
			}
			if ( writeInText.Trim().Length == 0 )
			{
				throw new ArgumentException( Resources.Polling_StringCannotBeEmpty, "writeInText" );
			}
			Polling.Provider.AddWriteInVote( Polling.MemberName, Polling.IPAddress, pollID, writeInText );
			Polling.SetUserVotedCookie( pollID );
			Polling.ClearCache();
		}

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="writeInText">The text written in by the user.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		public static void AddWriteInVote( Int32 pollID, String writeInText, Int32 voteRating )
		{
			if ( writeInText == null )
			{
				throw new ArgumentNullException( "writeInText" );
			}
			if ( writeInText.Trim().Length == 0 )
			{
				throw new ArgumentException( Resources.Polling_StringCannotBeEmpty, "writeInText" );
			}
			Polling.Provider.AddWriteInVote( Polling.MemberName, Polling.IPAddress, pollID, writeInText, voteRating );
			Polling.SetUserVotedCookie( pollID );
			Polling.ClearCache();
		}

		#endregion

		#region User Tracking
		/// <summary>
		/// Determines if the given member has voted on the given poll.
		/// </summary>
		/// <remarks>
		/// Not all providers are required to support tracking users as they vote on polls.
		/// As a fall back measure, a cookie is used to track user votes by default.
		/// </remarks>
		public static Boolean UserHasVoted( Int32 pollID )
		{

			if ( Polling.Provider.EnableCookieVoteTracking )
			{
				HttpContext context = HttpContext.Current;
				if ( context != null )
				{
					HttpCookie voteTracker = context.Request.Cookies.Get( voteTrackCookieName );
					if ( voteTracker != null )
					{
						String currentPollID = pollID.ToString( System.Globalization.CultureInfo.InvariantCulture );
						foreach ( String key in voteTracker.Values.AllKeys )
						{
							if ( key == currentPollID )
							{
								return true;
							}
						}
					}
				}
			}

			String member = Polling.MemberName;
			if ( member != null )
			{
				return Polling.Provider.UserHasVoted( member, pollID );
			}

			return false;
		}

		/// <summary>
		/// Adds a cookie that says the user voted on the current poll
		/// </summary>
		/// <param name="pollID"></param>
		private static void SetUserVotedCookie( Int32 pollID )
		{

			if ( !Polling.Provider.EnableCookieVoteTracking )
			{
				return;
			}

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				return;
			}

			HttpCookie voteTracker = context.Request.Cookies.Get( voteTrackCookieName );
			if ( voteTracker == null )
			{
				voteTracker = new HttpCookie( voteTrackCookieName );
			}
			voteTracker.Expires = DateTime.Now.AddYears( 1 );
			voteTracker.Values.Add( pollID.ToString( System.Globalization.CultureInfo.InvariantCulture ), "1" );
			context.Response.Cookies.Set( voteTracker );
		}

		/// <summary>
		/// Retrives the member's name from the Identity associated with the current request.
		/// </summary>
		internal static String MemberName
		{
			get
			{
				HttpContext context = HttpContext.Current;
				if ( context != null && context.Request.IsAuthenticated && context.User != null && context.User.Identity.IsAuthenticated )
				{
					return context.User.Identity.Name;
				}
				return null;
			}
		}
		/// <summary>
		/// Retrives the IP Address of the the current request.
		/// </summary>
		internal static String IPAddress
		{
			get
			{
				HttpContext context = HttpContext.Current;
				if ( context != null )
				{
					return context.Request.UserHostAddress;
				}
				return "";
			}
		}

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
		public static void InsertPoll( String text, DateTime startDate, VoteSelectionMode voteMode, Boolean enabled, Boolean allowWriteIns, String category )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			if ( !Enum.IsDefined( typeof( VoteSelectionMode ), voteMode ) )
			{
				throw new System.ComponentModel.InvalidEnumArgumentException( "voteMode", (Int32)voteMode, typeof( VoteSelectionMode ) );
			}
			if ( category == null )
			{
				category = String.Empty;
			}
			Polling.Provider.InsertPoll( text, startDate, voteMode, enabled, allowWriteIns, category );
			Polling.ClearCache();
		}

		/// <summary>
		/// Updates the poll specified by pollID.
		/// </summary>
		/// <param name="pollID">The ID of the poll to update.</param>
		/// <param name="text">The text of the question of the poll.</param>
		/// <param name="startDate">The date and time on which the poll will start.</param>
		/// <param name="voteMode">The selection mode of the poll, single or multiple.</param>
		/// <param name="enabled">Whether the poll is enabled or not.</param>
		/// <param name="allowWriteIns">Whether the poll allows write-in votes.</param>
		/// <param name="category">The category of the poll.</param>
		public static void UpdatePoll( Int32 pollID, String text, DateTime startDate, VoteSelectionMode voteMode, Boolean enabled, Boolean allowWriteIns, String category )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			if ( !Enum.IsDefined( typeof( VoteSelectionMode ), voteMode ) )
			{
				throw new System.ComponentModel.InvalidEnumArgumentException( "voteMode", (Int32)voteMode, typeof( VoteSelectionMode ) );
			}
			if ( category == null )
			{
				category = String.Empty;
			}
			Polling.Provider.UpdatePoll( pollID, text, startDate, voteMode, enabled, allowWriteIns, category );
			Polling.ClearCache();
		}

		/// <summary>
		/// Deletes the given poll.
		/// </summary>
		/// <param name="pollID">The ID of the poll to delete.</param>
		public static void DeletePoll( Int32 pollID )
		{
			Polling.Provider.DeletePoll( pollID );
			Polling.ClearCache();
		}

		/// <summary>
		/// Adds a new option to a poll.
		/// </summary>
		/// <param name="pollID">The poll to add the option to.</param>
		/// <param name="text">The text of the option.</param>
		/// <param name="displayOrder">The order in which the option will be displayed.</param>
		/// <returns>The ID assigned to the new option.</returns>
		public static void InsertOption( Int32 pollID, String text, Int32 displayOrder )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			Polling.Provider.InsertOption( pollID, text, displayOrder );
			Polling.ClearCache();
		}

		/// <summary>
		/// Updates the option specified by answerID.
		/// </summary>
		/// <param name="optionID">The ID of the option to update.</param>
		/// <param name="pollID">The ID poll to add the option to.</param>
		/// <param name="text">The text of the option.</param>
		/// <param name="displayOrder">The order in which the option will be displayed.</param>
		public static void UpdateOption( Int32 optionID, Int32 pollID, String text, Int32 displayOrder )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			Polling.Provider.UpdateOption( optionID, pollID, text, displayOrder );
			Polling.ClearCache();
		}

		/// <summary>
		/// Deletes the given option.
		/// </summary>
		/// <param name="optionID">The ID of the option to delete.</param>
		public static void DeleteOption( Int32 optionID )
		{
			Polling.Provider.DeleteOption( optionID );
			Polling.ClearCache();
		}

		/// <summary>
		/// Updates the write-in vote specified by writeInVoteID.
		/// </summary>
		/// <param name="voteID">The ID of the write-in vote to update.</param>
		/// <param name="text">The text of the answer.</param>
		public static void UpdateWriteInVote( Guid voteID, String text )
		{
			if ( text == null )
			{
				text = "";
			}
			Polling.Provider.UpdateWriteInVote( voteID, text );
			Polling.ClearCache();
		}

		/// <summary>
		/// Deletes the write-in vote specified by ID.
		/// </summary>
		/// <param name="voteID">The ID of the vote to delete.</param>
		public static void DeleteVote( Guid voteID )
		{
			Polling.Provider.DeleteVote( voteID );
			Polling.ClearCache();
		}

		/// <summary>
		/// Resets the given poll to no votes.
		/// </summary>
		/// <param name="pollID">The ID of the poll to reset.</param>
		public static void ResetPoll( Int32 pollID )
		{
			Polling.Provider.ResetPoll( pollID );
			Polling.ClearCache();
		}

		#endregion

		#region Selection

		/// <summary>
		/// Gets the current poll data and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <remarks>
		/// <p>Which poll is deemed "current" is up to the provider, but the built in providers use the current date as the determining factor.</p>
		/// </remarks>
		/// <returns>A <see cref="PollDataSet"/> containing the current poll.</returns>
		public static PollDataSet GetCurrentPoll()
		{
			PollDataSet p = null;

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				p = Polling.Provider.GetCurrentPoll();
			}
			else
			{
				lock ( cacheSyncObject )
				{
					String cacheKey = "MetaBuilders Polling Current PollDataSet";
					p = Polling.GetPollFromCache( cacheKey );
					if ( p == null )
					{
						p = Polling.Provider.GetCurrentPoll();
						Polling.InsertPollIntoCache( cacheKey, p );
					}
				}
			}

			return p ?? new PollDataSet();
		}

		/// <summary>
		/// Gets the current poll data and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <remarks>
		/// <p>Which poll is deemed "current" is up to the provider, but the built in providers use the current date and given category as the determining factor.</p>
		/// </remarks>
		/// <returns>A <see cref="PollDataSet"/> containing the current poll.</returns>
		public static PollDataSet GetCurrentPoll( String pollCategory )
		{
			if ( pollCategory == null )
			{
				pollCategory = "";
			}

			PollDataSet p = null;

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				p = Polling.Provider.GetCurrentPoll( pollCategory );
			}
			else
			{
				lock ( cacheSyncObject )
				{
					String cacheKey = "MetaBuilders Polling Current PollDataSet By Category " + pollCategory;
					p = Polling.GetPollFromCache( cacheKey );
					if ( p == null )
					{
						p = Polling.Provider.GetCurrentPoll( pollCategory );
						Polling.InsertPollIntoCache( cacheKey, p );
					}
				}
			}

			return p ?? new PollDataSet();
		}

		/// <summary>
		/// Gets the poll data specified by the pollID, and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <param name="pollID">The ID of the poll to retrieve.</param>
		/// <returns>A <see cref="PollDataSet"/> containing the specified poll data.</returns>
		public static PollDataSet GetPoll( Int32 pollID )
		{
			PollDataSet p = null;

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				p = Polling.Provider.GetPoll( pollID );
			}
			else
			{
				lock ( cacheSyncObject )
				{
					String cacheKey = "MetaBuilders Polling Current PollDataSet By ID " + pollID.ToString( System.Globalization.CultureInfo.InvariantCulture );
					p = Polling.GetPollFromCache( cacheKey );
					if ( p == null )
					{
						p = Polling.Provider.GetPoll( pollID );
						Polling.InsertPollIntoCache( cacheKey, p );
					}
				}
			}

			return p ?? new PollDataSet();
		}

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
		public static PollDataSet GetAllPolls()
		{
			PollDataSet p = Polling.Provider.GetAllPolls();
			return p ?? new PollDataSet();
		}

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls with the given filter.
		/// </summary>
		public static PollDataSet GetPollsByFilter( PollSelectionFilter filter )
		{
			PollDataSet p = Polling.Provider.GetPollsByFilter( filter );
			return p ?? new PollDataSet();
		}

		#endregion

		#region Utility
		internal static PollDataSet CreatePlaceholderPoll()
		{
			if ( fake == null )
			{
				fake = new PollDataSet();
				fake.Polls.AddPollsRow( "Example Question", DateTime.Now, VoteSelectionMode.Single.ToString(), true, false, "" );
				fake.Options.AddOptionsRow( fake.Polls[0], "Example Answer 1", 0, 1 );
				fake.Options.AddOptionsRow( fake.Polls[0], "Example Answer 2", 1, 3 );
				fake.Options.AddOptionsRow( fake.Polls[0], "Example Answer 3", 2, 2 );
				fake.Votes.AddVotesRow( Guid.NewGuid(), "PlaceHolder", fake.Polls[0], fake.Options[0], null, DateTime.Now, 1, "127.0.0.1" );
				fake.Votes.AddVotesRow( Guid.NewGuid(), "PlaceHolder", fake.Polls[0], fake.Options[1], null, DateTime.Now, 1, "127.0.0.1" );
				fake.Votes.AddVotesRow( Guid.NewGuid(), "PlaceHolder", fake.Polls[0], fake.Options[1], null, DateTime.Now, 1, "127.0.0.1" );
				fake.Votes.AddVotesRow( Guid.NewGuid(), "PlaceHolder", fake.Polls[0], fake.Options[1], null, DateTime.Now, 1, "127.0.0.1" );
				fake.Votes.AddVotesRow( Guid.NewGuid(), "PlaceHolder", fake.Polls[0], fake.Options[2], null, DateTime.Now, 1, "127.0.0.1" );
				fake.Votes.AddVotesRow( Guid.NewGuid(), "PlaceHolder", fake.Polls[0], fake.Options[2], null, DateTime.Now, 1, "127.0.0.1" );

				PollDataSet.VotesRow writeInVote = fake.Votes.NewVotesRow();
				writeInVote.VoteID = Guid.NewGuid();
				writeInVote.Member = "PlaceHolder";
				writeInVote.PollsRow = fake.Polls[0];
				writeInVote.WriteInText = "Write In Vote 1";
				writeInVote.VoteRating = 1;
				writeInVote.DateVoted = DateTime.Now;
				writeInVote.IPAddress = "127.0.0.1";
				fake.Votes.AddVotesRow( writeInVote );

				fake.Polls.AddPollsRow( "Quesion2", DateTime.Now, VoteSelectionMode.Multiple.ToString(), true, false, "" );
				fake.Options.AddOptionsRow( fake.Polls[1], "Option1", 0, 0 );
				fake.Options.AddOptionsRow( fake.Polls[1], "Option2", 1, 0 );
				fake.Options.AddOptionsRow( fake.Polls[1], "Option3", 2, 0 );
			}

			return fake;
		}

		private static PollDataSet fake;

		#endregion

		#region Data Cache

		private static readonly Object cacheSyncObject;

		private static void ClearCache()
		{
			Initialize();

			lock ( cacheSyncObject )
			{
				foreach ( String cacheItemKey in cacheItems )
				{
					HttpContext.Current.Cache.Remove( cacheItemKey );
				}
				cacheItems.Clear();
			}
		}

		private static void InsertPollIntoCache( String key, PollDataSet poll )
		{
			Initialize();

			HttpContext.Current.Cache.Insert( key, poll, null, DateTime.MaxValue, TimeSpan.FromMinutes( 1 ) );
			if ( !cacheItems.Contains( key ) )
			{
				cacheItems.Add( key );
			}
		}

		private static PollDataSet GetPollFromCache( String key )
		{
			Initialize();

			return HttpContext.Current.Cache[key] as PollDataSet;
		}

		private static System.Collections.Specialized.StringCollection cacheItems = new System.Collections.Specialized.StringCollection();

		#endregion


		private static String voteTrackCookieName;
		private static PollingProvider provider;
		private static Object syncLock;
		private static PollingProviderCollection providers;

	}
}
