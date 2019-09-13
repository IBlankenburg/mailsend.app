using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Validates whether the value of an associated input control can be parsed as a DateTime.
	/// </summary>
	/// <remarks>
	/// Unlike the built in CompareValidator set to DataTypeCheck and Date, this validator supports Time values.
	/// Client-side validation is only supported on browsers supporting VBScript.
	/// </remarks>
	/// <example>
	/// The following is an example of using the DateTimeValidator with a TextBox
	/// <code><![CDATA[
	/// <%@ Register TagPrefix="mbtv" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.TypeValidators" %>
	/// <html><body><form runat="server">
	/// DateTime Value: <asp:TextBox runat="server" Id="MyDate" />
	/// <mbtv:DateTimeValidator runat="server" ControlToValidate="MyDate" ErrorMessage="Not A DateTime" Display="Dynamic" />
	/// <br /><br />
	/// <asp:Button runat="server" Text="Smack" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	public class DateTimeValidator : System.Web.UI.WebControls.BaseValidator {

		/// <summary>
		/// Determines if the value can be parsed as a DateTime.
		/// </summary>
		/// <returns>true is the value can be parsed as a DateTime, false if not.</returns>
		protected override bool EvaluateIsValid() {
			String value = this.GetControlValidationValue(this.ControlToValidate);
			if ( value == null || value.Trim().Length == 0) {
				return true;
			}

			DateTime foo;
			return DateTime.TryParse( value, out foo );
		}

		/// <summary>
		/// Overrides <see cref="WebControl.AddAttributesToRender"/>.
		/// </summary>
		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer) {
			base.AddAttributesToRender(writer);
			if (this.RenderUplevel && this.EnableClientScript && this.Page.Request.Browser.VBScript) {
				writer.AddAttribute( "evaluationfunction", "MetaBuilders_DateTimeValidatorEvaluateIsValid" );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender(System.EventArgs e) {
			base.OnPreRender(e);
			this.RegisterClientScript();
		}

		/// <summary>
		/// Registers the client-side script for the validator
		/// </summary>
		/// <remarks>
		/// Only browsers supporting VBScript receive the clientscript.
		/// </remarks>
		protected virtual void RegisterClientScript() {
			if (this.RenderUplevel && this.EnableClientScript && this.Page.Request.Browser.VBScript) {
				Page.ClientScript.RegisterClientScriptBlock( typeof( DateTimeValidator ), "Validation Script", ValidatorScripts.DateTimeValidator_Script, false );
			}
		}
	}
}