using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls.Design
{
	/// <summary>
	/// Displays a DropDownList of all button controls within the control's container.
	/// </summary>
	/// <exclude />
	public class ButtonControlConverter : StringConverter
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
					 serverControl.ID != null &&
					 serverControl.ID.Length != 0 &&
					 IncludeControl( serverControl )
					)
				{
					availableControls.Add( serverControl.ID );
				}
			}
			availableControls.Sort( Comparer.Default );
			return availableControls.ToArray();
		}

		#endregion

		//Override this method to customize which controls show up in the list
		/// <exclude />
		protected virtual Boolean IncludeControl( Control serverControl )
		{
			if ( serverControl is IButtonControl )
			{
				return true;
			}
			if ( serverControl is System.Web.UI.HtmlControls.HtmlInputButton )
			{
				return true;
			}
			if ( serverControl is System.Web.UI.HtmlControls.HtmlButton )
			{
				return true;
			}
			return false;
		}
	}
}

