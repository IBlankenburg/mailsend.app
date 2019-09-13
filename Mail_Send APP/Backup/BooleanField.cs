using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace MetaBuilders.WebControls
{

	/// <summary>Represents a Boolean field in a data-bound control.</summary>
	public class BooleanField : DataControlField
	{

		#region Properties

		/// <summary>
		/// Gets or sets the field in the datasource to bind to.
		/// </summary>
		[
		Description( "BoundField_DataField" ),
		TypeConverter( "System.Web.UI.Design.DataSourceBooleanViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" ),
		Category( "Data" ),
		DefaultValue( "" )
		]
		public virtual string DataField
		{
			get
			{
				if ( this._dataField == null )
				{
					object obj1 = base.ViewState[ "DataField" ];
					if ( obj1 != null )
					{
						this._dataField = (string)obj1;
					}
					else
					{
						this._dataField = string.Empty;
					}
				}
				return this._dataField;
			}
			set
			{
				if ( !object.Equals( value, base.ViewState[ "DataField" ] ) )
				{
					base.ViewState[ "DataField" ] = value;
					this._dataField = value;
					this.OnFieldChanged();
				}
			}
		}
		private String _dataField;

		/// <summary>
		/// Gets or sets a value that indicates whether the items in the <see cref="BooleanField"/> can be edited.
		/// </summary>
		/// <value>true if the items in the <see cref="BooleanField"/> can be edited; otherwise, false. The default value is false.</value>
		/// <remarks>
		/// Use the ReadOnly property to specify whether the items in the <see cref="BooleanField"/> can be edited.
		/// This property can also be used to programmatically determine whether the column is read-only.
		/// </remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "ReadOnly" ), 
		Description( "Gets or sets a value that indicates whether the items in the BooleanField can be edited." ),
		DefaultValue( false ),
		Category( "Behavior" ),
		]
		public virtual bool ReadOnly
		{
			get
			{
				object obj1 = base.ViewState[ "ReadOnly" ];
				if ( obj1 != null )
				{
					return (bool)obj1;
				}
				return false;
			}
			set
			{
				object obj1 = base.ViewState[ "ReadOnly" ];
				if ( ( obj1 == null ) || ( ( (bool)obj1 ) != value ) )
				{
					base.ViewState[ "ReadOnly" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the method of editing the field.
		/// </summary>
		/// <value>One of the <see cref="BooleanSelectionMode"/> values.</value>
		/// <remarks>
		/// The <see cref="BooleanSelectionMode.CheckBox"/> setting does not support entering null data.
		/// </remarks>
		[
		Description( "Gets or sets the method of editing the field." ),
		DefaultValue( BooleanSelectionMode.DropDownList ),
		Category( "Behavior" )
		]
		public virtual BooleanSelectionMode SelectionMode
		{
			get
			{
				Object savedState = this.ViewState[ "SelectionMode" ];
				if ( savedState != null )
				{
					return (BooleanSelectionMode)savedState;
				}
				return MetaBuilders.WebControls.BooleanSelectionMode.DropDownList;
			}
			set
			{
				Object state = this.ViewState[ "SelectionMode" ];
				if ( state == null || (BooleanSelectionMode)state != value )
				{
					this.ViewState[ "SelectionMode" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="String"/> shown for a True value in the data source.
		/// </summary>
		[
		Description( "Gets or sets the String shown for a True value in the data source." ),
		DefaultValue( "True" ),
		Category( "Appearance" )
		]
		public virtual String TrueString
		{
			get
			{
				Object state = this.ViewState[ "DataTrueString" ];
				if ( state != null )
				{
					return (String)state;
				}
				return "True";
			}
			set
			{
				Object state = this.ViewState[ "DataTrueString" ];
				if ( state == null || (String)state != value )
				{
					this.ViewState[ "DataTrueString" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="String"/> shown for a False value in the data source.
		/// </summary>
		[
		Description( "Gets or sets the String shown for a False value in the data source." ),
		DefaultValue( "False" ),
		Category( "Appearance" )
		]
		public virtual String FalseString
		{
			get
			{
				Object savedData = this.ViewState[ "DataFalseString" ];
				if ( savedData != null )
				{
					return (String)savedData;
				}
				return "False";
			}
			set
			{
				Object savedData = this.ViewState[ "DataFalseString" ];
				if ( savedData == null || (String)savedData != value )
				{
					this.ViewState[ "DataFalseString" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="String"/> shown for a Null value in the data source.
		/// </summary>
		[
		Description( "Gets or sets the String shown for a Null value in the data source." ),
		DefaultValue( "Null" ),
		Category( "Appearance" )
		]
		public virtual String NullString
		{
			get
			{
				Object savedData = this.ViewState[ "DataNullString" ];
				if ( savedData != null )
				{
					return (String)savedData;
				}
				return "Null";
			}
			set
			{
				Object savedData = this.ViewState[ "DataNullString" ];
				if ( savedData == null || (String)savedData != value )
				{
					this.ViewState[ "DataNullString" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the edit controls include a null choice.
		/// </summary>
		/// <remarks>
		/// This must be set to false when using the <see cref="BooleanSelectionMode.CheckBox"/> value for the <see cref="SelectionMode"/> property.
		/// When set to false, an exception will be thrown unless <see cref="NullDefault"/> is also set.
		/// </remarks>
		[
		Description( "Gets or sets whether the edit controls include a null choice." ),
		DefaultValue( true ),
		Category( "Behavior" )
		]
		public virtual Boolean AllowNull
		{
			get
			{
				Object savedData = this.ViewState[ "AllowNull" ];
				if ( savedData != null )
				{
					return (Boolean)savedData;
				}
				return true;
			}
			set
			{
				Object savedData = this.ViewState[ "AllowNull" ];
				if ( savedData == null || (Boolean)savedData != value )
				{
					this.ViewState[ "AllowNull" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the default value of an edit control when <see cref="AllowNull"/> is set to false and the value is null.
		/// </summary>
		/// <remarks>
		/// This field is required, and only neccesary when, <see cref="AllowNull"/> is set to false.
		/// </remarks>
		[
		Description( "Gets or sets the default value of an edit control when AllowNull is set to false and the value is null." ),
		DefaultValue( false ),
		Category( "Behavior" )
		]
		public virtual Boolean NullDefault
		{
			get
			{
				Object nullDefault = this.ViewState[ "NullDefault" ];
				if ( nullDefault != null )
				{
					return (Boolean)nullDefault;
				}
				return false;
			}
			set
			{
				Object state = this.ViewState[ "NullDefault" ];
				if ( state == null || (Boolean)state != value )
				{
					this.ViewState[ "NullDefault" ] = value;
					this.OnFieldChanged();
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>Copies the properties of the current <see cref="BooleanField"></see> object to the specified <see cref="T:System.Web.UI.WebControls.DataControlField"></see> object.</summary>
		protected override void CopyProperties( DataControlField newField )
		{
			base.CopyProperties( newField );

			BooleanField bField = newField as BooleanField;
			bField.DataField = this.DataField;
			bField.ReadOnly = this.ReadOnly;
			bField.SelectionMode = this.SelectionMode;
			bField.TrueString = this.TrueString;
			bField.FalseString = this.FalseString;
			bField.NullString = this.NullString;
			bField.AllowNull = this.AllowNull;
			bField.NullDefault = this.NullDefault;

		}

		/// <summary>Creates an empty <see cref="BooleanField"></see> object.</summary>
		protected override DataControlField CreateField()
		{
			return new BooleanField();
		}

		/// <summary>Fills the specified <see cref="T:System.Collections.IDictionary"></see> object with the values from the specified <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"></see> object.</summary>
		/// <param name="cell">The <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"></see> that contains the values to retrieve.</param>
		/// <param name="dictionary">A <see cref="T:System.Collections.IDictionary"></see> used to store the values of the specified cell.</param>
		/// <param name="rowState">One of the <see cref="T:System.Web.UI.WebControls.DataControlRowState"></see> values.</param>
		/// <param name="includeReadOnly">true to include the values of read-only fields; otherwise, false.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public override void ExtractValuesFromCell( IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly )
		{
			Control editor = null;
			String field = this.DataField;
			Boolean valueSet = false;
			Object currentValue = null;

			if ( cell.Controls.Count > 0 )
			{
				editor = cell.Controls[ 0 ];

				CheckBox checkBoxEditor = editor as CheckBox;
				if ( ( checkBoxEditor != null ) && ( includeReadOnly || checkBoxEditor.Enabled ) )
				{
					valueSet = true;
					currentValue = checkBoxEditor.Checked;
				}
				ListControl listEditor = editor as ListControl;
				if ( ( listEditor != null ) && ( includeReadOnly || listEditor.Enabled ) )
				{
					valueSet = true;
					if ( listEditor.SelectedValue == Boolean.TrueString )
					{
						currentValue = true;
					}
					else if ( listEditor.SelectedValue == Boolean.FalseString )
					{
						currentValue = false;
					}
					else
					{
						currentValue = DBNull.Value;
					}
				}
			}

			if ( valueSet )
			{
				if ( dictionary.Contains( field ) )
				{
					dictionary[ field ] = currentValue;
				}
				else
				{
					dictionary.Add( field, currentValue );
				}
			}
		}

		/// <exclude />
		public override void InitializeCell( DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex )
		{
			base.InitializeCell( cell, cellType, rowState, rowIndex );
			if ( cellType == DataControlCellType.DataCell )
			{
				this.InitializeDataCell( cell, rowState );
			}
		}

		/// <summary>Initializes the specified <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"></see> object to the specified row state.</summary>
		/// <param name="cell">The <see cref="T:System.Web.UI.WebControls.DataControlFieldCell"></see> to initialize.</param>
		/// <param name="rowState">One of the <see cref="T:System.Web.UI.WebControls.DataControlRowState"></see> values.</param>
		protected virtual void InitializeDataCell( DataControlFieldCell cell, DataControlRowState rowState )
		{

			Boolean isEditState = ( ( rowState & DataControlRowState.Edit ) == DataControlRowState.Edit );
			Boolean isInsertState = ( ( rowState & DataControlRowState.Insert ) == DataControlRowState.Insert );

			Control container = null;

			if ( isInsertState || ( isEditState && !ReadOnly ) )
			{
				WebControl editorControl = this.CreateEditorControl();
				editorControl.ToolTip = this.HeaderText;
				cell.Controls.Add( editorControl );
				container = editorControl;
			}
			else if ( !String.IsNullOrEmpty( this.DataField ) )
			{
				container = cell;
			}

			if ( container != null && this.Visible )
			{
				container.DataBinding += new EventHandler( OnDataBindField );
			}
		}

		/// <summary>Binds the value of a field to a check box in the <see cref="T:System.Web.UI.WebControls.CheckBoxField"></see> object.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data. </param>
		/// <param name="sender">The source of the event. </param>
		/// <exception cref="T:System.Web.HttpException">The control to which the field value is bound is not a <see cref="T:System.Web.UI.WebControls.CheckBox"></see> control.- or -The field value cannot be converted to a Boolean value. </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "1#" )]
		protected virtual void OnDataBindField( object sender, EventArgs e )
		{
			Control container = sender as Control;

			Object dataItem = DataBinder.GetDataItem( container.NamingContainer );
			this.EnsureDataFieldExists( dataItem );
			SqlBoolean fieldValue = this.GetUnderlyingValue( dataItem );
			String formattedValue = this.FormatDataValue( fieldValue );
			
			TableCell cell = container as TableCell;
			if ( cell != null )
			{
				cell.Text = formattedValue;
				return;
			}

			CheckBox checkEditor = container as CheckBox;
			if ( checkEditor != null )
			{
				if ( fieldValue.IsFalse )
				{
					checkEditor.Checked = false;
				}
				else if ( fieldValue.IsTrue )
				{
					checkEditor.Checked = true;
				}
				else
				{
					if ( AllowNull )
					{
						throw new InvalidOperationException( Resources.BooleanField_CheckBoxNotNull );
					}
					else
					{
						if ( !checkEditor.Enabled )
						{
							checkEditor.Visible = false;
						}
						else
						{
							checkEditor.Checked = this.NullDefault;
						}
					}
				}
				return;
			}

			ListControl listEditor = container as ListControl;
			if ( listEditor != null )
			{
				listEditor.SelectedIndex = listEditor.Items.IndexOf( listEditor.Items.FindByText( formattedValue ) );
				return;
			}



		}

		/// <summary>
		/// Creates the control which provides the editing interface for the field.
		/// </summary>
		/// <returns>The created control.</returns>
		protected virtual WebControl CreateEditorControl()
		{
			WebControl editor = null;
			ListControl listEditor = null;
			switch ( this.SelectionMode )
			{
				case BooleanSelectionMode.CheckBox:
					editor = new CheckBox();
					break;
				case BooleanSelectionMode.DropDownList:
					editor = new DropDownList();
					listEditor = (DropDownList)editor;
					if ( this.AllowNull )
					{
						listEditor.Items.Add( new ListItem( this.NullString, System.DBNull.Value.ToString() ) );
					}
					listEditor.Items.Add( new ListItem( this.TrueString, System.Boolean.TrueString ) );
					listEditor.Items.Add( new ListItem( this.FalseString, System.Boolean.FalseString ) );
					break;
				case BooleanSelectionMode.RadioButtonList:
					editor = new RadioButtonList();
					listEditor = (RadioButtonList)editor;
					if ( this.AllowNull )
					{
						listEditor.Items.Add( new ListItem( this.NullString, System.DBNull.Value.ToString() ) );
					}
					listEditor.Items.Add( new ListItem( this.TrueString, System.Boolean.TrueString ) );
					listEditor.Items.Add( new ListItem( this.FalseString, System.Boolean.FalseString ) );
					break;
			}
			return editor;
		}

		/// <summary>Determines whether the controls contained in a <see cref="T:System.Web.UI.WebControls.CheckBoxField"></see> object support callbacks.</summary>
		public override void ValidateSupportsCallback()
		{
		}

		/// <summary>
		/// find the correct item in the list from the dataitem's value
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected virtual SqlBoolean GetUnderlyingValue( Object dataItem )
		{
			SqlBoolean underlyingValue;
			object dataItemDescriptorValue = dataItem;
			if ( this.boundFieldDesc != null )
			{
				dataItemDescriptorValue = this.boundFieldDesc.GetValue( dataItem );
			}
			try
			{
				underlyingValue = SqlBoolean.Parse( dataItemDescriptorValue.ToString() );
			}
			catch
			{
				underlyingValue = SqlBoolean.Null;
			}
			return underlyingValue;
		}

		/// <summary>
		/// Formats the dataValue for display.
		/// </summary>
		protected virtual string FormatDataValue( SqlBoolean dataValue )
		{

			if ( dataValue.IsTrue )
			{
				return this.TrueString;
			}
			else if ( dataValue.IsFalse )
			{
				return this.FalseString;
			}
			else
			{
				return this.NullString;
			}

		}

		// make sure the bound field exists in the datasource
		private void EnsureDataFieldExists( Object dataItem )
		{
			if ( !( this.boundFieldDescValid ) )
			{
				if ( !String.IsNullOrEmpty( this.DataField ) )
				{
					this.boundFieldDesc = System.ComponentModel.TypeDescriptor.GetProperties( dataItem ).Find( this.DataField, true );
					if ( this.boundFieldDesc == null && !( this.DesignMode ) )
					{
						throw new HttpException( "Field Not Found: " + this.DataField );
					}
					this.boundFieldDescValid = true;
				}
			}
		}

		private PropertyDescriptor boundFieldDesc;
		private Boolean boundFieldDescValid;

		#endregion

	}
}
