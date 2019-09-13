using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MetaBuilders.WebControls {

	internal class SurroundButtonDownLevelRenderer : MultiViewContentsRenderer {
		
		public SurroundButtonDownLevelRenderer( MultiViewBar owner )
			: base( owner ) {
		}

		public override void Render( HtmlTextWriter writer ) {
			if ( this.Owner.Items.Count > 0 ) {

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderBeginTag( "tr" );
				}

				foreach( MultiViewItem item in this.Owner.Items ) {

					base.RenderDownLevelItemButton( writer, item );
					if ( item.Title == this.ActiveItem ) {
						base.RenderDownLevelItemContent( writer, item );
					}
				}

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderEndTag();
				}

			}
		}

	}
}
