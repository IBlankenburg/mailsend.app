using System;
using System.Web.UI.HtmlControls;

namespace MetaBuilders.WebControls
{
	/// <summary>
	/// The BooleanSelectionMode enumeration defines different types of editing interfaces for boolean data in a datagrid.
	/// </summary>
	public enum BooleanSelectionMode
	{
		/// <summary>
		/// Allows editing the field with a simple <see cref="HtmlInputCheckBox"/> control.
		/// </summary>
		CheckBox,
		/// <summary>
		/// Allows editing the field with a <see cref="DropDownList"/> control.
		/// </summary>
		DropDownList,
		/// <summary>
		/// Allows editing the field with a <see cref="RadioButtonList"/> control.
		/// </summary>
		RadioButtonList
	}
}
