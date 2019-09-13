using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MetaBuilders.WebControls {

	internal class TopButtonDownLevelRenderer : MultiViewContentsRenderer {
		
		public TopButtonDownLevelRenderer( MultiViewBar owner )
			: base( owner ) {
		}

		public override void Render( HtmlTextWriter writer ) {
			if ( this.Owner.Items.Count > 0 ) {

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderBeginTag( "tr" );
				}

				RenderButtons( writer );
				RenderCurrentItem( writer );

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderEndTag();
				}

			}
		}

		protected virtual void RenderButtons( HtmlTextWriter writer ) {
			foreach( MultiViewItem item in this.Owner.Items ) {
				base.RenderDownLevelItemButton( writer, item );
			}
		}

		private void RenderCurrentItem( HtmlTextWriter writer ) {
			foreach( MultiViewItem item in this.Owner.Items ) {
				if ( item.Title == this.ActiveItem ) {
					base.RenderDownLevelItemContent( writer, item );
				}
			}
		}

	}
}
