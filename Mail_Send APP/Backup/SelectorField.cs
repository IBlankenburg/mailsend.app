using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.Design.WebControls;
using System.Web;
using System.Collections.Generic;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A databinding field which lets the user select rows with a checkbox or radiobutton.
	/// </summary>
	public partial class SelectorField : DataControlField
	{

		#region Properties

		/// <summary>
		/// Gets or sets the name of the data field to bind to the LookupField object.
		/// </summary>
		/// <remarks>
		/// This property is not required with the SelectorField, but instead only when there is a boolean or bit
		/// field which represents the selected item.
		/// </remarks>
		[
		Description( "Gets or sets the name of the data field to bind to the SelectorField object." ),
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
					object savedState = this.ViewState["DataField"];
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
				if ( !Object.Equals( value, this.ViewState["DataField"] ) )
				{
					this.ViewState["DataField"] = value;
					this._dataField = value;
					this.OnFieldChanged();
				}
			}
		}
		private String _dataField;

		/// <summary>
		/// Gets or sets a Boolean indicating if the user will be shown a checkbox for Select-All functionality.
		/// </summary>
		[
		Description( "Gets or sets a Boolean indicating if the user will be shown a checkbox for Select-All functionality." ),
		Category( "Behavior" ),
		DefaultValue( false ),
		]
		public virtual Boolean AllowSelectAll
		{
			get
			{
				Object state = ViewState["AllowSelectAll"];
				if ( state != null )
				{
					return (Boolean)state;
				}
				return false;
			}
			set
			{
				Object state = ViewState["AllowSelectAll"];
				if ( state == null || (Boolean)state != value )
				{
					ViewState["AllowSelectAll"] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the mode of selection of data items.
		/// </summary>
		[
		Description( "Gets or sets the mode of selection of data items." ),
		Category( "Behavior" ),
		DefaultValue( ListSelectionMode.Single ),
		]
		public virtual ListSelectionMode SelectionMode
		{
			get
			{
				Object state = ViewState["SelectionMode"];
				if ( state != null )
				{
					return (ListSelectionMode)state;
				}
				return ListSelectionMode.Single;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( ListSelectionMode ), value ) )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}

				Object state = ViewState["SelectionMode"];
				if ( state == null || (ListSelectionMode)state != value )
				{
					ViewState["SelectionMode"] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets if the field's controls will postback automaticly when the selection changes.
		/// </summary>
		[
		Description( "Gets or sets if the field's controls will postback automaticly when the selection changes." ),
		Category( "Behavior" ),
		DefaultValue( false ),
		]
		public virtual Boolean AutoPostBack
		{
			get
			{
				Object state = ViewState["AutoPostBack"];
				if ( state != null )
				{
					return (Boolean)state;
				}
				return false;
			}
			set
			{
				Object state = ViewState["AutoPostBack"];
				if ( state == null || (Boolean)state != value )
				{
					ViewState["AutoPostBack"] = value;
					this.OnFieldChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets an array which contains the indexes of the fields which are or will be selected.
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays" ), Description( "Gets or sets an array which contains the indexes of the fields which are or will be selected." ),
		Browsable( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ),
		]
		public Int32[] SelectedIndexes
		{
			get
			{
				List<Int32> selections = new List<int>();
				foreach ( ISelectorFieldControl selector in this.selectors )
				{
					if ( selector.Selected )
					{
						selections.Add( selector.Index );
					}
				}
				return selections.ToArray();
			}
			set
			{
				foreach ( ISelectorFieldControl selector in this.selectors )
				{
					selector.Selected = ( Array.IndexOf( value, selector.Index ) >= 0 );
				}
			}
		}

		/// <summary>
		/// Gets the ID of the field.
		/// </summary>
		[
		Description( "Gets the ID of the field." ),
		Browsable( false ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ),
		]
		public String FieldId
		{
			get
			{
				if ( _cachedFieldId == null )
				{
					if ( !String.IsNullOrEmpty( this.DataField ) )
					{
						_cachedFieldId = this.DataField;
					}
					else
					{
						GridView parent = this.Control as GridView;
						if ( parent != null )
						{
							_cachedFieldId = "SelectorField" + parent.Columns.IndexOf( this ).ToString( CultureInfo.InvariantCulture );
						}
						else
						{
							_cachedFieldId = "SelectorField";
						}
					}
					_cachedFieldId = this.Control.ClientID + "_" + _cachedFieldId;
				}
				return _cachedFieldId;
			}
		}
		String _cachedFieldId;

		#endregion

		#region Methods

		/// <exclude/>
		protected override DataControlField CreateField()
		{
			return new SelectorField();
		}

		/// <exclude />
		protected virtual Control CreateSelectorControl()
		{
			Control _selectorControl = null;
			switch ( this.SelectionMode )
			{
				case ListSelectionMode.Multiple:
					_selectorControl = new SelectorFieldCheckBox( this );
					break;
				case ListSelectionMode.Single:
					_selectorControl = new SelectorFieldRadioButton( this );
					break;
			}
			return _selectorControl;
		}

		/// <exclude/>
		protected override void CopyProperties( DataControlField newField )
		{
			base.CopyProperties( newField );

			SelectorField selector = newField as SelectorField;
			selector.DataField = this.DataField;
			selector.AllowSelectAll = this.AllowSelectAll;
			selector.SelectionMode = this.SelectionMode;
			selector.AutoPostBack = this.AutoPostBack;

		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods" )]
		public override void ExtractValuesFromCell( System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly )
		{
			if ( String.IsNullOrEmpty( this.DataField ) )
			{
				return;
			}

			if ( cell == null || dictionary == null )
			{
				return;
			}

			Object value = null;

			if ( cell.Controls.Count > 0 )
			{
				ISelectorFieldControl selector = cell.Controls[0] as ISelectorFieldControl;
				if ( selector != null )
				{
					value = selector.Selected;
				}
			}
			if ( value != null )
			{
				if ( dictionary.Contains( this.DataField ) )
				{
					dictionary[this.DataField] = value;
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

			this.selectors = new List<ISelectorFieldControl>();

			ClientScriptManager script = this.Control.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( SelectorField ), "MetaBuilders.WebControls.Embedded.SelectorFieldScript.js" );
			script.RegisterStartupScript( typeof( SelectorField ), "Startup", "MetaBuilders_SelectorField_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_SelectorField_Init" ), true );
			return baseValue;
		}

		/// <exclude/>
		public override void InitializeCell( DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, Int32 rowIndex )
		{
			base.InitializeCell( cell, cellType, rowState, rowIndex );
			if ( cellType == DataControlCellType.DataCell )
			{
				this.InitializeDataCell( cell, rowState, rowIndex );
			}
			if ( cellType == DataControlCellType.Header )
			{
				this.InitializeHeaderCell( cell, rowState );
			}
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters" )]
		protected virtual void InitializeHeaderCell( DataControlFieldCell cell, DataControlRowState rowState )
		{
			if ( this.AllowSelectAll && this.SelectionMode == ListSelectionMode.Multiple )
			{
				Control selectAll = new SelectorFieldCheckAllBox( this );
				( (ISelectorFieldControl)selectAll ).AutoPostBack = this.AutoPostBack;

				String originalText = this.HeaderText;

				cell.Text = String.Empty;
				cell.Controls.Add( selectAll );
				cell.Controls.Add( new LiteralControl( originalText ) );
			}
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters" )]
		protected virtual void InitializeDataCell( DataControlFieldCell cell, DataControlRowState rowState, Int32 rowIndex )
		{
			if ( ( rowState & DataControlRowState.Insert ) == DataControlRowState.Insert )
			{
				return;
			}

			Control selector = this.CreateSelectorControl();
			cell.Controls.Add( selector );

			ISelectorFieldControl iSelector = selector as ISelectorFieldControl;
			iSelector.AutoPostBack = this.AutoPostBack;
			iSelector.Index = rowIndex;

			this.selectors.Add( iSelector );

			if ( !String.IsNullOrEmpty( this.DataField ) )
			{
				selector.DataBinding += new EventHandler( selector_DataBinding );
			}
		}

		private void selector_DataBinding( object sender, EventArgs e )
		{
			this.DataBindField( sender as Control );
		}

		/// <exclude/>
		protected virtual void DataBindField( Control selector )
		{
			Object dataItem = DataBinder.GetDataItem( selector.NamingContainer );
			Object itemValueObject = DataBinder.GetPropertyValue( dataItem, this.DataField );
			Boolean dataValue = ( itemValueObject != null ) ? Convert.ToBoolean( itemValueObject, CultureInfo.InvariantCulture ) : false;

			ISelectorFieldControl iSelector = selector as ISelectorFieldControl;
			iSelector.Selected = dataValue;
		}

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
		protected virtual String GetDesignTimeValue()
		{
			switch ( this.SelectionMode )
			{
				case ListSelectionMode.Multiple:
					return "<input type='checkbox' />";
				case ListSelectionMode.Single:
				default:
					return "<input type='radio' />";
			}
		}

		#endregion

		#region Private

		private List<ISelectorFieldControl> selectors;

		#endregion

	}
}
