using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls
{
	public partial class ComboBox
	{
		private class DefaultButtonTemplate : ITemplate
		{
			public DefaultButtonTemplate()
			{
			}

			#region ITemplate Members

			public void InstantiateIn( Control container )
			{
				if ( container == null )
				{
					return;
				}

				image = new Image();
				image.AlternateText = Resources.ComboBox_DefaultAltText;
				image.Style[ HtmlTextWriterStyle.VerticalAlign ] = "bottom";
				container.Controls.Add( image );
			}

			#endregion

			internal void SetImageUrl()
			{
				image.ImageUrl = image.Page.ClientScript.GetWebResourceUrl( typeof( ComboBox ), "MetaBuilders.WebControls.Embedded.DropDownButton_XpStandard.gif" );
			}

			Image image;
		}
	}
}
