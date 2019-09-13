using System;
using System.Web.UI;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Generic implementation of ITemplate that simply raises an event when the template needs to be created.
	/// </summary>
	/// <remarks>
	/// This class makes it easier to create template controls completely from code. Instead of creating a seperate ITemplate class implementation, use this one and attach to its event.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// <script runat="server" language="C#" >
	///
	/// 	protected void Page_Load( Object sender, EventArgs e ) {
	/// 		if ( !Page.IsPostBack ) {
	/// 			Repeater1.DataSource = new String[] {"one", "two", "three" };
	/// 			Repeater1.DataBind();
	/// 		}
	/// 	}
	///
	/// 	protected void Repeater1_Init( Object sender, EventArgs e ) {
	/// 		MetaBuilders.WebControls.RuntimeTemplate template = new MetaBuilders.WebControls.RuntimeTemplate();
	/// 		((Repeater)sender).ItemTemplate = template;
	/// 		template.CreateTemplate += new MetaBuilders.WebControls.RuntimeTemplateEventHandler( CreateRepeater1Template );
	/// 	}
	///
	/// 	protected void CreateRepeater1Template( Object sender, MetaBuilders.WebControls.RuntimeTemplateEventArgs e ) {
	/// 		Label label = new Label();
	/// 		e.Container.Controls.Add( label );
	/// 		label.DataBinding += new EventHandler( DataBindLabel );
	///
	/// 		e.Container.Controls.Add( new LiteralControl( "<br>" ) );
	/// 	}
	/// 	
	/// 	protected void DataBindLabel( Object sender, EventArgs e ) {
	///			Label label = sender as Label;
	///			RepeaterItem container = label.NamingContainer as RepeaterItem;
	///			label.Text = container.DataItem.ToString();
	/// 	}
	/// 	
	/// </script>
	/// <html><body><form runat="server">
	/// 		<asp:Repeater runat="server" Id="Repeater1" OnInit="Repeater1_Init" />
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	public class RuntimeTemplate : ITemplate {
		
		/// <summary>
		/// The event that is raised when a template instantiation is required.
		/// </summary>
		public event RuntimeTemplateEventHandler CreateTemplate;
		/// <summary>
		/// Raises the CreateTemplate event.
		/// </summary>
		protected virtual void OnCreateTemplate(RuntimeTemplateEventArgs e ) {
			if ( this.CreateTemplate != null ) {
				this.CreateTemplate(this,e);
			}
		}

		#region ITemplate Members
		void ITemplate.InstantiateIn(Control container) {
			this.InstantiateIn(container);
		}

		/// <summary>
		/// Raises the OnCreateTemplate event for the given container.
		/// </summary>
		protected virtual void InstantiateIn( Control container )
		{
			this.OnCreateTemplate(new RuntimeTemplateEventArgs(container));
		}
		#endregion

	}
}
