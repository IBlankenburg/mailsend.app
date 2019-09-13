using System;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MetaBuilders.WebControls {

	internal class BottomButtonDownLevelRenderer : MultiViewContentsRenderer {
		
		public BottomButtonDownLevelRenderer( MultiViewBar owner )
			: base( owner ) {
		}

		public override void Render( HtmlTextWriter writer ) {
			if ( this.Owner.Items.Count > 0 ) {

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderBeginTag( "tr" );
				}

				RenderHeader( writer );
				RenderCurrentItem( writer );
				RenderButtons( writer );

				if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
					writer.RenderEndTag();
				}

			}
		}

		private void RenderHeader( HtmlTextWriter writer ) {
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderBeginTag( "tr" );
			}
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Horizontal ) {
				if ( this.Owner.HeaderStyle.BackColor != System.Drawing.Color.Empty ) {
					writer.AddStyleAttribute( HtmlTextWriterStyle.BackgroundColor, System.Drawing.ColorTranslator.ToHtml( this.Owner.HeaderStyle.BackColor ) );
				}
			}
			writer.RenderBeginTag( "td" );

			this.Owner.HeaderStyle.AddAttributesToRender( writer );
			writer.AddStyleAttribute( "padding", "3px" );
			writer.RenderBeginTag( "div" );
			writer.Write( this.ActiveItem );
			writer.RenderEndTag();

			writer.RenderEndTag();
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderEndTag();
			}
		}

		private void RenderCurrentItem( HtmlTextWriter writer ) {
			foreach( MultiViewItem item in this.Owner.Items ) {
				if ( item.Title == this.ActiveItem ) {
					base.RenderDownLevelItemContent( writer, item );
				}
			}
		}

		protected virtual void RenderButtons( HtmlTextWriter writer ) {
			foreach ( MultiViewItem item in this.Owner.Items ) {
				base.RenderDownLevelItemButton( writer, item );
			}

		}

	}
}
