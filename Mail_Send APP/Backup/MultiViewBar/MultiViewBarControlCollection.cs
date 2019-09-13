using System;
using System.Web.UI;
using System.Globalization;

namespace MetaBuilders.WebControls
{

	internal class MultiViewBarControlCollection : System.Web.UI.ControlCollection {

		public MultiViewBarControlCollection( Control owner ) : base( owner ) {
		}

		public override void Add( Control child ) {
			if ( child is MultiViewItem ) {
				base.Add( child );
				return;
			}

			if ( !( child is System.Web.UI.LiteralControl ) ) {
				throw new InvalidOperationException( String.Format( CultureInfo.InvariantCulture, Resources.MultiViewBarControlCollection_MultiViewItems_Only, child.GetType().FullName ) );
			}

			collectionChanged();
		}

		public override void AddAt( int index, Control child ) {
			if ( child is MultiViewItem ) {
				base.AddAt( index, child );
				return;
			}

			if ( !( child is LiteralControl ) ) {
				throw new InvalidOperationException( String.Format(CultureInfo.InvariantCulture, Resources.MultiViewBarControlCollection_MultiViewItems_Only, child.GetType().FullName ) );
			}

			collectionChanged();
		}

		public override void Remove( Control value ) {
			base.Remove( value );
			collectionChanged();
		}

		public override void RemoveAt( int index ) {
			base.RemoveAt( index );
			collectionChanged();
		}

		public override void Clear() {
			base.Clear();
			collectionChanged();
		}

		private void collectionChanged() {
			MultiViewBar parent = this.Owner as MultiViewBar;
			if ( parent != null ) {
				parent.OnItemsChanged( EventArgs.Empty );
			}
		}

	}
}
