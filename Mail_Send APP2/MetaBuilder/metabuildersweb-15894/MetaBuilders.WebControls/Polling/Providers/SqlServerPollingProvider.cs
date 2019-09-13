using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Configuration.Provider;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Manages storage of polling information for an ASP.NET application in a Microsoft Sql Server database.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is used by the <see cref="Polling"/> class to provide polling services for an ASP.NET application using a Microsoft Sql Server database. 
	/// Included in the MetaBuilders Polling package is a script file to run against your Sql Server database, which adds the tables which the <see cref="SqlServerPollingProvider"/> expects.</para>
	/// </remarks>
	/// <example>
	/// The following example shows the web.config file for an ASP.NET application configured to use an <see cref="SqlServerPollingProvider"/>.
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
	public class SqlServerPollingProvider : PollingProvider
	{

		#region Connection

		/// <summary>
		/// Gets a <see cref="SqlConnection"/> set to the connection string specified in web.config.
		/// </summary>
		protected SqlConnection CreateConnection()
		{
			return new SqlConnection( this.ConnectionString );
		}

		private String ConnectionString
		{
			get
			{
				return connectionString;
			}
		}
		private String connectionString;
		private String connectionStringName;

		private void ExecuteNonQuery( SqlCommand cmd )
		{
			using ( SqlConnection cn = this.CreateConnection() )
			{
				cmd.Connection = cn;
				cmd.Connection.Open();
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
				name = "SqlServerPollingProvider";
			}
			if ( string.IsNullOrEmpty( config["description"] ) )
			{
				config.Remove( "description" );
				config.Add( "description", "SqlServer polling provider" );
			}

			base.Initialize( name, config );

			this.enableCookieVoteTracking = ConfigHelper.GetBooleanValue( config, "enableCookieVoteTracking", true );
			config.Remove( "enableCookieVoteTracking" );

			this.connectionStringName = config["connectionStringName"];
			this.connectionString = ConfigHelper.GetConnectionString( this.connectionStringName, name );
			config.Remove( "connectionStringName" );

			if ( config.Count > 0 )
			{
				String firstInvalidKey = config.GetKey( 0 );
				if ( firstInvalidKey != null && firstInvalidKey.Length > 0 )
				{
					throw new ProviderException( String.Format( CultureInfo.InvariantCulture, Resources.Config_UnknownAttribute, firstInvalidKey ) );
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
			SqlCommand cmd = new SqlCommand( "Insert Into " + VotesTable + " ( Member, PollID, OptionID, DateVoted, VoteRating, IPAddress ) Values ( @Member, @PollID, @OptionID, @DateVoted, @VoteRating, @IPAddress )" );
			cmd.Parameters.Add( "@Member", SqlDbType.NVarChar, 255 ).Value = ( member != null ) ? (Object)member : (Object)DBNull.Value;
			cmd.Parameters.Add( "@PollID", SqlDbType.Int ).Value = pollID;
			cmd.Parameters.Add( "@OptionID", SqlDbType.Int ).Value = optionID;
			cmd.Parameters.Add( "@DateVoted", SqlDbType.DateTime ).Value = DateTime.Now;
			cmd.Parameters.Add( "@VoteRating", SqlDbType.Int ).Value = voteRating;
			cmd.Parameters.Add( "@IPAddress", SqlDbType.NVarChar, 50 ).Value = ( ipAddress != null ) ? ipAddress : "";

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="writeInText">The text written in by the user.</param>
		protected internal override void AddWriteInVote( String member, String ipAddress, Int32 pollID, String writeInText )
		{
			AddWriteInVote( member, ipAddress, pollID, writeInText, 1 );
		}

		/// <summary>
		/// Logs a vote for the given poll with the given write-in answer.
		/// </summary>
		/// <param name="member">The name of the member who voted</param>
		/// <param name="ipAddress">The IP Address of the user who voted</param>
		/// <param name="pollID">The poll being voted on.</param>
		/// <param name="writeInText">The text written in by the user.</param>
		/// <param name="voteRating">The rating of the vote.</param>
		protected internal override void AddWriteInVote( String member, String ipAddress, Int32 pollID, String writeInText, Int32 voteRating )
		{
			if ( writeInText == null || writeInText.Length == 0 )
			{
				return;
			}
			SqlCommand cmd = new SqlCommand( "Insert Into " + VotesTable + " ( Member, PollID, WriteInText, DateVoted, VoteRating, IPAddress ) Values ( @Member, @PollID, @WriteInText, @DateVoted, @VoteRating, @IPAddress )" );
			cmd.Parameters.Add( "@Member", SqlDbType.NVarChar, 255 ).Value = ( member != null ) ? (Object)member : (Object)DBNull.Value;
			cmd.Parameters.Add( "@PollID", SqlDbType.Int ).Value = pollID;
			cmd.Parameters.Add( "@WriteInText", SqlDbType.NVarChar, 255 ).Value = writeInText;
			cmd.Parameters.Add( "@DateVoted", SqlDbType.DateTime ).Value = DateTime.Now;
			cmd.Parameters.Add( "@VoteRating", SqlDbType.Int ).Value = voteRating;
			cmd.Parameters.Add( "@IPAddress", SqlDbType.NVarChar, 50 ).Value = ( ipAddress != null ) ? ipAddress : "";

			ExecuteNonQuery( cmd );
		}

		#endregion

		#region User Tracking

		/// <summary>
		/// Determines if the given member has voted on the given poll.
		/// </summary>
		protected internal override Boolean UserHasVoted( String member, Int32 pollID )
		{
			if ( member == null )
			{
				return false;
			}

			SqlCommand cmd = new SqlCommand( "Select Count(*) From " + VotesTable + " Where Member = @Member And PollID = @PollID" );
			cmd.Parameters.Add( "@Member", SqlDbType.NVarChar ).Value = member;
			cmd.Parameters.Add( "@PollID", SqlDbType.Int ).Value = pollID;

			Object result = null;

			using ( SqlConnection cn = this.CreateConnection() )
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
		/// <returns>The ID assigned to the poll.</returns>
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
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "INSERT INTO " + PollsTable + " (Text, StartDate, VoteMode, Enabled, AllowWriteIns, Category) VALUES (@Text, @StartDate, @VoteMode, @Enabled, @AllowWriteIns, @Category)";
			cmd.Parameters.Add( "@Text", SqlDbType.NVarChar, 255 ).Value = text;
			cmd.Parameters.Add( "@StartDate", SqlDbType.DateTime ).Value = startDate;
			cmd.Parameters.Add( "@VoteMode", SqlDbType.NVarChar ).Value = voteMode.ToString();
			cmd.Parameters.Add( "@Enabled", SqlDbType.Bit ).Value = enabled;
			cmd.Parameters.Add( "@AllowWriteIns", SqlDbType.Bit ).Value = allowWriteIns;
			cmd.Parameters.Add( "@Category", SqlDbType.NVarChar, 255 ).Value = category;

			ExecuteNonQuery( cmd );
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
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "UPDATE " + PollsTable + " SET Text = @Text, StartDate = @StartDate, VoteMode = @VoteMode, Enabled = @Enabled, AllowWriteIns = @AllowWriteIns, Category = @Category WHERE (PollID = @PollID)";
			cmd.Parameters.Add( "@Text", SqlDbType.NVarChar, 255 ).Value = text;
			cmd.Parameters.Add( "@StartDate", SqlDbType.DateTime ).Value = startDate;
			cmd.Parameters.Add( "@VoteMode", SqlDbType.NVarChar ).Value = voteMode.ToString();
			cmd.Parameters.Add( "@Enabled", SqlDbType.Bit ).Value = enabled;
			cmd.Parameters.Add( "@AllowWriteIns", SqlDbType.Bit ).Value = allowWriteIns;
			cmd.Parameters.Add( "@Category", SqlDbType.NVarChar, 255 ).Value = category;
			cmd.Parameters.Add( "@PollID", SqlDbType.Int ).Value = pollID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Deletes the given poll.
		/// </summary>
		/// <param name="pollID">The ID of the poll to delete.</param>
		protected internal override void DeletePoll( Int32 pollID )
		{
			SqlCommand cmd = new SqlCommand( "Delete From " + PollsTable + " Where PollID = @PollID" );
			cmd.Parameters.Add( "@PollID", SqlDbType.Int, 4, "PollID" ).Value = pollID;

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

			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "INSERT INTO " + OptionsTable + " (PollID, Text, DisplayOrder) VALUES (@PollID, @Text, @DisplayOrder)";
			cmd.Parameters.Add( "@PollID", System.Data.SqlDbType.Int ).Value = pollID;
			cmd.Parameters.Add( "@Text", System.Data.SqlDbType.NVarChar, 255 ).Value = text;
			cmd.Parameters.Add( "@DisplayOrder", System.Data.SqlDbType.Int ).Value = displayOrder;

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

			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "UPDATE " + OptionsTable + " SET PollID = @PollID, Text = @Text, DisplayOrder = @DisplayOrder WHERE (OptionID = @Original_OptionID)";
			cmd.Parameters.Add( "@PollID", System.Data.SqlDbType.Int ).Value = pollID;
			cmd.Parameters.Add( "@Text", System.Data.SqlDbType.NVarChar, 255 ).Value = text;
			cmd.Parameters.Add( "@DisplayOrder", System.Data.SqlDbType.Int ).Value = displayOrder;
			cmd.Parameters.Add( "@Original_OptionID", System.Data.SqlDbType.Int ).Value = optionID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Deletes the given option.
		/// </summary>
		/// <param name="optionID">The ID of the option to delete.</param>
		protected internal override void DeleteOption( Int32 optionID )
		{
			SqlCommand cmd = new SqlCommand( "Delete From " + OptionsTable + " Where OptionID = @OptionID" );
			cmd.Parameters.Add( "@OptionID", SqlDbType.Int, 4, "OptionID" ).Value = optionID;

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

			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "UPDATE " + VotesTable + " SET WriteInText = @WriteInText WHERE (VoteID = @VoteID)";
			cmd.Parameters.Add( "@WriteInText", SqlDbType.NVarChar, 255 ).Value = text;
			cmd.Parameters.Add( "@VoteID", SqlDbType.UniqueIdentifier ).Value = voteID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Deletes a vote specified by ID.
		/// </summary>
		/// <param name="voteID">The ID of the vote to delete.</param>
		protected internal override void DeleteVote( System.Guid voteID )
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "Delete From " + VotesTable + " Where VoteID = @VoteID";
			cmd.Parameters.Add( "@VoteID", SqlDbType.UniqueIdentifier ).Value = voteID;

			ExecuteNonQuery( cmd );
		}

		/// <summary>
		/// Resets the given poll to no votes.
		/// </summary>
		/// <param name="pollID">The ID of the poll to reset.</param>
		protected internal override void ResetPoll( Int32 pollID )
		{
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "Delete From " + VotesTable + " Where PollID = @PollID";
			cmd.Parameters.Add( "@PollID", SqlDbType.Int ).Value = pollID;

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

			PollDataSet value = new PollDataSet();

			using ( SqlConnection cn = this.CreateConnection() )
			{
				SqlDataAdapter a = new SqlDataAdapter( "Select Top 1 * From " + PollsTable + " Where StartDate <= @StartDate And Enabled = 1 Order By StartDate desc", cn );
				a.SelectCommand.Parameters.Add( "@StartDate", SqlDbType.DateTime ).Value = DateTime.Now;
				a.TableMappings.Add( PollsTable, value.Polls.TableName );

				a.Fill( value.Polls );
			}

			if ( value.Polls.Count == 1 )
			{
				this.GetResults( value );
			}
			return value;
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

			PollDataSet value = new PollDataSet();

			using ( SqlConnection cn = this.CreateConnection() )
			{
				SqlDataAdapter a = new SqlDataAdapter( "Select Top 1 * From " + PollsTable + " Where StartDate <= @StartDate And Enabled = 1 And Category = @Category Order By StartDate desc", cn );
				a.SelectCommand.Parameters.Add( "@StartDate", SqlDbType.DateTime ).Value = DateTime.Now;
				a.SelectCommand.Parameters.Add( "@Category", SqlDbType.NVarChar ).Value = pollCategory;
				a.TableMappings.Add( PollsTable, value.Polls.TableName );

				a.Fill( value.Polls );
			}

			if ( value.Polls.Count == 1 )
			{
				this.GetResults( value );
			}
			return value;
		}
		/// <summary>
		/// Gets the poll data specified by the pollID, and returns it in a <see cref="PollDataSet"/>.
		/// </summary>
		/// <param name="pollID">The ID of the poll to retrieve.</param>
		/// <returns>A <see cref="PollDataSet"/> containing the specified poll data.</returns>
		protected internal override PollDataSet GetPoll( Int32 pollID )
		{

			PollDataSet value = new PollDataSet();

			using ( SqlConnection cn = this.CreateConnection() )
			{
				SqlDataAdapter a = new SqlDataAdapter( "Select * From " + PollsTable + " Where PollID = @PollID", cn );
				a.SelectCommand.Parameters.Add( "@PollID", SqlDbType.Int ).Value = pollID;
				a.TableMappings.Add( PollsTable, value.Polls.TableName );
				a.Fill( value.Polls );
			}

			if ( value.Polls.Count == 1 )
			{
				this.GetResults( value );
			}
			return value;
		}

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls.
		/// </summary>
		protected internal override PollDataSet GetAllPolls()
		{

			PollDataSet value = new PollDataSet();

			using ( SqlConnection cn = this.CreateConnection() )
			{
				SqlDataAdapter da;

				value.EnforceConstraints = false;

				da = new SqlDataAdapter( "Select * From " + PollsTable, cn );
				da.TableMappings.Add( PollsTable, value.Polls.TableName );
				da.Fill( value.Polls );

				da = new SqlDataAdapter( "Select * From " + OptionsView, cn );
				da.TableMappings.Add( OptionsView, value.Options.TableName );
				da.Fill( value.Options );

				da = new SqlDataAdapter( "Select * From " + VotesTable, cn );
				da.TableMappings.Add( VotesTable, value.Votes.TableName );
				da.Fill( value.Votes );

				value.EnforceConstraints = true;
			}

			return value;
		}

		/// <summary>
		/// Gets a <see cref="PollDataSet"/> containg all the polls with the given filter.
		/// </summary>
		protected internal override PollDataSet GetPollsByFilter( PollSelectionFilter filter )
		{

			PollDataSet polls = new PollDataSet();

			using ( SqlConnection cn = this.CreateConnection() )
			{
				SqlDataAdapter da = null;

				da = new SqlDataAdapter();
				da.SelectCommand = CreateSelectionCommand( "*", filter );
				da.SelectCommand.Connection = cn;
				da.TableMappings.Add( PollsTable, polls.Polls.TableName );
				da.Fill( polls.Polls );

				da = new SqlDataAdapter();
				da.SelectCommand = CreateSelectionCommand( "PollID", filter );
				da.SelectCommand.CommandText = "Select * From " + OptionsView + " Where PollID In ( " + da.SelectCommand.CommandText + " )";
				da.SelectCommand.Connection = cn;
				da.TableMappings.Add( OptionsView, polls.Options.TableName );
				da.Fill( polls.Options );

				da = new SqlDataAdapter();
				da.SelectCommand = CreateSelectionCommand( "PollID", filter );
				da.SelectCommand.CommandText = "Select * From " + VotesTable + " Where PollID In ( " + da.SelectCommand.CommandText + " )";
				da.SelectCommand.Connection = cn;
				da.TableMappings.Add( VotesTable, polls.Votes.TableName );
				da.Fill( polls.Votes );
			}

			return polls;


		}

		private static SqlCommand CreateSelectionCommand( String fields, PollSelectionFilter filter )
		{
			SqlCommand cmd = new SqlCommand();

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
					sql.Append( " Category Like @Category" );
					cmd.Parameters.Add( "@Category", SqlDbType.NVarChar ).Value = "%" + filter.Category + "%";
				}
				if ( filter.Text != null )
				{
					if ( sql[sql.Length - 1] != ' ' )
					{
						sql.Append( " And " );
					}
					sql.Append( " Text Like @Text" );
					cmd.Parameters.Add( "@Text", SqlDbType.NVarChar ).Value = "%" + filter.Text + "%";
				}
				if ( !filter.BeginDate.IsNull )
				{
					if ( sql[sql.Length - 1] != ' ' )
					{
						sql.Append( " And " );
					}
					sql.Append( " StartDate >= @StartDate" );
					cmd.Parameters.Add( "@StartDate", SqlDbType.DateTime ).Value = filter.BeginDate.Value;
				}
				if ( !filter.EndDate.IsNull )
				{
					if ( sql[sql.Length - 1] != ' ' )
					{
						sql.Append( " And " );
					}
					sql.Append( " StartDate <= @EndDate" );
					cmd.Parameters.Add( "@EndDate", SqlDbType.DateTime ).Value = filter.EndDate.Value;
				}
			}
			cmd.CommandText = sql.ToString();
			return cmd;
		}
		#endregion

		#endregion

		#region Misc

		private void GetResults( PollDataSet poll )
		{
			poll.EnforceConstraints = false;

			using ( SqlConnection cn = this.CreateConnection() )
			{

				foreach ( PollDataSet.PollsRow pollRow in poll.Polls )
				{


					SqlDataAdapter a = new SqlDataAdapter( "Select * From " + OptionsView + " Where PollID = @PollID", cn );
					a.SelectCommand.Parameters.Add( "@PollID", SqlDbType.Int, 4, "PollID" ).Value = pollRow.PollID;
					a.TableMappings.Add( OptionsView, poll.Options.TableName );
					a.Fill( poll.Options );


					a = new SqlDataAdapter( "Select * From " + VotesTable + " Where PollID = @PollID", cn );
					a.SelectCommand.Parameters.Add( "@PollID", SqlDbType.Int, 4, "PollID" ).Value = pollRow.PollID;
					a.TableMappings.Add( VotesTable, poll.Votes.TableName );
					a.Fill( poll.Votes );

				}
			}

			poll.EnforceConstraints = true;
		}

		#endregion

		private const String PollsTable = "MetaBuilders_Polling_Polls";
		private const String OptionsTable = "MetaBuilders_Polling_Options";
		private const String OptionsView = "MetaBuilders_Polling_Options_VoteAmounts";
		private const String VotesTable = "MetaBuilders_Polling_Votes";

	}
}
