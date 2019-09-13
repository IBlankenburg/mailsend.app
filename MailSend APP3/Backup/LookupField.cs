using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.Design.WebControls;
using System.IO;
using System.Web;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Represents a data field that is a forieign key which looks up into another table.
	/// It is displayed as either a DropDownList, ListBox, or RadioButtonList in a data-bound control.
	/// </summary>
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), 
	]
	public class LookupField : DataControlField
	{

		#region Properties

		#region Main Properties

		/// <summary>
		/// Gets or sets the type of editor control used to select the value for the field.
		/// </summary>
		[
		Description( "Gets or sets the type of editor control used to select the value for the field." ),
		Category( "Appearance" ),
		DefaultValue( ListType.DropDown ),
		]
		public virtual ListType ListType
		{
			get
			{
				Object savedState = ViewState[ "ListType" ];
				if ( savedState != null )
				{
					return (ListType)savedState;
				}
				return ListType.DropDown;
			}
			set
			{
				Object savedState = this.ViewState[ "ListType" ];
				if ( ( savedState == null ) || ( ( ( (ListType)savedState ) != value ) ) && Enum.IsDefined( typeof( ListType ), value ) )
				{
					this.ViewState[ "ListType" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the name of the data field to bind to the LookupField object.
		/// </summary>
		[
		Description( "Gets or sets the name of the data field to bind to the LookupField object." ),
		Category( "Data" ),
		DefaultValue( "" ),
		TypeConverter( typeof( DataSourceViewSchemaConverter ) ),
		]
		public virtual String DataField
		{
			get
			{
				if ( this._dataField == null )
				{
					object savedState = this.ViewState[ "DataField" ];
					if ( savedState != null )
					{
						this._dataField = (String)savedState;
					}
					else
					{
						this._dataField = String.Empty;
					}
				}
				return this._dataField;
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "DataField" ] ) )
				{
					this.ViewState[ "DataField" ] = value;
					this._dataField = value;
					this.OnFieldChanged();
				}
			}
		}
		private String _dataField;

		/// <summary>
		/// Gets or sets a Boolean value deciding if the editor will allow null values.
		/// </summary>
		[
		Description( "Gets or sets a Boolean value deciding if the editor will allow null values." ),
		Category( "Data" ),
		DefaultValue( _AllowNullsDefault ),
		]
		public virtual Boolean AllowNulls
		{
			get
			{
				Object state = ViewState[ "AllowNulls" ];
				if ( state != null )
				{
					return (Boolean)state;
				}
				return _AllowNullsDefault;
			}
			set
			{
				Object savedState = this.ViewState[ "AllowNulls" ];
				if ( ( savedState == null ) || ( ( (Boolean)savedState ) != value ) )
				{
					this.ViewState[ "AllowNulls" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const Boolean _AllowNullsDefault = false;

		/// <summary>
		/// Gets or sets the caption displayed for a field when the field's value is null.
		/// </summary>
		[
		Description( "Gets or sets the caption displayed for a field when the field's value is null." ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		]
		public virtual String NullDisplayValue
		{
			get
			{
				object savedState = this.ViewState[ "NullDisplayValue" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "NullDisplayValue" ] ) )
				{
					this.ViewState[ "NullDisplayValue" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the value of the field can be modified in edit mode.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "ReadOnly" ), 
		Description( "Gets or sets a value indicating whether the value of the field can be modified in edit mode." ),
		Category( "Behavior" ),
		DefaultValue( false )
		]
		public virtual Boolean ReadOnly
		{
			get
			{
				Object savedState = this.ViewState[ "ReadOnly" ];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				Object savedState = this.ViewState[ "ReadOnly" ];
				if ( ( savedState == null ) || ( ( (Boolean)savedState ) != value ) )
				{
					this.ViewState[ "ReadOnly" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		#endregion

		#region Lookup Data Properties

		/// <summary>
		/// Gets or sets the data source for the selection list when the field is in edit mode.
		/// </summary>
		[
		Description( "Gets or sets the data source for the selection list when the field is in edit mode." ),
		DefaultValue( null ),
		Browsable( false ),
		]
		public virtual Object LookupDataSource
		{
			get
			{
				return ViewState[ "LookupDataSource" ];
			}
			set
			{
				if ( !Object.Equals( value, ViewState[ "LookupDataSource" ] ) )
				{
					ViewState[ "LookupDataSource" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the ID of the data source control for the selection list when the field is in edit mode.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID" ), 
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member" ), 
		Description( "Gets or sets the ID of the data source control for the selection list when the field is in edit mode." ),
		Category( "Data" ),
		DefaultValue( "" ),
		]
		public virtual String LookupDataSourceID
		{
			get
			{
				object savedState = this.ViewState[ "LookupDataSourceID" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "LookupDataSourceID" ] ) )
				{
					this.ViewState[ "LookupDataSourceID" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the data source member for the selection list when the field is in edit mode.
		/// </summary>
		[
		Description( "Gets or sets the data source member for the selection list when the field is in edit mode." ),
		Category( "Data" ),
		DefaultValue( "" ),
		]
		public virtual String LookupDataMember
		{
			get
			{
				object savedState = this.ViewState[ "LookupDataMember" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "LookupDataMember" ] ) )
				{
					this.ViewState[ "LookupDataMember" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source to be used as the value for the selection list when the field is in edit mode.
		/// </summary>
		[
		Description( "Gets or sets the field of the data source to be used as the value for the selection list when the cell is in edit mode." ),
		Category( "Data" ),
		DefaultValue( "" ),
		]
		public virtual String LookupDataValueField
		{
			get
			{
				object savedState = this.ViewState[ "LookupDataValueField" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "LookupDataValueField" ] ) )
				{
					this.ViewState[ "LookupDataValueField" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the field of the data source to be used as the visible text for the selection list when the cell is in edit mode.
		/// </summary>
		[
		Description( "Gets or sets the field of the data source to be used as the visible text for the selection list when the cell is in edit mode." ),
		Category( "Data" ),
		DefaultValue( "" ),
		]
		public virtual String LookupDataTextField
		{
			get
			{
				object savedState = this.ViewState[ "LookupDataTextField" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "LookupDataTextField" ] ) )
				{
					this.ViewState[ "LookupDataTextField" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the string that specifies the display format for items in the column.
		/// </summary>
		[
		Description( "Gets or sets the string that specifies the display format for items in the column." ),
		Category( "Data" ),
		DefaultValue( "" ),
		]
		public virtual String LookupDataFormatString
		{
			get
			{
				object savedState = this.ViewState[ "LookupDataFormatString" ];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				if ( !Object.Equals( value, this.ViewState[ "LookupDataFormatString" ] ) )
				{
					this.ViewState[ "LookupDataFormatString" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the databound status of LookupList.
		/// </summary>
		protected virtual Boolean ListIsDataBound
		{
			get
			{
				object savedValue;

				savedValue = this.ViewState[ "ListIsDataBound" ];
				if ( savedValue != null )
				{
					return (Boolean)savedValue;
				}
				return false;
			}
			set
			{
				this.ViewState[ "ListIsDataBound" ] = value;
			}
		}

		#endregion

		#region ListBox Type Properties

		/// <summary>
		/// Gets or sets the number of rows displayed in the ListBox control.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.ListBox.</para>
		/// <para>This property represents the height of the ListBox editor control.</para>
		/// </remarks>
		[
		Description( "Gets or sets the number of rows displayed in the ListBox control." ),
		MbwcCategory( "ListBoxAppearance" ),
		DefaultValue( _ListBoxRowsDefault ),
		]
		public virtual Int32 ListBoxRows
		{
			get
			{
				Object state = ViewState[ "ListBoxRows" ];
				if ( state != null )
				{
					return (Int32)state;
				}
				return _ListBoxRowsDefault;
			}
			set
			{
				Object savedState = this.ViewState[ "ListBoxRows" ];
				if ( savedState == null || ( (Int32)savedState ) != value )
				{
					ViewState[ "ListBoxRows" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const Int32 _ListBoxRowsDefault = 8;

		#endregion

		#region Radio Type Properties

		/// <summary>
		/// Gets or sets the distance (in pixels) between the border and the contents of the table cell.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.RadioButtons
		/// and the RadioRepeatLayout property is set to RepeatLayout.Table.</para>
		/// <para>Use this property to control the spacing between the contents of a cell and the cell's border.</para>
		/// <para>The padding amount specified is added to all four sides of a cell. 
		/// It uses the height of the tallest cell in the table and the width of the widest cell in the table. 
		/// The resulting cell size is applied uniformly to all cells in the table. 
		/// Individual cell sizes cannot be specified.</para>
		/// </remarks>
		[
		Description( "Gets or sets the distance (in pixels) between the border and the contents of the table cell." ),
		MbwcCategory( "RadioButtonsAppearance" ),
		DefaultValue( _RadioCellPaddingDefault ),
		]
		public virtual Int32 RadioCellPadding
		{
			get
			{
				Object state = ViewState[ "RadioCellPadding" ];
				if ( state != null )
				{
					return (Int32)state;
				}
				return _RadioCellPaddingDefault;
			}
			set
			{
				Object savedState = this.ViewState[ "RadioCellPadding" ];
				if ( savedState == null || (Int32)savedState != value )
				{
					ViewState[ "RadioCellPadding" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const Int32 _RadioCellPaddingDefault = -1;

		/// <summary>
		/// Gets or sets the distance (in pixels) between adjacent table cells.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.RadioButtons
		/// and the RadioRepeatLayout property is set to RepeatLayout.Table.</para>
		/// <para>Use this property to control the spacing between individual cells in the table. 
		/// This property is applied both vertically and horizontally.</para>
		/// </remarks>
		[
		Description( "Gets or sets the distance (in pixels) between adjacent table cells." ),
		MbwcCategory( "RadioButtonsAppearance" ),
		DefaultValue( _RadioCellSpacingDefault ),
		]
		public virtual Int32 RadioCellSpacing
		{
			get
			{
				Object state = ViewState[ "RadioCellSpacing" ];
				if ( state != null )
				{
					return (Int32)state;
				}
				return _RadioCellSpacingDefault;
			}
			set
			{
				Object savedState = ViewState[ "RadioCellSpacing" ];
				if ( savedState == null || (Int32)savedState != value )
				{
					ViewState[ "RadioCellSpacing" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const Int32 _RadioCellSpacingDefault = -1;

		/// <summary>
		/// Gets or sets the number of columns to display in the RadioButtonList control when editing the field in radiobutton mode.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.RadioButtons.</para>
		/// <para>Use this property to specify the number of columns that display items in the RadioButtonList control. 
		/// If this property is not set, the RadioButtonList control will display all items in the list in a single column.</para>
		/// </remarks>
		[
		Description( "Gets or sets the number of columns to display in the RadioButtonList control when editing the field in radiobutton mode." ),
		MbwcCategory( "RadioButtonsAppearance" ),
		DefaultValue( _RadioRepeatColumnsDefault ),
		]
		public virtual Int32 RadioRepeatColumns
		{
			get
			{
				Object state = ViewState[ "RadioRepeatColumns" ];
				if ( state != null )
				{
					return (Int32)state;
				}
				return _RadioRepeatColumnsDefault;
			}
			set
			{
				Object savedState = ViewState[ "RadioRepeatColumns" ];
				if ( savedState == null || (Int32)savedState != value )
				{
					ViewState[ "RadioRepeatColumns" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const Int32 _RadioRepeatColumnsDefault = 0;

		/// <summary>
		/// Gets or sets the direction in which the radio buttons are displayed when editing the field in radiobutton mode.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.RadioButtons.</para>
		/// <para>Use this property to specify the display direction of the RadioButtonList control.</para>
		/// </remarks>
		[
		Description( "Gets or sets the direction in which the radio buttons are displayed when editing the field in radiobutton mode." ),
		MbwcCategory( "RadioButtonsAppearance" ),
		DefaultValue( _RepeatDirectionDefault ),
		]
		public virtual RepeatDirection RadioRepeatDirection
		{
			get
			{
				Object state = ViewState[ "RadioRepeatDirection" ];
				if ( state != null )
				{
					return (RepeatDirection)state;
				}
				return _RepeatDirectionDefault;
			}
			set
			{
				Object savedState = ViewState[ "RadioRepeatDirection" ];
				if ( savedState == null || (RepeatDirection)savedState != value )
				{
					ViewState[ "RadioRepeatDirection" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const RepeatDirection _RepeatDirectionDefault = RepeatDirection.Vertical;

		/// <summary>
		/// Gets or sets the layout of radio buttons within the group when editing the field in radiobutton mode.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.RadioButtons.</para>
		/// <para>Use this property to specify whether the items in the RadioButtonList control are displayed in a table. 
		/// If this property is set to RepeatLayout.Table, the items in the list are displayed in a table.
		/// If this property is set to RepeatLayout.Flow, the items in the list are displayed without a table structure.</para>
		/// </remarks>
		[
		Description( "Gets or sets the layout of radio buttons within the group when editing the field in radiobutton mode." ),
		MbwcCategory( "RadioButtonsAppearance" ),
		DefaultValue( _RepeatLayoutDefault ),
		]
		public virtual RepeatLayout RadioRepeatLayout
		{
			get
			{
				Object state = ViewState[ "RadioRepeatLayout" ];
				if ( state != null )
				{
					return (RepeatLayout)state;
				}
				return _RepeatLayoutDefault;
			}
			set
			{
				Object state = ViewState[ "RadioRepeatLayout" ];
				if ( state == null || (RepeatLayout)state != value )
				{
					ViewState[ "RadioRepeatLayout" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const RepeatLayout _RepeatLayoutDefault = RepeatLayout.Flow;

		/// <summary>
		/// Gets or sets the text alignment for the radio buttons within the group when editing the field in radiobutton mode.
		/// </summary>
		/// <remarks>
		/// <para>This property applies only when the ListType property is set to ListType.RadioButtons.</para>
		/// <para>Use this property to specify whether the text associated with the radio buttons appears on the left or right. 
		/// If this property is set to TextAlign.Right, the text is displayed to the right of the radio button. 
		/// If this property is set to TextAlign.Left, the text is displayed to the left of the radio button.</para>
		/// </remarks>
		[
		Description( "Gets or sets the text alignment for the radio buttons within the group when editing the field in radiobutton mode." ),
		MbwcCategory( "RadioButtonsAppearance" ),
		DefaultValue( _RadioTextAlignDefault ),
		]
		public virtual TextAlign RadioTextAlign
		{
			get
			{
				Object state = ViewState[ "RadioTextAlign" ];
				if ( state != null )
				{
					return (TextAlign)state;
				}
				return _RadioTextAlignDefault;
			}
			set
			{
				Object state = ViewState[ "RadioTextAlign" ];
				if ( state == null || (TextAlign)state != value )
				{
					ViewState[ "RadioTextAlign" ] = value;
					this.OnFieldChanged();
				}
			}
		}
		private const TextAlign _RadioTextAlignDefault = TextAlign.Right;

		#endregion

		#endregion

		#region ViewState

		/// <exclude />
		protected override void LoadViewState( object savedState )
		{
			
			Pair state = savedState as Pair;
			if ( state == null )
			{
				base.LoadViewState( null );
				return;
			}

			base.LoadViewState( state.First );
			( (IStateManager)this.storageList.Items ).LoadViewState( state.Second );
		}

		/// <exclude />
		protected override object SaveViewState()
		{
			Object baseState = base.SaveViewState();
			Object listState = null;
			if ( this._storageList != null )
			{
				listState = ( (IStateManager)this._storageList.Items ).SaveViewState();
			}
			if ( baseState != null || listState != null )
			{
				return new Pair( baseState, listState );
			}
			return null;
		}

		#endregion

		#region Methods

		/// <exclude/>
		protected override DataControlField CreateField()
		{
			return new LookupField();
		}

		/// <exclude/>
		protected override void CopyProperties( DataControlField newField )
		{
			base.CopyProperties( newField );

			LookupField lookup = newField as LookupField;
			lookup.ListType = this.ListType;
			lookup.DataField = this.DataField;
			lookup.AllowNulls = this.AllowNulls;
			lookup.NullDisplayValue = this.NullDisplayValue;
			lookup.ReadOnly = this.ReadOnly;
			lookup.LookupDataFormatString = this.LookupDataFormatString;
			lookup.LookupDataMember = this.LookupDataMember;
			lookup.LookupDataSource = this.LookupDataSource;
			lookup.LookupDataSourceID = this.LookupDataSourceID;
			lookup.LookupDataTextField = this.LookupDataTextField;
			lookup.LookupDataValueField = this.LookupDataValueField;
			lookup.ListBoxRows = this.ListBoxRows;
			lookup.RadioCellPadding = this.RadioCellPadding;
			lookup.RadioCellSpacing = this.RadioCellSpacing;
			lookup.RadioRepeatColumns = this.RadioRepeatColumns;
			lookup.RadioRepeatDirection = this.RadioRepeatDirection;
			lookup.RadioRepeatLayout = this.RadioRepeatLayout;
			lookup.RadioTextAlign = this.RadioTextAlign;

		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override void ExtractValuesFromCell( System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly )
		{
			Object value = null;

			if ( cell == null || dictionary == null )
			{
				return;
			}

			if ( cell.Controls.Count > 0 )
			{
				Int32 indexOfEditorList = ( cell.Controls.Count == 2 ) ? 0 : 1;
				ListControl valueList = cell.Controls[ indexOfEditorList ] as ListControl;
				if ( valueList != null )
				{
					value = valueList.SelectedValue;
				}
			}
			if ( value != null || this.AllowNulls )
			{
				if ( dictionary.Contains( this.DataField ) )
				{
					dictionary[ this.DataField ] = value;
				}
				else
				{
					dictionary.Add( this.DataField, value );
				}
			}
		}

		/// <exclude/>
		public override bool Initialize( bool sortingEnabled, Control control )
		{
			Boolean baseValue = base.Initialize( sortingEnabled, control );
			this.Control.DataBinding += new EventHandler( Control_DataBinding );
			if ( this.storageList != null && this.storageList.Page != null )
			{
				this.storageList.Parent.Controls.Remove( this.storageList );
			}

			return baseValue;
		}

		void Control_DataBinding( object sender, EventArgs e )
		{
			this.ListIsDataBound = false;
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override void InitializeCell( DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex )
		{
			base.InitializeCell( cell, cellType, rowState, rowIndex );

			if ( cellType == DataControlCellType.DataCell )
			{
				if ( this.storageList.Parent == null )
				{
					cell.Controls.Add( this.storageList );
				}
				this.InitializeDataCell( cell, rowState );
			}
		}

		/// <exclude/>
		protected virtual void InitializeDataCell( DataControlFieldCell cell, DataControlRowState rowState )
		{
			if ( this.DesignMode )
			{
				cell.Text = this.GetDesignTimeValue();
				return;
			}

			
			System.Web.UI.Control container = null;

			Boolean isEditState = ( ( rowState & DataControlRowState.Edit ) == DataControlRowState.Edit );
			Boolean isInsertState = ( ( rowState & DataControlRowState.Insert ) == DataControlRowState.Insert );

			ListControl list = this.CreateList();
			cell.Controls.Add( list );
			list.ToolTip = this.HeaderText;
			list.Attributes["_Inserting"] = isInsertState.ToString();
			list.Visible = false;

			Label text = new Label();
			cell.Controls.Add( text );
			text.Visible = false;


			if ( isInsertState || ( isEditState && !ReadOnly ) )
			{
				list.Visible = true;
				container = list;
			}
			else if ( DataField.Length != 0 )
			{
				text.Visible = true;
				container = text;
			}

			if ( ( container != null ) && this.Visible )
			{
				container.DataBinding += new EventHandler( container_DataBinding );
			}
		}

		private void container_DataBinding( object sender, EventArgs e )
		{
			this.DataBindField( (System.Web.UI.Control)sender );
		}

		/// <exclude/>
		protected virtual void DataBindField( System.Web.UI.Control container )
		{

			if ( !ListIsDataBound )
			{
				BindList();
				ListIsDataBound = true;
			}

			Label containerText = container as Label;
			if ( containerText != null )
			{
				containerText.Text = this.GetViewValue( container );
				return;
			}

			ListControl containerList = container as ListControl;
			if ( containerList != null )
			{
				containerList.Items.Clear();
				foreach ( ListItem item in this.storageList.Items )
				{
					containerList.Items.Add( new ListItem( item.Text, item.Value ) );
				}
				Boolean isInsert = Boolean.Parse( containerList.Attributes[ "_Inserting" ] );
				containerList.Attributes.Remove( "_Inserting" );
				if ( !isInsert )
				{
					String editValue = this.GetEditValue( container );
					containerList.SelectedIndex = containerList.Items.IndexOf( containerList.Items.FindByValue( editValue ) );
				}
			}
		}

		/// <exclude/>
		protected virtual void BindList()
		{
			this.storageList.DataMember = this.LookupDataMember;
			this.storageList.DataSource = this.LookupDataSource;
			this.storageList.DataSourceID = this.LookupDataSourceID;
			this.storageList.DataTextField = this.LookupDataTextField;
			this.storageList.DataTextFormatString = this.LookupDataFormatString;
			this.storageList.DataValueField = this.LookupDataValueField;
			this.storageList.AppendDataBoundItems = this.AllowNulls;
			if ( this.AllowNulls )
			{
				this.storageList.Items.Clear();
				this.storageList.Items.Add( new ListItem( this.NullDisplayValue, "" ) );
			}

			storageList.DataBind();
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		protected virtual String GetDesignTimeValue()
		{
			WebControl renderControl = null;
			
			if ( this.ListType == ListType.RadioButtons)
			{
				RadioButton radio = new RadioButton();
				radio.Text = Resources.DataBound;
				renderControl = radio;
			}
			else
			{
				ListControl list = this.CreateList( false );
				list.Items.Add( Resources.DataBound );
				renderControl = list;
			}

			renderControl.ControlStyle.CopyFrom( this.ControlStyle );
			
			TextWriter underWriter = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
			HtmlTextWriter writer = new HtmlTextWriter( underWriter );
			renderControl.RenderControl( writer );
			writer.Close();
			return underWriter.ToString();
		}

		/// <exclude/>
		protected virtual String GetViewValue( System.Web.UI.Control container )
		{
			Object dataItem = DataBinder.GetDataItem( container.NamingContainer );
			Object itemValueObject = DataBinder.GetPropertyValue( dataItem, this.DataField );
			String itemValue = ( itemValueObject != null ) ? itemValueObject.ToString() : "";

			String _dataValue;
			if ( String.IsNullOrEmpty( itemValue ) && this.AllowNulls )
			{
				_dataValue = this.NullDisplayValue;
			}
			else
			{
				ListItem item = this.storageList.Items.FindByValue( itemValue );
				_dataValue = item.Text;
			}

			return _dataValue;
		}

		/// <exclude/>
		protected virtual String GetEditValue( System.Web.UI.Control container )
		{
			Object dataItem = DataBinder.GetDataItem( container.NamingContainer );
			Object itemValueObject = DataBinder.GetPropertyValue( dataItem, this.DataField );
			return ( itemValueObject != null ) ? itemValueObject.ToString() : "";
		}

		private void ApplyEditableProperties( ListControl listControl )
		{
			ListBox box = listControl as ListBox;
			if ( box != null )
			{
				box.Rows = this.ListBoxRows;
				return;
			}

			RadioButtonList radios = listControl as RadioButtonList;
			if ( radios != null )
			{
				radios.CellPadding = this.RadioCellPadding;
				radios.CellSpacing = this.RadioCellSpacing;
				radios.RepeatColumns = this.RadioRepeatColumns;
				radios.RepeatDirection = this.RadioRepeatDirection;
				radios.RepeatLayout = this.RadioRepeatLayout;
				radios.TextAlign = this.RadioTextAlign;
				return;
			}

		}

		/// <exclude/>
		private ListControl CreateList()
		{
			return CreateList( true );
		}

		/// <exclude />
		private ListControl CreateList( Boolean attachToPreRender )
		{
			ListControl _valueList;
			switch ( this.ListType )
			{
				case ListType.ListBox:
					_valueList = new ListBox();
					break;
				case ListType.RadioButtons:
					_valueList = new RadioButtonList();
					break;
				default:
					_valueList = new DropDownList();
					break;
			}
			if ( attachToPreRender )
			{
				_valueList.PreRender += new EventHandler( _valueList_PreRender );
			}
			else
			{
				ApplyEditableProperties( _valueList );
			}
			return _valueList;
		}

		void _valueList_PreRender( object sender, EventArgs e )
		{
			ApplyEditableProperties( sender as ListControl );
		}

		#endregion

		private ListControl _storageList;
		private ListControl storageList
		{
			get
			{
				if ( _storageList == null )
				{
					_storageList = new ListBox();
					_storageList.Visible = false;
				}
				return _storageList;
			}
		}

	}
}
