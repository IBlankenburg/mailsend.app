using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.ComponentModel.Design;


namespace MetaBuilders.WebControls.Design
{
	internal partial class DualListDesigner
	{

		private class DualListActionList : DesignerActionList
		{
			#region Ctor

			public DualListActionList( DualListDesigner designer )
				: base( designer.Component )
			{
				_designer = designer;
			}

			#endregion

			#region Action Backing Methods

			[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
			public void EditLeftItems()
			{
				this._designer.EditLeftItems();
			}

			[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
			public void EditRightItems()
			{
				this._designer.EditRightItems();
			}

			#endregion

			#region Action Backing Properties

			[  System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
			public Boolean EnableMoveAll
			{
				get
				{
					return ( (DualList)this._designer.Component ).EnableMoveAll;
				}
				set
				{
					PropertyDescriptor property = TypeDescriptor.GetProperties( this._designer.Component )[ "EnableMoveAll" ];
					property.SetValue( this._designer.Component, value );
				}
			}

			[  System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" )]
			public Boolean EnableMoveUpDown
			{
				get
				{
					return ( (DualList)this._designer.Component ).EnableMoveUpDown;
				}
				set
				{
					PropertyDescriptor property = TypeDescriptor.GetProperties( this._designer.Component )[ "EnableMoveUpDown" ];
					property.SetValue( this._designer.Component, value );
				}
			}

			#endregion

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection actions = new DesignerActionItemCollection();
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties( this._designer.Component );
				PropertyDescriptor actionableProperty;

				actionableProperty = properties[ "LeftItems" ];
				if ( actionableProperty != null && actionableProperty.IsBrowsable )
				{
					DesignerActionMethodItem editLeftItemsAction = new DesignerActionMethodItem( this, "EditLeftItems", Resources.DualList_EditLeftItems, "LeftData", Resources.DualList_EditLeftItemsEffectDescription );
					actions.Add( editLeftItemsAction );
				}

				actionableProperty = properties[ "RightItems" ];
				if ( actionableProperty != null && actionableProperty.IsBrowsable )
				{
					actions.Add( new DesignerActionMethodItem( this, "EditRightItems", Resources.DualList_EditRightItems, "RightData", Resources.DualList_EditRightItemsEffectDescription ) );
				}

				DesignerActionHeaderItem behaviorHeader = new DesignerActionHeaderItem( "Behavior", "Behavior" );
				actions.Add( behaviorHeader );

				actionableProperty = properties[ "EnableMoveAll" ];
				if ( ( actionableProperty != null ) && actionableProperty.IsBrowsable )
				{
					actions.Add( new DesignerActionPropertyItem( "EnableMoveAll", Resources.DualList_ToggleEnableMoveAll, "Behavior", Resources.DualList_ToggleEnableMoveAllDescription ) );
				}

				actionableProperty = properties[ "EnableMoveUpDown" ];
				if ( ( actionableProperty != null ) && actionableProperty.IsBrowsable )
				{
					actions.Add( new DesignerActionPropertyItem( "EnableMoveUpDown", Resources.DualList_ToggleEnableMoveUpDown, "Behavior", Resources.DualList_ToggleEnableMoveUpDownDescription ) );
				}

				return actions;
			}

			DualListDesigner _designer;
		}
	}
}
