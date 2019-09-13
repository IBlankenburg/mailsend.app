using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.ComponentModel;
using System.Resources;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The ResizeMonitor control raises server-side events when the user changes their browser window size.
	/// </summary>
	/// <example>
	/// This is an example usage of the ResizeMonitor control.
	/// <code>
	/// <![CDATA[
	/// <%@ Page language="C#" %>
	/// <%@ Register tagprefix="mbrp" namespace="MetaBuilders.WebControls" assembly="MetaBuilders.WebControls.ResizeMonitor" %>
	/// <script runat="server">
	///		protected virtual void ClientResize( Object sender, EventArgs e ) {
	///			Result.Text = "Resized to " + rpb.Width.ToString() + "X" + rpb.Height.ToString();
	///		}
	/// </script>
	/// <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
	/// <html><body><form runat="server">
	///		<mbrp:ResizeMonitor runat="server" id="rpb" OnResize="ClientResize" />
	///		<asp:Label runat="server" id="Result" EnableViewState="false" />
	///		<div style="border:1px solid black;height:90%;width:90%;">foo</div>
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	[
	Designer(typeof(ResizeMonitorDesigner))
	]
	public class ResizeMonitor : System.Web.UI.Control, INamingContainer, IPostBackEventHandler {

		#region Properties
		/// <summary>
		/// The length of time, in milliseconds, that the control will wait before firing a postback after a window resize.
		/// </summary>
		[
		Description("The length of time, in milliseconds, that the control will wait before firing a postback after a window resize."),
		Category("Behavior"),
		DefaultValue(500)
		]
		public virtual Int32 Delay {
			get {
				Object savedState = this.ViewState["Delay"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return 500;
			}
			set {
				this.ViewState["Delay"] = value;
			}
		}

		/// <summary>
		/// Gets the resized width of the client browser.
		/// </summary>
		[
		Description(""),
		Browsable(false)
		]
		public virtual Int32 Width {
			get {
				Object savedState = this.ViewState["Width"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return -1;
			}
		}
		/// <summary>
		/// Sets the resized width of the client browser.
		/// </summary>
		protected virtual void SetWidth( Int32 value ) {
			this.ViewState["Width"] = value;
		}

		/// <summary>
		/// Gets the resized height of the client browser.
		/// </summary>
		[
		Description(""),
		Browsable(false)
		]
		public virtual Int32 Height {
			get {
				Object savedState = this.ViewState["Height"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return -1;
			}
		}
		/// <summary>
		/// Sets the resized height of the client browser.
		/// </summary>
		protected virtual void SetHeight( Int32 value ) {
			this.ViewState["Height"] = value;
		}
		#endregion

		#region Events
		/// <summary>
		/// The event which fires when the user resizes the browser window.
		/// </summary>
		public event EventHandler Resize;
		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		protected virtual void OnResize( EventArgs e ) {
			if ( Resize != null ) {
				Resize( this, e );
			}
		}
		#endregion

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender(System.EventArgs e) {
			base.OnPreRender(e);
			RegisterClientScript();
			this.widthHolder.Value = this.Width.ToString(CultureInfo.InvariantCulture);
			this.heightHolder.Value = this.Height.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
			this.RenderChildren(writer);
		}


		#region Client Script

		/// <summary>
		/// Registers the clientside script with the page.
		/// </summary>
		protected virtual void RegisterClientScript() {
			if (Page == null)
			{
				return;
			}

			ClientScriptManager script = Page.ClientScript;
			script.RegisterClientScriptResource(typeof(ResizeMonitor), "MetaBuilders.WebControls.Embedded.ResizeMonitorScript.js");

			RegisterPostbackScript();

			script.RegisterStartupScript( typeof( ResizeMonitor), StartupScriptName, " ResizeMonitor_Init(); ", true);
		}

		/// <summary>
		/// Registers the clientside postback script with the page.
		/// </summary>
		private void RegisterPostbackScript() {
			ClientScriptManager scriptMan = Page.ClientScript;

			if (!scriptMan.IsClientScriptBlockRegistered(PostbackScriptName))
			{
				System.Text.StringBuilder script = new System.Text.StringBuilder();
				script.Append("\n");
				script.Append("function ResizeMonitor_Postback() {\n");
				script.Append("ResizeMonitor_SaveDimensions();\n");
				script.Append(scriptMan.GetPostBackEventReference(this, ""));
				script.Append(";\n}\n");
				script.Append(this.GetClientVariableDeclaration("TimeoutLength", this.Delay.ToString(CultureInfo.InvariantCulture)));
				script.Append(this.GetClientVariableDeclaration("WidthHolder", this.widthHolder.UniqueID, true ) );
				script.Append(this.GetClientVariableDeclaration("HeightHolder", this.heightHolder.UniqueID, true ) );
				script.Append("\n");

				scriptMan.RegisterClientScriptBlock( typeof( ResizeMonitor ),PostbackScriptName, script.ToString(), true );
			}
		}

		/// <summary>
		/// Creates a clientside variable with the given name and value.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The initial value of the variable.</param>
		/// <returns>A string containing the script variable declaration.</returns>
		private String GetClientVariableDeclaration( String name, String value ) {
			return GetClientVariableDeclaration( name, value, false );
		}

		/// <summary>
		/// Creates a clientside variable with the given name and value, quoted or not.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The initial value of the variable.</param>
		/// <param name="quote">Whether the value should be quoted or not.</param>
		/// <returns>A string containing the script variable declaration.</returns>
		private String GetClientVariableDeclaration( String name, String value, Boolean quote ) {
			if ( quote ) {
				return "document.ResizeMonitor_" + name + " = '" + value + "';\n";
			} else {
				return "document.ResizeMonitor_" + name + " = " + value + ";\n";
			}
		}



		private String StartupScriptName = "MetaBuilders.WebControls.ResizeMonitor Startup";
		private String PostbackScriptName = "MetaBuilders.WebControls.ResizeMonitor Postback";
		#endregion

		#region Implementation of IPostBackEventHandler
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument) {
			this.RaisePostBackEvent(eventArgument);
		}

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		protected virtual void RaisePostBackEvent(String eventArgument)
		{
			OnResize(EventArgs.Empty);
		}
		#endregion

		#region Size Tracking
		/// <summary>
		/// Overrides <see cref="Control.CreateChildControls"/>
		/// </summary>
		protected override void CreateChildControls() {
			widthHolder = new System.Web.UI.HtmlControls.HtmlInputHidden();
			widthHolder.ID = "ResizeWidth";
			this.Controls.Add(widthHolder);
			widthHolder.ServerChange += new EventHandler( widthChanged );

			heightHolder = new System.Web.UI.HtmlControls.HtmlInputHidden();
			heightHolder.ID = "ResizeHeight";
			this.Controls.Add(heightHolder);
			heightHolder.ServerChange += new EventHandler( heightChanged );
		}

		/// <summary>
		/// Handles the ServerChange event of widthHolder
		/// </summary>
		private void widthChanged( Object sender, EventArgs e ) {
			this.SetWidth(Int32.Parse(this.widthHolder.Value, CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Handles the ServerChange event of heightHolder
		/// </summary>
		private void heightChanged( Object sender, EventArgs e ) {
			this.SetHeight( Int32.Parse(this.heightHolder.Value, System.Globalization.CultureInfo.InvariantCulture) );
		}

		private System.Web.UI.HtmlControls.HtmlInputHidden widthHolder;
		private System.Web.UI.HtmlControls.HtmlInputHidden heightHolder;
		#endregion
	}
}
