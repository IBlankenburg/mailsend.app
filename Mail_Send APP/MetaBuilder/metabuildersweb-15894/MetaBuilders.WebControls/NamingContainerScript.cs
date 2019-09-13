using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// <para>The NamingContainerScript control allows for a smoother and more robust mechanism for creating adhoc clientside script
	/// which communicates between controls which are placed within a serverside naming container.</para>
	/// </summary>
	/// <remarks>
	/// <para>Asp.net takes over the clientside id attribute of any serverside control, such that the naming container's ID is a prefix on the ID of the control.
	/// This behavior makes it difficult to add simple script to a control's clientside event handlers that reference other controls, as the clientisde id attribute
	/// will be unknown at design time. The current workaround is to use serverside code to emit the ClientID of the controls, and load those ids within your script.
	/// This is a big departure from the traditional straightforward method of simply naming the html element by id.
	/// </para>
	/// <para>This control provides a simpler solution by bringing to the clientside, the NamingContainer concept which the serverside uses.
	/// By placing a NamingContainerScript control in the markup, javascript is emited which adds new properties to your html elements
	/// which allows them to reference eachother by ID through a new NamingContainer property.
	/// </para>
	/// </remarks>
	/// <example>
	/// <para>The following is a simple example where two textboxes contained within an ascx UserControl influence eachother with jscript.</para>
	/// <![CDATA[
	/// <%@ Control Language="C#" ClassName="Child" %>
	/// <%@ Register Assembly="NamingContainerScript" Namespace="MetaBuilders.WebControls" TagPrefix="nm" %>
	/// <asp:TextBox runat="server" ID="Foo" onchange="this.NamingContainer.Bar.value = this.value;" />
	/// <asp:TextBox runat="server" ID="Bar" onchange="this.NamingContainer.Foo.value = this.value;" />
	/// <nm:NamingContainerScript ID="NamingContainerScript1" runat="server" />
	/// ]]>
	/// <para>The onchange handlers for these TextBox controls run on the clientside.
	/// The handlers simply move the current value of the textbox to the related TextBox.
	/// The jscript code references the related element through the NamingContainer property, which has been dynamically added by
	/// the NamingContainerScript control. The new NamingContainer property has as properties, the serverside IDs of all controls within the serverside
	/// NamingContainer.
	/// </para>
	/// </example>
	[
	ToolboxData( "<{0}:NamingContainerScript runat=server />" ),
	System.ComponentModel.Designer( typeof( NamingContainerScriptDesigner ) ),
	NonVisualControl(),
	]
	public class NamingContainerScript : Control {

		/// <exclude />
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection( this );
		}

		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			RegisterClientScript();
		}

		private const String scriptKey = "MetaBuilders.WebControls.NamingContainerScript";
		private const String arrayName = "MetaBuilders_NamingContainers";

		private void RegisterClientScript() {
			if ( !Enabled ) {
				return;
			}

			if ( Page == null ) {
				return;
			}

			Control container = this.NamingContainer;
			if ( container == null || container.ClientID.Length == 0 ) {
				return;
			}

			ClientScriptManager script = Page.ClientScript;

            script.RegisterClientScriptResource(typeof(NamingContainerScript), "MetaBuilders.WebControls.Embedded.NamingContainerScript.js");
			if ( container == Page ) {
				script.RegisterArrayDeclaration( arrayName, "{ ID:'', Name:'' }" );
			} else {
				script.RegisterArrayDeclaration( arrayName, "{ ID:'" + container.ClientID + "', Name:'" + container.UniqueID + "' }" );
			}
			script.RegisterStartupScript( typeof( NamingContainerScript ), scriptKey, "MetaBuilders_NamingContainer_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_NamingContainer_Init" ), true );
		}

		/// <summary>
		/// The Visible attribute is not valid on this control
		/// </summary>
		[
		EditorBrowsableAttribute( EditorBrowsableState.Never ),
		BrowsableAttribute( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )
		]
		public override bool Visible {
			get {
				return base.Visible;
			}
			set {
				base.Visible = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the control behavior is enabled.
		/// </summary>
		[
		Description( "Gets or sets whether the control behavior is enabled." ),
		DefaultValue( true ),
		Bindable( true ),
		Category( "Behavior" ),
		Themeable( false ),
		]
		public virtual Boolean Enabled {
			get {
				Object state = ViewState["Enabled"];
				return ( state != null ) ? (Boolean)state : true;
			}
			set {
				ViewState["Enabled"] = value;
			}
		}

	}
}
