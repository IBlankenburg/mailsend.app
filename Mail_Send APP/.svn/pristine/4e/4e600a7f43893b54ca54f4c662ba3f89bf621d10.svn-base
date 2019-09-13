using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace MetaBuilders.WebControls.Design
{
	/// <summary>
	/// The converter to show controls in a DropDownList for the TargetControl property of the ExpandingButtons controls.
	/// </summary>
	public class ExpandingButtonTargetControlConverter : StringConverter
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
		public override StandardValuesCollection GetStandardValues( ITypeDescriptorContext context )
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
					 !String.IsNullOrEmpty(serverControl.ID ) &&
					 IncludeControl( serverControl )
					)
				{
					availableControls.Add( serverControl.ID );
				}
			}
			availableControls.Sort( Comparer<String>.Default );
			return availableControls.ToArray();
		}

		#endregion

		//Override this method to customize which controls show up in the list
		/// <exclude />
		protected virtual Boolean IncludeControl( Control serverControl )
		{
			return true;
		}
	}
}

