using System;
using System.Web.UI.Design;
using System.Drawing;
using System.Globalization;

namespace MetaBuilders.WebControls.Design
{
	
	/// <summary>
	/// Designer for the AdSenseLinkUnit control.
	/// </summary>
	public class AdSenseLinkUnitDesigner : ControlDesigner
	{

		/// <exclude />
		public override void Initialize( System.ComponentModel.IComponent component )
		{
			base.Initialize( component );

			this.ad = component as AdSenseLinkUnit;
			if ( ad == null )
			{
				throw new InvalidOperationException( Resources.AdSenseLinkUnitDesigner_InvalidComponent );
			}
			
		}

		/// <exclude />
		public override string GetDesignTimeHtml()
		{
			String width = ad.GetWidth();
			String height = ad.GetHeight();
			String backColor = ( ad.BackColor != Color.Empty ) ? ColorTranslator.ToHtml( ad.BackColor ) : "gray";
			String textColor = ( ad.TitleColor != Color.Empty ) ? ColorTranslator.ToHtml( ad.TitleColor ) : "black";
			String borderColor = ( ad.BorderColor != Color.Empty ) ? ColorTranslator.ToHtml( ad.BorderColor ) : "black";

			return String.Format(CultureInfo.InvariantCulture, "<div style='vertical-align:middle;text-align:center;border-width:2px;border-style:solid;width:{0}px;height:{1}px;background:{2};color:{3};border-color:{4};'><span style='vertical-align:middle;'>AdSense LinkUnit</span></div>", width, height, backColor, textColor, borderColor );
		}

		AdSenseLinkUnit ad;

	}
}
