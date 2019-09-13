using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The <see cref="PollView"/> control is the interface for reading and voting on polls.
	/// </summary>
	/// <remarks>
	/// <para>The <see cref="PollView"/> control has two views or modes. One view allows for voting on
	/// a poll. The basic interface in this view shows the text of the poll, the options with inputs to select them, and a vote button.
	/// The other view shows the results of a poll. It shows the text of the poll and the options with bar graphs showing how many
	/// votes each has received. By default, the <see cref="PollView"/> control will automaticly switch between these views, depending
	/// on if the user has voted on the shown poll or not.</para>
	/// <para>
	/// It's possible to customize the look of the <see cref="PollView"/> control using styles and templates.
	/// The control contains styles for all interface elements, as well as header and footer templates that can databind
	/// to the current poll data. Also, all text elements, such as button text, can be modified.
	/// </para>
	/// <para>
	/// The <see cref="PollView"/> control gets its poll data from the <see cref="Polling"/> class.
	/// By default, it displays the poll returned by <see cref="Polling.GetCurrentPoll()"/>, 
	/// however this can be changed by setting either the <see cref="PollCategory"/> or <see cref="CustomPollID"/> properties.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following is an example page that uses the PollView control.
	/// <code>
	/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
	/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
	/// &lt;mb:PollView id="PollView1" runat="server"
	///   BorderWidth="2px"
	///   BorderColor="Black"
	///   BackColor="LightGray"
	///   ResultsButtonType="LinkButton"
	///   &gt;
	///   &lt;BarStyle
	///     ForeColor="Orange"
	///     BackColor="White"
	///     BorderColor="Black"
	///     BorderWidth="1px"
	///   /&gt;
	/// &lt;/mb:PollView&gt;
	/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;	
	/// </code>
	/// </example>
	[
	Designer( typeof( PollViewDesigner ) ),
	Themeable(true),
	DefaultEvent( "VoteReceived" ),
	]
	public class PollView : System.Web.UI.WebControls.CompositeControl
	{

		#region Events

		/// <summary>
		/// The event that is raised when a user submits a vote.
		/// </summary>
		/// <remarks>
		/// This event allows you to either cancel or change a vote-set.
		/// Common uses would be to delete write-in votes which contain inappropriate
		/// language, or try to send out-of-band notifications when your poll is voted on.
		/// </remarks>
		/// <example>
		/// The following is an example of using the <c>VoteReceived</c> event to cancel the vote
		/// when the write-in text contains "badword".
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;script runat="server" language="c#"&gt;
		/// protected void PollView1_VoteReceived( Object sender, VoteEventArgs e  ) {
		///   foreach( PollDataSet.VotesRow vote in e.PendingVotes ) {
		///     if ( !vote.IsWriteInTextNull() &amp;&amp; vote.WriteInText.IndexOf( "badword" ) &gt;= 0 ) {
		///       e.Cancel = true;
		///     }
		///   }
		/// }
		/// &lt;/script&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		///   &lt;mb:PollView id="PollView1" runat="server" OnVoteReceived="PollView1_VoteReceived" /&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		public event EventHandler<VoteEventArgs> VoteReceived
		{
			add
			{
				Events.AddHandler( eventVoteReceived, value );
			}
			remove
			{
				Events.RemoveHandler( eventVoteReceived, value );
			}
		}

		/// <summary>
		/// Raises the <see cref="PollView.VoteReceived"/> event.
		/// </summary>
		protected virtual void OnVoteReceived( VoteEventArgs e )
		{
			EventHandler<VoteEventArgs> handler = Events[eventVoteReceived] as EventHandler<VoteEventArgs>;
			if ( handler != null )
			{
				handler( this, e );
			}
		}
		private static readonly Object eventVoteReceived = new Object();

		#endregion

		#region Templates

		/// <summary>
		/// The template used to display content in the footer of the control.
		/// </summary>
		/// <remarks>
		/// Content in this template can be databound to the data available in currently displayed poll.
		/// </remarks>
		/// <example>
		/// The following is an example footer template that databinds to current poll data.
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;script runat="server" language="c#" &gt;
		///   protected String GetTotalVotes( PollDataSet pollData ) {
		///     return pollData.Votes.Count.ToString();
		///   }
		/// &lt;/script&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		///   &lt;mb:PollView id="PollView1" runat="server" BorderWidth="2px" BorderColor="Black"&gt;
		///     &lt;FooterTemplate&gt;
		///       &lt;div&gt;Total Votes: &lt;%# GetTotalVotes( Container.PollData ) %&gt;&lt;/div&gt;
		///     &lt;/FooterTemplate&gt;
		///   &lt;/mb:PollView&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "The template used to display content in the footer of the control." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		TemplateContainer( typeof( PollViewTemplateContainer ) ),
		]
		public ITemplate FooterTemplate
		{
			get
			{
				return _footerTemplate;
			}
			set
			{
				_footerTemplate = value;
				this.ChildControlsCreated = false;
			}
		}
		private ITemplate _footerTemplate;

		/// <summary>
		/// The template used to display content in the header of the control.
		/// </summary>
		/// <remarks>
		/// Content in this template can be databound to the data available in currently displayed poll.
		/// </remarks>
		/// <example>
		/// The following is an example header template that databinds to current poll data.
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;script runat="server" language="c#" &gt;
		///   protected String GetTotalVotes( PollDataSet pollData ) {
		///     return pollData.Votes.Count.ToString();
		///   }
		/// &lt;/script&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		///   &lt;mb:PollView id="PollView1" runat="server" BorderWidth="2px" BorderColor="Black"&gt;
		///     &lt;HeaderTemplate&gt;
		///       &lt;div&gt;Total Votes: &lt;%# GetTotalVotes( Container.PollData ) %&gt;&lt;/div&gt;
		///     &lt;/HeaderTemplate&gt;
		///   &lt;/mb:PollView&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "The template used to display content in the header of the control." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		TemplateContainer( typeof( PollViewTemplateContainer ) ),
		]
		public ITemplate HeaderTemplate
		{
			get
			{
				return _headerTemplate;
			}
			set
			{
				_headerTemplate = value;
				this.ChildControlsCreated = false;
			}
		}
		private ITemplate _headerTemplate;

		/// <summary>
		/// The template used to display content when there is no poll to display.
		/// </summary>
		/// <remarks>
		/// By default a label is shown when there is no poll to display.
		/// The text of this label can be set with the <see cref="PollView.NoPollText"/> property.
		/// The style of this label can be set with the <see cref="PollView.NoPollLabelStyle"/> property.
		/// However, this template property can be used to override the default label to show
		/// more complex content.
		/// </remarks>
		/// <example>
		/// The following is an example no-poll template.
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		///   &lt;mb:PollView id="PollView2" runat="server" BorderWidth="2px" BorderColor="Black" &gt;
		///     &lt;NoPollTemplate&gt;
		///       Sorry, there's no poll to vote on right now.
		///     &lt;/NoPollTemplate&gt;
		///   &lt;/mb:PollView&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "The template used to display content when there is no poll to display." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		]
		public ITemplate NoPollTemplate
		{
			get
			{
				return _noPollTemplate;
			}
			set
			{
				_noPollTemplate = value;
				this.ChildControlsCreated = false;
			}
		}
		private ITemplate _noPollTemplate;

		/// <summary>
		/// The template used to display content in the header area of the list of write-in votes.
		/// </summary>
		/// <remarks>
		/// This template does not support being databound.
		/// By default the header area of the write-in list shows a label which can customized using
		/// the <see cref="WriteInHeaderText"/> and <see cref="WriteInHeaderStyle"/> style, 
		/// as well as an opening tag (UL) for the unordered list of write in votes.
		/// However, this default display can be changed by setting the <see cref="WriteInHeaderTemplate"/>.
		/// </remarks>
		/// <example>
		/// The following is an example that specifies custom templates for <see cref="WriteInHeaderTemplate"/>,
		/// <see cref="WriteInItemTemplate"/>, and <see cref="WriteInFooterTemplate"/>.
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		/// &lt;mb:PollView id="PollView1" runat="server" BorderWidth="2px" BorderColor="Black"&gt;
		/// &lt;WriteInHeaderTemplate&gt;
		/// &lt;hr&gt;
		/// Write Ins
		/// &lt;ol&gt;
		/// &lt;/WriteInHeaderTemplate&gt;
		/// &lt;WriteInItemTemplate&gt;
		/// &lt;li&gt;&lt;b&gt;&lt;%# DataBinder.Eval( Container.DataItem, "WriteInText" ) %&gt;&lt;/b&gt;&lt;/li&gt;
		/// &lt;/WriteInItemTemplate&gt;
		/// &lt;WriteInFooterTemplate&gt;
		/// &lt;/ol&gt;
		/// &lt;/WriteInFooterTemplate&gt;
		/// &lt;/mb:PollView&gt;
		/// &lt;mbp:PollManager runat="server" /&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "The template used to display content in the header area of the list of write-in votes." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		TemplateContainer( typeof( RepeaterItem ) ),
		]
		public ITemplate WriteInHeaderTemplate
		{
			get
			{
				return _writeInHeaderTemplate;
			}
			set
			{
				_writeInHeaderTemplate = value;
				ChildControlsCreated = false;
			}
		}
		private ITemplate _writeInHeaderTemplate;

		/// <summary>
		/// The template used to display content for each write-in vote.
		/// </summary>
		/// <remarks>
		/// This template is where each write-in vote is databound to an interface.
		/// The default template will place the text of the write-in vote within an LI html tag.
		/// The property which holds the text of the wring-in vote is named "WriteInText".
		/// </remarks>
		/// <example>
		/// The following is an example that specifies custom templates for <see cref="WriteInHeaderTemplate"/>,
		/// <see cref="WriteInItemTemplate"/>, and <see cref="WriteInFooterTemplate"/>.
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		/// &lt;mb:PollView id="PollView1" runat="server" BorderWidth="2px" BorderColor="Black"&gt;
		/// &lt;WriteInHeaderTemplate&gt;
		/// &lt;hr&gt;
		/// Write Ins
		/// &lt;ol&gt;
		/// &lt;/WriteInHeaderTemplate&gt;
		/// &lt;WriteInItemTemplate&gt;
		/// &lt;li&gt;&lt;b&gt;&lt;%# DataBinder.Eval( Container.DataItem, "WriteInText" ) %&gt;&lt;/b&gt;&lt;/li&gt;
		/// &lt;/WriteInItemTemplate&gt;
		/// &lt;WriteInFooterTemplate&gt;
		/// &lt;/ol&gt;
		/// &lt;/WriteInFooterTemplate&gt;
		/// &lt;/mb:PollView&gt;
		/// &lt;mb:PollManager runat="server" /&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "The template used to display content for each write-in vote." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		TemplateContainer( typeof( RepeaterItem ) ),
		]
		public ITemplate WriteInItemTemplate
		{
			get
			{
				return _writeInItemTemplate;
			}
			set
			{
				_writeInItemTemplate = value;
				ChildControlsCreated = false;
			}
		}
		private ITemplate _writeInItemTemplate;

		/// <summary>
		/// The template used to display content in the footer area of the list of write-in votes.
		/// </summary>
		/// <remarks>
		/// This template does not support being databound.
		/// By default the footer area of closes the unordered list tag (UL) opened in the default <see cref="WriteInHeaderTemplate"/>.
		/// However, this default display can be changed by setting the <see cref="WriteInFooterTemplate"/>.
		/// </remarks>
		/// <example>
		/// The following is an example that specifies custom templates for <see cref="WriteInHeaderTemplate"/>,
		/// <see cref="WriteInItemTemplate"/>, and <see cref="WriteInFooterTemplate"/>.
		/// <code>
		/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
		/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
		/// &lt;mb:PollView id="PollView1" runat="server" BorderWidth="2px" BorderColor="Black"&gt;
		/// &lt;WriteInHeaderTemplate&gt;
		/// &lt;hr&gt;
		/// Write Ins
		/// &lt;ol&gt;
		/// &lt;/WriteInHeaderTemplate&gt;
		/// &lt;WriteInItemTemplate&gt;
		/// &lt;li&gt;&lt;b&gt;&lt;%# DataBinder.Eval( Container.DataItem, "WriteInText" ) %&gt;&lt;/b&gt;&lt;/li&gt;
		/// &lt;/WriteInItemTemplate&gt;
		/// &lt;WriteInFooterTemplate&gt;
		/// &lt;/ol&gt;
		/// &lt;/WriteInFooterTemplate&gt;
		/// &lt;/mb:PollView&gt;
		/// &lt;mb:PollManager runat="server" /&gt;
		/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
		/// </code>
		/// </example>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "The template used to display content in the footer area of the list of write-in votes." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		TemplateContainer( typeof( RepeaterItem ) ),
		]
		public ITemplate WriteInFooterTemplate
		{
			get
			{
				return _writeInFooterTemplate;
			}
			set
			{
				_writeInFooterTemplate = value;
				ChildControlsCreated = false;
			}
		}
		private ITemplate _writeInFooterTemplate;


		#endregion

		#region Appearance Properties

		/// <summary>
		/// Gets or sets the url of the image displayed in the background of the PollView.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the url of the image displayed in the background of the control." ),
		DefaultValue( "" ),
		Editor( typeof( ImageUrlEditor ), typeof( UITypeEditor ) ),
		UrlProperty(),
		]
		public virtual String BackImageUrl
		{
			get
			{
				Object savedState = this.ViewState["BackImageUrl"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState["BackImageUrl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="ResultDisplayType"/> of the bars in the list or results.
		/// </summary>
		/// <value>One of the <see cref="ResultDisplayType"/> values. The default is <see cref="ResultDisplayType.Count"/>.</value>
		/// <remarks>
		/// With <see cref="ResultDisplayType.Percentage"/>, the widths of the bars will add up to width of the control as a whole.
		/// With <see cref="ResultDisplayType.Count"/>, the item with the highest count will fill the width of the control.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the ResultDisplayType of the bars in the list of results." ),
		Bindable( true ),
		DefaultValue( ResultDisplayType.Count ),
		]
		public virtual ResultDisplayType ResultsBarDisplay
		{
			get
			{
				Object state = ViewState["ResultsBarDisplay"];
				if ( state != null )
				{
					return (ResultDisplayType)state;
				}
				return ResultDisplayType.Count;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( ResultDisplayType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( ResultDisplayType ) );
				}
				ViewState["ResultsBarDisplay"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="ResultDisplayType"/> of the vote count display in the list of results.
		/// </summary>
		/// <value>One of the <see cref="ResultDisplayType"/> values. The default is <see cref="ResultDisplayType.Percentage"/></value>
		/// <remarks>
		/// With <see cref="ResultDisplayType.Percentage"/>, the vote count of each item will be the percentage of votes which that option received.
		/// With <see cref="ResultDisplayType.Count"/>, the vote count will show the number of votes which that option received.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the ResultDisplayType of the vote count display in the list or results." ),
		Bindable( true ),
		DefaultValue( ResultDisplayType.Percentage ),
		]
		public virtual ResultDisplayType ResultsVoteCountDisplay
		{
			get
			{
				Object state = ViewState["ResultsVoteCountDisplay"];
				if ( state != null )
				{
					return (ResultDisplayType)state;
				}
				return ResultDisplayType.Percentage;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( ResultDisplayType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( ResultDisplayType ) );
				}
				ViewState["ResultsVoteCountDisplay"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the order of the options in the result view.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the order of the options in the result view." ),
		DefaultValue( OptionOrder.Auto ),
		]
		public virtual OptionOrder ResultOptionOrder
		{
			get
			{
				Object savedState = this.ViewState["ResultOptionOrder"];
				if ( savedState != null )
				{
					return (OptionOrder)savedState;
				}
				return OptionOrder.Auto;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( OptionOrder ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( OptionOrder ) );
				}
				this.ViewState["ResultOptionOrder"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the the button for viewing the poll results.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the visibility of the the button for viewing the poll results." ),
		DefaultValue( true ),
		]
		public virtual Boolean ShowResultsButton
		{
			get
			{
				Object savedState = this.ViewState["ShowResultsButton"];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return true;
			}
			set
			{
				this.ViewState["ShowResultsButton"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of button to display for voting.
		/// </summary>
		/// <value>One of the <see cref="PollViewButtonType"/> values. The default value is PushButton.</value>
		/// <remarks>
		/// Use this property to specify whether the voting button is displayed as link or push button. 
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the type of button to display for voting." ),
		DefaultValue( PollViewButtonType.PushButton ),
		]
		public virtual PollViewButtonType VoteButtonType
		{
			get
			{
				Object state = ViewState["VoteButtonType"];
				if ( state != null )
				{
					return (PollViewButtonType)state;
				}
				return PollViewButtonType.PushButton;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( PollViewButtonType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( PollViewButtonType ) );
				}
				this.ViewState["VoteButtonType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of button to display for the button which shows the current poll results.
		/// </summary>
		/// <value>One of the <see cref="PollViewButtonType"/> values. The default value is PushButton.</value>
		/// <remarks>
		/// Use this property to specify whether the voting button is displayed as link or push button. 
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the type of button to display for the button which shows the current poll results" ),
		DefaultValue( PollViewButtonType.PushButton ),
		]
		public virtual PollViewButtonType ResultsButtonType
		{
			get
			{
				Object state = ViewState["ResultsButtonType"];
				if ( state != null )
				{
					return (PollViewButtonType)state;
				}
				return PollViewButtonType.PushButton;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( PollViewButtonType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( PollViewButtonType ) );
				}
				this.ViewState["ResultsButtonType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of button to display for the button which shows the write-in poll entries.
		/// </summary>
		/// <value>One of the <see cref="PollViewButtonType"/> values. The default value is PushButton.</value>
		/// <remarks>
		/// Use this property to specify whether the voting button is displayed as link or push button. 
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the type of button to display for the button which shows the write-in poll entries" ),
		DefaultValue( PollViewButtonType.PushButton ),
		]
		public virtual PollViewButtonType WriteinsButtonType
		{
			get
			{
				Object state = ViewState["WriteinsButtonType"];
				if ( state != null )
				{
					return (PollViewButtonType)state;
				}
				return PollViewButtonType.PushButton;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( PollViewButtonType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( PollViewButtonType ) );
				}
				this.ViewState["WriteinsButtonType"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the direction in which the submit buttons of the control are aligned.
		/// </summary>
		/// <remarks>
		/// The submit buttons are the Vote button and the View Results button.
		/// The default value of <see cref="RepeatDirection.Vertical"/> puts the View Results button
		/// below the Vote button. Setting <see cref="SubmitButtonAlignment"/> to <see cref="RepeatDirection.Horizontal"/>
		/// will cause the View Results button to render to the right of the Vote button.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the direction in which the submit buttons of the control are aligned." ),
		DefaultValue( RepeatDirection.Vertical ),
		]
		public RepeatDirection SubmitButtonAlignment
		{
			get
			{
				Object state = ViewState["SubmitButtonAlignment"];
				if ( state != null )
				{
					return (RepeatDirection)state;
				}
				return RepeatDirection.Vertical;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( RepeatDirection ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( RepeatDirection ) );
				}
				ViewState["SubmitButtonAlignment"] = value;
			}
		}

		#endregion

		#region UI Text Properties

		/// <summary>
		/// Gets or sets the Text of the button for viewing the poll results.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the Text of the button for viewing the poll results." ),
		MbwcDefaultValue( "PollView_ShowResultsText" ),
		]
		public virtual String ResultsButtonText
		{
			get
			{
				Object state = ViewState["ResultsButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollView_ShowResultsText;
			}
			set
			{
				ViewState["ResultsButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Text of the Vote button.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the Text of the Vote button." ),
		MbwcDefaultValue( "PollView_VoteText" ),
		]
		public virtual String VoteButtonText
		{
			get
			{
				Object state = ViewState["VoteButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollView_VoteText;
			}
			set
			{
				ViewState["VoteButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the header in the write-in votes view section.
		/// </summary>
		/// <remarks>
		/// This property is only effective when the <see cref="WriteInHeaderTemplate"/> is not set.
		/// That template replaces the area which holds the header label.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the text of the header in the write-in votes view section." ),
		MbwcDefaultValue( "PollView_WriteInHeaderText" ),
		]
		public virtual String WriteInHeaderText
		{
			get
			{
				Object savedState = this.ViewState["WriteInHeaderText"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return Resources.PollView_WriteInHeaderText;
			}
			set
			{
				this.ViewState["WriteInHeaderText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Text of the button which allows you to show write-in votes.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the Text of the button which allows you to show write-in votes." ),
		MbwcDefaultValue( "PollView_ShowWriteInButtonText" ),
		]
		public virtual String ShowWriteInButtonText
		{
			get
			{
				Object savedState = this.ViewState["ShowWriteInButtonText"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return Resources.PollView_ShowWriteInButtonText;
			}
			set
			{
				this.ViewState["ShowWriteInButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label which displays a message when there is no poll to show.
		/// </summary>
		/// <remarks>
		/// This text applies to the default message displayed when no poll data is present.
		/// To change the style of this message, edit the <see cref="NoPollLabelStyle"/> property.
		/// This property has no effect when the <see cref="NoPollTemplate"/> is defined,
		/// as that template is used instead of the default Label.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label which displays a message when there is no poll to show." ),
		MbwcDefaultValue( "PollView_NoPollText" ),
		]
		public virtual String NoPollText
		{
			get
			{
				Object savedState = this.ViewState["NoPollText"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return Resources.PollView_NoPollText;
			}
			set
			{
				this.ViewState["NoPollText"] = value;
			}
		}

		#endregion

		#region Style Properties

		/// <summary>
		/// Gets the style applied to the label which displays a message when there is no poll to show.
		/// </summary>
		/// <remarks>
		/// This style applies to the default message displayed when no poll data is present.
		/// To change the text of this message, edit the <see cref="PollView.NoPollText"/> property.
		/// This style has no effect when the <see cref="PollView.NoPollTemplate"/> is defined,
		/// as that template is used instead of the default Label.
		/// </remarks>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the label which displays a message when there is no poll to show." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		]
		public Style NoPollLabelStyle
		{
			get
			{
				if ( _noPollLabelStyle == null )
				{
					_noPollLabelStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_noPollLabelStyle ).TrackViewState();
					}
				}
				return _noPollLabelStyle;
			}
		}
		private Style _noPollLabelStyle;

		/// <summary>
		/// Gets the style which the bars will use in result view.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style which the bars will use in result view." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual BarStyle BarStyle
		{
			get
			{
				if ( _barStyle == null )
				{
					_barStyle = new BarStyle();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_barStyle ).TrackViewState();
					}
				}
				return _barStyle;
			}
		}
		private BarStyle _barStyle;

		/// <summary>
		/// Gets the style for the vote button
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style for the vote button" ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style VoteButtonStyle
		{
			get
			{
				if ( _voteButtonStyle == null )
				{
					_voteButtonStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_voteButtonStyle ).TrackViewState();
					}
				}
				return _voteButtonStyle;
			}
		}
		private Style _voteButtonStyle;

		/// <summary>
		/// Gets the style for the question
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style for the question" ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style QuestionStyle
		{
			get
			{
				if ( _questionStyle == null )
				{
					_questionStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_questionStyle ).TrackViewState();
					}
				}
				return _questionStyle;
			}
		}
		private Style _questionStyle;

		/// <summary>
		/// Gets the style for the options
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style for the options" ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style OptionStyle
		{
			get
			{
				if ( optionStyle == null )
				{
					optionStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)optionStyle ).TrackViewState();
					}
				}
				return optionStyle;
			}
		}

		/// <summary>
		/// Gets the style for the button for viewing the poll results.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style for the button for viewing the poll results." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style ResultsButtonStyle
		{
			get
			{
				if ( _resultsButtonStyle == null )
				{
					_resultsButtonStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_resultsButtonStyle ).TrackViewState();
					}
				}
				return _resultsButtonStyle;
			}
		}
		private Style _resultsButtonStyle;

		/// <summary>
		/// Gets the style applied to the textbox used for write-in votes.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the textbox used for write-in votes." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style WriteInTextBoxStyle
		{
			get
			{
				if ( _writeInTextBoxStyle == null )
				{
					_writeInTextBoxStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_writeInTextBoxStyle ).TrackViewState();
					}
				}
				return _writeInTextBoxStyle;
			}
		}
		private Style _writeInTextBoxStyle;

		/// <summary>
		/// Gets the style applied to the header in the write-in votes view section.
		/// </summary>
		/// <remarks>
		/// This property is only effective when the <see cref="WriteInHeaderTemplate"/> is not set.
		/// That template replaces the area which holds the header label.
		/// </remarks>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the header for the write-in votes view section." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style WriteInHeaderStyle
		{
			get
			{
				if ( _writeInHeaderStyle == null )
				{
					_writeInHeaderStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_writeInHeaderStyle ).TrackViewState();
					}
				}
				return _writeInHeaderStyle;
			}
		}
		private Style _writeInHeaderStyle;

		/// <summary>
		/// Gets the style for the vote button
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style for the button for showing write in votes." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style ShowWriteInButtonStyle
		{
			get
			{
				if ( _showWriteInButtonStyle == null )
				{
					_showWriteInButtonStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)_showWriteInButtonStyle ).TrackViewState();
					}
				}
				return _showWriteInButtonStyle;
			}
		}
		private Style _showWriteInButtonStyle;

		#endregion

		#region Behavior Properties

		/// <summary>
		/// Gets or sets the active view of the PollView control.
		/// </summary>
		/// <value>One of the <see cref="PollViewState"/> values. The default is PollViewState.Auto.</value>
		[
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets the active view of the PollView control." ),
		DefaultValue( PollViewState.Auto ),
		]
		public virtual PollViewState ActiveView
		{
			get
			{
				Object savedState = this.ViewState["ActiveView"];
				if ( savedState != null )
				{
					return (PollViewState)savedState;
				}
				return PollViewState.Auto;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( PollViewState ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( PollViewState ) );
				}
				this.ViewState["ActiveView"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if voting will be availble to non-authenticated users.
		/// </summary>
		/// <remarks>
		/// When <see cref="ActiveView"/> is set to <see cref="PollViewState.Auto"/>, 
		/// this property helps determine which view will be displayed.
		/// When set to true, a non-authenticated user will only be able to see the results,
		/// and will be unable to vote.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets if voting will be availble to non-authenticated users." ),
		DefaultValue( false ),
		]
		public virtual Boolean VoteRequiresAuthentication
		{
			get
			{
				Object savedState = this.ViewState["VoteRequiresAuthentication"];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState["VoteRequiresAuthentication"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if entering write-in votes will be availble to non-authenticated users.
		/// </summary>
		/// <remarks>
		/// When <see cref="VoteRequiresAuthentication"/> is set to true, this property will have no effect.
		/// However, set <see cref="VoteRequiresAuthentication"/> to false, and <see cref="WriteInVoteRequiresAuthentication"/>
		/// to allow anybody to vote on pre-approved options, but only allow logged in users to write in votes.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets if entering write-in votes will be availble to non-authenticated users." ),
		DefaultValue( false ),
		]
		public Boolean WriteInVoteRequiresAuthentication
		{
			get
			{
				Object state = ViewState["WriteInVoteRequiresAuthentication"];
				if ( state != null )
				{
					return (Boolean)state;
				}
				return false;
			}
			set
			{
				ViewState["WriteInVoteRequiresAuthentication"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the url that the user will be redirected to after voting.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets the url that the user will be redirected to after voting." ),
		DefaultValue( "" ),
		Editor( typeof( UrlEditor ), typeof( UITypeEditor ) ),
		UrlProperty( "" ),
		]
		public virtual String VoteRedirectUrl
		{
			get
			{
				Object savedState = this.ViewState["VoteRedirectUrl"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState["VoteRedirectUrl"] = value;
			}
		}

		#endregion

		#region Data Properties

		/// <summary>
		/// Gets or sets the ID of the poll that will be loaded.
		/// </summary>
		/// <value>The ID of the poll which will be loaded. A value of <b>-1</b> means that the default current poll will be loaded.</value>
		/// <remarks>
		/// PollView, by default, will display the poll returned by the Polling API's <see cref="Polling.GetCurrentPoll()"/> method.
		/// However, this can be changed by setting a custom poll ID.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Data" ),
		Description( "Gets or sets the ID of the poll that will be loaded." ),
		DefaultValue( -1 ),
		]
		public virtual Int32 CustomPollID
		{
			get
			{
				Object savedState = this.ViewState["CustomPollID"];
				if ( savedState != null )
				{
					return (Int32)savedState;
				}
				return -1;
			}
			set
			{
				if ( value < -1 )
				{
					throw new ArgumentException( Resources.PollView_CustomPollID_Invalid );
				}
				this.ViewState["CustomPollID"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the category of the poll shown when PollView retrieves the current poll.
		/// </summary>
		/// <remarks>
		/// PollView, by default, will display the poll returned by the Polling API's <see cref="Polling.GetCurrentPoll()"/> method.
		/// When this property is set, the current poll is filtered by the given category.
		/// This property will have no effect when the <see cref="CustomPollID"/> property is set.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Data" ),
		Description( "Gets or sets the category of the poll shown when PollView retrieves the current poll." ),
		DefaultValue( "" ),
		]
		public virtual String PollCategory
		{
			get
			{
				Object state = this.ViewState["PollCategory"];
				if ( state != null )
				{
					return (String)state;
				}
				return "";
			}
			set
			{
				this.ViewState["PollCategory"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ID of the currently loaded poll.
		/// </summary>
		[
		Description( "Gets or sets the ID of the currently loaded poll." ),
		]
		protected virtual Int32 LoadedPollID
		{
			get
			{
				Object savedState = this.ViewState["LoadedPollID"];
				if ( savedState != null )
				{
					return (Int32)savedState;
				}
				return 0;
			}
			set
			{
				this.ViewState["LoadedPollID"] = value;
			}
		}

		#endregion

		#region Design Time Properties

		/// <summary>
		/// Gets or sets if the example poll shown at design time allows write-ins.
		/// </summary>
		[
		DesignOnly( true ),
		MbwcCategory( "DesignTime" ),
		Description( "Gets or sets if the example poll shown at design time allows write-ins." ),
		DefaultValue( false ),
		]
		public virtual Boolean DesignTimePollAllowsWriteIns
		{
			get
			{
				return _designTimePollAllowsWriteIns;
			}
			set
			{
				_designTimePollAllowsWriteIns = value;
			}
		}
		private Boolean _designTimePollAllowsWriteIns = false;

		/// <summary>
		/// Gets or sets if the example poll shown at design time shows write-in votes.
		/// </summary>
		[
		DesignOnly( true ),
		MbwcCategory( "DesignTime" ),
		Description( "Gets or sets if the example poll shown at design time shows write-in votes." ),
		DefaultValue( false ),
		]
		public virtual Boolean DesignTimePollShowsWriteInVotes
		{
			get
			{
				return _designTimePollShowsWriteInVotes;
			}
			set
			{
				_designTimePollShowsWriteInVotes = value;
			}
		}
		private Boolean _designTimePollShowsWriteInVotes = false;

		/// <summary>
		/// Gets or sets if the <see cref="PollView"/> control binds to an example poll at design time.
		/// </summary>
		[
		DesignOnly( true ),
		MbwcCategory( "DesignTime" ),
		Description( "Gets or sets if the PollView control binds to an example poll at design time." ),
		DefaultValue( true ),
		]
		public virtual Boolean DesignTimePollExists
		{
			get
			{
				return _designTimePollExists;
			}
			set
			{
				_designTimePollExists = value;
			}
		}
		private Boolean _designTimePollExists = true;

		#endregion

		#region Override Properties

		/// <summary>
		/// Overrides <see cref="WebControl.TagKey"/> to return <see cref="HtmlTextWriterTag.Table"/>.
		/// </summary>
		/// <exclude />
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Table;
			}
		}

		#endregion

		#region Lifecycle

		/// <summary>
		/// Overrides <see cref="Control.CreateChildControls"/>.
		/// </summary>
		/// <exclude />
		protected override void CreateChildControls()
		{
			this.Controls.Clear();

			TableRow mainRow = new TableRow();
			this.Controls.Add( mainRow );
			mainCell = new TableCell();
			mainCell.Width = Unit.Percentage( 100 );
			mainRow.Controls.Add( mainCell );

			#region Empty View

			noPollPanel = new PlaceHolder();
			mainCell.Controls.Add( noPollPanel );

			noPollLabel = new Label();
			noPollLabel.Visible = ( this._noPollTemplate == null );
			noPollPanel.Controls.Add( noPollLabel );

			if ( this._noPollTemplate != null )
			{
				this._noPollTemplate.InstantiateIn( noPollPanel );
			}

			#endregion

			#region Not Empty View

			pollPanel = new PlaceHolder();
			mainCell.Controls.Add( pollPanel );

			headerContainer = new PollViewTemplateContainer( this );
			pollPanel.Controls.Add( headerContainer );
			if ( this._headerTemplate != null )
			{
				this._headerTemplate.InstantiateIn( headerContainer );
			}

			this.question = new Label();
			pollPanel.Controls.Add( question );

			#region Vote View

			this.inputViewPanel = new PlaceHolder();
			pollPanel.Controls.Add( inputViewPanel );

			this.optionList = new VotingList();
			this.inputViewPanel.Controls.Add( optionList );

			this.voter = new PollViewButton();
			this.voter.ID = "voter";
			this.voter.Click += new EventHandler( voter_Click );
			this.voter.Style["margin-top"] = "3px";
			this.inputViewPanel.Controls.Add( voter );

			this.buttonSeperator = new Literal();
			this.buttonSeperator.ID = "buttonSeperator";
			this.inputViewPanel.Controls.Add( buttonSeperator );

			this.viewResults = new PollViewButton();
			this.viewResults.ID = "viewResults";
			this.viewResults.Click += new EventHandler( viewResults_Click );
			this.viewResults.Style["margin-top"] = "3px";
			this.inputViewPanel.Controls.Add( this.viewResults );

			#endregion

			#region Results View

			this.resultViewPanel = new PlaceHolder();
			this.resultViewPanel.Visible = false;
			pollPanel.Controls.Add( this.resultViewPanel );

			this.resultsList = new PollResultsList();
			this.resultViewPanel.Controls.Add( this.resultsList );

			this.showWriteInsButton = new PollViewButton();
			this.showWriteInsButton.ID = "showWriteInsButton";
			this.showWriteInsButton.Click += new EventHandler( showWriteInsButton_Click );
			this.resultViewPanel.Controls.Add( this.showWriteInsButton );

			this.writeInVotesPanel = new PlaceHolder();
			this.writeInVotesPanel.Visible = false;
			this.resultViewPanel.Controls.Add( writeInVotesPanel );

			this.writeInVotesList = new Repeater();
			this.writeInVotesList.HeaderTemplate = ( WriteInHeaderTemplate != null ) ? WriteInHeaderTemplate : new PollView.WriteInVoteHeaderTemplate();
			this.writeInVotesList.ItemTemplate = ( WriteInItemTemplate != null ) ? WriteInItemTemplate : new PollView.WriteInVoteTemplate();
			this.writeInVotesList.FooterTemplate = ( WriteInFooterTemplate != null ) ? WriteInFooterTemplate : new PollView.WriteInVoteFooterTemplate();
			this.writeInVotesPanel.Controls.Add( this.writeInVotesList );

			#endregion

			footerContainer = new PollViewTemplateContainer( this );
			pollPanel.Controls.Add( footerContainer );
			if ( this._footerTemplate != null )
			{
				this._footerTemplate.InstantiateIn( footerContainer );
			}

			#endregion

		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			this.EnsureChildControls();
			if ( !this.Page.IsPostBack )
			{
				this.LoadPoll( true );
			}
		}

		/// <summary>
		/// Overrides <see cref="WebControl.AddAttributesToRender"/>.
		/// </summary>
		/// <exclude />
		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{
			base.AddAttributesToRender( writer );
			if ( this.BackImageUrl.Length > 0 )
			{
				writer.AddAttribute( "background", this.ResolveUrl( this.BackImageUrl ), true );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		/// <exclude />
		protected override void Render( HtmlTextWriter writer )
		{
			if ( this.Page != null )
			{
				this.Page.VerifyRenderingInServerForm( this );
			}
			this.EnsureChildControls();
			if ( HttpContext.Current == null )
			{
				LoadPoll( false );
			}
			this.ApplyActiveView();
			this.ApplyProperties();
			base.Render( writer );
		}

		/// <summary>
		/// Applies the active view
		/// </summary>
		protected virtual void ApplyActiveView()
		{

			HttpContext context = HttpContext.Current;

			PollViewState activeView = this.ActiveView;
			if ( activeView == PollViewState.Auto )
			{
				activeView = GetAutoView( this.LoadedPollID, this.VoteRequiresAuthentication );
			}
			if ( this.optionList.AllowWriteInVote && this.WriteInVoteRequiresAuthentication )
			{
				if ( context != null && !context.Request.IsAuthenticated )
				{
					this.optionList.AllowWriteInVote = false;
				}
			}

			this.inputViewPanel.Visible = ( activeView == PollViewState.Input );
			this.resultViewPanel.Visible = ( activeView == PollViewState.Result );

			if ( context != null )
			{
				this.showWriteInsButton.Visible = !( this.writeInVotesPanel.Visible ) && this.writeInVotesList.Items.Count > 0;
				this.pollPanel.Visible = this.LoadedPollID > 0;
				this.noPollPanel.Visible = this.LoadedPollID <= 0;
			}
			else
			{
				this.showWriteInsButton.Visible = this.DesignTimePollAllowsWriteIns;
				this.writeInVotesPanel.Visible = this.DesignTimePollAllowsWriteIns && this.DesignTimePollShowsWriteInVotes;
				this.pollPanel.Visible = this.DesignTimePollExists;
				this.noPollPanel.Visible = !this.DesignTimePollExists;
			}


		}

		/// <summary>
		/// Applies the current properties to child controls
		/// </summary>
		protected virtual void ApplyProperties()
		{

			this.viewResults.Visible = this.ShowResultsButton;

			this.resultsList.VoteCountDisplay = this.ResultsVoteCountDisplay;
			this.resultsList.BarDisplay = this.ResultsBarDisplay;

			this.showWriteInsButton.Text = this.ShowWriteInButtonText;
			this.noPollLabel.Text = this.NoPollText;
			this.voter.Text = this.VoteButtonText;
			this.viewResults.Text = this.ResultsButtonText;

			this.voter.ButtonType = this.VoteButtonType;
			this.viewResults.ButtonType = this.ResultsButtonType;
			this.showWriteInsButton.ButtonType = this.WriteinsButtonType;

			this.optionList.ControlStyle.Reset();
			this.optionList.ForeColor = this.ForeColor;
			this.optionList.ControlStyle.CopyFrom( this.optionStyle );
			this.optionList.ControlStyle.Width = Unit.Percentage( 100 );

			this.resultsList.ControlStyle.Reset();
			this.resultsList.ForeColor = this.ForeColor;
			this.resultsList.ControlStyle.CopyFrom( this.optionStyle );

			ApplyStyle( noPollLabel, _noPollLabelStyle );
			this.resultsList.BarStyle.Reset();
			this.resultsList.BarStyle.CopyFrom( this._barStyle );
			ApplyStyle( this.voter, this._voteButtonStyle );
			ApplyStyle( this.question, this._questionStyle );
			ApplyStyle( this.viewResults, this._resultsButtonStyle );
			this.optionList.WriteInTextBoxStyle.Reset();
			this.optionList.WriteInTextBoxStyle.CopyFrom( this._writeInTextBoxStyle );
			ApplyStyle( this.showWriteInsButton, this._showWriteInButtonStyle );

			WriteInVoteHeaderTemplate defaultHeader = this.writeInVotesList.HeaderTemplate as WriteInVoteHeaderTemplate;
			if ( defaultHeader != null && defaultHeader.writeInHeading != null )
			{
				defaultHeader.writeInHeading.Text = this.WriteInHeaderText;
				ApplyStyle( defaultHeader.writeInHeading, this._writeInHeaderStyle );
			}

			if ( this.SubmitButtonAlignment == RepeatDirection.Vertical )
			{
				this.buttonSeperator.Text = "<br>";
			}
			else
			{
				this.buttonSeperator.Text = "&nbsp;";
			}

			#region Trickle Enabled

			this.question.Enabled = this.Enabled;
			this.optionList.Enabled = this.Enabled;
			this.voter.Enabled = this.Enabled;
			this.viewResults.Enabled = this.Enabled;
			this.resultsList.Enabled = this.Enabled;
			this.showWriteInsButton.Enabled = this.Enabled;

			#endregion

		}

		#endregion

		#region ViewState

		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>.
		/// </summary>
		/// <exclude />
		protected override void LoadViewState( object savedState )
		{
			Pair pair = savedState as Pair;
			if ( pair != null )
			{
				base.LoadViewState( pair.First );
				if ( pair.Second != null )
				{
					( (IStateManager)this.OptionStyle ).LoadViewState( pair.Second );
				}
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>.
		/// </summary>
		/// <exclude />
		protected override object SaveViewState()
		{
			Pair pair = new Pair();
			pair.First = base.SaveViewState();
			if ( this.optionStyle != null )
			{
				pair.Second = ( (IStateManager)this.optionStyle ).SaveViewState();
			}
			return pair;
		}

		/// <summary>
		/// Overrides <see cref="Control.TrackViewState"/>.
		/// </summary>
		/// <exclude />
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ( this.optionStyle != null )
			{
				( (IStateManager)this.optionStyle ).TrackViewState();
			}
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Loads the polldata in the given <see cref="PollDataSet"/>.
		/// </summary>
		protected virtual void LoadPoll( Boolean usePollingApi )
		{
			PollDataSet data = null;

			if ( usePollingApi )
			{
				if ( this.CustomPollID != -1 )
				{
					data = Polling.GetPoll( this.CustomPollID );
				}
				else
				{
					if ( this.PollCategory.Length == 0 )
					{
						data = Polling.GetCurrentPoll();
					}
					else
					{
						data = Polling.GetCurrentPoll( this.PollCategory );
					}
				}
			}
			else
			{
				data = Polling.CreatePlaceholderPoll();
				while ( data.Polls.Count > 1 )
				{
					data.Polls[data.Polls.Count - 1].Delete();
				}
				data.Polls[0].AllowWriteIns = this.DesignTimePollAllowsWriteIns;
			}

			if ( data == null || data.Polls.Count == 0 )
			{
				return;
			}

			PollDataSet.PollsRow thePoll = data.Polls[0];

			this.LoadedPollID = thePoll.PollID;

			this.question.Text = thePoll.Text;
			this.optionList.AllowWriteInVote = thePoll.AllowWriteIns;
			this.showWriteInsButton.Visible = thePoll.AllowWriteIns;

			String selectionMode = thePoll.VoteMode;
			this.optionList.SelectionMode = (VoteSelectionMode)Enum.Parse( typeof( VoteSelectionMode ), selectionMode, true );

			System.Data.DataView orderedDataSource;

			orderedDataSource = new System.Data.DataView( data.Options );
			orderedDataSource.Sort = data.Options.DisplayOrderColumn.ColumnName + " asc";

			this.optionList.DataSource = orderedDataSource;
			this.optionList.DataTextField = data.Options.TextColumn.ColumnName;
			this.optionList.DataValueField = data.Options.OptionIDColumn.ColumnName;
			this.optionList.DataBind();

			orderedDataSource = new System.Data.DataView( data.Options );
			switch ( this.ResultOptionOrder )
			{
				case OptionOrder.Auto:
					orderedDataSource.Sort = data.Options.DisplayOrderColumn.ColumnName + " asc";
					break;
				case OptionOrder.VoteCountAscending:
					orderedDataSource.Sort = data.Options.VoteAmountColumn.ColumnName + " asc";
					break;
				case OptionOrder.VoteCountDescending:
					orderedDataSource.Sort = data.Options.VoteAmountColumn.ColumnName + " desc";
					break;
			}
			this.resultsList.DataSource = orderedDataSource;
			this.resultsList.DataTextField = data.Options.TextColumn.ColumnName;
			this.resultsList.DataValueField = data.Options.VoteAmountColumn.ColumnName;
			this.resultsList.DataBind();

			DataView writeIns = new DataView( data.Votes );
			writeIns.RowFilter = "Isnull( WriteInText, 'Null Column' ) <> 'Null Column'";
			this.writeInVotesList.DataSource = writeIns;
			this.writeInVotesList.DataBind();

			this.headerContainer.SetPollData( data );
			this.headerContainer.DataBind();

			this.footerContainer.SetPollData( data );
			this.footerContainer.DataBind();
		}

		/// <summary>
		/// Determines the correct <see cref="PollViewState"/> for the current user on the given pollID.
		/// </summary>
		/// <param name="pollID">The ID of the poll the user is viewing.</param>
		/// <param name="voteRequiresAuthentication">Indicates if authentication is required to vote.</param>
		protected static PollViewState GetAutoView( Int32 pollID, Boolean voteRequiresAuthentication )
		{

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				return PollViewState.Input;
			}

			if ( voteRequiresAuthentication && !context.Request.IsAuthenticated )
			{
				return PollViewState.Result;
			}
			else
			{
				if ( Polling.UserHasVoted( pollID ) )
				{
					return PollViewState.Result;
				}
				else
				{
					return PollViewState.Input;
				}
			}

		}

		private static void ApplyStyle( WebControl target, Style style )
		{
			target.ControlStyle.Reset();
			target.ControlStyle.CopyFrom( style );
		}

		/// <summary>
		/// Records the selected options voted on.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity" )]
		protected virtual void AddVote()
		{
			HttpContext context = HttpContext.Current;
			if ( context != null )
			{
				if ( this.VoteRequiresAuthentication && !context.Request.IsAuthenticated )
				{
					return;
				}
			}

			String writeInOptionValue = "-1";

			if ( !Polling.UserHasVoted( this.LoadedPollID ) )
			{

				PollDataSet.VotesDataTable postedVotes = new PollDataSet.VotesDataTable();
				postedVotes.BeginLoadData();


				foreach ( ListItem item in this.optionList.Items )
				{
					if ( item.Value == writeInOptionValue )
					{
						if ( this.optionList.AllowWriteInVote && ( item.Selected || this.optionList.SelectionMode == VoteSelectionMode.Rating ) )
						{

							if ( context != null && this.WriteInVoteRequiresAuthentication && !context.Request.IsAuthenticated )
							{
								continue;
							}

							String writeInValue = item.Text.Trim();
							if ( writeInValue.Length > 0 )
							{
								PollDataSet.VotesRow writeInVote = postedVotes.NewVotesRow();
								writeInVote.Member = Polling.MemberName;
								writeInVote.IPAddress = Polling.IPAddress;
								writeInVote.PollID = this.LoadedPollID;
								writeInVote.WriteInText = writeInValue;
								writeInVote.VoteRating = 1;

								if ( this.optionList.SelectionMode == VoteSelectionMode.Rating )
								{
									Int32 rating = Int32.Parse( item.Attributes["rating"], System.Globalization.CultureInfo.InvariantCulture );
									Int32 voteRating = this.optionList.Items.Count - rating + 1;
									if ( rating <= this.optionList.Items.Count )
									{
										writeInVote.VoteRating = voteRating;
									}
								}
								postedVotes.AddVotesRow( writeInVote );
							}

						}
					}
					else
					{

						if ( item.Selected || this.optionList.SelectionMode == VoteSelectionMode.Rating )
						{
							PollDataSet.VotesRow optionVote = postedVotes.NewVotesRow();
							optionVote.Member = Polling.MemberName;
							optionVote.IPAddress = Polling.IPAddress;
							optionVote.PollID = this.LoadedPollID;
							optionVote.OptionID = Convert.ToInt32( item.Value, System.Globalization.CultureInfo.InvariantCulture );
							optionVote.VoteRating = 1;

							if ( this.optionList.SelectionMode == VoteSelectionMode.Rating )
							{

								Int32 rating = Int32.Parse( item.Attributes["rating"], System.Globalization.CultureInfo.InvariantCulture );
								Int32 voteRating = this.optionList.Items.Count - rating + 1;
								if ( rating <= this.optionList.Items.Count )
								{
									optionVote.VoteRating = voteRating;
								}
							}

							postedVotes.AddVotesRow( optionVote );
						}
					}
				}

				if ( postedVotes.Count > 0 )
				{
					VoteEventArgs e = new VoteEventArgs( postedVotes );
					this.OnVoteReceived( e );

					if ( !e.Cancel )
					{

						foreach ( PollDataSet.VotesRow vote in postedVotes )
						{
							if ( vote.IsWriteInTextNull() )
							{
								Polling.AddVote( vote.PollID, vote.OptionID, vote.VoteRating );
							}
							else
							{
								Polling.AddWriteInVote( vote.PollID, vote.WriteInText, vote.VoteRating );
							}
						}

						if ( this.VoteRedirectUrl.Length > 0 )
						{
							this.Page.Response.Redirect( this.VoteRedirectUrl );
						}

						this.ActiveView = PollViewState.Result;
						this.LoadPoll( true );

					}
				}
			}
		}

		/// <summary>
		/// Puts the PollView control into the Result view.
		/// </summary>
		protected virtual void ViewResults()
		{
			this.ActiveView = PollViewState.Result;
		}

		private void voter_Click( Object sender, EventArgs e )
		{
			AddVote();
		}

		private void viewResults_Click( Object sender, EventArgs e )
		{
			ViewResults();
		}

		private void showWriteInsButton_Click( Object sender, EventArgs e )
		{
			this.showWriteInsButton.Visible = false;
			this.writeInVotesPanel.Visible = true;
		}
		
		#endregion

		#region Child Controls

		private TableCell mainCell;

		private Label question;

		private PlaceHolder noPollPanel;
		private Label noPollLabel;

		private PlaceHolder pollPanel;

		private PlaceHolder inputViewPanel;
		private VotingList optionList;
		private PollViewButton voter;
		private Literal buttonSeperator;
		private PollViewButton viewResults;

		private PlaceHolder resultViewPanel;
		private PollResultsList resultsList;
		private PollViewButton showWriteInsButton;

		private PlaceHolder writeInVotesPanel;
		//private Label writeInHeading;
		private Repeater writeInVotesList;

		private Style optionStyle;

		private PollViewTemplateContainer footerContainer;
		private PollViewTemplateContainer headerContainer;

		#endregion

		#region Write In Votes Templates

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable" )]
		private class WriteInVoteHeaderTemplate : ITemplate
		{
			public void InstantiateIn( Control container )
			{
				container.Controls.Add( new LiteralControl( "<hr>" ) );
				this.writeInHeading = new Label();
				container.Controls.Add( writeInHeading );
				container.Controls.Add( new LiteralControl( "<ul style='margin-top:0px;margin-bottom:0px;margin-left:15px;padding:0px;'>" ) );
			}

			public Label writeInHeading;

		}
		private class WriteInVoteFooterTemplate : ITemplate
		{
			public void InstantiateIn( Control container )
			{
				container.Controls.Add( new LiteralControl( "</ul>" ) );
			}

		}
		private class WriteInVoteTemplate : ITemplate
		{
			public void InstantiateIn( Control container )
			{
				Label item = new Label();
				item.DataBinding += new EventHandler( item_DataBinding );
				container.Controls.Add( new LiteralControl( "<li style='margin:0px;padding:0px;'>" ) );
				container.Controls.Add( item );
				container.Controls.Add( new LiteralControl( "</li>" ) );
			}

			private void item_DataBinding( object sender, EventArgs e )
			{
				Label item = sender as Label;
				RepeaterItem container = item.NamingContainer as RepeaterItem;

				item.Text = HttpUtility.HtmlEncode( DataBinder.Eval( container.DataItem, "WriteInText" ).ToString() );
			}
		}

		#endregion

	}
}
