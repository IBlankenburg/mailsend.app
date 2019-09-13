using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MetaBuilders.WebControls {

	internal class TopButtonUpLevelRenderer : MultiViewContentsRenderer {
		
		public TopButtonUpLevelRenderer( MultiViewBar owner )
			: base( owner ) {
		}

		public override void Render( HtmlTextWriter writer ) {
			if ( this.Owner.Items.Count > 0 ) {

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderBeginTag( "tr" );
				}

				RenderButtons( writer );
				RenderItems( writer );

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderEndTag();
				}
			}
		}

		private void RenderItems( HtmlTextWriter writer ) {
			foreach( MultiViewItem item in this.Owner.Items ) {
				base.RenderUpLevelItemContent( writer, item );
			}
		}

		protected virtual void RenderButtons( HtmlTextWriter writer ) {
			foreach ( MultiViewItem item in this.Owner.Items ) {
				base.RenderUplevelItemButton( writer, item );
			}
		}
	}
}
