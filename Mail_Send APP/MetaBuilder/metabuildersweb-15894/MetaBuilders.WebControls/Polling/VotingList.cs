using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A ListControl that can be switched between allowing multiple (checkbox), single (radiobutton), or rated (dropdownlist) selection.
	/// </summary>
	internal class VotingList : System.Web.UI.WebControls.ListControl, INamingContainer, IRepeatInfoUser, IPostBackDataHandler
	{

		#region Properties
		/// <summary>
		/// Gets or sets the selection mode of the SelectionList.
		/// </summary>
		[
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets the selection mode of the SelectionList." ),
		DefaultValue( VoteSelectionMode.Single ),
		]
		public virtual VoteSelectionMode SelectionMode
		{
			get
			{
				Object savedState = this.ViewState["SelectionMode"];
				if ( savedState != null )
				{
					return (VoteSelectionMode)savedState;
				}
				return VoteSelectionMode.Single;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( VoteSelectionMode ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( VoteSelectionMode ) );
				}
				this.ViewState["SelectionMode"] = value;
				if ( this.controlToRepeat != null )
				{
					if ( this.Controls.Contains( this.controlToRepeat ) )
					{
						this.Controls.Remove( this.controlToRepeat );
					}
					this.controlToRepeat = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets if the list will include an item which supports entering a write-in vote.
		/// </summary>
		[
		Bindable( true ),
		Category( "Behavior" ),
		Description( "Gets or sets if the list will include an item which supports entering a write-in vote." ),
		DefaultValue( false ),
		]
		public virtual Boolean AllowWriteInVote
		{
			get
			{
				Object savedState = this.ViewState["AllowWriteInVote"];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState["AllowWriteInVote"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the distance (in pixels) between the border and contents of the cell.
		/// </summary>
		/// <value>The distance (in pixels) between the border and contents of the cell. The default is -1, which indicates this property is not set.</value>
		/// <remarks>
		/// <p>Use this property to control the spacing between the contents of a cell and the cell's border in the <see cref="VotingList"/> control.</p>
		/// <p>The padding amount specified is added to all four sides of a cell with the height of the tallest cell and width of the widest cell in the CheckBoxList control. The resulting cell size is applied uniformly to all cells in the <see cref="VotingList"/> control.</p></remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the distance (in pixels) between the border and contents of the cell." ),
		DefaultValue( -1 ),
		]
		public virtual Int32 CellPadding
		{
			get
			{
				if ( !base.ControlStyleCreated )
				{
					return -1;
				}
				return ( (TableStyle)base.ControlStyle ).CellPadding;
			}
			set
			{
				( (TableStyle)base.ControlStyle ).CellPadding = value;
			}
		}

		/// <summary>
		/// Gets or sets the distance (in pixels) between cells.
		/// </summary>
		/// <value>The distance (in pixels) between cells. The default is -1, which indicates that this property is not set.</value>
		/// <remarks>Use this property to control the spacing between individual cells in the <see cref="VotingList"/> control. This property is applied both vertically and horizontally.</remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the distance (in pixels) between cells." ),
		DefaultValue( -1 ),
		]
		public virtual Int32 CellSpacing
		{
			get
			{
				if ( !base.ControlStyleCreated )
				{
					return -1;
				}
				return ( (TableStyle)base.ControlStyle ).CellSpacing;
			}
			set
			{
				( (TableStyle)base.ControlStyle ).CellSpacing = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of columns to display in the <see cref="VotingList"/> control.
		/// </summary>
		/// <value>The number of columns to display in the <see cref="VotingList"/> control. The default is 0, which indicates this property is not set.</value>
		/// <remarks>Use this property to specify the number of columns that display items in the CheckBoxList control. If this property is not set, the <see cref="VotingList"/> control displays all list items in a single column.</remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the number of columns to display in the SelectionList control." ),
		DefaultValue( 0 ),
		]
		public virtual Int32 RepeatColumns
		{
			get
			{
				Object savedState = this.ViewState["RepeatColumns"];
				if ( savedState != null )
				{
					return (Int32)savedState;
				}
				return 0;
			}
			set
			{
				if ( value < 0 )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}
				this.ViewState["RepeatColumns"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the control displays vertically or horizontally.
		/// </summary>
		/// <value>One of the <see cref="RepeatDirection"/> values. The default is <b>Vertical</b>.</value>
		/// <remarks>Use this property to specify the display direction of the <see cref="VotingList"/> control.</remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets a value that indicates whether the control displays vertically or horizontally." ),
		DefaultValue( RepeatDirection.Vertical ),
		]
		public virtual RepeatDirection RepeatDirection
		{
			get
			{
				Object savedState = this.ViewState["RepeatDirection"];
				if ( savedState != null )
				{
					return (RepeatDirection)savedState;
				}
				return RepeatDirection.Vertical;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( RepeatDirection ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( RepeatDirection ) );
				}
				this.ViewState["RepeatDirection"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the layout of the input controls.
		/// </summary>
		/// <value>One of the <see cref="RepeatLayout"/> values. The default is <b>Table</b>.</value>
		/// <remarks>Use this property to specify whether the items in the <see cref="VotingList"/> control are displayed in a table. If this property is set to <b>RepeatLayout.Table</b>, the items in the list are displayed in a table. If this property is set to <b>RepeatLayout.Flow</b>, the items in the list are displayed without a table structure.</remarks>
		[
		Bindable( true ),
		Category( "Appearance" ),
		Description( "Gets or sets the layout of the input controls." ),
		DefaultValue( RepeatLayout.Table ),
		]
		public virtual RepeatLayout RepeatLayout
		{
			get
			{
				Object savedState = this.ViewState["RepeatLayout"];
				if ( savedState != null )
				{
					return (RepeatLayout)savedState;
				}
				return RepeatLayout.Table;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( RepeatLayout ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( RepeatLayout ) );
				}
				this.ViewState["RepeatLayout"] = value;
			}
		}

		/// <summary>
		/// Gets the style applied to the textbox used for write-in votes.
		/// </summary>
		[
		Category( "Style" ),
		Description( "Gets the style applied to the textbox used for write-in votes." ),
		DefaultValue( null ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		PersistenceMode( PersistenceMode.InnerProperty )
		]
		public virtual Style WriteInTextBoxStyle
		{
			get
			{
				if ( writeInTextBoxStyle == null )
				{
					writeInTextBoxStyle = new Style();
					if ( this.IsTrackingViewState )
					{
						( (IStateManager)writeInTextBoxStyle ).TrackViewState();
					}
				}
				return writeInTextBoxStyle;
			}
		}
		private Style writeInTextBoxStyle;
		#endregion

		#region ViewState
		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>
		/// </summary>
		protected override void LoadViewState( object savedState )
		{
			Pair p = savedState as Pair;
			if ( p == null )
			{
				base.LoadViewState( null );
				return;
			}

			base.LoadViewState( p.First );

			if ( p.Second != null )
			{
				( (IStateManager)WriteInTextBoxStyle ).LoadViewState( p.Second );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>.
		/// </summary>
		/// <returns></returns>
		protected override object SaveViewState()
		{
			Pair p = new Pair();
			p.First = base.SaveViewState();
			if ( writeInTextBoxStyle == null )
			{
				p.Second = null;
			}
			else
			{
				p.Second = ( (IStateManager)writeInTextBoxStyle ).SaveViewState();
			}

			if ( p.First == null && p.Second == null )
			{
				return null;
			}
			else
			{
				return p;
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.TrackViewState"/>.
		/// </summary>
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ( writeInTextBoxStyle != null )
			{
				( (IStateManager)writeInTextBoxStyle ).TrackViewState();
			}
		}


		#endregion

		#region Rendering

		/// <exclude />
		protected override Style CreateControlStyle()
		{
			return new TableStyle( base.ViewState );
		}

		/// <exclude />
		protected override Control FindControl( string id, int pathOffset )
		{
			return this;
		}

		/// <exclude />
		public override void DataBind()
		{
			base.DataBind( true );
			this.Items.Add( new ListItem( "", writeInItemValue ) );
		}

		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );
			this.CreateControlToRepeat();
		}

		/// <exclude />
		protected override void Render( HtmlTextWriter writer )
		{
			if ( this.controlToRepeat == null )
			{
				this.CreateControlToRepeat();
			}

			if ( this.Font.Size == FontUnit.Empty )
			{
				this.Font.Size = FontUnit.Parse( "1em", System.Globalization.CultureInfo.InvariantCulture );
			}

			Boolean setTabIndexDirty = false;

			this.controlToRepeatTabIndex = this.TabIndex;

			if ( controlToRepeatTabIndex != 0 )
			{
				if ( !base.ViewState.IsItemDirty( "TabIndex" ) )
				{
					setTabIndexDirty = true;
				}
				base.TabIndex = 0;
			}

			if ( this.SelectionMode == VoteSelectionMode.Rating )
			{
				DropDownList rateControl = (DropDownList)this.controlToRepeat;
				Int32 rateCount = this.Items.Count;
				if ( !this.AllowWriteInVote )
				{
					rateCount--;
				}
				for ( Int32 i = 1; i <= rateCount; i++ )
				{
					rateControl.Items.Add( ( i ).ToString( System.Globalization.CultureInfo.InvariantCulture ) );
				}
			}

			if ( !this.AllowWriteInVote && this.Items.Count > 0 )
			{
				this.Items.RemoveAt( this.Items.Count - 1 );
			}

			RepeatInfo repeatInfo = new RepeatInfo();
			Style inputControlStyle = ( this.ControlStyleCreated ) ? this.ControlStyle : null;
			repeatInfo.RepeatColumns = this.RepeatColumns;
			repeatInfo.RepeatDirection = this.RepeatDirection;
			repeatInfo.RepeatLayout = this.RepeatLayout;
			repeatInfo.RenderRepeater( writer, this, inputControlStyle, this );

			if ( this.controlToRepeatTabIndex != 0 )
			{
				base.TabIndex = this.controlToRepeatTabIndex;
			}
			if ( setTabIndexDirty )
			{
				base.ViewState.SetItemDirty( "TabIndex", false );
			}
			this.controlToRepeat = null;
		}

		#endregion

		#region IRepeatInfoUser Members

		bool IRepeatInfoUser.HasHeader
		{
			get
			{
				return false;
			}
		}

		bool IRepeatInfoUser.HasSeparators
		{
			get
			{
				return false;
			}
		}

		bool IRepeatInfoUser.HasFooter
		{
			get
			{
				return false;
			}
		}

		Style IRepeatInfoUser.GetItemStyle( System.Web.UI.WebControls.ListItemType itemType, int repeatIndex )
		{
			return null;
		}

		void IRepeatInfoUser.RenderItem( System.Web.UI.WebControls.ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer )
		{

			Boolean isWriteInItem = ( this.Items[repeatIndex].Value == this.writeInItemValue );
			if ( isWriteInItem && !this.AllowWriteInVote )
			{
				return;
			}

			#region Set up the write in textbox if needed

			TextBox writeInBox = null;
			if ( isWriteInItem )
			{
				writeInBox = new TextBox();
				this.Controls.Add( writeInBox );
				writeInBox.ID = "WriteIn";
				writeInBox.Text = this.Items[repeatIndex].Text;
				writeInBox.Enabled = this.Enabled;

				writeInBox.Width = Unit.Percentage( 100 );

				if ( writer is Html32TextWriter )
				{
					writeInBox.Attributes["size"] = "5";
				}

				if ( this.writeInTextBoxStyle != null )
				{
					writeInBox.ApplyStyle( this.writeInTextBoxStyle );
				}
			}

			#endregion

			#region Setup the voting control

			this.controlToRepeat.ID = repeatIndex.ToString( System.Globalization.NumberFormatInfo.InvariantInfo );
			this.controlToRepeat.TabIndex = controlToRepeatTabIndex;
			this.controlToRepeat.Enabled = this.Enabled;

			if ( this.SelectionMode == VoteSelectionMode.Single || this.SelectionMode == VoteSelectionMode.Multiple )
			{
				CheckBox checkControl = (CheckBox)this.controlToRepeat;
				if ( !isWriteInItem )
				{
					checkControl.Text = this.Items[repeatIndex].Text;
					checkControl.TextAlign = TextAlign.Right;
					;
				}
				else
				{
					checkControl.Text = "";
				}
				checkControl.Checked = this.Items[repeatIndex].Selected;
			}
			else
			{
				String itemRating = this.Items[repeatIndex].Attributes["rating"];
				if ( itemRating != null && itemRating.Length != 0 )
				{
					DropDownList rateControl = (DropDownList)this.controlToRepeat;
					rateControl.SelectedValue = itemRating;
				}
			}

			if ( this.SelectionMode == VoteSelectionMode.Single )
			{
				( (RadioButton)this.controlToRepeat ).GroupName = this.UniqueID;
				this.controlToRepeat.Attributes["value"] = base.Items[repeatIndex].Value;
			}

			#endregion

			#region Render The Item

			if ( isWriteInItem )
			{
				writer.Write( "<table cellpadding='0' cellspacing='0' border='0' width='100%'><tr><td nowrap>" );
				this.controlToRepeat.RenderControl( writer );
				writer.Write( "</td><td nowrap style='width:100%;padding-left:3px;padding-right:10px;'>" );
				writeInBox.RenderControl( writer );
				writer.Write( "</td></tr></table>" );
			}
			else
			{
				this.controlToRepeat.RenderControl( writer );
				if ( this.SelectionMode == VoteSelectionMode.Rating )
				{
					writer.Write( " " );
					writer.Write( base.Items[repeatIndex].Text );
				}
			}

			#endregion

		}

		int IRepeatInfoUser.RepeatedItemCount
		{
			get
			{
				return base.Items.Count;
			}
		}

		#endregion

		#region IPostBackDataHandler Members

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			if ( this.SelectionMode == VoteSelectionMode.Single )
			{
				if ( this.selIndexChanged )
				{
					this.OnSelectedIndexChanged( EventArgs.Empty );
				}
			}
			else
			{
				this.OnSelectedIndexChanged( EventArgs.Empty );
			}
		}

		bool IPostBackDataHandler.LoadPostData( string postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{

			string writeInID = postDataKey.Substring( UniqueID.Length + 1 );
			if ( writeInID == "WriteIn" )
			{
				Int32 itemCount = this.Items.Count;
				ListItem lastItem = this.Items[itemCount - 1];
				String postedText = postCollection[postDataKey];
				lastItem.Text = postedText;
				return false;
			}


			switch ( this.SelectionMode )
			{
				case VoteSelectionMode.Single:
					string postedValue = postCollection[postDataKey];
					int currentSelectedIndex = this.SelectedIndex;
					for ( Int32 i = 0; ( i < Items.Count ); i++ )
					{
						if ( postedValue == Items[i].Value )
						{
							if ( i != currentSelectedIndex )
							{
								this.selIndexChanged = true;
								base.SelectedIndex = i;
							}
							return true;
						}

					}

					return false;
				case VoteSelectionMode.Multiple:
					string checkBoxID = postDataKey.Substring( UniqueID.Length + 1 );
					int postedIndex = int.Parse( checkBoxID, System.Globalization.CultureInfo.InvariantCulture );

					if ( ( postedIndex >= 0 ) && ( postedIndex < Items.Count ) )
					{
						Boolean checkBoxSelected = ( postCollection[postDataKey] != null );
						if ( Items[postedIndex].Selected != checkBoxSelected )
						{
							Items[postedIndex].Selected = checkBoxSelected;
							if ( !this.hasNotifiedOfChange )
							{
								this.hasNotifiedOfChange = true;
								return true;
							}

						}

					}
					return false;
				case VoteSelectionMode.Rating:
					String postedRating = postCollection[postDataKey];

					String DropDownID = postDataKey.Substring( UniqueID.Length + 1 );
					Int32 postedDropDownIndex = Int32.Parse( DropDownID, System.Globalization.CultureInfo.InvariantCulture );

					if ( postedDropDownIndex >= 0 && postedDropDownIndex < Items.Count )
					{
						Items[postedDropDownIndex].Attributes["rating"] = postedRating;
					}

					return false;
			}
			return false;
		}

		#endregion

		#region Private

		private void CreateControlToRepeat()
		{

			switch ( this.SelectionMode )
			{
				case VoteSelectionMode.Single:
					this.controlToRepeat = new RadioButton();
					break;
				case VoteSelectionMode.Multiple:
					this.controlToRepeat = new CheckBox();
					break;
				case VoteSelectionMode.Rating:
					this.controlToRepeat = new DropDownList();
					break;
			}

			this.controlToRepeat.ID = "0";
			this.controlToRepeat.EnableViewState = false;
			base.Controls.Add( this.controlToRepeat );

			if ( this.SelectionMode == VoteSelectionMode.Multiple )
			{
				if ( this.Page != null )
				{
					for ( Int32 num1 = 0; num1 < this.Items.Count; num1++ )
					{
						this.controlToRepeat.ID = num1.ToString( System.Globalization.NumberFormatInfo.InvariantInfo );
						this.Page.RegisterRequiresPostBack( this.controlToRepeat );
					}
				}
			}

		}

		private WebControl controlToRepeat;
		private Boolean hasNotifiedOfChange;
		private Boolean selIndexChanged = false;
		private Int16 controlToRepeatTabIndex;
		private String writeInItemValue = "-1";
		#endregion
	}
}
