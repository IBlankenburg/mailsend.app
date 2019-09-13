using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	internal class SurroundButtonUpLevelRenderer : MultiViewContentsRenderer {
		
		public SurroundButtonUpLevelRenderer( MultiViewBar owner )
			: base( owner ) {
		}

		public override void Render( HtmlTextWriter writer ) {
			if ( this.Owner.Items.Count > 0 ) {

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderBeginTag( "tr" );
				}

				foreach( MultiViewItem item in this.Owner.Items ) {
					base.RenderUplevelItemButton( writer, item );
					base.RenderUpLevelItemContent( writer, item );
				}

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderEndTag();
				}

			}
		}

	}
}
