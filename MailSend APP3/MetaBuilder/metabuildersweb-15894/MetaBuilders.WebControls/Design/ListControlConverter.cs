using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Collections;

namespace MetaBuilders.WebControls.Design
{
	/// <summary>
	/// Displays a DropDownList of all list controls within the control's container.
	/// </summary>
	/// <exclude />
	class ListControlConverter : TypeConverter
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
			return false;
		}

		#endregion

		#region Display Control IDs In List

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues( ITypeDescriptorContext context )
		{
			if ( ( context == null ) || ( context.Container == null ) )
			{
				return null;
			}
			Object[] serverControls = this.GetControls( context.Container );
			if ( serverControls != null )
			{
				return new StandardValuesCollection( serverControls );
			}
			return null;
		}

		private object[] GetControls( IContainer container )
		{
			ArrayList availableControls = new ArrayList();
			foreach ( IComponent component in container.Components )
			{
				Control serverControl = component as Control;
				if ( serverControl != null &&
					 !( serverControl is Page ) &&
					 serverControl.ID != null &&
					 serverControl.ID.Length != 0 &&
					 IncludeControl( serverControl )
					)
				{
					availableControls.Add( serverControl.ID );
				}
			}

			availableControls.Sort( StringComparer.Ordinal );
			return availableControls.ToArray();
		}

		#endregion

		//Override this method to customize which controls show up in the list
		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		private Boolean IncludeControl( Control serverControl )
		{
			if ( serverControl is ListControl )
			{
				return true;
			}
			return false;
		}
	}
}
