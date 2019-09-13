using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

namespace MetaBuilders.WebControls
{
	/// <summary>
	/// This radio button is much like the built in <see cref="RadioButton"/> control, but unlike it, has a <see cref="GroupName"/> that can be global, ignoring naming containers.
	/// </summary>
	/// <remarks>
	/// Be <b>very careful</b> when using this control, 
	/// as it is quite easy to break other controls on the page 
	/// because of the disrespect for naming containers in the "name" attribute of the rendered html input.
	/// </remarks>
	[
	Description( "A RadioButton with a global GroupName" ),
	DefaultEvent( "CheckedChanged" ),
	DefaultProperty( "Text" ),
	ControlValueProperty( "Checked" ),
	DataBindingHandler( typeof( TextDataBindingHandler ) ),
	Designer( typeof( MetaBuilders.WebControls.Design.GlobalRadioButtonDesigner ) ),
	SupportsEventValidation,
	]
	public class GlobalRadioButton : WebControl, IPostBackDataHandler, ICheckBoxControl
	{

		/// <summary>
		/// Creates a new instance of the GlobalRadioButton class.
		/// </summary>
		public GlobalRadioButton()
			: base( HtmlTextWriterTag.Input )
		{
		}

		#region Events

		private static readonly Object EventCheckedChanged = new Object();

		/// <summary>
		/// Raises the <see cref="CheckedChanged"/> event.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected virtual void OnCheckedChanged( EventArgs e )
		{
			EventHandler ecc = (EventHandler)this.Events[ GlobalRadioButton.EventCheckedChanged ];
			if ( ecc != null )
				ecc.Invoke( this, e );
		}

		/// <summary>
		/// The event that occurs when the checked property has changed.
		/// </summary>
		public event EventHandler CheckedChanged
		{
			add
			{
				this.Events.AddHandler( GlobalRadioButton.EventCheckedChanged, value );
			}
			remove
			{
				this.Events.AddHandler( GlobalRadioButton.EventCheckedChanged, value );
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether the state automatically posts back to the server when clicked.
		/// </summary>
		[
		Description( "Gets or sets a value indicating whether the state automatically posts back to the server when clicked." ),
		Category( "Behavior" ),
		Themeable( false ),
		DefaultValue( false ),
		]
		public virtual Boolean AutoPostBack
		{
			get
			{
				Object savedState = this.ViewState[ "AutoPostBack" ];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState[ "AutoPostBack" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether validation is performed when the GlobalRadioButton control is clicked.
		/// </summary>
		[
		Description( "Whether the control causes validation to fire." ),
		Category( "Behavior" ),
		Themeable( false ),
		DefaultValue( false ),
		]
		public virtual Boolean CausesValidation
		{
			get
			{
				object savedState = this.ViewState[ "CausesValidation" ];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState[ "CausesValidation" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is checked.
		/// </summary>
		[
		Description( "Gets or sets a value indicating whether the control is checked." ),
		Category( "Appearance" ),
		Themeable( false ),
		Bindable( true, BindingDirection.TwoWay ),
		DefaultValue( false ),
		]
		public virtual Boolean Checked
		{
			get
			{
				Object savedState = this.ViewState[ "Checked" ];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState[ "Checked" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text label associated with the control.
		/// </summary>
		[
		Description( "Gets or sets the text label associated with the control." ),
		Category( "Appearance" ),
		Bindable( true ),
		Localizable( true ),
		DefaultValue( "" ),
		]
		public virtual String Text
		{
			get
			{
				Object savedState = this.ViewState[ "Text" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return String.Empty;
			}
			set
			{
				this.ViewState[ "Text" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the alignment of the text label associated with the control.
		/// </summary>
		[
		Description( "Gets or sets the alignment of the text label associated with the control." ),
		Category( "Appearance" ),
		DefaultValue( TextAlign.Right ),
		]
		public virtual TextAlign TextAlign
		{
			get
			{
				Object savedState = this.ViewState[ "TextAlign" ];
				if ( savedState != null )
				{
					return (TextAlign)savedState;
				}
				return TextAlign.Right;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( System.Web.UI.WebControls.TextAlign ), value ) )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}
				this.ViewState[ "TextAlign" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the group that the radio button belongs to.
		/// </summary>
		[
		Description( "Gets or sets the name of the group that the radio button belongs to." ),
		Category( "Behavior" ),
		Themeable( false ),
		DefaultValue( "" ),
		]
		public virtual String GroupName
		{
			get
			{
				Object savedState = this.ViewState[ "GroupName" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return String.Empty;
			}
			set
			{
				this.ViewState[ "GroupName" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the GroupName will span across naming containers on the page.
		/// </summary>
		/// <remarks>
		/// Set this property to true to enable the "global" functionality of the control.
		/// Set this property to false to make it behave like a normal RadioButton.
		/// </remarks>
		[
		Description( "Gets or sets if the GroupName will span across naming containers on the page." ),
		Category( "Behavior" ),
		Themeable( false ),
		DefaultValue( true ),
		]
		public virtual Boolean GlobalGroup
		{
			get
			{
				Object savedState = this.ViewState[ "GlobalGroup" ];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return true;
			}
			set
			{
				this.ViewState[ "GlobalGroup" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether validation is performed when the CheckBox control is clicked.
		/// </summary>
		[
		Description( "The group that should be validated when the control causes a postback." ),
		Category( "Behavior" ),
		Themeable( false ),
		DefaultValue( "" ),
		]
		public virtual string ValidationGroup
		{
			get
			{
				string text1 = (string)this.ViewState[ "ValidationGroup" ];
				if ( text1 != null )
				{
					return text1;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState[ "ValidationGroup" ] = value;
			}
		}

		/// <summary>
		/// Gets the group name for the control as it will exist in the name attribute of the html.
		/// </summary>
		/// <exclude/>
		protected virtual String UniqueGroupName
		{
			get
			{
				String groupName = this.GroupName;
				if ( !this.GlobalGroup )
				{
					String uniqueID = this.UniqueID;
					Int32 indexOfColon = uniqueID.LastIndexOf( ':' );
					if ( indexOfColon >= 0 )
					{
						groupName = uniqueID.Substring( 0, indexOfColon + 1 ) + groupName;
					}

				}
				return groupName;
			}
		}

		/// <summary>
		/// Gets the content to be put in the html value attribute.
		/// </summary>
		/// <exclude/>
		protected virtual String ValueAttribute
		{
			get
			{
				String value = this.Attributes[ "value" ];
				if ( value == null )
				{
					value = this.UniqueID;
				}
				return value;
			}
		}

		#endregion

		#region Lifecycle

		/// <summary>
		/// Overrides the OnPreRender method.
		/// </summary>
		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#" )]
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );

			if ( this.Page != null && this.IsEnabled )
			{
				this.Page.RegisterRequiresPostBack( this );
				if ( this.AutoPostBack )
				{
					this.Page.ClientScript.GetPostBackEventReference( this, "" );
				}
			}

			if ( this.GroupName.Length == 0 )
			{
				this.GroupName = this.UniqueID;
			}

		}

		/// <summary>
		/// Overrides the Render method.
		/// </summary>
		/// <exclude/>
		protected override void Render( HtmlTextWriter writer )
		{

			if ( this.Page != null )
			{
				this.Page.VerifyRenderingInServerForm( this );
				this.Page.ClientScript.RegisterForEventValidation( this.UniqueGroupName, this.ValueAttribute );
			}

			Boolean doCreateSpan = false;

			if ( this.ControlStyleCreated )
			{
				if ( !( this.ControlStyle.IsEmpty ) )
				{
					this.ControlStyle.AddAttributesToRender( writer, this );
					doCreateSpan = true;
				}
			}
			if ( !( this.IsEnabled ) )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Disabled, "disabled" );
				doCreateSpan = true;
			}

			if ( this.ToolTip.Length > 0 )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Title, this.ToolTip );
				doCreateSpan = true;
			}

			if ( this.Attributes.Count != 0 )
			{
				String valueAttribute = this.Attributes[ "value" ];

				if ( valueAttribute != null )
				{
					this.Attributes.Remove( "value" );
				}
				if ( this.Attributes.Count != 0 )
				{
					this.Attributes.AddAttributes( writer );
					doCreateSpan = true;
				}
				if ( valueAttribute != null )
				{
					this.Attributes[ "value" ] = valueAttribute;
				}
			}

			if ( doCreateSpan )
			{
				writer.RenderBeginTag( HtmlTextWriterTag.Span );
			}

			if ( this.Text.Length != 0 )
			{
				if ( this.TextAlign == System.Web.UI.WebControls.TextAlign.Left )
				{
					this.RenderLabel( writer, this.Text, this.ClientID );
					this.RenderInputTag( writer, this.ClientID );
				}
				else
				{
					this.RenderInputTag( writer, this.ClientID );
					this.RenderLabel( writer, this.Text, this.ClientID );
				}
			}
			else
			{
				this.RenderInputTag( writer, this.ClientID );
			}

			if ( doCreateSpan )
			{
				writer.RenderEndTag();
			}
		}

		/// <summary>
		/// Renders the input tag portion of the control.
		/// </summary>
		/// <exclude/>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID" ), 
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "1#" ),
		]
		protected virtual void RenderInputTag( HtmlTextWriter writer, String clientID )
		{
			String onClick = this.GetClickAttribute();

			writer.AddAttribute( HtmlTextWriterAttribute.Id, clientID );
			writer.AddAttribute( HtmlTextWriterAttribute.Type, "radio" );
			writer.AddAttribute( HtmlTextWriterAttribute.Name, this.UniqueGroupName );
			writer.AddAttribute( HtmlTextWriterAttribute.Value, this.ValueAttribute );

			if ( this.Checked )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Checked, "checked" );
			}
			if ( !this.IsEnabled )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Disabled, "disabled" );
			}

			if ( this.AutoPostBack && !this.Checked )
			{
				PostBackOptions options = new PostBackOptions( this, "" );
				options.AutoPostBack = true;
				if ( this.CausesValidation )
				{
					options.PerformValidation = true;
					options.ValidationGroup = this.ValidationGroup;
				}
				onClick = onClick + " " + this.Page.ClientScript.GetPostBackEventReference( options );
				writer.AddAttribute( HtmlTextWriterAttribute.Onclick, onClick );
			}
			else if ( !String.IsNullOrEmpty( onClick ) )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Onclick, onClick );
			}


			if ( this.AccessKey.Length > 0 )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Accesskey, this.AccessKey );
			}

			if ( this.TabIndex != 0 )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Tabindex, this.TabIndex.ToString( System.Globalization.NumberFormatInfo.InvariantInfo ) );
			}

			writer.RenderBeginTag( HtmlTextWriterTag.Input );
			writer.RenderEndTag();

		}

		/// <summary>
		/// Renders the label portion of the control.
		/// </summary>
		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID" ), 
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "2#" )]
		protected virtual void RenderLabel( HtmlTextWriter writer, String text, String clientID )
		{
			writer.AddAttribute( HtmlTextWriterAttribute.For, clientID );
			writer.RenderBeginTag( HtmlTextWriterTag.Label );
			writer.Write( text );
			writer.RenderEndTag();
		}

		private String GetClickAttribute()
		{
			String result = "";
			if ( this.HasAttributes )
			{
				result = this.Attributes[ "onclick" ] ?? "";
				if ( !String.IsNullOrEmpty( result ) )
				{
					if ( !result.Trim().EndsWith( ";", StringComparison.Ordinal ) )
					{
						result += ";";
					}
					this.Attributes.Remove( "onclick" );
				}
			}
			return result;
		}

		#endregion

		#region Implementation of IPostBackDataHandler

		#region Explicit

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		Boolean IPostBackDataHandler.LoadPostData( String postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{
			return this.LoadPostData( postDataKey, postCollection );
		}

		#endregion

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		protected virtual void RaisePostDataChangedEvent()
		{
			if ( this.CausesValidation )
			{
				this.Page.Validate( this.ValidationGroup );
			}
			this.OnCheckedChanged( EventArgs.Empty );
		}

		/// <exclude/>
		protected virtual Boolean LoadPostData( String postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{
			String postedValue = postCollection[ this.UniqueGroupName ];
			Boolean thisValueWasPosted = ( postedValue != null && postedValue == this.ValueAttribute );
			Boolean isThisRadioNewlySelected = false;

			if ( thisValueWasPosted )
			{
				if ( this.Page != null )
				{
					this.Page.ClientScript.ValidateEvent( this.UniqueGroupName, postedValue );
				}
				if ( this.Checked )
				{
					return isThisRadioNewlySelected;
				}
				else
				{
					this.Checked = true;
					isThisRadioNewlySelected = true;
				}
			}
			else if ( this.Checked )
			{
				this.Checked = false;
			}

			// the result is that we only return true from LoadPostData when the radio is selected.
			// this is because return True means that we want the CheckedChanged event to fire,
			// but with a group of radio buttons, we only want it to fire once,
			// so only the radio which changes to selected fires its event.
			return isThisRadioNewlySelected;

		}

		#endregion

	}
}
