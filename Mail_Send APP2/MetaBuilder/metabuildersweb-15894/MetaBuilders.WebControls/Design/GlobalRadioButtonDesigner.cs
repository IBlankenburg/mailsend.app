using System;
using System.Web.UI.Design;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>
	/// The Designer for the GlobalRadioButton control.
	/// </summary>
	/// <exclude/>
	[
	SupportsPreviewControl( true ), 
	]
	public class GlobalRadioButtonDesigner : ControlDesigner
	{
		/// <exclude/>
		public override string GetDesignTimeHtml()
		{
			GlobalRadioButton radio = (GlobalRadioButton)this.ViewControl;

			String originalText = radio.Text;
			Boolean noTextSet = String.IsNullOrEmpty( originalText );

			if ( noTextSet )
			{
				radio.Text = "[" + radio.ID + "]";
			}
			
			String result = base.GetDesignTimeHtml();

			if ( noTextSet )
			{
				radio.Text = originalText;
			}

			return result;
		}


	}
}
