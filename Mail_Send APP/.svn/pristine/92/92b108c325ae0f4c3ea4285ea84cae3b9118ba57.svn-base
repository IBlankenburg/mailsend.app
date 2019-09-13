using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel;

namespace MetaBuilders.WebControls {


	/// <summary>
	/// ParsingContainer can parse the value of its <see cref="Content"/> property to parse and use controls loaded as a string.
	/// </summary>
	/// <example>
	/// The following example demonstrates how to use the ParsingContainer on a page.
	/// <code>
	/// <![CDATA[
	/// <%@ Page Language="C#" %>
	/// <%@ Register TagPrefix="mbpc" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.ParsingContainer" %>
	/// <script runat="server">
	/// 	protected void Page_Load( Object sender, EventArgs e ) {
	/// 		if ( !Page.IsPostBack ) {
	/// 			String[] stuff = {
	/// 				"<asp:Panel runat='server'><asp:Label runat='server' Text='First Item' /></asp:Panel>",
	/// 				"<asp:Panel runat='server'><asp:Label runat='server' Text='Second Item' /></asp:Panel>",
	/// 				"<asp:Panel runat='server'><asp:Label runat='server' Text='Third Item' /></asp:Panel>"
	/// 			};
	/// 			Stuff.DataSource = stuff;
	/// 			Stuff.DataBind();
	/// 		}
	/// 	}
	/// </script>
	/// <html><body><form runat="server">
	/// 	<asp:Repeater runat="server" Id="Stuff" >
	/// 		<ItemTemplate>
	/// 			<mbpc:ParsingContainer runat="server" Content=<%# Container.DataItem.ToString() %> />
	/// 		</ItemTemplate>
	/// 	</asp:Repeater>
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	[
	DefaultProperty("Content"),
	ParseChildren(false),
	PersistChildren(false),
	Designer(typeof(ParsingContainerDesigner)),
	ControlBuilder(typeof(ParsingContainerControlBuilder)),
	]
	public class ParsingContainer : System.Web.UI.WebControls.WebControl {

		/// <summary>
		/// The <see cref="String"/> to parse into a child control tree.
		/// </summary>
		/// <remarks>
		/// If the value of this property is not parsable, a parsing exception will thrown when the control runs its CreateChildControls method, which may be called at varying times in the page lifecycle.
		/// </remarks>
		[
		Description("The String to parse into a child control tree."),
		Category("Behavior"),
		DefaultValue(""),
		Bindable(true),
		]
		public virtual String Content {
			get {
				String savedState = this.ViewState["Content"] as String;

				if (savedState == null) {
					return "";
				}
				else {
					return savedState;
				}
			}
			set {
				this.ViewState["Content"] = value;
				this.ChildControlsCreated = false;
				
			}
		}

		/// <summary>
		/// Creates child controls by parsing the <see cref="Content"/> property.
		/// </summary>
		protected override void CreateChildControls() {
			if ( this.Page == null || this.Content == null ) {
				return;
			}

			System.Web.UI.Control foo = this.Page.ParseControl(this.Content);
			this.Controls.Clear();
			this.Controls.Add(foo);
		}

		#region Composite Control Pattern

		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>
		/// </summary>
		/// <param name="savedState"></param>
		protected override void LoadViewState(object savedState) {
			base.LoadViewState (savedState);
			this.EnsureChildControls();
		}


		/// <summary>
		/// Overrides <see cref="Control.OnDataBinding"/>
		/// </summary>
		protected override void OnDataBinding(EventArgs e) {
			base.OnDataBinding (e);
			this.EnsureChildControls();
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>
		/// </summary>
		protected override void Render(HtmlTextWriter writer) {
			this.EnsureChildControls();
			base.Render(writer);
		}

		/// <summary>
		/// Overrides <see cref="Control.Controls"/>
		/// </summary>
		[
		Browsable(false),
		EditorBrowsable(EditorBrowsableState.Never),
		]
		public override ControlCollection Controls {
			get {
				this.EnsureChildControls();
				return base.Controls;
			}
		}
		#endregion

	}
}
