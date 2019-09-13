using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using MetaBuilders.WebControls.Design;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Represents a control that acts as a panel containing two alternately visible views.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Each template must contain at least one control which has a CommandName property and a Command event which bubbles up.
	/// The three built in controls which support this are <see cref="Button"/>, <see cref="ImageButton"/>, and <see cref="LinkButton"/>.
	/// </p>
	/// <p>
	/// In the ExpandedTemplate, the button should have a CommandName of "Contract", and should visually indicate that the button will switch to the contracted view.
	/// Set the <see cref="ExpandingPanel.ContractionControl"/> property to this control.
	/// </p>
	/// <p>
	/// In the ContractedTemplate, the button should have a CommandName of "Expand", and should visually indicate that the button will switch to the expanded view.
	/// Set the <see cref="ExpandingPanel.ExpansionControl"/> property to this control.
	/// </p>
	/// </remarks>
	/// <example>
	/// Here's an example of a simple ExpandingPanel.
	/// <code>
	/// <![CDATA[
	/// <%@ Page Language="C#" %>
	/// <%@ Register TagPrefix="mbexp" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.ExpandingPanel"%>
	/// <script runat="server">
	///     
	/// </script>
	/// <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
	/// <html>
	///     <head>
	///     </head>
	/// 	<body>
	/// 	<form runat="server">
	/// 
	///         <mbexp:ExpandingPanel runat="server" id="Exp" ExpansionControl="ExpandButton" ContractionControl="ContractButton" >
	/// 			<ExpandedTemplate>
	/// 				<table cellspacing="0" cellpadding="3" style="border:1px solid black;width:200px">
	/// 					<tr><td style="border:1px solid black;background:#aaaaff;" align="right"><asp:Button runat="server" Text="Contract!" CommandName="Contract" id="ContractButton"/></td></tr>
	/// 					<tr><td>This is what you see when the doodad is expanded</td></tr>
	/// 				</table>
	/// 			</ExpandedTemplate>
	/// 			<ContractedTemplate>
	/// 				<table cellspacing="0" cellpadding="3"  style="border:1px solid black;width:200px">
	/// 					<tr ><td style="border:1px solid black;background:#aaaaff;" align="right"><asp:Button runat="server" Text="Expand!" CommandName="Expand" id="ExpandButton" /></td></tr>
	/// 				</table>
	/// 			</ContractedTemplate>
	///         </mbexp:ExpandingPanel>
	///         
	/// 	</form>
	/// 	
	/// 	</body>
	/// </html>
	/// ]]>
	/// </code>
	/// </example>
	[
	Designer( typeof( ExpandingPanelDesigner ) )
	]
	public class ExpandingPanel : CompositeControl
	{

		/// <summary>
		/// The command signalling to expand the panel.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate" )]
		[Browsable( false )]
		public static readonly String ExpandCommand = "Expand";
		/// <summary>
		/// The command signalling to contract the panel.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate" )]
		[Browsable( false )]
		public static readonly String ContractCommand = "Contract";
		/// <summary>
		/// The name of the client script library giving easy access to keypair cookies.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate" )]
		[Browsable( false )]
		public static readonly String CookieJarScript = "CookieJarScript Modified KeyPair";

		#region public properties

		/// <summary>
		/// Gets or sets a value that indicates whether the panel control is expanded.
		/// </summary>
		[
		Description( "Gets or sets a value that indicates whether the panel control is expanded." ),
		Bindable( true ),
		DefaultValue( false ),
		Category( "Appearance" )
		]
		public virtual Boolean Expanded
		{
			get
			{
				object savedData;
				savedData = this.ViewState[ "Expanded" ];
				if ( savedData != null )
				{
					return (Boolean)savedData;
				}
				return false;
			}
			set
			{
				this.ViewState[ "Expanded" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the control which gives the Expand command.
		/// </summary>
		[
		Description( "Gets or sets the control which gives the Expand command." ),
		Bindable( true ),
		DefaultValue( "" ),
		Category( "Behavior" )
		]
		public virtual String ExpansionControl
		{
			get
			{
				object savedData;
				savedData = this.ViewState[ "ExpansionControl" ];
				if ( savedData != null )
				{
					return (String)savedData;
				}
				return "";
			}
			set
			{
				this.ViewState[ "ExpansionControl" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the control which gives the Contract command.
		/// </summary>
		[
		Description( "Gets or sets the control which gives the Contract command." ),
		Bindable( true ),
		DefaultValue( "" ),
		Category( "Behavior" )
		]
		public virtual String ContractionControl
		{
			get
			{
				object savedData;
				savedData = this.ViewState[ "ContractionControl" ];
				if ( savedData != null )
				{
					return (String)savedData;
				}
				return "";
			}
			set
			{
				this.ViewState[ "ContractionControl" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Web.UI.ITemplate"/> that defines how the expanded view is rendered.
		/// </summary>
		[
		Browsable( false ),
		DefaultValue( null ),
		Description( "Gets or sets the System.Web.UI.ITemplate that defines how the expanded view is rendered." ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		TemplateInstance( TemplateInstance.Single ),
		]
		public ITemplate ExpandedTemplate
		{
			get
			{
				return this.expandedTemplate;
			}
			set
			{
				this.expandedTemplate = value;
				this.ChildControlsCreated = false;
				this.EnsureChildControls();
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Web.UI.ITemplate"/> that defines how the contracted view is rendered.
		/// </summary>
		[
		Browsable( false ),
		DefaultValue( null ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		Description( "Gets or sets the System.Web.UI.ITemplate that defines how the contracted view is rendered." ),
		TemplateInstance( TemplateInstance.Single ),
		]
		public ITemplate ContractedTemplate
		{
			get
			{
				return this.contractedTemplate;
			}
			set
			{
				this.contractedTemplate = value;
				this.ChildControlsCreated = false;
				this.EnsureChildControls();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="ExpandingPanel"/> is switchable on the client-side.
		/// </summary>
		[
		Description( "Gets or sets a value indicating whether the ExpandingPanel is switchable on the client-side." ),
		Bindable( true ),
		DefaultValue( true ),
		Category( "Behavior" )
		]
		public virtual Boolean EnableClientScript
		{
			get
			{
				object savedData;
				savedData = this.ViewState[ "EnableClientScript" ];
				if ( savedData != null )
				{
					return (Boolean)savedData;
				}
				return true;
			}
			set
			{
				this.ViewState[ "EnableClientScript" ] = value;
			}
		}

		private Boolean EnableClientScriptInternal
		{
			get
			{
				return this.EnableClientScript && BrowserSupportsScript;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		private Boolean BrowserSupportsScript
		{
			get
			{
				if ( HttpContext.Current != null )
				{
					HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
					if ( String.Compare( browser.Browser, "opera", StringComparison.OrdinalIgnoreCase ) == 0 )
					{
						return false;
					}

				}
				return true;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="ExpandingPanel"/> stores its expanded state with a cookie.
		/// </summary>
		[
		Description( "Gets or sets a value indicating whether the ExpandingPanel stores its expanded state with a cookie." ),
		Bindable( true ),
		DefaultValue( true ),
		Category( "Behavior" )
		]
		public virtual Boolean EnableCookieState
		{
			get
			{
				object savedData;
				savedData = this.ViewState[ "EnableCookieState" ];
				if ( savedData != null )
				{
					return (Boolean)savedData;
				}
				return true;
			}
			set
			{
				this.ViewState[ "EnableCookieState" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the length of time which the expanded state cookie exists on the client.
		/// </summary>
		/// <remarks>When EnabledCookieState is set to false, this property has no effect.</remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LifeSpan" ), Description( "Gets or sets the length of time which the expanded state cookie exists on the client." ),
		Bindable( true ),
		Category( "Behavior" )
		]
		public virtual TimeSpan CookieStateLifeSpan
		{
			get
			{
				object savedData;
				savedData = this.ViewState[ "CookieStateLifeSpan" ];
				if ( savedData != null )
				{
					return (TimeSpan)savedData;
				}
				return TimeSpan.FromDays( 30 );
			}
			set
			{
				this.ViewState[ "CookieStateLifeSpan" ] = value;
			}
		}
		/// <summary>
		/// Determines if the <see cref="CookieStateLifeSpan"/> property is not the default value.
		/// </summary>
		/// <returns>true, if it is not default.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LifeSpan" )]
		protected virtual Boolean ShouldSerializeCookieStateLifeSpan()
		{
			return !( this.CookieStateLifeSpan == TimeSpan.FromDays( 30 ) );
		}

		#endregion

		#region Render Path

		/// <summary>
		/// Overrides <see cref="Control.OnLoad"/>.
		/// </summary>
		protected override void OnLoad( System.EventArgs e )
		{
			if ( this.EnableCookieState && this.Page != null && !this.Page.IsPostBack )
			{
				this.LoadStateCookie();
			}
			base.OnLoad( e );
		}
		
		

		/// <summary>
		/// Overrides <see cref="Control.CreateChildControls"/>.
		/// </summary>
		protected override void CreateChildControls()
		{
			this.Controls.Clear();

			expandedContainer = new Panel();
			expandedContainer.ID = expandedContainerId;
			if ( this.expandedTemplate != null )
			{
				this.expandedTemplate.InstantiateIn( expandedContainer );
			}
			this.Controls.Add( expandedContainer );

			contractedContainer = new Panel();
			contractedContainer.ID = contractedContainerId;
			if ( this.contractedTemplate != null )
			{
				this.contractedTemplate.InstantiateIn( contractedContainer );
			}
			this.Controls.Add( contractedContainer );

			tracker = new HtmlInputHidden();
			tracker.Name = trackerId;
			tracker.ID = trackerId;
			tracker.ServerChange += new EventHandler( this.ClientExpanedStateChanged );
			this.Controls.Add( tracker );
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>
		/// </summary>
		protected override void OnPreRender( System.EventArgs e )
		{
			base.OnPreRender( e );

			this.expandedContainer.MergeStyle( this.ControlStyle );
			this.contractedContainer.MergeStyle( this.ControlStyle );
			this.tracker.Value = this.Expanded.ToString();

			this.SetPanelVisibility( this.expandedContainer, this.Expanded );
			this.SetPanelVisibility( this.contractedContainer, !this.Expanded );
			if ( this.EnableClientScriptInternal )
			{
				this.RegisterClientScript();
				this.tracker.Visible = true;
			}
			else
			{
				this.tracker.Visible = false;
			}
			if ( this.EnableCookieState && this.Page != null )
			{
				this.SaveStateCookie();
			}

			ControlPropertiesValid();
			this.preRenderCalled = true;
		}

		/// <summary>
		/// Sets the visibility of of the given panel.
		/// </summary>
		/// <param name="container">The <see cref="Panel"/> to change.</param>
		/// <param name="visibility">The visibility value to use.</param>
		/// <remarks>
		/// This will either set the serverside or clientside visibility,
		/// depending on settings and browser.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		protected virtual void SetPanelVisibility( Panel container, Boolean visibility )
		{
			if ( this.EnableClientScriptInternal )
			{
				if ( visibility )
				{
					container.Style[ "display" ] = "block";
				}
				else
				{
					container.Style[ "display" ] = "none";
				}
			}
			else
			{
				container.Visible = visibility;
			}
		}

		/// <summary>
		/// Overrides <see cref="WebControl.Render"/>.
		/// </summary>
		protected override void Render( System.Web.UI.HtmlTextWriter writer )
		{
			if ( !this.preRenderCalled )
			{
				this.EnsureChildControls();
				this.expandedContainer.MergeStyle( this.ControlStyle );
				this.contractedContainer.MergeStyle( this.ControlStyle );
				this.SetPanelVisibility( this.expandedContainer, this.Expanded );
				this.SetPanelVisibility( this.contractedContainer, !this.Expanded );
			}

			if ( HttpContext.Current != null )
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				if ( ( String.Compare( browser.Browser, "netscape", StringComparison.OrdinalIgnoreCase ) == 0 ) && ( browser.MajorVersion > 4 ) )
				{
					this.requiresContainer = true;
				}
			}
			base.Render( writer );
		}

		/// <summary>
		/// Overrides <see cref="WebControl.RenderBeginTag"/>.
		/// </summary>
		/// <param name="writer"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override void RenderBeginTag( System.Web.UI.HtmlTextWriter writer )
		{
			if ( this.requiresContainer )
			{
				writer.Write( "<div>" );
			}
		}

		/// <summary>
		/// Overrides <see cref="WebControl.RenderEndTag"/>.
		/// </summary>
		/// <param name="writer"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override void RenderEndTag( System.Web.UI.HtmlTextWriter writer )
		{
			if ( this.requiresContainer )
			{
				writer.Write( "</div>" );
			}
		}
		
		#endregion

		#region State Cookie
		/// <summary>
		/// Loads state from the state cookie.
		/// </summary>
		protected virtual void LoadStateCookie()
		{
			HttpCookie cookie = this.Page.Request.Cookies[ "ExpandingPanel" ];
			if ( cookie != null && cookie.HasKeys )
			{
				String savedState = cookie.Values[ this.ID ];
				if ( !String.IsNullOrEmpty( savedState ) )
				{
					Boolean expandedState;
					if ( Boolean.TryParse( savedState, out expandedState ) )
					{
						this.Expanded = expandedState;
					}
				}
			}
		}

		/// <summary>
		/// Saves state to the state cookie.
		/// </summary>
		protected virtual void SaveStateCookie()
		{
			HttpCookie cookie = this.Page.Request.Cookies[ "ExpandingPanel" ];
			if ( cookie == null )
			{
				cookie = new HttpCookie( "ExpandingPanel" );
			}
			cookie.Values[ this.ID ] = this.Expanded.ToString();
			cookie.Expires = System.DateTime.Now.Add( this.CookieStateLifeSpan );
			cookie.Path = this.Page.Request.ApplicationPath;
			this.Page.Response.Cookies.Add( cookie );
		}
		#endregion

		#region Action Handling
		/// <summary>
		/// Overrides <see cref="Control.OnBubbleEvent"/>.
		/// </summary>
		/// <remarks>
		/// Catches the Expand and Contract commands from their respective templates.
		/// </remarks>
		protected override bool OnBubbleEvent( object source, EventArgs args )
		{

			Boolean result = false;
			CommandEventArgs ce = args as CommandEventArgs;
			if ( ce != null )
			{

				if ( String.Compare( ce.CommandName, ExpandingPanel.ExpandCommand, StringComparison.OrdinalIgnoreCase ) == 0 )
				{
					this.Expanded = true;
					result = true;
				}

				if ( String.Compare( ce.CommandName, ExpandingPanel.ContractCommand, StringComparison.OrdinalIgnoreCase ) == 0 )
				{
					this.Expanded = false;
					result = true;
				}

			}
			return result;
		}

		/// <summary>
		/// when clientside rendering is enabled, this will alert the control to clientside changes in the Expanded state.
		/// </summary>
		private void ClientExpanedStateChanged( Object sender, EventArgs e )
		{
			Boolean newValue;
			if ( Boolean.TryParse( tracker.Value, out newValue ) )
			{
				this.Expanded = newValue;
			}
		}

		#endregion

		#region Property Validation

		/// <summary>
		/// Ensures that the current properties are valid.
		/// </summary>
		protected virtual Boolean PropertiesValid
		{
			get
			{
				if ( !( this.propertiesChecked ) )
				{
					this.propertiesValid = this.ControlPropertiesValid();
					this.propertiesChecked = true;
				}
				return this.propertiesValid;
			}
		}

		/// <summary>
		/// Examines the current properties to ensure their validity.
		/// </summary>
		protected virtual bool ControlPropertiesValid()
		{
			if ( String.IsNullOrEmpty( this.ExpansionControl ) )
			{
				throw new HttpException( Resources.ExpandingPanel_ExpansionControlMustBeSet );
			}
			this.CheckActionControlProperty( this.ExpansionControl, ExpandingPanel.ExpandCommand );

			if ( String.IsNullOrEmpty( this.ContractionControl ) )
			{
				throw new HttpException( Resources.ExpandingPanel_ContractionControlMustBeSet );
			}
			this.CheckActionControlProperty( this.ContractionControl, ExpandingPanel.ContractCommand );

			return true;
		}

		/// <summary>
		/// Finds the given control anywhere within the control.
		/// </summary>
		private Control FindControlInContainers( String controlName )
		{

			Control control;

			control = this.NamingContainer.FindControl( controlName );
			if ( control != null )
			{
				return control;
			}
			control = this.FindControl( controlName );
			if ( control != null )
			{
				return control;
			}
			control = this.expandedContainer.FindControl( controlName );
			if ( control != null )
			{
				return control;
			}
			control = this.contractedContainer.FindControl( controlName );
			if ( control != null )
			{
				return control;
			}
			return null;
		}

		/// <summary>
		/// Makes sure the control exists and the command is correct.
		/// </summary>
		protected virtual void CheckActionControlProperty( String controlId, String commandName )
		{
			Control target = this.FindControlInContainers( controlId );
			if ( target == null )
			{
				throw new HttpException( String.Format( CultureInfo.InvariantCulture, Resources.ControlCannotBeFound, controlId ) );
			}

			String discoveredCommandName = "";

			IButtonControl targetButton = target as IButtonControl;
			if ( targetButton == null )
			{

				PropertyDescriptor commandProperty = TypeDescriptor.GetProperties( target ).Find( "CommandName", true );
				if ( commandProperty == null )
				{
					throw new HttpException( String.Format( CultureInfo.InvariantCulture, Resources.ExpandingPanel_ControlMissingCommandNameProperty, controlId ) );
				}

				discoveredCommandName = Convert.ToString( commandProperty.GetValue( target ), CultureInfo.InvariantCulture );
			}
			else
			{
				discoveredCommandName = targetButton.CommandName;
			}


			if ( String.IsNullOrEmpty( discoveredCommandName ) )
			{
				throw new HttpException( Resources.CommandNameCannotBeNull );
			}

			if ( String.Compare( discoveredCommandName, commandName, StringComparison.OrdinalIgnoreCase ) != 0 )
			{
				throw new HttpException( String.Format( CultureInfo.InvariantCulture, Resources.CommandNamesDoNotMatch, discoveredCommandName, commandName ) );
			}

		}
		
		#endregion

		#region Client Script

		/// <summary>
		/// Registers the script for clientside behavior.
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			ClientScriptManager script = this.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( ExpandingPanel ), "MetaBuilders.WebControls.Embedded.ExpandingPanelScript.js" );
			if ( this.EnableCookieState )
			{
				script.RegisterClientScriptResource( typeof( ExpandingPanel ), "MetaBuilders.WebControls.Embedded.CookieJarScript.js" );
			}

			String arrayDeclaration =
				"{ ExpansionID:'"
				+ this.GetControlRenderID( this.ExpansionControl ) + "', "
				+ " ContractionID:'"
				+ this.GetControlRenderID( this.ContractionControl ) + "', "
				+ " ExpandedContainerID:'"
				+ this.expandedContainer.ClientID + "', "
				+ " ContractedContainerID:'"
				+ this.contractedContainer.ClientID + "', "
				+ " TrackerID:'"
				+ this.tracker.ClientID + "', "
				+ " AppPath:'"
				+ this.Page.Request.ApplicationPath + "', "
				+ " CookieLife:'"
				+ System.DateTime.Now.Add( this.CookieStateLifeSpan ).ToString( "D", DateTimeFormatInfo.InvariantInfo ) + "', "
				+ " PanelID:'"
				+ this.ID
				+ "' }";
			script.RegisterArrayDeclaration( "MetaBuilders_ExpandingPanels", arrayDeclaration );
			script.RegisterStartupScript( typeof( ExpandingPanel ), "Startup", "MetaBuilders_ExpandingPanel_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_ExpandingPanel_Init" ), true );
		}

		/// <summary>
		/// Gets the full <see cref="Control.ClientID"/> for the given control name.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member" )]
		protected virtual String GetControlRenderID( String controlName )
		{
			Control control = this.FindControlInContainers( controlName );
			if ( control != null )
			{
				return control.ClientID;
			}
			return "";
		}

		#endregion

		#region Private

		private Boolean requiresContainer = false;
		private HtmlInputHidden tracker;
		private Boolean propertiesValid;
		private Boolean propertiesChecked;
		private Panel expandedContainer;
		private Panel contractedContainer;
		private ITemplate expandedTemplate;
		private ITemplate contractedTemplate;
		private Boolean preRenderCalled = false;

		private static String expandedContainerId = "expandedContainer";
		private static String contractedContainerId = "contractedContainer";
		private static String trackerId = "tracker";
		#endregion

	}
}
