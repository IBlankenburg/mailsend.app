using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A clientscript-enabled checkbox which toggles the visibility of another control on the page.
	/// </summary>
	/// <remarks>
	/// This checkbox has no server-side events, as the action it performs is done on the clientside.
	/// </remarks>
	[
	DefaultProperty( "Text" ),
	]
	public class ExpandingCheckBox : System.Web.UI.WebControls.WebControl, INamingContainer
	{

		#region Properties

		/// <summary>
		/// Gets or sets the ID of the control which is the target of the visibility-toggling behavior.
		/// </summary>
		[
		Description( "Gets or sets the ID of the control which is the target of the visibility-toggling behavior." ),
		Category( "Behavior" ),
		DefaultValue( "" ),
		Bindable( true ),
		Themeable( false ),
		TypeConverter( typeof( MetaBuilders.WebControls.Design.ExpandingButtonTargetControlConverter ) ),
		]
		public virtual String ControlToToggle
		{
			get
			{
				object savedState = this.ViewState[ "ControlToToggle" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return "";
				}
			}
			set
			{
				this.ViewState[ "ControlToToggle" ] = value;
				this.cachedTargetControl = null;
			}
		}

		/// <summary>
		/// Gets or sets the expanded state of the target control.
		/// </summary>
		[
		Description( "Gets or sets the expanded state of the target control." ),
		Category( "Appearance" ),
		DefaultValue( true ),
		Bindable( true ),
		]
		public virtual Boolean Expanded
		{
			get
			{
				this.EnsureChildControls();
				return this.checkBox.Checked;
			}
			set
			{
				this.EnsureChildControls();
				this.checkBox.Checked = value;
			}
		}

		/// <summary>
		/// Gets or sets the text of the checkbox.
		/// </summary>
		[
		Description( "Gets or sets the text of the checkbox." ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Bindable( true ),
		]
		public virtual String Text
		{
			get
			{
				this.EnsureChildControls();
				return this.checkBox.Text;
			}
			set
			{
				this.EnsureChildControls();
				this.checkBox.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the textalign of the checkbox.
		/// </summary>
		[
		Description( "Gets or sets the textalign of the checkbox." ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Bindable( true ),
		]
		public virtual TextAlign TextAlign
		{
			get
			{
				this.EnsureChildControls();
				return this.checkBox.TextAlign;
			}
			set
			{
				this.EnsureChildControls();
				this.checkBox.TextAlign = value;
			}
		}

		#endregion

		#region Render Path
		/// <summary>
		/// Overrides <see cref="Control.CreateChildControls"/>.
		/// </summary>
		protected override void CreateChildControls()
		{
			this.Controls.Clear();

			this.checkBox = new System.Web.UI.WebControls.CheckBox();
			checkBox.ID = "clicker";

			this.Controls.Add( checkBox );
		}


		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			this.RegisterClientScript();
			this.ApplyExpansionState();
		}

		/// <summary>
		/// Overrides <see cref="WebControl.RenderContents"/>.
		/// </summary>
		protected override void RenderContents( HtmlTextWriter writer )
		{
			this.RenderChildren( writer );
		}

		#endregion

		/// <summary>
		/// Overrides <see cref="Control.Controls"/>.
		/// </summary>
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}


		/// <summary>
		/// Applies the <see cref="Expanded"/> value to the server control set by <see cref="ControlToToggle"/>.
		/// </summary>
		protected virtual void ApplyExpansionState()
		{

			if ( this.Expanded )
			{
				if ( this.targetStyle[ "display" ] != null )
				{
					this.targetStyle.Remove( "display" );
				}
			}
			else
			{
				this.targetStyle[ "display" ] = "none";
			}
		}




		/// <summary>
		/// Registers the clientscript with the page.
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			ExpandingButtonScriptUtil.RegisterScriptForControl( this.checkBox, this.targetControl, null, "true", "false" );
		}


		private Control targetControl
		{
			get
			{
				if ( cachedTargetControl == null )
				{
					this.cachedTargetControl = this.NamingContainer.FindControl( this.ControlToToggle );
				}
				return this.cachedTargetControl;
			}
		}
		private Control cachedTargetControl = null;

		private System.Web.UI.CssStyleCollection targetStyle
		{
			get
			{
				WebControl targetWebControl = this.targetControl as WebControl;
				if ( targetWebControl != null )
				{
					return targetWebControl.Style;
				}
				System.Web.UI.HtmlControls.HtmlControl targetHtmlControl = this.targetControl as System.Web.UI.HtmlControls.HtmlControl;
				if ( targetHtmlControl != null )
				{
					return targetHtmlControl.Style;
				}
				return null;
			}
		}

		private System.Web.UI.WebControls.CheckBox checkBox;

	}
}
