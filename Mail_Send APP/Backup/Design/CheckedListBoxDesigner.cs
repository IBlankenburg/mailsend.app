using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls.Design
{

	/// <exclude />
	public class CheckedListBoxDesigner : ListControlDesigner
	{

		/// <exclude />
		protected override void PostFilterProperties( System.Collections.IDictionary properties )
		{
			base.PostFilterProperties( properties );

			ReplaceDefaultValue( properties, "BorderStyle", new DefaultValueAttribute( BorderStyle.Inset ) );
			ReplaceDefaultValue( properties, "BorderWidth", new DefaultValueAttribute( typeof( Unit ), "2px" ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands" )]
		private static void ReplaceDefaultValue( IDictionary properties, String name, DefaultValueAttribute newDefault )
		{
			PropertyDescriptor propertyToChange = (PropertyDescriptor)properties[ name ];
			Attribute[] newAttributes = new Attribute[ propertyToChange.Attributes.Count ];
			for ( int i = 0; i < propertyToChange.Attributes.Count; i++ )
			{
				Attribute attr = propertyToChange.Attributes[ i ];
				if ( attr is DefaultValueAttribute )
				{
					newAttributes[ i ] = newDefault;
				}
				else
				{
					newAttributes[ i ] = propertyToChange.Attributes[ i ];
				}
			}
			propertyToChange = TypeDescriptor.CreateProperty( typeof( CheckedListBox ), propertyToChange, newAttributes );
			properties[ name ] = propertyToChange;
		}

	}
}
