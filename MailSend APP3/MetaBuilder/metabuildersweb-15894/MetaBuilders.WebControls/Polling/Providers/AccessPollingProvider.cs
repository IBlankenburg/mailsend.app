using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Web;
using System.Configuration.Provider;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Manages storage of polling information for an ASP.NET application in a Microsoft Access database.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is used by the <see cref="Polling"/> class to provide polling services for an ASP.NET application using a Microsoft Access database. 
	/// The polling data used by the <see cref="AccessPollingProvider"/> is contained in an Access database that is included in the MetaBuilders Polling download.
	/// </para>
	/// <para>
	/// If you already have an Access database for your application, then you can use the same database for the <see cref="Polling"/> class.
	/// Simply add the structure present in the template database to your own.
	/// Also, make sure that the identity used by asp.net ( usually ASPNET or NETWORK SERVICE ) has read and write permissions on the database file.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following example shows the web.config file for an ASP.NET application configured to use an <see cref="AccessPollingProvider"/>.
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
	///     &lt;polling defaultProvider="AccessPollingProvider" &gt;
	///       &lt;providers&gt;
	///         &lt;add 
	///				name="AccessPollingProvider" 
	///				type="MetaBuilders.WebControls.AccessPollingProvider" 
	///				connectionStringName="AccessPollingConnection" 
	///         /&gt;
	///       &lt;/providers&gt;
	///     &lt;/polling&gt;
	///   &lt;/metabuilders&gt;
	/// 
	///   &lt;connectionStrings&gt;
	///     &lt;add name="AccessPollingConnection" connectionString="Provider=Microsoft.Jet.OLEDB.4.0; Data Source=|DataDirectory|MetaBuildersPolling.mdb" providerName="System.Data.OleDb" /&gt;
	///   &lt;/connectionStrings&gt;
	/// 
	/// &lt;/configuration&gt;
	/// </code>
	/// </example>
	public class AccessPollingProvider : PollingProvider
	{

		#region Connection

		/// <summary>
		/// Gets an <see cref="OleDbConnection"/> set to the connection string specified in web.config.
		/// </summary>
		protected OleDbConnection CreateConnection()
		{
			this.EnsureDatabase();
			return new OleDbConnection( this.ConnectionString );
		}

		private Boolean databaseFileCreated = false;

		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )
		]
		private void EnsureDatabase()
		{
			if ( databaseFileCreated )
			{
				return;
			}

			if ( !this.ConnectionString.ToUpperInvariant().Contains( "|DATADIRECTORY|" ) )
			{
				databaseFileCreated = true;
				return;
			}

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				return;
			}

			try
			{

				String appDataPath = context.Server.MapPath( "~/App_Data" );
				String dbName = StringBetweenStrings( this.ConnectionString, "|DATADIRECTORY|", ".MDB" ) + ".mdb";
				String filePath = Path.Combine( appDataPath, dbName );

				if ( !Directory.Exists( appDataPath ) )
				{
					Directory.CreateDirectory( appDataPath );
				}

				if ( !File.Exists( filePath ) )
				{
					String resourceName = "MetaBuilders.WebControls.Polling.Polling.mdb";
					using ( Stream dataBaseStream = typeof( AccessPollingProvider ).Assembly.GetManifestResourceStream( resourceName ) )
					using ( System.IO.FileStream file = new FileStream( filePath, FileMode.CreateNew, FileAccess.Write ) )
					{
						if ( dataBaseStream != null && file != null )
						{

							const int bufLen = 1000;
							byte[] buffer = new byte[bufLen];
							int bytesRead;
							while ( ( bytesRead = dataBaseStream.Read( buffer, 0, bufLen ) ) > 0 )
							{
								file.Write( buffer, 0, bytesRead );
							}
						}
						else
						{
							if ( dataBaseStream == null )
							{
								throw new ProviderException( Resources.AccessProvider_MissingAccessDatabaseResource );
							}
							if ( file == null )
							{
								throw new ProviderException( String.Format( System.Globalization.CultureInfo.InvariantCulture, Resources.AccessProvider_CantCreateFile, filePath ) );
							}
						}
					}
				}

				databaseFileCreated = true;

			}
			catch ( ProviderException )
			{
				throw;
			}
			catch ( Exception )
			{
				// gulp
			}

		}

		private static String StringBetweenStrings( String input, String before, String after )
		{
			if ( input == null )
			{
				throw new ArgumentNullException( "input" );
			}
			if ( before == null )
			{
				throw new ArgumentNullException( "before" );
			}
			if ( after == null )
			{
				throw new ArgumentNullException( "after" );
			}

			Int32 startOfBetween = 0;
			if ( before.Length != 0 )
			{
				Int32 indexOfBefore = input.IndexOf( before, StringComparison.OrdinalIgnoreCase );
				if ( indexOfBefore > -1 )
				{
					startOfBetween = indexOfBefore + before.Length;
				}
			}

			if ( after.Length == 0 )
			{
				return input.Substring( startOfBetween );
			}
			else
			{
				Int32 startOfEnd = input.LastIndexOf( after, StringComparison.OrdinalIgnoreCase );
				if ( startOfEnd == -1 )
				{
					return input.Substring( startOfBetween );
				}
				Int32 lengthOfBetween = startOfEnd - startOfBetween;
				return input.Substring( startOfBetween, lengthOfBetween );
			}
		}



		private String ConnectionString
		{
			get
			{
				return connectionString;
			}
		}
		private String connectionString;

		private void ExecuteNonQuery( OleDbCommand cmd )
		{
			using ( OleDbConnection cn = this.CreateConnection() )
			{
				cmd.Connection = cn;
				cn.Open();
				cmd.ExecuteNonQuery();
			}
		}
		#endregion

		#region PollingProvider Implementation

		/// <summary>
		/// Initializes the provider with the given name and configuration values.
		/// </summary>
		public override void Initialize( String name, System.Collections.Specialized.NameValueCollection config )
		{
			if ( config == null )
			{
				throw new ArgumentNullException( "config" );
			}
			if ( String.IsNullOrEmpty( name ) )
			{
				name = "AccessPollingProvider";
			}
			if ( string.IsNullOrEmpty( config["description"] ) )
			{
				config.Remove( "description" );
				config.Add( "description", "Access polling provider" );
			}

			base.Initialize( name, config );

			this.enableCookieVoteTracking = ConfigHelper.GetBooleanValue( config, "enableCookieVoteTracking", true );
			config.Remove( "enableCookieVoteTracking" );

			String connectionStringName = config["connectionStringName"];
			String connectionStringDirect = config["connectionString"];
			if ( String.IsNullOrEmpty( connectionStringDirect ) )
			{
				this.connectionString = ConfigHelper.GetConnectionString( connectionStringName, name );
			}
			else
			{
				this.connectionString = connectionStringDirect;
			}

			config.Remove( "connectionStringName" );
			config.Remove( "connectionString" );
			config.Remove( "name" );
			config.Remove( "description" );

			if ( config.Count > 0 )
			{
				String firstInvalidKey = config.GetKey( 0 );
				if ( !String.IsNullOrEmpty( firstInvalidKey ) )
				{
					throw new ProviderException( String.Format( System.Globalization.CultureInfo.InvariantCulture, Resources.Config_UnknownAttribute, firstInvalidKey ) );
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the polling provider is configured to use cookies to track when a user votes on a poll.
		/// </summary>
		public override Boolean EnableCookieVoteTracking
		{
			get
			{
				return enableCookieVoteTracking;
			}
		}
		private Boolean enableCookieVoteTracking;


		#region Voting

		/// <summary>
		/// Logs a vote for the given poll and option.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="optionID">The option being voted for.</param>
		protected internal override void AddVote( String member, String ipAddress, Int32 pollID, Int32 optionID )
		{
			AddVote( member, ipAddress, pollID, optionID, 1 );
		}

		/// <summary>
		/// Logs a vote for the given poll and option.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="optionID">The option being voted for.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		protected internal override void AddVote( String member, String ipAddress, Int32 pollID, Int32 optionID, Int32 voteRating )
		{
			OleDbCommand cmd = new OleDbCommand( "Insert Into " + VotesTable + " ( Member, PollID, OptionID, DateVoted, VoteRating, IPAddress ) Values ( ?, ?, ?, ?, ?, ? )" );
			cmd.Parameters.Add( "@Member", OleDbType.VarWChar, 255 ).Value = ( member != null ) ? (Object)member : (Object)DBNull.Value;
			cmd.Parameters.Add( "@PollID", OleDbType.Integer ).Value = pollID;
			cmd.Parameters.Add( "@OptionID", OleDbType.Integer ).Value = optionID;
			cmd.Parameters.Add( "@DateVoted", OleDbType.Date ).Value = DateTime.Now;
			cmd.Parameters.Add( "@VoteRating", OleDbType.Integer ).Value = voteRating;
			cmd.Parameters.Add( "@IPAddress", OleDbType.VarWChar, 50 ).Value = ( ipAddress != null ) ? ipAddress : "";

			ExecuteNonQuery( cmd );
		}


		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="writeInText">The text written in by the user.</param>
		protected internal override void AddWriteInVote( String member, String ipAddress, Int32 pollID, String writeInText )
		{
			AddWriteInVote( member, ipAddress, pollID, writeInText, 1 );
		}

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="writeInText">The text written in by the user.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		protected internal override void AddWriteInVote( String member, String ipAddress, Int32 pollID, String writeInText, Int32 voteRating )
		{
			if ( writeInText == null || writeInText.Length == 0 )
			{
				return;
			}
			OleDbCommand cmd = new OleDbCommand( "Insert Into " + VotesTable + " ( Member, PollID, WriteInText, DateVoted, VoteRating, IPAddress ) Values ( ?, ?, ?, ?, ?, ? )" );
			cmd.Parameters.Add( "@Member", OleDbType.VarWChar, 255 ).Value = ( member != null ) ? (Object)member : (Object)DBNull.Value;
			cmd.Parameters.Add( "@PollID", OleDbType.Integer ).Value = pollID;
			cmd.Parameters.Add( "@WriteInText", OleDbType.VarWChar, 255 ).Value = writeInText;
			cmd.Parameters.Add( "@DateVoted", OleDbType.Date ).Value = DateTime.Now;
			cmd.Parameters.Add( "@VoteRating", OleDbType.Integer ).Value = voteRating;
			cmd.Parameters.Add( "@IPAddress", OleDbType.VarWChar, 50 ).Value = ( ipAddress != null ) ? ipAddress : "";

			ExecuteNonQuery( cmd );
		}

		#endregion

		#region User Tracking

		/// <summary>
		/// Determines if the given member has voted on the given poll.
		/// </summary>
		protected internal override Boolean UserHasVoted( String member, Int32 pollID )
		{
			OleDbCommand cmd = new OleDbCommand( "Select Count(*) From " + VotesTable + " Where Member = ? And PollID = ?" );
			cmd.Parameters.Add( "@Member", OleDbType.VarWChar ).Value = member;
			cmd.Parameters.Add( "@PollID", OleDbType.Integer ).Value = pollID;
			Object result = null;

			using ( OleDbConnection cn = this.CreateConnection() )
			{
				cmd.Connection = cn;
				cn.Open();
				result = cmd.ExecuteScalar();
			}

			if ( result != null )
			{
				try
				{
					Int32 voteCount = Convert.ToInt32( result, System.Globalization.CultureInfo.InvariantCulture );
					return voteCount != 0;
				}
				catch ( FormatException )
				{
					return false;
				}
				catch ( OverflowException )
				{
					return false;
				}
			}
			return false;
		}

		#endregion

		#region Managment

		/// <summary>
		/// Adds a new poll with the given properties.
		/// </summary>
		/// <param name="text">The text of the question of the poll.</param>
		/// <param name="startDate">The date and time on which the poll will start.</param>
		/// <param name="voteMode">The selection mode of the poll, single or multiple.</param>
		/// <param name="enabled">Whether the poll is enabled or not.</param>
		/// <param name="allowWriteIns">Whether the poll will allow writes-in votes.</param>
		/// <param name="category">The category of the poll.</param>
		protected internal override void InsertPoll( String text, DateTime startDate, VoteSelectionMode voteMode, Boolean enabled, Boolean allowWriteIns, String category )
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
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "INSERT INTO " + PollsTable + " ([Text], StartDate, VoteMode, Enabled, AllowWriteIns, Category ) VALUES (?, ?, ?, ?, ?, ?)";
			cmd.Parameters.Add( "@Question", OleDbType.VarWChar, 255, "Text" ).Value = text;
			cmd.Parameters.Add( "@StartDate", OleDbType.Date, 8, "StartDate" ).Value = startDate;
			cmd.Parameters.Add( "@VoteMode", OleDbType.VarWChar, 10, "VoteMode" ).Value = voteMode.ToString();
			cmd.Parameters.Add( "@Enabled", OleDbType.Boolean, 1, "Enabled" ).Value = enabled;
			cmd.Parameters.Add( "@AllowWriteIns", OleDbType.Boolean, 1, "AllowWriteIns" ).Value = allowWriteIns;
			cmd.Parameters.Add( "@Category", OleDbType.VarWChar, 255, "Category" ).Value = category;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Updates the poll specified by pollID.
		/// </summary>
		/// <param name="pollID">The ID of he poll to update.</param>
		/// <param name="text">The text of the question of the poll.</param>
		/// <param name="startDate">The date and time on which the poll will start.</param>
		/// <param name="voteMode">The selection mode of the poll, single or multiple.</param>
		/// <param name="enabled">Whether the poll is enabled or not.</param>
		/// <param name="allowWriteIns">Whether the poll will allow write-in votes.</param>
		/// <param name="category">The category of the poll.</param>
		protected internal override void UpdatePoll( Int32 pollID, String text, DateTime startDate, VoteSelectionMode voteMode, Boolean enabled, Boolean allowWriteIns, String category )
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
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "UPDATE " + PollsTable + " SET [Text] = ?, StartDate = ?, VoteMode = ?, Enabled = ?, AllowWriteIns = ?, Category = ? WHERE (PollID = ?)";
			cmd.Parameters.Add( "@Question", OleDbType.VarWChar, 255, "Question" ).Value = text;
			cmd.Parameters.Add( "@StartDate", OleDbType.Date, 8, "StartDate" ).Value = startDate;
			cmd.Parameters.Add( "@VoteMode", OleDbType.VarWChar, 10, "VoteMode" ).Value = voteMode.ToString();
			cmd.Parameters.Add( "@Enabled", OleDbType.Boolean, 1, "Enabled" ).Value = enabled;
			cmd.Parameters.Add( "@AllowWriteIns", OleDbType.Boolean, 1, "AllowWriteIns" ).Value = allowWriteIns;
			cmd.Parameters.Add( "@Category", OleDbType.VarWChar, 255, "Category" ).Value = category;
			cmd.Parameters.Add( "@Original_PollID", OleDbType.Integer, 4, "PollID" ).Value = pollID;


			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Deletes the given poll.
		/// </summary>
		/// <param name="pollID">The ID of the poll to delete.</param>
		protected internal override void DeletePoll( Int32 pollID )
		{
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "DELETE FROM " + PollsTable + " WHERE (PollID = ?)";
			cmd.Parameters.Add( "@PollID", OleDbType.Integer, 4, "PollID" ).Value = pollID;

			ExecuteNonQuery( cmd );
		}


		/// <summary>
		/// Adds a new option to a poll.
		/// </summary>
		/// <param name="pollID">The poll to add the option to.</param>
		/// <param name="text">The text of the option.</param>
		/// <param name="displayOrder">The order in which the option will be displayed.</param>
		protected internal override void InsertOption( Int32 pollID, String text, Int32 displayOrder )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "INSERT INTO " + OptionsTable + " (PollID, [Text], DisplayOrder) VALUES (?, ?, ?)";
			cmd.Parameters.Add( "@PollID", OleDbType.Integer, 4, "PollID" ).Value = pollID;
			cmd.Parameters.Add( "@Text", OleDbType.VarWChar, 255, "Text" ).Value = text;
			cmd.Parameters.Add( "@DisplayOrder", OleDbType.Integer, 4, "DisplayOrder" ).Value = displayOrder;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Updates the option specified by optionID.
		/// </summary>
		/// <param name="optionID">The ID of the option to update.</param>
		/// <param name="pollID">The poll associated to the option.</param>
		/// <param name="text">The text of the option.</param>
		/// <param name="displayOrder">The order in which the option will be displayed.</param>
		protected internal override void UpdateOption( Int32 optionID, Int32 pollID, String text, Int32 displayOrder )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "UPDATE " + OptionsTable + " SET PollID = ?, [Text] = ?, DisplayOrder = ? WHERE (OptionID = ?)";
			cmd.Parameters.Add( "@PollID", System.Data.OleDb.OleDbType.Integer, 4, "PollID" ).Value = pollID;
			cmd.Parameters.Add( "@Text", System.Data.OleDb.OleDbType.VarWChar, 255, "Text" ).Value = text;
			cmd.Parameters.Add( "@DisplayOrder", System.Data.OleDb.OleDbType.Integer, 4, "DisplayOrder" ).Value = displayOrder;
			cmd.Parameters.Add( "@OptionID", System.Data.OleDb.OleDbType.Integer, 4, "OptionID" ).Value = optionID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Deletes the given option.
		/// </summary>
		/// <param name="optionID">The ID of the option to delete.</param>
		protected internal override void DeleteOption( Int32 optionID )
		{
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "Delete From " + OptionsTable + " Where OptionID = ?";
			cmd.Parameters.Add( "@OptionID", System.Data.OleDb.OleDbType.Integer, 4, "OptionID" ).Value = optionID;

			ExecuteNonQuery( cmd );
		}


		/// <summary>
		/// Updates the write-in vote specified by voteID.
		/// </summary>
		/// <param name="voteID">The ID of the write-in vote to update.</param>
		/// <param name="text">The text of the answer.</param>
		protected internal override void UpdateWriteInVote( System.Guid voteID, String text )
		{
			if ( text == null )
			{
				text = "";
			}
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "UPDATE " + VotesTable + " SET WriteInText = ? WHERE (VoteID = ?)";
			cmd.Parameters.Add( "@Text", OleDbType.VarWChar, 255, "Text" ).Value = text;
			cmd.Parameters.Add( "@VoteID", OleDbType.Guid ).Value = voteID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Deletes a vote specified by ID.
		/// </summary>
		/// <param name="voteID">The ID of the vote to delete.</param>
		protected internal override void DeleteVote( System.Guid voteID )
		{
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "Delete From " + VotesTable + " Where VoteID = ?";
			cmd.Parameters.Add( "@VoteID", OleDbType.Guid ).Value = voteID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Resets the given poll to no votes.
		/// </summary>
		/// <param name="pollID">The ID of the poll to reset.</param>
		protected internal override void ResetPoll( Int32 pollID )
		{
			OleDbCommand cmd = new OleDbCommand();
			cmd.CommandText = "Delete From " + VotesTable + " Where PollID = ?";
			cmd.Parameters.Add( "@PollID", OleDbType.Integer ).Value = pollID;

			ExecuteNonQuery( cmd );

		}

		#endregion

		#region Selection

		/// <summary>
		/// Gets the current poll data and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <returns>A <see cref="PollDataSet"/> containing the current poll.</returns>
		protected internal override PollDataSet GetCurrentPoll()
		{

			PollDataSet poll = new PollDataSet();

			using ( OleDbConnection cn = this.CreateConnection() )
			{
				OleDbDataAdapter da = new OleDbDataAdapter( "Select Top 1 * From " + PollsTable + " Where StartDate <= ? And Enabled = ? Order By StartDate desc", cn );
				da.SelectCommand.Parameters.Add( "@StartDate", OleDbType.Date ).Value = DateTime.Now;
				da.SelectCommand.Parameters.Add( "@Enabled", OleDbType.Boolean ).Value = true;
				da.TableMappings.Add( PollsTable, poll.Polls.TableName );
				da.Fill( poll.Polls );
			}

			if ( poll.Polls.Count == 1 )
			{
				this.GetResults( poll );
			}
			return poll;
		}

		/// <summary>
		/// Gets the current poll data for the given category and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <returns>A <see cref="PollDataSet"/> containing the current poll in the given category.</returns>
		protected internal override PollDataSet GetCurrentPoll( String pollCategory )
		{
			if ( pollCategory == null )
			{
				pollCategory = "";
			}

			PollDataSet poll = new PollDataSet();

			using ( OleDbConnection cn = this.CreateConnection() )
			{
				OleDbDataAdapter da = new OleDbDataAdapter( "Select Top 1 * From " + PollsTable + " Where StartDate <= ? And Enabled = ? And Category = ? Order By StartDate desc", cn );
				da.SelectCommand.Parameters.Add( "@StartDate", OleDbType.Date ).Value = DateTime.Now;
				da.SelectCommand.Parameters.Add( "@Enabled", OleDbType.Boolean ).Value = true;
				da.SelectCommand.Parameters.Add( "@Category", OleDbType.VarWChar ).Value = pollCategory;
				da.TableMappings.Add( PollsTable, poll.Polls.TableName );
				da.Fill( poll.Polls );
			}

			if ( poll.Polls.Count == 1 )
			{
				this.GetResults( poll );
			}
			return poll;
		}
		/// <summary>
		/// Gets the poll data specified by the pollID, and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <param name="pollID">The ID of the poll to retrieve.</param>
		/// <returns>A <see cref="PollDataSet"/> containing the specified poll data.</returns>
		protected internal override PollDataSet GetPoll( Int32 pollID )
		{

			PollDataSet poll = new PollDataSet();

			using ( OleDbConnection cn = this.CreateConnection() )
			{
				OleDbDataAdapter da = new OleDbDataAdapter( "Select * From " + PollsTable + " Where PollID = ?", cn );
				da.SelectCommand.Parameters.Add( "@PollID", OleDbType.Integer ).Value = pollID;
				da.TableMappings.Add( PollsTable, poll.Polls.TableName );
				da.Fill( poll.Polls );
			}

			if ( poll.Polls.Count == 1 )
			{
				this.GetResults( poll );
			}

			return poll;
		}

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls.
		/// </summary>
		protected internal override PollDataSet GetAllPolls()
		{
			PollDataSet polls = new PollDataSet();

			using ( OleDbConnection cn = this.CreateConnection() )
			{
				OleDbDataAdapter da;

				da = new OleDbDataAdapter( "Select * From " + PollsTable, cn );
				da.TableMappings.Add( PollsTable, polls.Polls.TableName );
				da.Fill( polls.Polls );

				da = new OleDbDataAdapter( "Select * From " + OptionsView, cn );
				da.TableMappings.Add( OptionsView, polls.Options.TableName );
				da.Fill( polls.Options );

				da = new OleDbDataAdapter( "Select * From " + VotesTable, cn );
				da.TableMappings.Add( VotesTable, polls.Votes.TableName );
				da.Fill( polls.Votes );
			}

			return polls;
		}

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls with the given filter.
		/// </summary>
		protected internal override PollDataSet GetPollsByFilter( PollSelectionFilter filter )
		{

			PollDataSet polls = new PollDataSet();

			using ( OleDbConnection cn = this.CreateConnection() )
			{
				OleDbDataAdapter da = null;

				da = new OleDbDataAdapter();
				da.SelectCommand = CreateSelectionCommand( "*", filter );
				da.SelectCommand.Connection = cn;
				da.TableMappings.Add( PollsTable, polls.Polls.TableName );
				da.Fill( polls.Polls );

				da = new OleDbDataAdapter();
				da.SelectCommand = CreateSelectionCommand( "PollID", filter );
				da.SelectCommand.CommandText = "Select * From " + OptionsView + " Where PollID In ( " + da.SelectCommand.CommandText + " )";
				da.SelectCommand.Connection = cn;
				da.TableMappings.Add( OptionsView, polls.Options.TableName );
				da.Fill( polls.Options );

				da = new OleDbDataAdapter();
				da.SelectCommand = CreateSelectionCommand( "PollID", filter );
				da.SelectCommand.CommandText = "Select * From " + VotesTable + " Where PollID In ( " + da.SelectCommand.CommandText + " )";
				da.SelectCommand.Connection = cn;
				da.TableMappings.Add( VotesTable, polls.Votes.TableName );
				da.Fill( polls.Votes );
			}

			return polls;


		}

		private static OleDbCommand CreateSelectionCommand( String fields, PollSelectionFilter filter )
		{
			OleDbCommand cmd = new OleDbCommand();

			System.Text.StringBuilder sql = new System.Text.StringBuilder();
			sql.Append( "Select " );
			sql.Append( fields );
			sql.Append( " From " );
			sql.Append( PollsTable );
			if ( filter.IsFilterSet )
			{
				sql.Append( " Where " );
				if ( filter.Category != null )
				{
					sql.Append( " Category Like ?" );
					cmd.Parameters.Add( "@Category", OleDbType.VarWChar ).Value = "%" + filter.Category + "%";
				}
				if ( filter.Text != null )
				{
					if ( sql[sql.Length - 1] != ' ' )
					{
						sql.Append( " And " );
					}
					sql.Append( " Text Like ?" );
					cmd.Parameters.Add( "@Text", OleDbType.VarWChar ).Value = "%" + filter.Text + "%";
				}
				if ( !filter.BeginDate.IsNull )
				{
					if ( sql[sql.Length - 1] != ' ' )
					{
						sql.Append( " And " );
					}
					sql.Append( " StartDate >= ?" );
					cmd.Parameters.Add( "@StartDate", OleDbType.Date ).Value = filter.BeginDate.Value;
				}
				if ( !filter.EndDate.IsNull )
				{
					if ( sql[sql.Length - 1] != ' ' )
					{
						sql.Append( " And " );
					}
					sql.Append( " StartDate <= ?" );
					cmd.Parameters.Add( "@EndDate", OleDbType.Date ).Value = filter.EndDate.Value;
				}
			}
			cmd.CommandText = sql.ToString();
			return cmd;
		}

		#endregion

		#endregion

		#region Utility

		private void GetResults( PollDataSet polls )
		{

			polls.EnforceConstraints = false;

			using ( OleDbConnection cn = this.CreateConnection() )
			{

				foreach ( PollDataSet.PollsRow pollRow in polls.Polls )
				{

					OleDbDataAdapter answersAdapter = new OleDbDataAdapter( "Select * From " + OptionsView + " Where PollID = ?", cn );
					answersAdapter.SelectCommand.Parameters.Add( "@PollID", OleDbType.Integer, 4, "PollID" ).Value = pollRow.PollID;
					answersAdapter.TableMappings.Add( OptionsView, polls.Options.TableName );
					answersAdapter.Fill( polls.Options );

					OleDbDataAdapter writeInsAdapter = new OleDbDataAdapter( "Select * From " + VotesTable + " Where PollID = ?", cn );
					writeInsAdapter.SelectCommand.Parameters.Add( "@PollID", OleDbType.Integer, 4, "PollID" ).Value = pollRow.PollID;
					writeInsAdapter.TableMappings.Add( VotesTable, polls.Votes.TableName );
					writeInsAdapter.Fill( polls.Votes );

				}
			}

			try
			{
				polls.EnforceConstraints = true;
			}
			catch ( System.Data.ConstraintException )
			{
				foreach ( DataTable table in polls.Tables )
				{
					if ( table.HasErrors )
					{
						foreach ( DataRow row in table.GetErrors() )
						{
							System.Diagnostics.Debug.WriteLine( row.RowError );
						}
					}
				}
				throw;
			}
		}

		#endregion

		private const String PollsTable = "MetaBuilders_Polling_Polls";
		private const String OptionsTable = "MetaBuilders_Polling_Options";
		private const String OptionsView = "MetaBuilders_Polling_Options_VoteAmounts";
		private const String VotesTable = "MetaBuilders_Polling_Votes";

	}
}
