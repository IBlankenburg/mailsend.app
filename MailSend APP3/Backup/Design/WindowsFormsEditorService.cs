using System;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace MetaBuilders.WebControls.Design
{
	internal partial class DualListDesigner
	{
		/// <summary>
		/// The ListItemsCollectionEditor used in the DualListDesigner's Smart Tag requires an implementation of IServiceProvider
		/// which provides an IWindowsFormsEditorService.
		/// Unfortunately all the built in versions are marked internal, so one must be trivally implemented.
		/// </summary>
		protected internal sealed class WindowsFormsEditorService : IWindowsFormsEditorService, IServiceProvider
		{

			public WindowsFormsEditorService( ComponentDesigner componentDesigner )
			{
				this._componentDesigner = componentDesigner;
			}

			#region IServiceProvider

			public Object GetService( Type serviceType )
			{
				if ( serviceType == typeof( IWindowsFormsEditorService ) )
				{
					return this;
				}
				if ( this._componentDesigner != null && this._componentDesigner.Component != null && this._componentDesigner.Component.Site != null )
				{
					return this._componentDesigner.Component.Site.GetService( serviceType );
				}
				return null;
			}

			#endregion

			#region IWindowsFormsEditorService

			void IWindowsFormsEditorService.CloseDropDown()
			{
			}

			void IWindowsFormsEditorService.DropDownControl( Control control )
			{
			}

			DialogResult IWindowsFormsEditorService.ShowDialog( Form dialog )
			{
				IUIService uiService = (IUIService)GetService( typeof( IUIService ) );
				if ( uiService != null )
				{
					return uiService.ShowDialog( dialog );
				}
				return dialog.ShowDialog();
			}

			#endregion

			private ComponentDesigner _componentDesigner;
		}
	}
}