using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Choices for the type of list to show as the editor for the LookupField DataControlField.
	/// </summary>
	public enum ListType
	{
		/// <summary>
		/// Indicates that the editor will be a DropDownList control.
		/// </summary>
		DropDown = 0,

		/// <summary>
		/// Indicates that the editor will be a ListBox control.
		/// </summary>
		ListBox = 1,

		/// <summary>
		/// Indicates that the editor will be a RadioButtonList control.
		/// </summary>
		RadioButtons = 2
	}
}
