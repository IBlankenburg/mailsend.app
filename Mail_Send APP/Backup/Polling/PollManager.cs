using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MetaBuilders.WebControls.Design;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The <see cref="PollManager"/> control allows for management of the polls stored by the <see cref="Polling"/> feature.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="PollManager"/> control contains all the interface elements required to manage your application's polls.
	/// The styles and text used in the interface can be changed via properties on the control.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following is an example page that contains the poll manager control.
	/// <code>
	/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
	/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
	///   &lt;mb:PollManager runat="server" /&gt;
	/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
	/// </code>
	/// </example>
	[
	PersistChildren( false ),
	ParseChildren( true ),
	DefaultEvent( "PollDataChanged" ),
	]
	public class PollManager : CompositeControl
	{

		#region Events

		#region PollDataChanged Event

		/// <summary>
		/// The event that is raised when the <see cref="PollManager"/> control changes polling data.
		/// </summary>
		public event EventHandler PollDataChanged
		{
			add
			{
				Events.AddHandler( eventPollDataChanged, value );
			}
			remove
			{
				Events.RemoveHandler( eventPollDataChanged, value );
			}
		}

		/// <summary>
		/// Raises the <see cref="PollDataChanged"/> event.
		/// </summary>
		protected void OnPollDataChanged( EventArgs e )
		{
			EventHandler handler = Events[eventPollDataChanged] as EventHandler;
			if ( handler != null )
			{
				handler( this, e );
			}
		}

		private static readonly Object eventPollDataChanged = new Object();

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the role required to enact changes to poll data.
		/// </summary>
		/// <remarks>
		/// <para>When set to a non-empty string, the user's roles will be checked to ensure
		/// the user has the authorization to manage poll data. If the user is not
		/// a member of the specified role, then a security exception is thrown and no
		/// changes are made to the poll data.
		/// </para>
		/// <para>The role is checked with the <c>HttpContext.Current.User.IsInRole</c> method.
		/// It is up to the application developer to ensure that users are given roles in their application.
		/// </para>
		/// </remarks>
		[
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets the role required to enact changes to poll data." ),
		DefaultValue( "" ),
		]
		public virtual String RequiredManagementRole
		{
			get
			{
				Object state = ViewState["RequiredManagementRole"];
				if ( state != null )
				{
					return (String)state;
				}
				return "";
			}
			set
			{
				ViewState["RequiredManagementRole"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the text of the confirmation message shown when clicking a delete button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the confirmation message shown when clicking a delete button." ),
		MbwcDefaultValue( "PollManager_Default_DeleteConfirmationMessage" ),
		]
		public String DeleteConfirmationMessage
		{
			get
			{
				Object state = ViewState["DeleteConfirmationMessage"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_DeleteConfirmationMessage;
			}
			set
			{
				ViewState["DeleteConfirmationMessage"] = value;
			}
		}


		#region General Styles

		/// <summary>
		/// Gets the style applied to the titles of each area of the manager.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the titles of each area of the manager." ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		NotifyParentProperty( true ),
		]
		public Style TitleStyle
		{
			get
			{
				if ( _titleStyle == null )
				{
					_titleStyle = new Style();
					if ( IsTrackingViewState )
					{
						( (IStateManager)_titleStyle ).TrackViewState();
					}
				}
				return _titleStyle;
			}
		}
		private Style _titleStyle;

		/// <summary>
		/// Gets the style applied to the labels in the manager.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the labels in the manager." ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		NotifyParentProperty( true ),
		]
		public Style LabelStyle
		{
			get
			{
				if ( _labelStyle == null )
				{
					_labelStyle = new Style();
					if ( IsTrackingViewState )
					{
						( (IStateManager)_labelStyle ).TrackViewState();
					}
				}
				return _labelStyle;
			}
		}
		private Style _labelStyle;

		/// <summary>
		/// Gets the style applied to the fieldsets.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the fieldsets." ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		NotifyParentProperty( true ),
		]
		public Style FieldSetStyle
		{
			get
			{
				if ( _fieldSetStyle == null )
				{
					_fieldSetStyle = new Style();
					if ( IsTrackingViewState )
					{
						( (IStateManager)_fieldSetStyle ).TrackViewState();
					}
				}
				return _fieldSetStyle;
			}
		}
		private Style _fieldSetStyle;

		#endregion

		#region Poll Editor Properties

		/// <summary>
		/// Gets or sets the text shown as the poll editor area title.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown as the poll editor area title." ),
		MbwcDefaultValue( "PollManager_Default_PollEditorTitleText" ),
		]
		public String PollEditorTitleText
		{
			get
			{
				Object state = ViewState["PollEditorTitleText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollEditorTitleText;
			}
			set
			{
				ViewState["PollEditorTitleText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text to be shown for the <b>New Poll</b> button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text to be shown for the New Poll button." ),
		MbwcDefaultValue( "PollManager_Default_NewPollButtonText" ),
		]
		public String NewPollButtonText
		{
			get
			{
				Object state = ViewState["NewPollButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_NewPollButtonText;
			}
			set
			{
				ViewState["NewPollButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Update Poll button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Update Poll button." ),
		MbwcDefaultValue( "PollManager_Default_UpdatePollButtonText" ),
		]
		public String UpdatePollButtonText
		{
			get
			{
				Object state = ViewState["UpdatePollButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_UpdatePollButtonText;
			}
			set
			{
				ViewState["UpdatePollButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Delete Poll button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Delete Poll button." ),
		MbwcDefaultValue( "PollManager_Default_DeletePollButtonText" ),
		]
		public String DeletePollButtonText
		{
			get
			{
				Object state = ViewState["DeletePollButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_DeletePollButtonText;
			}
			set
			{
				ViewState["DeletePollButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Clear Poll Votes button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Clear Poll Votes button." ),
		MbwcDefaultValue( "PollManager_Default_ClearPollVotesButtonText" ),
		]
		public String ClearPollVotesButtonText
		{
			get
			{
				Object state = ViewState["ClearPollVotesButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_ClearPollVotesButtonText;
			}
			set
			{
				ViewState["ClearPollVotesButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown when no polls are shown in the poll editor.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown when no polls are shown in the poll editor." ),
		MbwcDefaultValue( "PollManager_Default_EmptyPollsLabelText" ),
		]
		public String EmptyPollsLabelText
		{
			get
			{
				Object state = ViewState["EmptyPollsLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_EmptyPollsLabelText;
			}
			set
			{
				ViewState["EmptyPollsLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the title of the New Poll area.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the title of the New Poll area." ),
		MbwcDefaultValue( "PollManager_Default_NewPollTitleText" ),
		]
		public String NewPollTitleText
		{
			get
			{
				Object state = ViewState["NewPollTitleText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_NewPollTitleText;
			}
			set
			{
				ViewState["NewPollTitleText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Add New Poll button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Add New Poll button." ),
		MbwcDefaultValue( "PollManager_Default_AddNewPollButtonText" ),
		]
		public String AddNewPollButtonText
		{
			get
			{
				Object state = ViewState["AddNewPollButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_AddNewPollButtonText;
			}
			set
			{
				ViewState["AddNewPollButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Cancel New Poll button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Cancel New Poll button." ),
		MbwcDefaultValue( "PollManager_Default_CancelNewPollButtonText" ),
		]
		public String CancelNewPollButtonText
		{
			get
			{
				Object state = ViewState["CancelNewPollButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_CancelNewPollButtonText;
			}
			set
			{
				ViewState["CancelNewPollButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's ID property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's ID property." ),
		MbwcDefaultValue( "PollManager_Default_PollIdLabelText" ),
		]
		public String PollIdLabelText
		{
			get
			{
				Object state = ViewState["PollIdLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollIdLabelText;
			}
			set
			{
				ViewState["PollIdLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's Text property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's Text property." ),
		MbwcDefaultValue( "PollManager_Default_PollTextLabelText" ),
		]
		public String PollTextLabelText
		{
			get
			{
				Object state = ViewState["PollTextLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollTextLabelText;
			}
			set
			{
				ViewState["PollTextLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's Start Date property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's Start Date property." ),
		MbwcDefaultValue( "PollManager_Default_PollStartDateLabelText" ),
		]
		public String PollStartDateLabelText
		{
			get
			{
				Object state = ViewState["PollStartDateLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollStartDateLabelText;
			}
			set
			{
				ViewState["PollStartDateLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's Vote Mode property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's Vote Mode property." ),
		MbwcDefaultValue( "PollManager_Default_PollVoteModeLabelText" ),
		]
		public String PollVoteModeLabelText
		{
			get
			{
				Object state = ViewState["PollVoteModeLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollVoteModeLabelText;
			}
			set
			{
				ViewState["PollVoteModeLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's Enabled property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's Enabled property." ),
		MbwcDefaultValue( "PollManager_Default_PollEnabledLabelText" ),
		]
		public String PollEnabledLabelText
		{
			get
			{
				Object state = ViewState["PollEnabledLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollEnabledLabelText;
			}
			set
			{
				ViewState["PollEnabledLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's Allow Write-Ins property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's Allow Write-Ins property." ),
		MbwcDefaultValue( "PollManager_Default_PollAllowWriteInsLabelText" ),
		]
		public String PollAllowWriteInsLabelText
		{
			get
			{
				Object state = ViewState["PollAllowWriteInsLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollAllowWriteInsLabelText;
			}
			set
			{
				ViewState["PollAllowWriteInsLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Poll's Category property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Poll's Category property." ),
		MbwcDefaultValue( "PollManager_Default_PollCategoryLabelText" ),
		]
		public String PollCategoryLabelText
		{
			get
			{
				Object state = ViewState["PollCategoryLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_PollCategoryLabelText;
			}
			set
			{
				ViewState["PollCategoryLabelText"] = value;
			}
		}

		#endregion

		#region Options Editor Properties

		/// <summary>
		/// Gets or sets the title of the poll option editor area.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the title of the poll option editor area." ),
		MbwcDefaultValue( "PollManager_Default_OptionEditorTitleText" ),
		]
		public String OptionEditorTitleText
		{
			get
			{
				Object state = ViewState["OptionEditorTitleText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_OptionEditorTitleText;
			}
			set
			{
				ViewState["OptionEditorTitleText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the title of the New Option area.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the title of the New Option area." ),
		MbwcDefaultValue( "PollManager_Default_AddOptionTitleText" ),
		]
		public String AddOptionTitleText
		{
			get
			{
				Object state = ViewState["AddOptionTitleText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_AddOptionTitleText;
			}
			set
			{
				ViewState["AddOptionTitleText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Add Option button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Add Option button." ),
		MbwcDefaultValue( "PollManager_Default_AddOptionButtonText" ),
		]
		public String AddOptionButtonText
		{
			get
			{
				Object state = ViewState["AddOptionButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_AddOptionButtonText;
			}
			set
			{
				ViewState["AddOptionButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Option's Text property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Option's Text property." ),
		MbwcDefaultValue( "PollManager_Default_OptionTextLabelText" ),
		]
		public String OptionTextLabelText
		{
			get
			{
				Object state = ViewState["OptionTextLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_OptionTextLabelText;
			}
			set
			{
				ViewState["OptionTextLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Option's Display Order property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Option's Display Order property." ),
		MbwcDefaultValue( "PollManager_Default_OptionDisplayOrderLabelText" ),
		]
		public String OptionDisplayOrderLabelText
		{
			get
			{
				Object state = ViewState["OptionDisplayOrderLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_OptionDisplayOrderLabelText;
			}
			set
			{
				ViewState["OptionDisplayOrderLabelText"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the text of delete buttons in the option editor grid.
		/// </summary>
		/// <remarks>
		/// For best results, set this property at design time.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of delete buttons in the option editor grid." ),
		MbwcDefaultValue( "PollManager_DeleteText" ),
		]
		public String OptionDeleteButtonText
		{
			get
			{
				Object state = ViewState["OptionDeleteButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_DeleteText;
			}
			set
			{
				String originalValue = ViewState["OptionDeleteButtonText"] as String;
				ViewState["OptionDeleteButtonText"] = value;
				if ( originalValue != value )
				{
					EnsureOptionEditorButtonText();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text of edit buttons in the option editor grid.
		/// </summary>
		/// <remarks>
		/// For best results, set this property at design time.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of edit buttons in the option editor grid." ),
		MbwcDefaultValue( "PollManager_EditText" ),
		]
		public String OptionEditButtonText
		{
			get
			{
				Object state = ViewState["OptionEditButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_EditText;
			}
			set
			{
				String originalValue = ViewState["OptionEditButtonText"] as String;
				ViewState["OptionEditButtonText"] = value;
				if ( originalValue != value )
				{
					this.EnsureOptionEditorButtonText();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text of update buttons in the option editor grid.
		/// </summary>
		/// <remarks>
		/// For best results, set this property at design time.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of update buttons in the option editor grid." ),
		MbwcDefaultValue( "PollManager_UpdateText" ),
		]
		public String OptionUpdateButtonText
		{
			get
			{
				Object state = ViewState["OptionUpdateButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_UpdateText;
			}
			set
			{
				String originalValue = ViewState["OptionUpdateButtonText"] as String;
				ViewState["OptionUpdateButtonText"] = value;
				if ( originalValue != value )
				{
					this.EnsureOptionEditorButtonText();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text of cancel buttons in the option editor grid.
		/// </summary>
		/// <remarks>
		/// For best results, set this property at design time.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of cancel buttons in the option editor grid." ),
		MbwcDefaultValue( "PollManager_CancelText" ),
		]
		public String OptionCancelButtonText
		{
			get
			{
				Object state = ViewState["OptionCancelButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_CancelText;
			}
			set
			{
				String originalValue = ViewState["OptionCancelButtonText"] as String;
				ViewState["OptionCancelButtonText"] = value;
				if ( originalValue != value )
				{
					this.EnsureOptionEditorButtonText();
				}
			}
		}


		#endregion

		#region Filter Properties

		/// <summary>
		/// Gets or sets the title of the Filter area.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the title of the Filter area." ),
		MbwcDefaultValue( "PollManager_Default_FilterTitleText" ),
		]
		public String FilterTitleText
		{
			get
			{
				Object state = ViewState["FilterTitleText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_FilterTitleText;
			}
			set
			{
				ViewState["FilterTitleText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text shown in the Apply Filter button.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text shown in the Apply Filter button." ),
		MbwcDefaultValue( "PollManager_Default_ApplyFilterButtonText" ),
		]
		public String ApplyFilterButtonText
		{
			get
			{
				Object state = ViewState["ApplyFilterButtonText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_ApplyFilterButtonText;
			}
			set
			{
				ViewState["ApplyFilterButtonText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text for the label of the Filter's Poll Text property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text for the label of the Filter's Poll Text property." ),
		MbwcDefaultValue( "PollManager_Default_FilterPollTextLabelText" ),
		]
		public String FilterPollTextLabelText
		{
			get
			{
				Object state = ViewState["FilterPollTextLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_FilterPollTextLabelText;
			}
			set
			{
				ViewState["FilterPollTextLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text for the label of the Filter's Category property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text for the label of the Filter's Category property." ),
		MbwcDefaultValue( "PollManager_Default_FilterCategoryLabelText" ),
		]
		public String FilterCategoryLabelText
		{
			get
			{
				Object state = ViewState["FilterCategoryLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_FilterCategoryLabelText;
			}
			set
			{
				ViewState["FilterCategoryLabelText"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the label for the Filter's Active Dates property.
		/// </summary>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the text of the label for the Filter's Active Dates property." ),
		MbwcDefaultValue( "PollManager_Default_FilterActiveDatesLabelText" ),
		]
		public String FilterActiveDatesLabelText
		{
			get
			{
				Object state = ViewState["FilterActiveDatesLabelText"];
				if ( state != null )
				{
					return (String)state;
				}
				return Resources.PollManager_Default_FilterActiveDatesLabelText;
			}
			set
			{
				ViewState["FilterActiveDatesLabelText"] = value;
			}
		}


		#endregion

		/// <summary>
		/// Overrides <see cref="WebControl.TagKey"/>
		/// </summary>
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Div;
			}
		}


		private Int32 CurrentPollID
		{
			get
			{
				if ( pollGrid.DataKeys.Count > 0 )
				{
					return Convert.ToInt32( pollGrid.DataKeys[0], System.Globalization.CultureInfo.InvariantCulture );
				}
				return -1;
			}
		}

		#endregion

		#region ViewState

		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>.
		/// </summary>
		protected override void LoadViewState( Object savedState )
		{
			if ( savedState == null )
			{
				base.LoadViewState( null );
				return;
			}

			Object[] state = savedState as Object[];
			if ( state == null || state.Length != 4 )
			{
				throw new ArgumentException( "Invalid ViewState", "savedState" );
			}

			base.LoadViewState( state[0] );
			if ( state[1] != null )
			{
				( (IStateManager)this.TitleStyle ).LoadViewState( state[1] );
			}
			if ( state[2] != null )
			{
				( (IStateManager)this.LabelStyle ).LoadViewState( state[2] );
			}
			if ( state[3] != null )
			{
				( (IStateManager)this.FieldSetStyle ).LoadViewState( state[3] );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.TrackViewState"/>.
		/// </summary>
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ( this._titleStyle != null )
			{
				( (IStateManager)_titleStyle ).TrackViewState();
			}
			if ( this._labelStyle != null )
			{
				( (IStateManager)_labelStyle ).TrackViewState();
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>.
		/// </summary>
		/// <returns></returns>
		protected override Object SaveViewState()
		{
			Object[] state = new Object[4];

			state[0] = base.SaveViewState();
			if ( this._titleStyle != null )
			{
				state[1] = ( (IStateManager)_titleStyle ).SaveViewState();
			}
			if ( this._labelStyle != null )
			{
				state[2] = ( (IStateManager)_labelStyle ).SaveViewState();
			}
			if ( this._fieldSetStyle != null )
			{
				state[3] = ( (IStateManager)_fieldSetStyle ).SaveViewState();
			}


			for ( Int32 i = 0; i < state.Length; i++ )
			{
				if ( state[i] != null )
				{
					return state;
				}
			}
			return null;
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

			this.editorInterface = new PlaceHolder();
			this.Controls.Add( editorInterface );
			BuildEditorInterface();

			this.addInterface = new PlaceHolder();
			this.Controls.Add( addInterface );
			BuildAddInterface();

			this.addInterface.Visible = false;

		}

		private void BuildEditorInterface()
		{

			#region General Design

			Table editorTable = new Table();
			editorTable.CellPadding = 10;
			editorTable.CellSpacing = 0;
			editorTable.BorderWidth = Unit.Pixel( 0 );
			editorTable.Width = Unit.Percentage( 100 );
			this.editorInterface.Controls.Add( editorTable );

			TableRow mainRow = new TableRow();
			editorTable.Rows.Add( mainRow );

			TableCell pollCell = new TableCell();
			pollCell.VerticalAlign = VerticalAlign.Top;
			mainRow.Cells.Add( pollCell );

			TableCell optionsCell = new TableCell();
			optionsCell.Width = Unit.Percentage( 100 );
			optionsCell.VerticalAlign = VerticalAlign.Top;
			mainRow.Cells.Add( optionsCell );

			TableRow filterRow = new TableRow();
			editorTable.Rows.Add( filterRow );

			TableCell filterCell = new TableCell();
			filterCell.ColumnSpan = 2;
			filterRow.Cells.Add( filterCell );

			#endregion

			#region Poll Editor

			pollFieldSet = new PollManagerArea();
			pollCell.Controls.Add( pollFieldSet );

			this.EmptyPollGridLabel = new Label();
			this.EmptyPollGridLabel.Visible = false;
			pollFieldSet.Controls.Add( EmptyPollGridLabel );

			this.pollGrid = new DataGrid();
			this.pollGrid.BorderWidth = Unit.Pixel( 0 );
			this.pollGrid.AllowPaging = true;
			this.pollGrid.AllowCustomPaging = true;
			this.pollGrid.PageSize = 1;
			this.pollGrid.AutoGenerateColumns = false;
			this.pollGrid.ShowHeader = false;
			this.pollGrid.DataKeyField = "PollID";
			this.pollGrid.CellPadding = 2;
			this.pollGrid.UpdateCommand += new DataGridCommandEventHandler( pollGrid_UpdateCommand );
			this.pollGrid.DeleteCommand += new DataGridCommandEventHandler( pollGrid_DeleteCommand );
			this.pollGrid.ItemCommand += new DataGridCommandEventHandler( pollGrid_ItemCommand );
			this.pollGrid.PageIndexChanged += new DataGridPageChangedEventHandler( pollGrid_PageIndexChanged );
			this.pollGrid.PagerStyle.Mode = PagerMode.NumericPages;

			TemplateColumn pollColumn = new TemplateColumn();
			pollColumn.ItemTemplate = new PollEditingTemplate();
			this.pollGrid.Columns.Add( pollColumn );

			pollFieldSet.Controls.Add( pollGrid );

			this.AddNewPoll = new Button();
			this.AddNewPoll.ID = "AddNewPoll";
			this.AddNewPoll.CausesValidation = false;
			this.AddNewPoll.Click += new EventHandler( AddNewPoll_Click );
			pollFieldSet.Controls.Add( this.AddNewPoll );

			#endregion

			#region Option Editor

			this.optionEditorInterface = new PlaceHolder();
			optionsCell.Controls.Add( optionEditorInterface );

			optionEditorFieldSet = new PollManagerArea();
			optionEditorInterface.Controls.Add( optionEditorFieldSet );

			this.optionsGrid = new DataGrid();
			optionsGrid.ID = "optionsGrid";
			optionsGrid.AutoGenerateColumns = false;
			optionsGrid.DataKeyField = "OptionID";
			optionsGrid.Width = Unit.Percentage( 100 );
			optionsGrid.EditCommand += new DataGridCommandEventHandler( optionsGrid_EditCommand );
			optionsGrid.CancelCommand += new DataGridCommandEventHandler( optionsGrid_CancelCommand );
			optionsGrid.UpdateCommand += new DataGridCommandEventHandler( optionsGrid_UpdateCommand );
			optionsGrid.DeleteCommand += new DataGridCommandEventHandler( optionsGrid_DeleteCommand );

			EditCommandColumn editSelector = new EditCommandColumn();
			editSelector.ButtonType = ButtonColumnType.PushButton;
			editSelector.EditText = this.OptionEditButtonText;
			editSelector.CancelText = this.OptionCancelButtonText;
			editSelector.UpdateText = this.OptionUpdateButtonText;
			editSelector.CausesValidation = true;
			editSelector.ValidationGroup = "MetaBuilders_WebControls_PollManager__EditOption";
			optionsGrid.Columns.Add( editSelector );

			ButtonColumn deleteColumn = new ButtonColumn();
			deleteColumn.ButtonType = ButtonColumnType.PushButton;
			deleteColumn.CommandName = "Delete";
			deleteColumn.Text = this.OptionDeleteButtonText;
			optionsGrid.Columns.Add( deleteColumn );

			BoundColumn answerColumn = new BoundColumn();
			answerColumn.DataField = "Text";
			answerColumn.HeaderText = "";
			optionsGrid.Columns.Add( answerColumn );

			TemplateColumn displayOrderColumn = new TemplateColumn();
			displayOrderColumn.HeaderText = "";
			displayOrderColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
			displayOrderColumn.ItemTemplate = new DisplayOrderViewTemplate();
			displayOrderColumn.EditItemTemplate = new DisplayOrderEditTemplate();
			optionsGrid.Columns.Add( displayOrderColumn );

			optionEditorFieldSet.Controls.Add( optionsGrid );

			optionEditorInterface.Controls.Add( new LiteralControl( "<br />" ) );

			newOptionFieldSet = new PollManagerArea();
			optionEditorInterface.Controls.Add( newOptionFieldSet );

			newOptionFieldSet.Controls.Add( new LiteralControl( "<table><tr><td align='right' >" ) );

			this.NewOptionTextLabel = new Label();
			NewOptionTextLabel.ID = "NewOptionTextLabel";
			newOptionFieldSet.Controls.Add( NewOptionTextLabel );

			newOptionFieldSet.Controls.Add( new LiteralControl( "</td><td>" ) );

			this.NewOptionText = new TextBox();
			NewOptionText.ID = "NewOptionText";
			newOptionFieldSet.Controls.Add( NewOptionText );

			RequiredFieldValidator NewOptionTextRequired = new RequiredFieldValidator();
			NewOptionTextRequired.ControlToValidate = "NewOptionText";
			NewOptionTextRequired.Display = ValidatorDisplay.Dynamic;
			NewOptionTextRequired.SetFocusOnError = true;
			NewOptionTextRequired.ValidationGroup = "MetaBuilders_WebControls_PollManager__NewOption";
			NewOptionTextRequired.Text = Resources.PollManager_Validation_OptionTextRequired;
			newOptionFieldSet.Controls.Add( NewOptionTextRequired );

			newOptionFieldSet.Controls.Add( new LiteralControl( "</td></tr><tr><td align='right' >" ) );

			this.NewOptionDisplayOrderLabel = new Label();
			NewOptionDisplayOrderLabel.ID = "NewOptionDisplayOrderLabel";
			newOptionFieldSet.Controls.Add( NewOptionDisplayOrderLabel );

			newOptionFieldSet.Controls.Add( new LiteralControl( "</td><td>" ) );

			this.NewDisplayOrder = new TextBox();
			NewDisplayOrder.ID = "NewDisplayOrder";
			newOptionFieldSet.Controls.Add( NewDisplayOrder );

			CompareValidator NewDisplayOrderValidator = new CompareValidator();
			NewDisplayOrderValidator.ControlToValidate = "NewDisplayOrder";
			NewDisplayOrderValidator.ValidationGroup = "MetaBuilders_WebControls_PollManager__NewOption";
			NewDisplayOrderValidator.Operator = ValidationCompareOperator.DataTypeCheck;
			NewDisplayOrderValidator.Type = ValidationDataType.Integer;
			NewDisplayOrderValidator.Text = Resources.PollManager_Validation_DisplayOrderMustBeNumber;
			NewDisplayOrderValidator.SetFocusOnError = true;
			NewDisplayOrderValidator.Display = ValidatorDisplay.Dynamic;

			newOptionFieldSet.Controls.Add( new LiteralControl( "</td></tr><tr><td colspan='2' >" ) );

			this.AddNewOption = new Button();
			AddNewOption.ID = "AddNewOption";
			AddNewOption.Click += new EventHandler( AddNewOption_Click );
			this.AddNewOption.CausesValidation = true;
			this.AddNewOption.ValidationGroup = "MetaBuilders_WebControls_PollManager__NewOption";
			newOptionFieldSet.Controls.Add( AddNewOption );

			newOptionFieldSet.Controls.Add( new LiteralControl( "</td></tr></table>" ) );

			#endregion

			#region Filter

			filterFieldSet = new PollManagerArea();
			filterCell.Controls.Add( filterFieldSet );

			filterFieldSet.Controls.Add( new LiteralControl( "<table border='0' ><tr><td nowrap='nowrap' align='right' >" ) );

			this.PollFilterTextLabel = new Label();
			this.PollFilterTextLabel.ID = "PollFilterTextLabel";
			filterFieldSet.Controls.Add( PollFilterTextLabel );

			filterFieldSet.Controls.Add( new LiteralControl( "</td><td>" ) );

			this.PollFilterText = new TextBox();
			this.PollFilterText.ID = "PollFilterText";
			filterFieldSet.Controls.Add( this.PollFilterText );

			filterFieldSet.Controls.Add( new LiteralControl( "</td></tr><tr><td nowrap='nowrap' align='right' >" ) );

			this.PollFilterCategoryLabel = new Label();
			this.PollFilterCategoryLabel.ID = "PollFilterCategoryLabel";
			filterFieldSet.Controls.Add( PollFilterCategoryLabel );

			filterFieldSet.Controls.Add( new LiteralControl( "</td><td>" ) );

			this.PollFilterCategory = new TextBox();
			this.PollFilterCategory.ID = "PollFilterCategory";
			filterFieldSet.Controls.Add( this.PollFilterCategory );

			filterFieldSet.Controls.Add( new LiteralControl( "</td></tr><tr><td nowrap='nowrap' align='right' >" ) );

			this.PollFilterActiveDatesLabel = new Label();
			this.PollFilterActiveDatesLabel.ID = "PollFilterActiveDatesLabel";
			filterFieldSet.Controls.Add( PollFilterActiveDatesLabel );

			filterFieldSet.Controls.Add( new LiteralControl( "</td><td>" ) );

			this.PollFilterBeginDate = new TextBox();
			this.PollFilterBeginDate.ID = "PollFilterBeginDate";
			filterFieldSet.Controls.Add( this.PollFilterBeginDate );

			filterFieldSet.Controls.Add( new LiteralControl( " - " ) );
			this.PollFilterEndDate = new TextBox();
			this.PollFilterEndDate.ID = "PollFilterEndDate";
			filterFieldSet.Controls.Add( this.PollFilterEndDate );

			filterFieldSet.Controls.Add( new LiteralControl( "</td></tr><tr><td nowrap='nowrap' colspan='2' >" ) );

			this.PollFilterApply = new Button();
			this.PollFilterApply.ID = "PollFilterApply";
			this.PollFilterApply.Click += new EventHandler( PollFilterApply_Click );
			this.PollFilterApply.CausesValidation = false;
			filterFieldSet.Controls.Add( this.PollFilterApply );

			filterFieldSet.Controls.Add( new LiteralControl( "</td></tr></table>" ) );

			#endregion

		}

		private void BuildAddInterface()
		{

			newPollFieldSet = new PollManagerArea();
			this.addInterface.Controls.Add( newPollFieldSet );

			Table main = new Table();
			main.Width = Unit.Percentage( 100 );
			main.BorderWidth = Unit.Pixel( 0 );
			this.newPollFieldSet.Controls.Add( main );

			NewPollTextLabel = new Label();
			NewPollTextLabel.ID = "NewPollTextLabel";
			NewPollText = new TextBox();
			NewPollText.ID = "NewPollText";
			NewPollText.TextMode = TextBoxMode.MultiLine;
			AddControlRow( main, NewPollTextLabel, NewPollText );

			NewPollStartDateLabel = new Label();
			NewPollStartDateLabel.ID = "NewPollStartDateLabel";
			NewPollStartDate = new TextBox();
			NewPollStartDate.ID = "NewPollStartDate";
			AddControlRow( main, NewPollStartDateLabel, NewPollStartDate );

			NewPollVoteModeLabel = new Label();
			NewPollVoteModeLabel.ID = "NewPollVoteModeLabel";
			NewPollVoteMode = new DropDownList();
			NewPollVoteMode.ID = "NewPollVoteMode";
			NewPollVoteMode.Items.Add( "Single" );
			NewPollVoteMode.Items.Add( "Multiple" );
			NewPollVoteMode.Items.Add( "Rating" );
			AddControlRow( main, NewPollVoteModeLabel, NewPollVoteMode );

			NewPollEnabledLabel = new Label();
			NewPollEnabledLabel.ID = "NewPollEnabledLabel";
			NewPollEnabled = new CheckBox();
			NewPollEnabled.ID = "NewPollEnabled";
			AddControlRow( main, NewPollEnabledLabel, NewPollEnabled );

			NewPollAllowWriteInsLabel = new Label();
			NewPollAllowWriteInsLabel.ID = "NewPollAllowWriteInsLabel";
			NewPollAllowWriteIns = new CheckBox();
			NewPollAllowWriteIns.ID = "NewPollAllowWriteIns";
			AddControlRow( main, NewPollAllowWriteInsLabel, NewPollAllowWriteIns );

			NewPollCategoryLabel = new Label();
			NewPollCategoryLabel.ID = "NewPollCategoryLabel";
			NewPollCategory = new TextBox();
			NewPollCategory.ID = "NewPollCategory";
			AddControlRow( main, NewPollCategoryLabel, NewPollCategory );

			TableRow row = new TableRow();
			main.Rows.Add( row );

			TableCell buttonCell = new TableCell();
			buttonCell.ColumnSpan = 2;
			buttonCell.HorizontalAlign = HorizontalAlign.Left;
			row.Cells.Add( buttonCell );

			this.NewPollAdd = new Button();
			this.NewPollAdd.ID = "NewPollAdd";
			this.NewPollAdd.Click += new EventHandler( NewPollAdd_Click );
			this.NewPollAdd.CausesValidation = false;
			buttonCell.Controls.Add( NewPollAdd );

			buttonCell.Controls.Add( new LiteralControl( " " ) );

			this.NewPollCancel = new Button();
			this.NewPollCancel.ID = "NewPollCancel";
			this.NewPollCancel.Click += new EventHandler( NewPollCancel_Click );
			this.NewPollCancel.CausesValidation = false;
			buttonCell.Controls.Add( NewPollCancel );

		}

		private static void AddControlRow( Table main, Label label, Control editor )
		{
			TableRow row = new TableRow();
			TableCell labelCell = new TableCell();
			TableCell editCell = new TableCell();

			main.Rows.Add( row );
			row.Cells.Add( labelCell );
			row.Cells.Add( editCell );

			labelCell.HorizontalAlign = HorizontalAlign.Right;
			labelCell.Controls.Add( label );
			editCell.Controls.Add( editor );

			labelCell.Wrap = false;
			editCell.Width = Unit.Percentage( 100 );
		}


		/// <summary>
		/// Overrides <see cref="Control.OnInit"/>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit( EventArgs e )
		{
			base.OnInit( e );
			this.EnsureChildControls();
			if ( HttpContext.Current != null && this.Page != null && !this.Page.IsPostBack )
			{
				this.DataBindPoll();
			}
		}

		/// <exclude/>
		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{
			base.AddAttributesToRender( writer );
			if ( this.addInterface.Visible )
			{
				writer.AddStyleAttribute( "padding", "10px" );
			}
		}


		/// <summary>
		/// Overrides <see cref="Control.Render"/>
		/// </summary>
		protected override void Render( HtmlTextWriter writer )
		{
			this.EnsureChildControls();

			if ( HttpContext.Current == null )
			{
				PollDataSet data = Polling.CreatePlaceholderPoll();
				this.DataBindPoll( data );
			}

			ApplyProperties();

			base.Render( writer );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity" )]
		private void ApplyProperties()
		{

			#region Showing poll-editor/no-poll areas
			if ( this.pollGrid.Items.Count == 0 )
			{
				this.optionEditorInterface.Visible = false;
				this.pollGrid.Visible = false;
				this.EmptyPollGridLabel.Visible = true;
			}
			else
			{
				this.optionEditorInterface.Visible = true;
				this.pollGrid.Visible = true;
				this.EmptyPollGridLabel.Visible = false;
			}
			#endregion

			#region Poll Editor Controls

			// PollManagerArea
			if ( this._fieldSetStyle != null )
			{
				this.pollFieldSet.ControlStyle.CopyFrom( this._fieldSetStyle );
			}

			// Title
			this.pollFieldSet.Title = this.PollEditorTitleText;
			ApplyTitleStyle( this.pollFieldSet );

			// Labels
			this.EmptyPollGridLabel.Text = "<br>" + this.EmptyPollsLabelText + "<br>";
			this.ApplyLabelStyle( this.EmptyPollGridLabel );

			// Buttons
			this.AddNewPoll.Text = this.NewPollButtonText;
			this.AddNewPoll.Enabled = this.Enabled;

			// Controls inside the template
			if ( this.pollGrid.Items.Count > 0 )
			{
				DataGridItem item = this.pollGrid.Items[0];

				Button deletePollButton = item.FindControl( "DeletePoll" ) as Button;
				if ( deletePollButton != null )
				{
					deletePollButton.Text = this.DeletePollButtonText;
					deletePollButton.Attributes["onclick"] = "if ( !confirm( '" + this.DeleteConfirmationMessage.Replace( "'", @"/'" ) + "' ) ) { return false; } ";
					deletePollButton.Enabled = this.Enabled;
				}
				Button updatePollButton = item.FindControl( "UpdatePoll" ) as Button;
				if ( updatePollButton != null )
				{
					updatePollButton.Text = this.UpdatePollButtonText;
					updatePollButton.Enabled = this.Enabled;
				}
				Button clearPollButton = item.FindControl( "ClearPoll" ) as Button;
				if ( clearPollButton != null )
				{
					clearPollButton.Text = this.ClearPollVotesButtonText;
					clearPollButton.Enabled = this.Enabled;
				}

				Label idLabel = item.FindControl( "PollIDLabel" ) as Label;
				if ( idLabel != null )
				{
					idLabel.Text = PollIdLabelText;
					ApplyLabelStyle( idLabel );
				}
				Label textLabel = item.FindControl( "TextLabel" ) as Label;
				if ( textLabel != null )
				{
					textLabel.Text = PollTextLabelText;
					ApplyLabelStyle( textLabel );
				}
				Label startDateLabel = item.FindControl( "StartDateLabel" ) as Label;
				if ( startDateLabel != null )
				{
					startDateLabel.Text = PollStartDateLabelText;
					ApplyLabelStyle( startDateLabel );
				}
				Label voteModeLabel = item.FindControl( "VoteModeLabel" ) as Label;
				if ( voteModeLabel != null )
				{
					voteModeLabel.Text = PollVoteModeLabelText;
					ApplyLabelStyle( voteModeLabel );
				}
				Label enabledLabel = item.FindControl( "EnabledLabel" ) as Label;
				if ( enabledLabel != null )
				{
					enabledLabel.Text = PollEnabledLabelText;
					ApplyLabelStyle( enabledLabel );
				}
				Label allowWriteInsLabel = item.FindControl( "AllowWriteInsLabel" ) as Label;
				if ( allowWriteInsLabel != null )
				{
					allowWriteInsLabel.Text = PollAllowWriteInsLabelText;
					ApplyLabelStyle( allowWriteInsLabel );
				}
				Label categoryLabel = item.FindControl( "CategoryLabel" ) as Label;
				if ( categoryLabel != null )
				{
					categoryLabel.Text = PollCategoryLabelText;
					ApplyLabelStyle( categoryLabel );
				}

				String[] inputControls = { "Text", "StartDate", "VoteMode", "Enabled", "AllowWriteIns", "Category" };
				for ( Int32 i = 0; i < inputControls.Length; i++ )
				{
					WebControl editorControl = item.FindControl( inputControls[i] ) as WebControl;
					if ( editorControl != null )
					{
						editorControl.Enabled = this.Enabled;
					}
				}

			}

			#endregion

			#region New Poll Controls

			// PollManagerArea
			if ( this._fieldSetStyle != null )
			{
				this.newPollFieldSet.ControlStyle.CopyFrom( this._fieldSetStyle );
			}

			// Title
			this.newPollFieldSet.Title = this.NewPollTitleText;
			ApplyTitleStyle( this.newPollFieldSet );

			// Labels
			this.NewPollTextLabel.Text = this.PollTextLabelText;
			this.NewPollStartDateLabel.Text = this.PollStartDateLabelText;
			this.NewPollVoteModeLabel.Text = this.PollVoteModeLabelText;
			this.NewPollEnabledLabel.Text = this.PollEnabledLabelText;
			this.NewPollAllowWriteInsLabel.Text = this.PollAllowWriteInsLabelText;
			this.NewPollCategoryLabel.Text = this.PollCategoryLabelText;
			ApplyLabelStyle( this.NewPollTextLabel );
			ApplyLabelStyle( this.NewPollStartDateLabel );
			ApplyLabelStyle( this.NewPollVoteModeLabel );
			ApplyLabelStyle( this.NewPollEnabledLabel );
			ApplyLabelStyle( this.NewPollAllowWriteInsLabel );
			ApplyLabelStyle( this.NewPollCategoryLabel );

			// Buttons
			this.NewPollAdd.Text = this.AddNewPollButtonText;
			this.NewPollCancel.Text = this.CancelNewPollButtonText;
			this.NewPollAdd.Enabled = this.Enabled;
			this.NewPollCancel.Enabled = this.Enabled;

			// Inputs
			this.NewPollText.Enabled = this.Enabled;
			this.NewPollStartDate.Enabled = this.Enabled;
			this.NewPollVoteMode.Enabled = this.Enabled;
			this.NewPollEnabled.Enabled = this.Enabled;
			this.NewPollAllowWriteIns.Enabled = this.Enabled;
			this.NewPollCategory.Enabled = this.Enabled;

			#endregion

			#region Option Editor Controls

			// FieldSets
			if ( this._fieldSetStyle != null )
			{
				this.optionEditorFieldSet.ControlStyle.CopyFrom( this._fieldSetStyle );
			}
			if ( this._fieldSetStyle != null )
			{
				this.newOptionFieldSet.ControlStyle.CopyFrom( this._fieldSetStyle );
			}

			// Titles
			this.optionEditorFieldSet.Title = this.OptionEditorTitleText;
			this.newOptionFieldSet.Title = this.AddOptionTitleText;
			ApplyTitleStyle( this.optionEditorFieldSet );
			ApplyTitleStyle( this.newOptionFieldSet );

			// Buttons
			this.AddNewOption.Text = this.AddOptionButtonText;
			this.AddNewOption.Enabled = this.Enabled;

			// Option Editor Buttons
			if ( this.optionsGrid.Items.Count > 0 )
			{
				foreach ( DataGridItem item in this.optionsGrid.Items )
				{
					for ( Int32 col = 0; col < item.Cells.Count; col++ )
					{
						TableCell theCell = item.Cells[col];
						for ( Int32 i = 0; i < theCell.Controls.Count; i++ )
						{
							Button commandButton = theCell.Controls[i] as Button;
							if ( commandButton != null )
							{
								commandButton.Enabled = this.Enabled;
								if ( commandButton.CommandName == "Delete" )
								{
									commandButton.Attributes["onclick"] = "if ( !confirm( '" + DeleteConfirmationMessage.Replace( "'", @"/'" ) + "' ) ) { return false; } ";
								}
							}
							TextBox editControl = theCell.Controls[i] as TextBox;
							if ( editControl != null )
							{
								editControl.Enabled = this.Enabled;
							}
						}
					}
				}
			}

			// Labels
			this.NewOptionTextLabel.Text = this.OptionTextLabelText;
			this.NewOptionDisplayOrderLabel.Text = this.OptionDisplayOrderLabelText;
			ApplyLabelStyle( NewOptionTextLabel );
			ApplyLabelStyle( NewOptionDisplayOrderLabel );

			// This is a bit of a hack,
			// but it seems to be the only way to change the header text after databinding
			Table gridTable = this.optionsGrid.Controls[0] as Table;
			if ( gridTable != null )
			{
				TableRow headerRow = gridTable.Rows[0];

				headerRow.Cells[optionsGridTextColumnIndex].Text = "";
				Label label = new Label();
				label.Text = this.OptionTextLabelText;
				ApplyLabelStyle( label );
				headerRow.Cells[optionsGridTextColumnIndex].Controls.Add( label );

				headerRow.Cells[optionsGridDisplayOrderColumnIndex].Text = "";
				label = new Label();
				label.Text = this.OptionDisplayOrderLabelText;
				ApplyLabelStyle( label );
				headerRow.Cells[optionsGridDisplayOrderColumnIndex].Controls.Add( label );

			}

			// Inputs
			this.NewOptionText.Enabled = this.Enabled;
			this.NewDisplayOrder.Enabled = this.Enabled;


			#endregion

			#region Filter Controls

			// FieldSets
			if ( this._fieldSetStyle != null )
			{
				this.filterFieldSet.ControlStyle.CopyFrom( this._fieldSetStyle );
			}

			// Title
			this.filterFieldSet.Title = this.FilterTitleText;
			ApplyTitleStyle( this.filterFieldSet );

			// Labels
			this.PollFilterTextLabel.Text = this.FilterPollTextLabelText;
			this.PollFilterCategoryLabel.Text = this.FilterCategoryLabelText;
			this.PollFilterActiveDatesLabel.Text = this.FilterActiveDatesLabelText;
			ApplyLabelStyle( this.PollFilterTextLabel );
			ApplyLabelStyle( this.PollFilterCategoryLabel );
			ApplyLabelStyle( this.PollFilterActiveDatesLabel );

			// Button
			this.PollFilterApply.Text = this.ApplyFilterButtonText;
			this.PollFilterApply.Enabled = this.Enabled;

			// Inputs
			this.PollFilterText.Enabled = this.Enabled;
			this.PollFilterCategory.Enabled = this.Enabled;
			this.PollFilterBeginDate.Enabled = this.Enabled;
			this.PollFilterEndDate.Enabled = this.Enabled;

			#endregion

		}

		private void ApplyTitleStyle( PollManagerArea fieldSet )
		{
			fieldSet.TitleStyle.Reset();
			fieldSet.TitleStyle.Font.Size = FontUnit.Parse( "1.5em", System.Globalization.CultureInfo.InvariantCulture );
			if ( this._titleStyle != null )
			{
				fieldSet.TitleStyle.CopyFrom( _titleStyle );
			}
		}

		private void ApplyLabelStyle( Label label )
		{
			label.ControlStyle.Reset();
			if ( this._labelStyle != null )
			{
				label.ControlStyle.CopyFrom( _labelStyle );
			}
		}

		#endregion

		#region Poll Events

		private void DataBindPoll( PollDataSet currentData )
		{
			System.Data.DataView view = new System.Data.DataView( currentData.Polls );
			view.Sort = "StartDate desc";

			if ( view.Count <= this.pollGrid.CurrentPageIndex )
			{
				this.pollGrid.CurrentPageIndex = 0;
			}

			if ( view.Count == 0 )
			{
				this.pollGrid.DataSource = view;
			}
			else
			{
				PollDataSet.PollsRow currentPollRow = view[this.pollGrid.CurrentPageIndex].Row as PollDataSet.PollsRow;
				PollDataSet.PollsRow[] pollGridDataSource = new PollDataSet.PollsRow[1];
				pollGridDataSource[0] = currentPollRow;
				this.pollGrid.DataSource = pollGridDataSource;
			}

			this.pollGrid.VirtualItemCount = view.Count;
			this.pollGrid.DataBind();

			this.DataBindOptions( currentData );

		}

		private void DataBindPoll()
		{
			PollSelectionFilter filter = new PollSelectionFilter();

			if ( this.PollFilterText.Text.Length > 0 )
			{
				filter.Text = this.PollFilterText.Text;
			}
			if ( this.PollFilterCategory.Text.Length > 0 )
			{
				filter.Text = this.PollFilterCategory.Text;
			}
			if ( this.PollFilterBeginDate.Text.Length > 0 )
			{
				try
				{
					filter.BeginDate = new System.Data.SqlTypes.SqlDateTime( DateTime.Parse( this.PollFilterBeginDate.Text, System.Globalization.CultureInfo.InvariantCulture ) );
				}
				catch ( System.FormatException )
				{
					// gulp
				}
			}
			if ( this.PollFilterEndDate.Text.Length > 0 )
			{
				try
				{
					filter.EndDate = new System.Data.SqlTypes.SqlDateTime( DateTime.Parse( this.PollFilterEndDate.Text, System.Globalization.CultureInfo.InvariantCulture ) );
				}
				catch ( System.FormatException )
				{
					// gulp
				}
			}

			PollDataSet currentData;
			if ( filter.IsFilterSet )
			{
				currentData = Polling.GetPollsByFilter( filter );
			}
			else
			{
				currentData = Polling.GetAllPolls();
				if ( currentData.Polls.Count == 0 )
				{
					this.editorInterface.Visible = false;
					this.addInterface.Visible = true;
				}
			}

			DataBindPoll( currentData );

		}

		private void pollGrid_UpdateCommand( object source, DataGridCommandEventArgs e )
		{
			AuthorizeChange();

			DataGridItem container = e.Item;
			TextBox text = container.FindControl( "Text" ) as TextBox;
			TextBox startDate = container.FindControl( "StartDate" ) as TextBox;
			DropDownList voteMode = container.FindControl( "VoteMode" ) as DropDownList;
			CheckBox enabled = container.FindControl( "Enabled" ) as CheckBox;
			CheckBox allowWriteIns = container.FindControl( "AllowWriteIns" ) as CheckBox;
			TextBox category = container.FindControl( "Category" ) as TextBox;

			Int32 pollID = Convert.ToInt32( this.pollGrid.DataKeys[e.Item.ItemIndex], System.Globalization.CultureInfo.InvariantCulture );
			DateTime startDateTime = DateTime.Now;
			try
			{
				startDateTime = DateTime.Parse( startDate.Text, System.Globalization.CultureInfo.InvariantCulture );
			}
			catch ( FormatException )
			{
				//gulp!
			}
			VoteSelectionMode voteSelectionMode = (VoteSelectionMode)Enum.Parse( typeof( VoteSelectionMode ), voteMode.SelectedValue );

			Polling.UpdatePoll( pollID, text.Text, startDateTime, voteSelectionMode, enabled.Checked, allowWriteIns.Checked, category.Text );

			this.DataBindPoll();
			this.OnPollDataChanged( EventArgs.Empty );
		}

		private void pollGrid_DeleteCommand( object source, DataGridCommandEventArgs e )
		{
			AuthorizeChange();

			Int32 pollID = Convert.ToInt32( this.pollGrid.DataKeys[e.Item.ItemIndex], System.Globalization.CultureInfo.InvariantCulture );
			Polling.DeletePoll( pollID );

			this.DataBindPoll();
			this.OnPollDataChanged( EventArgs.Empty );
		}

		private void pollGrid_ItemCommand( object source, DataGridCommandEventArgs e )
		{
			if ( e.CommandName == "Clear" )
			{
				AuthorizeChange();

				Int32 pollID = Convert.ToInt32( this.pollGrid.DataKeys[e.Item.ItemIndex], System.Globalization.CultureInfo.InvariantCulture );
				Polling.ResetPoll( pollID );

				this.DataBindPoll();
				this.OnPollDataChanged( EventArgs.Empty );
			}
		}

		private void pollGrid_PageIndexChanged( object source, DataGridPageChangedEventArgs e )
		{
			this.pollGrid.CurrentPageIndex = e.NewPageIndex;
			DataBindPoll();
		}

		private void AddNewPoll_Click( Object sender, EventArgs e )
		{
			this.editorInterface.Visible = false;
			this.addInterface.Visible = true;
		}
		private void NewPollAdd_Click( Object sender, EventArgs e )
		{
			AuthorizeChange();

			DateTime startDate = DateTime.Now;
			try
			{
				startDate = DateTime.Parse( NewPollStartDate.Text, System.Globalization.CultureInfo.InvariantCulture );
			}
			catch ( FormatException )
			{
				// gulp!
			}
			VoteSelectionMode voteMode = (VoteSelectionMode)Enum.Parse( typeof( VoteSelectionMode ), NewPollVoteMode.SelectedValue );

			Polling.InsertPoll( NewPollText.Text, startDate, voteMode, this.NewPollEnabled.Checked, this.NewPollAllowWriteIns.Checked, this.NewPollCategory.Text );

			this.editorInterface.Visible = true;
			this.addInterface.Visible = false;

			this.DataBindPoll();
			this.OnPollDataChanged( EventArgs.Empty );
		}

		private void NewPollCancel_Click( Object sender, EventArgs e )
		{
			this.editorInterface.Visible = true;
			this.addInterface.Visible = false;
		}

		private void PollFilterApply_Click( Object sender, EventArgs e )
		{
			this.pollGrid.CurrentPageIndex = 0;
			this.pollGrid.EditItemIndex = -1;
			this.DataBindPoll();
		}

		#endregion

		#region Option Events

		private void DataBindOptions( PollDataSet data )
		{
			System.Data.DataView view = new System.Data.DataView( data.Options );
			view.RowFilter = "PollID = " + this.CurrentPollID.ToString( System.Globalization.CultureInfo.InvariantCulture );
			this.optionsGrid.DataSource = view;
			this.optionsGrid.DataBind();
		}

		private void DataBindOptions()
		{
			DataBindOptions( Polling.GetPoll( this.CurrentPollID ) );
		}

		private void optionsGrid_EditCommand( object source, DataGridCommandEventArgs e )
		{
			this.optionsGrid.EditItemIndex = e.Item.ItemIndex;
			this.DataBindOptions();
		}

		private void optionsGrid_CancelCommand( object source, DataGridCommandEventArgs e )
		{
			optionsGrid.EditItemIndex = -1;
			this.DataBindOptions();
		}

		private void optionsGrid_UpdateCommand( object source, DataGridCommandEventArgs e )
		{
			AuthorizeChange();

			if ( !Page.IsValid )
			{
				return;
			}

			Int32 optionID = Convert.ToInt32( optionsGrid.DataKeys[e.Item.ItemIndex], System.Globalization.CultureInfo.InvariantCulture );
			String optionText = ( (TextBox)e.Item.Cells[this.optionsGridTextColumnIndex].Controls[0] ).Text;
			String displayOrder = ( (TextBox)e.Item.Cells[this.optionsGridDisplayOrderColumnIndex].Controls[1] ).Text;
			Int32 displayOrderInt = GetInt( displayOrder );

			Polling.UpdateOption( optionID, this.CurrentPollID, optionText, displayOrderInt );

			optionsGrid.EditItemIndex = -1;
			this.DataBindOptions();
			this.OnPollDataChanged( EventArgs.Empty );
		}

		private void optionsGrid_DeleteCommand( object source, DataGridCommandEventArgs e )
		{
			AuthorizeChange();

			Int32 optionID = Convert.ToInt32( optionsGrid.DataKeys[e.Item.ItemIndex], System.Globalization.CultureInfo.InvariantCulture );
			Polling.DeleteOption( optionID );

			optionsGrid.EditItemIndex = -1;
			this.DataBindOptions();
			this.OnPollDataChanged( EventArgs.Empty );
		}

		private void AddNewOption_Click( object sender, EventArgs e )
		{
			AuthorizeChange();

			if ( !Page.IsValid )
			{
				return;
			}

			Int32 order = GetInt( this.NewDisplayOrder.Text );
			Polling.InsertOption( this.CurrentPollID, this.NewOptionText.Text, order );
			
			this.DataBindOptions();
			this.OnPollDataChanged( EventArgs.Empty );
			
		}

		private static Int32 GetInt( String value )
		{
			Int32 iValue;
			if ( Int32.TryParse( value, out iValue ) )
			{
				return iValue;
			}
			else
			{
				return 0;
			}
		}

		private Int32 optionsGridDeleteColumnIndex = 1;
		private Int32 optionsGridTextColumnIndex = 2;
		private Int32 optionsGridDisplayOrderColumnIndex = 3;

		#endregion

		#region Templates

		private class PollEditingTemplate : ITemplate
		{
			public void InstantiateIn( Control container )
			{

				Table main = new Table();
				main.Width = Unit.Percentage( 100 );
				container.Controls.Add( main );

				#region ID Row
				TableRow idRow = new TableRow();
				TableCell idLabel = new TableCell();
				TableCell idValue = new TableCell();
				main.Rows.Add( idRow );
				idRow.Cells.Add( idLabel );
				idRow.Cells.Add( idValue );

				idLabel.HorizontalAlign = HorizontalAlign.Right;
				idLabel.Wrap = false;

				Label pollIdLabel = new Label();
				pollIdLabel.ID = "PollIDLabel";
				idLabel.Controls.Add( pollIdLabel );

				Label pollId = new Label();
				pollId.ID = "PollID";
				pollId.DataBinding += new EventHandler( pollId_DataBinding );
				idValue.Controls.Add( pollId );
				#endregion

				AddTextBoxRow( main, "Text", true );
				AddTextBoxRow( main, "StartDate", false );

				#region VoteMode Row
				TableRow row = new TableRow();
				TableCell labelCell = new TableCell();
				TableCell editCell = new TableCell();
				main.Rows.Add( row );
				row.Cells.Add( labelCell );
				row.Cells.Add( editCell );

				labelCell.HorizontalAlign = HorizontalAlign.Right;
				labelCell.Wrap = false;

				Label VoteModeLabel = new Label();
				VoteModeLabel.ID = "VoteModeLabel";
				labelCell.Controls.Add( VoteModeLabel );

				DropDownList voteModeEditor = new DropDownList();
				voteModeEditor.ID = "VoteMode";
				voteModeEditor.Items.Add( "Single" );
				voteModeEditor.Items.Add( "Multiple" );
				voteModeEditor.Items.Add( "Rating" );
				voteModeEditor.DataBinding += new EventHandler( voteModeEditor_DataBinding );
				editCell.Controls.Add( voteModeEditor );
				#endregion

				AddCheckBoxRow( main, "Enabled" );
				AddCheckBoxRow( main, "AllowWriteIns" );
				AddTextBoxRow( main, "Category", false );

				#region Button Row
				TableRow buttonRow = new TableRow();
				TableCell buttonHolder = new TableCell();

				main.Rows.Add( buttonRow );
				buttonRow.Cells.Add( buttonHolder );

				buttonHolder.ColumnSpan = 2;
				buttonHolder.Controls.Add( new LiteralControl( "<br />" ) );

				AddButton( buttonHolder, "UpdatePoll", "Update" );
				buttonHolder.Controls.Add( new LiteralControl( " " ) );
				AddButton( buttonHolder, "ClearPoll", "Clear" );
				buttonHolder.Controls.Add( new LiteralControl( " " ) );

				Button deleter = new Button();
				deleter.CommandName = "Delete";
				deleter.ID = "DeletePoll";
				deleter.CausesValidation = false;
				buttonHolder.Controls.Add( deleter );

				buttonRow = new TableRow();
				buttonHolder = new TableCell();
				main.Rows.Add( buttonRow );
				buttonRow.Cells.Add( buttonHolder );
				buttonHolder.ColumnSpan = 2;
				#endregion

			}

			private static void AddButton( TableCell buttonHolder, String buttonID, String commandName )
			{
				Button smack = new Button();
				smack.ID = buttonID;
				smack.CommandName = commandName;
				smack.CausesValidation = false;
				buttonHolder.Controls.Add( smack );
			}

			private void AddTextBoxRow( Table main, String dataField, Boolean multiLine )
			{
				TableRow row = new TableRow();
				TableCell labelCell = new TableCell();
				TableCell editCell = new TableCell();

				main.Rows.Add( row );
				row.Cells.Add( labelCell );
				row.Cells.Add( editCell );

				labelCell.HorizontalAlign = HorizontalAlign.Right;
				labelCell.Wrap = false;

				Label itemLabel = new Label();
				itemLabel.ID = dataField + "Label";
				labelCell.Controls.Add( itemLabel );

				TextBox textEditor = new TextBox();
				textEditor.ID = dataField;
				if ( multiLine )
				{
					textEditor.TextMode = TextBoxMode.MultiLine;
				}
				textEditor.Attributes["dataField"] = dataField;
				textEditor.DataBinding += new EventHandler( textEditor_DataBinding );
				editCell.Controls.Add( textEditor );
			}

			private void textEditor_DataBinding( object sender, EventArgs e )
			{
				TextBox editor = sender as TextBox;
				DataGridItem Container = editor.NamingContainer as DataGridItem;
				editor.Text = DataBinder.Eval( Container.DataItem, editor.Attributes["dataField"] ).ToString();
				editor.Attributes.Remove( "dataField" );
			}

			private void AddCheckBoxRow( Table main, String dataField )
			{
				TableRow row = new TableRow();
				TableCell labelCell = new TableCell();
				TableCell editCell = new TableCell();

				main.Rows.Add( row );
				row.Cells.Add( labelCell );
				row.Cells.Add( editCell );


				labelCell.HorizontalAlign = HorizontalAlign.Right;
				labelCell.Wrap = false;

				Label itemLabel = new Label();
				itemLabel.ID = dataField + "Label";
				labelCell.Controls.Add( itemLabel );

				CheckBox editor = new CheckBox();
				editor.ID = dataField;
				editor.Attributes["dataField"] = dataField;
				editor.DataBinding += new EventHandler( checkEditor_DataBinding );
				editCell.Controls.Add( editor );
			}

			private void checkEditor_DataBinding( object sender, EventArgs e )
			{
				CheckBox editor = sender as CheckBox;
				DataGridItem Container = editor.NamingContainer as DataGridItem;
				editor.Checked = Convert.ToBoolean( DataBinder.Eval( Container.DataItem, editor.Attributes["dataField"] ), System.Globalization.CultureInfo.InvariantCulture );
				editor.Attributes.Remove( "dataField" );
			}

			private void voteModeEditor_DataBinding( object sender, EventArgs e )
			{
				DropDownList editor = sender as DropDownList;
				DataGridItem Container = editor.NamingContainer as DataGridItem;
				editor.SelectedValue = DataBinder.Eval( Container.DataItem, "VoteMode" ).ToString();
			}

			private void pollId_DataBinding( object sender, EventArgs e )
			{
				Label editor = sender as Label;
				DataGridItem Container = editor.NamingContainer as DataGridItem;
				editor.Text = DataBinder.Eval( Container.DataItem, "PollID" ).ToString();
			}
		}

		private class DisplayOrderViewTemplate : ITemplate
		{
			public void InstantiateIn( Control container )
			{
				Label viewer = new Label();
				viewer.Attributes["dataField"] = "DisplayOrder";
				viewer.DataBinding += new EventHandler( viewer_DataBinding );
				container.Controls.Add( viewer );
			}

			private void viewer_DataBinding( object sender, EventArgs e )
			{
				Label viewer = sender as Label;
				DataGridItem Container = viewer.NamingContainer as DataGridItem;
				viewer.Text = DataBinder.Eval( Container.DataItem, viewer.Attributes["dataField"] ).ToString();
				viewer.Attributes.Remove( "dataField" );
			}
		}

		private class DisplayOrderEditTemplate : ITemplate
		{
			public void InstantiateIn( Control container )
			{
				TextBox editor = new TextBox();
				editor.ID = "DisplayOrder";
				editor.Width = Unit.Parse( "3em", CultureInfo.InvariantCulture );
				editor.Style["text-align"] = "right";
				editor.Attributes["dataField"] = "DisplayOrder";
				editor.ValidationGroup = "MetaBuilders_WebControls_PollManager__EditOption";
				editor.DataBinding += new EventHandler( editor_DataBinding );

				CompareValidator val = new CompareValidator();
				val.ControlToValidate = "DisplayOrder";
				val.ValidationGroup = "MetaBuilders_WebControls_PollManager__EditOption";
				val.Operator = ValidationCompareOperator.DataTypeCheck;
				val.Type = ValidationDataType.Integer;
				val.Text = Resources.PollManager_Validation_DisplayOrderMustBeNumber;
				val.SetFocusOnError = true;
				val.Display = ValidatorDisplay.Dynamic;

				container.Controls.Add( val );
				container.Controls.Add( editor );
			}

			private void editor_DataBinding( object sender, EventArgs e )
			{
				TextBox editor = sender as TextBox;
				DataGridItem Container = editor.NamingContainer as DataGridItem;
				editor.Text = DataBinder.Eval( Container.DataItem, editor.Attributes["dataField"] ).ToString();
				editor.Attributes.Remove( "dataField" );
			}
		}


		#endregion

		#region Helpers

		private void AuthorizeChange()
		{
			HttpContext context = HttpContext.Current;
			if ( this.RequiredManagementRole.Length > 0 && context != null )
			{
				if ( !context.Request.IsAuthenticated || !context.User.IsInRole( this.RequiredManagementRole ) )
				{
					throw new System.Security.SecurityException( String.Format( System.Globalization.CultureInfo.InvariantCulture, Resources.PollManager_UnauthorizedUser, HttpContext.Current.User.Identity.Name ) );
				}
			}
		}
		
		private void EnsureOptionEditorButtonText()
		{
			if ( this.Page == null )
			{
				return;
			}

			if ( this.optionsGrid != null )
			{

				EditCommandColumn editorColumn = this.optionsGrid.Columns[0] as EditCommandColumn;
				editorColumn.EditText = this.OptionEditButtonText;
				editorColumn.UpdateText = this.OptionUpdateButtonText;
				editorColumn.CancelText = this.OptionCancelButtonText;
				ButtonColumn deleteColumn = this.optionsGrid.Columns[this.optionsGridDeleteColumnIndex] as ButtonColumn;
				deleteColumn.Text = this.OptionDeleteButtonText;

				if ( this.optionsGrid.DataSource != null )
				{
					this.optionsGrid.DataBind();
				}
				else
				{
					if ( this.CurrentPollID != -1 )
					{
						this.DataBindOptions();
						this.Page.Trace.Write( "Rebinding Options For Change In Text" );
					}
				}
			}
		}

		#endregion

		#region Child Controls

		private PlaceHolder editorInterface;
		private PlaceHolder addInterface;
		private PlaceHolder optionEditorInterface;

		private PollManagerArea pollFieldSet;
		private PollManagerArea optionEditorFieldSet;
		private PollManagerArea newOptionFieldSet;
		private PollManagerArea filterFieldSet;
		private PollManagerArea newPollFieldSet;

		private Label EmptyPollGridLabel;

		private Label NewPollTextLabel;
		private Label NewPollStartDateLabel;
		private Label NewPollVoteModeLabel;
		private Label NewPollEnabledLabel;
		private Label NewPollAllowWriteInsLabel;
		private Label NewPollCategoryLabel;
		private TextBox NewPollText;
		private TextBox NewPollStartDate;
		private DropDownList NewPollVoteMode;
		private CheckBox NewPollEnabled;
		private CheckBox NewPollAllowWriteIns;
		private TextBox NewPollCategory;

		private Label NewOptionTextLabel;
		private Label NewOptionDisplayOrderLabel;
		private TextBox NewOptionText;
		private TextBox NewDisplayOrder;
		private Button AddNewOption;

		private DataGrid pollGrid;
		private DataGrid optionsGrid;

		private Button AddNewPoll;

		private Label PollFilterTextLabel;
		private Label PollFilterCategoryLabel;
		private Label PollFilterActiveDatesLabel;
		private TextBox PollFilterText;
		private TextBox PollFilterCategory;
		private TextBox PollFilterBeginDate;
		private TextBox PollFilterEndDate;
		private Button PollFilterApply;

		private Button NewPollAdd;
		private Button NewPollCancel;

		#endregion

	}
}
