using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace MetaBuilders.WebControls.Design {

	/// <exclude />
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" )]
	public class MultiViewBarCurrentItemConverter : StringConverter
	{
		
		#region Make It A ComboBox

		/// <exclude />
		public override bool GetStandardValuesSupported( ITypeDescriptorContext context )
		{
			return true;
		}

		/// <exclude />
		public override bool GetStandardValuesExclusive( ITypeDescriptorContext context )
		{
			return true;
		}

		#endregion

		#region Display Item Titles In List

		/// <exclude />
		public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context )
		{
			if ( context == null || !(context.Instance is MultiViewBar ) ) {
				return null;
			}

			Object[] items = this.GetItems( ( context.Instance as MultiViewBar ) );
			if ( items != null ) {
				return new StandardValuesCollection( items ); 
			}
			return null; 
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		private object[] GetItems( MultiViewBar viewBar ) {
			if ( viewBar == null ) {
				return null;
			}

			ArrayList availableItems = new ArrayList();
			foreach( MultiViewItem item in viewBar.Items ) {
				availableItems.Add( item.Title );
			}
			return availableItems.ToArray(); 
		}

		#endregion

	}
}
