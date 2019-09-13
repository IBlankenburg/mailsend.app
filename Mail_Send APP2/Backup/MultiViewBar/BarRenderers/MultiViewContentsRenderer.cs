using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	internal abstract class MultiViewContentsRenderer {
		
		public MultiViewContentsRenderer( MultiViewBar owner ) {
			this.Owner = owner;
		}

		protected MultiViewBar Owner;

		public abstract void Render( HtmlTextWriter writer );

		protected String ActiveItem {
			get {
				if ( activeItem == null ) {
					activeItem = this.DetermineActiveItem();
				}
				return activeItem;
			}
		}
		private String activeItem;

		private String DetermineActiveItem() {
			String currentItem = this.Owner.CurrentItem;
			Boolean currentItemFound = false;
			foreach( MultiViewItem item in this.Owner.Items ) {
				if ( item.Title == currentItem ) {
					currentItemFound = true;
					break;
				}
			}
			if ( !currentItemFound ) {
				currentItem = this.Owner.Items[ 0 ].Title;
			}
			return currentItem;
		}


		protected void RenderUplevelItemButton( HtmlTextWriter writer, MultiViewItem item ) {
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderBeginTag( "tr" );
			}

			Boolean itemIsCurrent = ( item.Title == this.ActiveItem );

			if ( !itemIsCurrent ) {
				writer.AddAttribute( HtmlTextWriterAttribute.Onclick, this.Owner.Page.ClientScript.GetPostBackEventReference( this.Owner, item.Title ) );
				writer.AddStyleAttribute( "cursor", "pointer" );
			}

			MultiViewItemButtonStyle itemStyle = new MultiViewItemButtonStyle();
			if ( itemIsCurrent ) {
				itemStyle.CopyFrom( this.Owner.CurrentButtonStyle );
			}
			itemStyle.MergeWith( this.Owner.ButtonStyle );
			itemStyle.AddAttributesToRender( writer );
			writer.AddStyleAttribute( "padding", "5px" );
			writer.RenderBeginTag( "td" );

			if ( item.ImageUrl.Length != 0 ) {
				Image itemImageButton = new Image();
				if ( !itemIsCurrent ) {
					itemImageButton.Attributes[ "onclick" ] = this.Owner.Page.ClientScript.GetPostBackEventReference( this.Owner, item.Title );
				}
				itemImageButton.ImageUrl = item.ImageUrl;
				itemImageButton.ImageAlign = ImageAlign.Middle;
				itemImageButton.BorderWidth = Unit.Pixel( 0 );
				itemImageButton.ToolTip = item.Title;
				itemImageButton.AlternateText = item.Title;
				itemImageButton.RenderControl( writer );
				writer.Write( "&nbsp;" );
			}

			HyperLink buttonControl = new HyperLink();
			if ( !itemIsCurrent && this.Owner.Page != null ) {
				buttonControl.NavigateUrl = this.Owner.Page.ClientScript.GetPostBackClientHyperlink( this.Owner, item.Title );
			} else {
				buttonControl.NavigateUrl = "";
			}
			buttonControl.BorderWidth = Unit.Pixel( 0 );
			buttonControl.ForeColor = itemStyle.ForeColor;
			buttonControl.Text = item.Title;
			buttonControl.RenderControl( writer );

			writer.RenderEndTag();
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderEndTag();
			}

		}

		protected void RenderUpLevelItemContent( HtmlTextWriter writer, MultiViewItem item ) {
			if ( item.Title != this.ActiveItem ) {
				writer.AddStyleAttribute( "display", "none" );
			}

			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				if ( !this.Owner.Height.IsEmpty ) {
					writer.AddStyleAttribute( "height", "100%" );
				}
				writer.RenderBeginTag( "tr" );
				if ( !this.Owner.Height.IsEmpty ) {
					writer.AddStyleAttribute( "height", "100%" );
				}
			} else {
				if ( !this.Owner.Width.IsEmpty ) {
					writer.AddStyleAttribute( "width", "100%" );
				}
			}
			writer.RenderBeginTag( "td" );

			
			item.RenderControl( writer );
			
			
			writer.RenderEndTag();
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderEndTag();
			}
		}
	
		protected void RenderDownLevelItemButton( HtmlTextWriter writer, MultiViewItem item ) {
			Boolean itemIsCurrent = ( item.Title == this.ActiveItem );
			
			MultiViewItemButtonStyle itemStyle = new MultiViewItemButtonStyle();
			if ( itemIsCurrent ) {
				itemStyle.CopyFrom( this.Owner.CurrentButtonStyle );
			}
			itemStyle.MergeWith( this.Owner.ButtonStyle );

			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderBeginTag( "tr" );
			}

			itemStyle.AddAttributesToRender( writer );
			writer.AddStyleAttribute( "padding", "5px" );
			writer.RenderBeginTag( "td" );


			if ( item.ImageUrl.Length != 0 ) {
				Image itemImageButton;
				if ( itemIsCurrent ) {
					itemImageButton = new Image();
				} else {
					itemImageButton = new ImageButton();
					itemImageButton.ID = this.Owner.UniqueID + ":" + item.Title + "_Image";
				}
							
				itemImageButton.ImageUrl = item.ImageUrl;
				itemImageButton.ImageAlign = ImageAlign.Middle;
				itemImageButton.BorderWidth = Unit.Pixel( 0 );
				itemImageButton.ToolTip = item.Title;
				itemImageButton.AlternateText =  item.Title;
				itemImageButton.RenderControl( writer );
				writer.Write( "&nbsp;" );
			}

			if ( itemIsCurrent ) {
				writer.Write( item.Title );
			} else {
				Button itemButton = new Button();
				itemButton.ID = this.Owner.UniqueID + ":" + item.Title;
				itemButton.Text = item.Title;
				itemButton.ForeColor = itemStyle.ForeColor;
				itemButton.BackColor = itemStyle.BackColor;
				itemButton.RenderControl( writer );
			}
				
			writer.RenderEndTag();
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderEndTag();
			}

		}
		
		protected void RenderDownLevelItemContent( HtmlTextWriter writer, MultiViewItem item ) {
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				if ( !this.Owner.Height.IsEmpty ) {
					writer.AddStyleAttribute( "height", "100%" );
				}
				writer.RenderBeginTag( "tr" );
				if ( !this.Owner.Height.IsEmpty ) {
					writer.AddStyleAttribute( "height", "100%" );
				}
			} else {
				if ( !this.Owner.Width.IsEmpty ) {
					writer.AddStyleAttribute( "width", "100%" );
				}
			}

			writer.RenderBeginTag( "td" );

			item.RenderControl( writer );

			writer.RenderEndTag();
			if ( this.Owner.LayoutDirection == MultiViewLayoutDirection.Vertical ) {
				writer.RenderEndTag();
			}

		}
	}
}
