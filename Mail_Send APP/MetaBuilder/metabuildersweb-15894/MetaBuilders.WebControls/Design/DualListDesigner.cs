using System;
using System.Web.UI.Design.WebControls;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;

namespace MetaBuilders.WebControls.Design
{

	internal partial class DualListDesigner : ControlDesigner
	{

		#region Smart Tag

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection actions = new DesignerActionListCollection();
				actions.AddRange( base.ActionLists );
				actions.Add( new DualListActionList( this ) );
				return actions;
			}
		}

		internal void EditLeftItems()
		{
			PropertyDescriptor propertyToEdit = TypeDescriptor.GetProperties( this.Component )[ "LeftItems" ];
			ControlDesigner.InvokeTransactedChange( base.Component, new TransactedChangeCallback( this.EditItemsCallback ), propertyToEdit, Resources.DualList_EditLeftItemsEffectDescription, propertyToEdit );
		}

		internal void EditRightItems()
		{
			PropertyDescriptor propertyToEdit = TypeDescriptor.GetProperties( this.Component )[ "RightItems" ];
			ControlDesigner.InvokeTransactedChange( base.Component, new TransactedChangeCallback( this.EditItemsCallback ), propertyToEdit, Resources.DualList_EditRightItemsEffectDescription, propertyToEdit );
		}

		private bool EditItemsCallback( object context )
		{
			ListItemsCollectionEditor itemsEditor = new ListItemsCollectionEditor( typeof( ListItemCollection ) );

			IDesignerHost designerHost = (IDesignerHost)this.GetService( typeof( IDesignerHost ) );
			PropertyDescriptor editedProperty = (PropertyDescriptor)context;

			ITypeDescriptorContext typeDescriptorContext = new TypeDescriptorContext( designerHost, editedProperty, base.Component );
			IServiceProvider serviceProvider = new WindowsFormsEditorService( this );

			itemsEditor.EditValue( typeDescriptorContext, serviceProvider, editedProperty.GetValue( base.Component ) );
			return true;
		}
		
		#endregion

		public override string GetDesignTimeHtml()
		{
			this.CreateChildControls();
			return base.GetDesignTimeHtml();
		}

		protected virtual void CreateChildControls()
		{
			ICompositeControlDesignerAccessor designerAccessor = (ICompositeControlDesignerAccessor)base.ViewControl;
			designerAccessor.RecreateChildControls();

			this.ViewControlCreated = false;
			
			DataBindLists();
		}

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		private void DataBindLists()
		{
			DualList view = this.ViewControl as DualList;
			if ( view != null )
			{
				ApplyListDataSource( view.leftBox );
				ApplyListDataSource( view.rightBox );
			}
		}

		private static void ApplyListDataSource( ListBox viewList ) {
			Boolean isDataBound = ( viewList.DataSource != null || !String.IsNullOrEmpty( viewList.DataSourceID ) );
			if ( viewList.Items.Count == 0 || isDataBound )
			{
				if ( isDataBound )
				{
					viewList.Items.Clear();
					viewList.Items.Add( Resources.DataBound );
				}
				else
				{
					viewList.Items.Add( Resources.UnDataBound );
				}
			}
		}

	}
}
