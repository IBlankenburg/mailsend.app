using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A ListBox which suports grouping of the items.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Grouping is done via the html optgroup element.
	/// This means that the group headers are not selectable and sub-groups are not supported.
	/// </para>
	/// <para>
	/// To use groups when databinding to a datasource use the DataGroupField property.
	/// Set this property to the name of the field in your data source which states to which group the item belongs.
	/// For the items to group properly, sort your data source by group.
	/// </para>
	/// <para>
	/// To use groups when manually adding ListItems to the Items collection, use the Attributes collection of the ListItem.
	/// Set the "Group" attribute of the ListItem to the name of the group to which the item belongs.
	/// Makes sure your items are ordered firstly by group.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following is an example of manually adding the group attribute.
	/// <code>
	/// &lt;mbgl:GroupedListBox ID="GroupedListBox1" runat="server" Rows="5" &gt;
	/// 	&lt;asp:ListItem Group="Group1"&gt;1&lt;/asp:ListItem&gt;
	/// 	&lt;asp:ListItem Group="Group1"&gt;2&lt;/asp:ListItem&gt;
	/// 	&lt;asp:ListItem Group="Group2"&gt;1&lt;/asp:ListItem&gt;
	/// 	&lt;asp:ListItem Group="Group2"&gt;2&lt;/asp:ListItem&gt;
	/// &lt;/mbgl:GroupedListBox&gt;
	/// </code>
	/// </example>
	[
	Designer( typeof( MetaBuilders.WebControls.Design.GroupedListControlDesigner ) ),
	]
	public class GroupedListBox : ListBox
	{

		#region Properties

		/// <summary>
		/// Gets or sets the field of the data source that provides the group of the list items. 
		/// </summary>
		[
		Description( "Gets or sets the field of the data source that provides the group of the list items." ),
		Bindable( true ),
		Themeable( false ),
		Category( "Data" ),
		DefaultValue( "" ),
		TypeConverter( typeof( System.Web.UI.Design.DataFieldConverter ) ),
		]
		public virtual String DataGroupField
		{
			get
			{
				Object savedState = this.ViewState[ "DataGroupField" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return String.Empty;
			}
			set
			{
				this.ViewState[ "DataGroupField" ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the formatting string used to control how group data bound to the list control is displayed. 
		/// </summary>
		[
		Description( "Gets or sets the formatting string used to control how group data bound to the list control is displayed." ),
		Bindable( true ),
		Themeable( false ),
		Category( "Data" ),
		DefaultValue( _DataGroupFormatStringDefault ),
		]
		public virtual String DataGroupFormatString
		{
			get
			{
				Object state = ViewState[ "DataGroupFormatString" ];
				if ( state != null )
				{
					return (String)state;
				}
				return _DataGroupFormatStringDefault;
			}
			set
			{
				ViewState[ "DataGroupFormatString" ] = value;
			}
		}
		private const String _DataGroupFormatStringDefault = "";

		#endregion
		
		#region Methods

		/// <exlude/>
		protected override void PerformDataBinding( IEnumerable data )
		{
			base.PerformDataBinding( data );

			if ( data != null )
			{
				Boolean areDataFieldsSpecified = false;
				Boolean isFormatStringSpecified = false;

				String displayField = this.DataTextField;
				String valueField = this.DataValueField;

				if ( !this.AppendDataBoundItems )
				{
					this.Items.Clear();
				}

				if ( ( displayField.Length != 0 ) || ( valueField.Length != 0 ) )
				{
					areDataFieldsSpecified = true;
				}
				if ( DataTextFormatString.Length != 0 )
				{
					isFormatStringSpecified = true;
				}

				foreach ( object dataItem in data )
				{
					ListItem item = new ListItem();
					if ( areDataFieldsSpecified )
					{
						if ( displayField.Length > 0 )
						{
							item.Text = DataBinder.GetPropertyValue( dataItem, displayField, DataTextFormatString );
						}
						if ( valueField.Length > 0 )
						{
							item.Value = DataBinder.GetPropertyValue( dataItem, valueField, null );
						}
					}
					else
					{
						if ( isFormatStringSpecified )
						{
							item.Text = String.Format( CultureInfo.CurrentCulture, DataTextFormatString, dataItem );
						}
						else
						{
							item.Text = dataItem.ToString();
						}
						item.Value = dataItem.ToString();
					}
					if ( DataGroupField.Length > 0 )
					{
						item.Attributes[ "group" ] = DataBinder.GetPropertyValue( dataItem, DataGroupField, null );
					}
					this.Items.Add( item );
				}
			}
		}

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength" )]
		protected override void RenderContents( HtmlTextWriter writer )
		{

			String currentGroup = "";
			String itemGroup = "";
			Boolean groupOpen = false;
			Boolean selectedItemRendered = false;

			foreach ( ListItem item in this.Items )
			{
				if ( item.Enabled )
				{

					itemGroup = item.Attributes[ "group" ] ?? "";

					if ( itemGroup != currentGroup )
					{
						if ( !String.IsNullOrEmpty( currentGroup ) )
						{
							writer.WriteEndTag( "optgroup" );
							groupOpen = false;
						}
						if ( !String.IsNullOrEmpty( itemGroup ) )
						{
							writer.WriteBeginTag( "optgroup" );
							if ( String.IsNullOrEmpty( this.DataGroupFormatString ) )
							{
								writer.WriteAttribute( "label", itemGroup, true );
							}
							else
							{
								writer.WriteAttribute( "label", String.Format( CultureInfo.CurrentCulture, this.DataGroupFormatString, itemGroup ), true );
							}
							writer.Write( ">" );
							groupOpen = true;
						}
						currentGroup = item.Attributes[ "group" ];
					}

					writer.WriteBeginTag( "option" );
					if ( item.Selected )
					{
						if ( selectedItemRendered )
						{
							this.VerifyMultiSelect();
						}
						selectedItemRendered = true;
						writer.WriteAttribute( "selected", "selected" );
					}

					writer.WriteAttribute( "value", item.Value, true );

					item.Attributes.Remove( "group" );
					item.Attributes.Remove( "value" );
					item.Attributes.Remove( "selected" );

					item.Attributes.Render( writer );

					if ( this.Page != null )
					{
						this.Page.ClientScript.RegisterForEventValidation( this.UniqueID, item.Value );
					}

					writer.Write( ">" );
					System.Web.HttpUtility.HtmlEncode( item.Text, writer );
					writer.WriteEndTag( "option" );
					writer.WriteLine();

				}

			} // end foreach Item

			if ( groupOpen )
			{
				writer.WriteEndTag( "optgroup" );
				writer.WriteLine();
			}

		}

		#endregion

	}
}
