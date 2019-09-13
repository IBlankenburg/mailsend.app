using System;
using System.Web.UI.Design;
using System.ComponentModel;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>
	/// Simple designer for <see cref="OneClick"/> which shows a placeholder.
	/// </summary>
	/// <exclude/>
	public class OneClickDesigner : ControlDesigner
	{

		/// <exclude/>
		public override string GetDesignTimeHtml()
		{
			return this.CreatePlaceHolderDesignTimeHtml();
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands" )]
		protected override void PreFilterProperties( System.Collections.IDictionary properties )
		{
			base.PreFilterProperties( properties );

			string[] propertiesToHide = { "Visible", "EnableViewState" };

			foreach ( string propname in propertiesToHide )
			{
				PropertyDescriptor prop = (PropertyDescriptor)properties[ propname ];
				if ( prop != null )
				{
					AttributeCollection runtimeAttributes = prop.Attributes;
					// make a copy of the original attributes 
					// but make room for one extra attribute
					Attribute[] attrs = new Attribute[ runtimeAttributes.Count + 1 ];
					runtimeAttributes.CopyTo( attrs, 0 );
					attrs[ runtimeAttributes.Count ] = BrowsableAttribute.No;
					prop = TypeDescriptor.CreateProperty( this.GetType(), propname, prop.PropertyType, attrs );
					properties[ propname ] = prop;
				}
			}

		}

	}
}
