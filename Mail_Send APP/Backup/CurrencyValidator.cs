using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Validates whether the value of an associated input control can be parsed as currency.
	/// </summary>
	/// <remarks>
	/// Client-side validation is only supported on browsers supporting VBScript.
	/// </remarks>
	/// <example>
	/// The following is an example of using the CurrencyValidator with a TextBox
	/// <code><![CDATA[
	/// <%@ Register TagPrefix="mbtv" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.TypeValidators" %>
	/// <html><body><form runat="server">
	/// Currency Value: <asp:TextBox runat="server" Id="MyMoney" />
	/// <mbtv:CurrencyValidator runat="server" ControlToValidate="MyMoney" ErrorMessage="Not A Currency" Display="Dynamic" />
	/// <br /><br />
	/// <asp:Button runat="server" Text="Smack" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	public class CurrencyValidator : System.Web.UI.WebControls.BaseValidator
	{

		/// <summary>
		/// Determines if the value can be parsed as currency.
		/// </summary>
		/// <returns>true is the value can be parsed as currency, false if not.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		protected override bool EvaluateIsValid()
		{
			String value = this.GetControlValidationValue( this.ControlToValidate );
			if ( value == null || value.Trim().Length == 0 )
			{
				return true;
			}

			try
			{
				Decimal.Parse( value, System.Globalization.NumberStyles.Currency, CultureInfo.InvariantCulture );
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Overrides <see cref="WebControl.AddAttributesToRender"/>.
		/// </summary>
		protected override void AddAttributesToRender( System.Web.UI.HtmlTextWriter writer )
		{
			base.AddAttributesToRender( writer );
			if ( this.RenderUplevel && this.EnableClientScript && this.Page.Request.Browser.VBScript )
			{
				writer.AddAttribute( "evaluationfunction", "MetaBuilders_CurrencyValidatorEvaluateIsValid" );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender( System.EventArgs e )
		{
			base.OnPreRender( e );
			this.RegisterClientScript();
		}

		/// <summary>
		/// Registers the client-side script for the validator
		/// </summary>
		/// <remarks>
		/// Only browsers supporting VBScript receive the clientscript.
		/// </remarks>
		protected virtual void RegisterClientScript()
		{
			if ( this.RenderUplevel && this.EnableClientScript && this.Page.Request.Browser.VBScript )
			{
				Page.ClientScript.RegisterClientScriptBlock( typeof( CurrencyValidator ), "Validation Script", ValidatorScripts.CurrencyValidator_Script, false );
			}
		}
	}
}
