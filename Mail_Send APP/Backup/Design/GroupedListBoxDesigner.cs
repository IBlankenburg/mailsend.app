using System;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;
using System.ComponentModel.Design;

namespace MetaBuilders.WebControls.Design
{

	/// <exclude />
	public class GroupedListControlDesigner : ListControlDesigner
	{

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands" ), ]
		public override System.ComponentModel.Design.DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection collection = new DesignerActionListCollection();
				collection.AddRange( base.ActionLists );
				collection.Add( new GroupedListControlActionList( this ) );
				return collection;
			}
		}

		class GroupedListControlActionList : DesignerActionList
		{

			#region Ctor

			public GroupedListControlActionList( GroupedListControlDesigner designer )
				: base( designer.Component )
			{
				_designer = designer;
			}

			#endregion

			// Couldn't Get The Editor To Work :(
			//[Editor( typeof( DataGroupFieldTypeEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
			[  System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
			public String DataGroupField
			{
				get
				{
					PropertyDescriptor property = TypeDescriptor.GetProperties( this._designer.Component )[ "DataGroupField" ];
					Object value = property.GetValue( this._designer.Component );
					return (String)value;
				}
				set
				{
					PropertyDescriptor property = TypeDescriptor.GetProperties( this._designer.Component )[ "DataGroupField" ];
					property.SetValue( this._designer.Component, value );
				}
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection items = new DesignerActionItemCollection();

				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties( this._designer.Component );
				PropertyDescriptor actionableProperty;

				actionableProperty = properties[ "DataGroupField" ];
				if ( actionableProperty != null && actionableProperty.IsBrowsable )
				{
					DesignerActionPropertyItem dataGroupField = new DesignerActionPropertyItem( "DataGroupField", "DataGroupField", "Data" );
					items.Add( dataGroupField );
				}

				return items;

			}

			GroupedListControlDesigner _designer;

		}
	}
}
