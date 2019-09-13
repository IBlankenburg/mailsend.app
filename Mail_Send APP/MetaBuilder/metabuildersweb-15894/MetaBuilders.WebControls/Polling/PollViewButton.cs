using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A button which is shown on the PollView control.
	/// </summary>
	/// <remarks>
	/// This button is used to display a button which can be configured to be either a linkbutton or a pushbutton.
	/// By default, it is a pushbutton.
	/// </remarks>
	internal class PollViewButton : WebControl, IPostBackEventHandler
	{

		#region Properties

		/// <summary>
		/// Gets or sets the text caption displayed in the PollViewButton control.
		/// </summary>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the text caption displayed in the PollViewButton control." ),
		DefaultValue( "" ),
		]
		public String Text
		{
			get
			{
				Object state = ViewState["Text"];
				if ( state != null )
				{
					return (String)state;
				}
				return "";
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of button to display.
		/// </summary>
		/// <value>One of the <see cref="PollViewButtonType"/> values. The default value is PushButton.</value>
		/// <remarks>
		/// Use this property to specify whether the button is displayed as link or push button. 
		/// The default value is PushButton.
		/// </remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the type of button to display." ),
		DefaultValue( PollViewButtonType.PushButton ),
		]
		public PollViewButtonType ButtonType
		{
			get
			{
				Object state = ViewState["ButtonType"];
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
				ViewState["ButtonType"] = value;
			}
		}

		/// <summary>
		/// Overrides WebControl.TagKey
		/// </summary>
		protected override System.Web.UI.HtmlTextWriterTag TagKey
		{
			get
			{
				switch ( this.ButtonType )
				{
					case PollViewButtonType.PushButton:
						return HtmlTextWriterTag.Input;
					case PollViewButtonType.LinkButton:
						return HtmlTextWriterTag.A;
					default:
						return base.TagKey;
				}
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// The event which occurs when the button is clicked.
		/// </summary>
		public event EventHandler Click
		{
			add
			{
				this.Events.AddHandler( EventClick, value );
			}
			remove
			{
				this.Events.RemoveHandler( EventClick, value );
			}
		}

		/// <summary>
		/// Raises the Click event.
		/// </summary>
		protected void OnClick( EventArgs e )
		{
			EventHandler click = Events[EventClick] as EventHandler;
			if ( click != null )
			{
				click( this, e );
			}
		}

		private static readonly Object EventClick = new Object();

		#endregion

		#region Lifecycle

		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{
			if ( this.ButtonType == PollViewButtonType.PushButton )
			{
				AddPushButtonAttributes( writer );
			}
			if ( this.ButtonType == PollViewButtonType.LinkButton )
			{
				AddLinkButtonAttributes( writer );
			}
		}

		private void AddPushButtonAttributes( HtmlTextWriter writer )
		{

			writer.AddAttribute( HtmlTextWriterAttribute.Type, "submit" );
			writer.AddAttribute( HtmlTextWriterAttribute.Name, this.UniqueID );
			writer.AddAttribute( HtmlTextWriterAttribute.Value, this.Text );

			base.AddAttributesToRender( writer );
		}

		private void AddLinkButtonAttributes( HtmlTextWriter writer )
		{

			base.AddAttributesToRender( writer );

			if ( this.Enabled && !( this.Page == null ) )
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Href, this.Page.ClientScript.GetPostBackClientHyperlink( this, "" ) );
			}
		}

		protected override void Render( HtmlTextWriter writer )
		{
			if ( Page != null )
			{
				Page.VerifyRenderingInServerForm( this );
			}
			base.Render( writer );
		}


		protected override void RenderContents( HtmlTextWriter writer )
		{
			if ( this.ButtonType == PollViewButtonType.LinkButton )
			{
				writer.Write( this.Text );
			}
		}

		#endregion

		#region IPostBackEventHandler Members

		void IPostBackEventHandler.RaisePostBackEvent( string eventArgument )
		{
			this.RaisePostBackEvent( eventArgument );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventArgument" )]
		protected void RaisePostBackEvent( String eventArgument )
		{
			this.OnClick( EventArgs.Empty );
		}

		#endregion

	}
}
