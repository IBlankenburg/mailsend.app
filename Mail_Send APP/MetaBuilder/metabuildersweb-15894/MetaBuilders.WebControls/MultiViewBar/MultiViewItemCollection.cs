using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Represents a collection of <see cref="MultiViewItem"/> controls in a <see cref="MultiViewBar"/> control.
	/// </summary>
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1039:ListsAreStronglyTyped" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" ), 
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface" ), 
	System.ComponentModel.Editor( typeof( Design.MultiViewItemCollectionEditor ), typeof( System.Drawing.Design.UITypeEditor ) ),
	]
	public sealed class MultiViewItemCollection : System.Collections.IList {
		
		internal MultiViewItemCollection( MultiViewBar owner ) : base() {
			this.owner = owner;
		}

		private MultiViewBar owner;

		#region Strongly Typed IList

		/// <summary>
		/// Adds the given <see cref="MultiViewItem"/> to the collection.
		/// </summary>
		/// <returns>Returns the index of the item in the collection.</returns>
		public Int32 Add( MultiViewItem item ) {
			this.AddAt( -1, item );
			return ( this.owner.Controls.Count - 1 ); 
		}

		/// <summary>
		/// Adds the given <see cref="MultiViewItem"/> to the collection at the given index.
		/// </summary>
		public void AddAt( int index, MultiViewItem item ) {
			this.owner.Controls.AddAt( index, item );
		}

		/// <summary>
		/// Adds the given array of <see cref="MultiViewItem"/> controls to the collection.
		/// </summary>
		public void AddRange( MultiViewItem[] items ) {
			if ( items == null ) {
				throw new ArgumentNullException("items");
			}

			for ( Int32 i = 0; i < items.Length; i++ ) {
				this.Add( items[i] );
			}
 
		}

		/// <summary>
		/// Gets the index of the given item.
		/// </summary>
		public int GetItemIndex( MultiViewItem item ) {
			if( this.owner.HasControls() ) {
				return this.owner.Controls.IndexOf( item ); 
			}
			return -1;
		}

		/// <summary>
		/// Removes the given <see cref="MultiViewItem"/> from the collection.
		/// </summary>
		public void Remove( MultiViewItem item ) {
			this.owner.Controls.Remove( item );
		}

		/// <summary>
		/// Gets the <see cref="MultiViewItem" /> in the collection at the given index.
		/// </summary>
		public MultiViewItem this[ int index ] {
			get {
				return this.owner.Controls[ index ] as MultiViewItem;
			}
		}


		#endregion

		#region IList Members

		/// <summary>
		/// Returns false to indicate that this collection is not read only.
		/// </summary>
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		object IList.this[ int index ] {
			get {
				return this.owner.Controls[ index ];
			}
			set {
				this.RemoveAt( index );
				this.AddAt( index, (MultiViewItem)value );
			}
		}

		/// <summary>
		/// Removes the <see cref="MultiViewItem"/> at the given index.
		/// </summary>
		public void RemoveAt( int index ) {
			this.owner.Controls.RemoveAt( index );
		}

		void IList.Insert( int index, object value ) {
			this.owner.Controls.AddAt( index, (MultiViewItem)value );
		}

		void IList.Remove( object value ) {
			this.owner.Controls.Remove( (MultiViewItem)value );
		}

		bool IList.Contains( object value ) {
			return this.owner.Controls.Contains( (MultiViewItem)value );
		}

		/// <summary>
		/// Clears the collection.
		/// </summary>
		public void Clear() {
			if ( this.owner.HasControls() ) {
				foreach( Control control in this.owner.Controls ) {
					this.owner.Controls.Remove( control );
				}
			}
		}

		/// <summary>
		/// Gets the index in the collection of the given <see cref="MultiViewItem"/>
		/// </summary>
		public int IndexOf( object value ) {
			return this.owner.Controls.IndexOf( (MultiViewItem)value );
		}

		int IList.Add( object value ) {
			return this.Add( (MultiViewItem)value );
		}

		bool IList.IsFixedSize {
			get {
				return false;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Returns false to indicate that this collection is not syncronized.
		/// </summary>
		/// <exclude />
		public bool IsSynchronized {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the number of <see cref="MultiViewItem"/> controls contained in the collection.
		/// </summary>
		public int Count {
			get {
				if( this.owner.HasControls() ) {
					return this.owner.Controls.Count;
				}
				return 0;
			}
		}

		/// <summary>
		/// Copies the collection to the given array.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "index+1" )]
		public void CopyTo( Array array, int index ) {
			foreach( Object item in this ) {
				index++;
				array.SetValue( item, index );
			}
		}

		/// <summary>
		/// Returns the current collection object.
		/// </summary>
		/// <exclude />
		public object SyncRoot {
			get {
				return this;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator() {
			return this.owner.Controls.GetEnumerator();
		}

		#endregion

	}
}
