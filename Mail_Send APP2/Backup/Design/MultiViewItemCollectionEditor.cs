using System;
using System.Reflection;

namespace MetaBuilders.WebControls.Design {

	/// <exclude />
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), 
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" )
	]
	public class MultiViewItemCollectionEditor : System.ComponentModel.Design.CollectionEditor
	{

		/// <exclude />
		public MultiViewItemCollectionEditor( Type type )
			: base( type )
		{
		}

		/// <exclude />
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		/// <exclude />
		protected override object CreateInstance( Type itemType )
		{
			return Activator.CreateInstance(itemType, (BindingFlags)532, null, null, null); 
		}

	}
}
