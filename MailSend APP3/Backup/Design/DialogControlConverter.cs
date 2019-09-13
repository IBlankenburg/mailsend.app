using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace MetaBuilders.WebControls.Design {

	/// <summary>
	/// Gives better designer support for the DialogToOpen properties of DialogOpenButton and DialogOpenLink.
	/// </summary>
	/// <exclude/>
	public class DialogControlConverter : StringConverter {
		
		#region Make It A ComboBox
		/// <summary>
		/// Makes the converter a combobox
		/// </summary>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
			return true;
		}
		/// <summary>
		/// Makes the converter a combobox
		/// </summary>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
			return false;
		}
		#endregion
		
		#region Display Control IDs In List

		/// <summary>
		/// Gets a list of all the controls that derive from DialogWindowBase.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
			if ((context == null) || (context.Container == null)) {
				return null; 
			}
			Object[] serverControls = GetControls(context.Container);
			if (serverControls != null) {
				return new StandardValuesCollection(serverControls); 
			}
			return null; 
		}
		private static object[] GetControls(IContainer container) {
			ArrayList availableControls = new ArrayList();
			foreach( IComponent component in container.Components ) {
				Control serverControl = component as Control;
				if ( serverControl != null && 
					!(serverControl is Page) && 
					serverControl.ID != null && 
					serverControl.ID.Length != 0  && 
					IncludeControl(serverControl) 
					) {
					availableControls.Add(serverControl.ID);
				}
			}
			availableControls.Sort(Comparer.Default);
			return availableControls.ToArray(); 
		}
		#endregion
		
		private static Boolean IncludeControl( Control serverControl ) {
			return serverControl is DialogWindowBase;
		}
	}
}
