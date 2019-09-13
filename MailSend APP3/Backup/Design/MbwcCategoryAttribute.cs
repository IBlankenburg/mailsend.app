using System;
using System.ComponentModel;

namespace MetaBuilders.WebControls.Design
{
	internal sealed class MbwcCategoryAttribute : CategoryAttribute
	{
		public MbwcCategoryAttribute( String category )
			: base( category )
		{
		}

		protected override string GetLocalizedString( String value )
		{
			string localizedValue = base.GetLocalizedString( value );
			if ( localizedValue == null )
			{
				localizedValue = Resources.ResourceManager.GetString( "Category_" + value );
			}
			return localizedValue;
		}

		public override object TypeId
		{
			get
			{
				return typeof( CategoryAttribute );
			}
		}
 

	}
}
