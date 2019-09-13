using System;
using System.Web.UI.Design;
using System.Drawing;
using System.Globalization;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>
	/// Designer for the AdSenseAsUnit control.
	/// </summary>
	public class AdSenseAdUnitDesigner : ControlDesigner
	{

		/// <exclude />
		public override void Initialize( System.ComponentModel.IComponent component )
		{
			base.Initialize( component );

			this.adUnit = component as AdSenseAdUnit;
			if ( adUnit == null )
			{
				throw new InvalidOperationException( Resources.AdSenseAdUnitDesigner_InvalidComponent );
			}

		}

		/// <exclude />
		public override string GetDesignTimeHtml()
		{
			String width = adUnit.GetWidth();
			String height = adUnit.GetHeight();
			String backColor = ( adUnit.BackColor != Color.Empty ) ? ColorTranslator.ToHtml( adUnit.BackColor ) : "gray";
			String textColor = ( adUnit.TextColor != Color.Empty ) ? ColorTranslator.ToHtml( adUnit.TextColor ) : "black";
			String borderColor = ( adUnit.BorderColor != Color.Empty ) ? ColorTranslator.ToHtml( adUnit.BorderColor ) : "black";

			return String.Format( CultureInfo.InvariantCulture, "<div style='vertical-align:middle;text-align:center;border-width:2px;border-style:solid;width:{0}px;height:{1}px;background:{2};color:{3};border-color:{4};'><span style='vertical-align:middle;'>AdSense AdUnit</span></div>", width, height, backColor, textColor, borderColor );
		}

		AdSenseAdUnit adUnit;

	}
}
