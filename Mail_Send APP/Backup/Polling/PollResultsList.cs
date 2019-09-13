using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Displays the results of a poll as a bar graph list.
	/// </summary>
	internal class PollResultsList : System.Web.UI.WebControls.ListControl
	{

		#region Properties

		/// <summary>
		/// Gets the style of bars in the list.
		/// </summary>
		[
		Category( "Style" ),
		DesignerSerializationVisibility( DesignerSerializationVisibility.Content ),
		NotifyParentProperty( true ),
		PersistenceMode( PersistenceMode.InnerProperty ),
		]
		public virtual BarStyle BarStyle
		{
			get
			{
				if ( barStyle == null )
				{
					barStyle = new BarStyle();
					//					barStyle.BorderWidth = Unit.Pixel( 1 );
					//					barStyle.Height = Unit.Pixel( 10 );
					if ( IsTrackingViewState )
					{
						( (IStateManager)barStyle ).TrackViewState();
					}
				}
				return barStyle;
			}
		}
		private BarStyle barStyle;



		/// <summary>
		/// Gets or sets the <see cref="ResultDisplayType"/> of the bars in the list.
		/// </summary>
		/// <remarks>
		/// With <see cref="ResultDisplayType.Percentage"/>, the widths of the bars will add up to width of the control as a whole.
		/// With <see cref="ResultDisplayType.Count"/>, the item with the highest count will fill the width of the control.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the ResultDisplayType of the bars in the list." ),
		Bindable( true ),
		DefaultValue( ResultDisplayType.Count ),
		]
		public virtual ResultDisplayType BarDisplay
		{
			get
			{
				Object savedState = this.ViewState["BarDisplay"];
				if ( savedState != null )
				{
					return (ResultDisplayType)savedState;
				}
				return ResultDisplayType.Count;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( ResultDisplayType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( ResultDisplayType ) );
				}
				this.ViewState["BarDisplay"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="ResultDisplayType"/> of the vote count display in the list.
		/// </summary>
		/// <remarks>
		/// With <see cref="ResultDisplayType.Percentage"/>, the vote count of each item will be the percentage of votes which that answer received.
		/// With <see cref="ResultDisplayType.Count"/>, the vote count will show the number of votes which that answer received.
		/// </remarks>
		[
		Category( "Appearance" ),
		Description( "Gets or sets the ResultDisplayType of the vote count display in the list." ),
		Bindable( true ),
		DefaultValue( ResultDisplayType.Percentage ),
		]
		public virtual ResultDisplayType VoteCountDisplay
		{
			get
			{
				Object savedState = this.ViewState["VoteCountDisplay"];
				if ( savedState != null )
				{
					return (ResultDisplayType)savedState;
				}
				return ResultDisplayType.Percentage;
			}
			set
			{
				if ( !Enum.IsDefined( typeof( ResultDisplayType ), value ) )
				{
					throw new InvalidEnumArgumentException( "value", (Int32)value, typeof( ResultDisplayType ) );
				}
				this.ViewState["VoteCountDisplay"] = value;
			}
		}

		#endregion

		#region ViewState

		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>.
		/// </summary>
		/// <param name="savedState"></param>
		protected override void LoadViewState( object savedState )
		{
			if ( savedState == null )
			{
				base.LoadViewState( null );
				return;
			}

			Pair pair = savedState as Pair;
			if ( pair != null )
			{
				base.LoadViewState( pair.First );
				if ( pair.Second != null )
				{
					( (IStateManager)this.BarStyle ).LoadViewState( pair.Second );
				}
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>.
		/// </summary>
		/// <returns></returns>
		protected override object SaveViewState()
		{
			Pair pair = new Pair();
			pair.First = base.SaveViewState();
			if ( this.barStyle != null )
			{
				pair.Second = ( (IStateManager)this.barStyle ).SaveViewState();
			}
			if ( pair.First == null && pair.Second == null )
			{
				return null;
			}
			return pair;
		}

		/// <summary>
		/// Overrides <see cref="TrackViewState"/>.
		/// </summary>
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if ( this.barStyle != null )
			{
				( (IStateManager)barStyle ).TrackViewState();
			}
		}

		#endregion

		#region Life Cycle

		public override void DataBind()
		{
			base.DataBind( true );
		}

		/// <summary>
		/// Overrides <see cref="WebControl.RenderBeginTag"/>.
		/// </summary>
		public override void RenderBeginTag( HtmlTextWriter writer )
		{
			// No Opening Tag
		}

		/// <summary>
		/// Overrides <see cref="WebControl.RenderContents"/>.
		/// </summary>
		protected override void RenderContents( HtmlTextWriter writer )
		{

			Table outerTable = new Table();
			outerTable.ControlStyle.CopyFrom( this.ControlStyle );
			if ( outerTable.Font.Size == FontUnit.Empty )
			{
				outerTable.Font.Size = FontUnit.Parse( "1em", System.Globalization.CultureInfo.InvariantCulture );
			}
			outerTable.Width = Unit.Percentage( 100 );


			Double totalVotes = 0;
			Double barDisplayMax = 0;

			// First I need to get totals and percentages information
			foreach ( ListItem item in Items )
			{
				Int32 itemVoteCount = Int32.Parse( item.Value, System.Globalization.CultureInfo.InvariantCulture );
				totalVotes += itemVoteCount;
				switch ( this.BarDisplay )
				{
					case ResultDisplayType.Percentage:
						barDisplayMax = totalVotes;
						break;
					case ResultDisplayType.Count:
						if ( itemVoteCount > barDisplayMax )
						{
							barDisplayMax = itemVoteCount;
						}
						break;
				}
			}

			// Then I cycle thru the items and display the bar and vote count
			foreach ( ListItem item in Items )
			{

				TableRow rRow = new TableRow();
				outerTable.Rows.Add( rRow );

				TableCell barHolder = new TableCell();
				barHolder.Width = Unit.Percentage( 100 );
				rRow.Cells.Add( barHolder );

				Label answerName = new Label();
				answerName.Text = item.Text + "<br>";
				barHolder.Controls.Add( answerName );

				HorizontalBar bar = new HorizontalBar();
				bar.EnableViewState = false;
				bar.Percentage = ( barDisplayMax != 0 ) ? ( Double.Parse( item.Value, System.Globalization.CultureInfo.InvariantCulture ) / barDisplayMax ) * 100 : 0;
				;
				bar.ControlStyle.CopyFrom( this.BarStyle );
				if ( bar.Height == Unit.Empty )
				{
					bar.Height = Unit.Pixel( 10 );
				}
				if ( bar.BorderWidth == Unit.Empty )
				{
					bar.BorderWidth = Unit.Pixel( 1 );
				}
				if ( bar.ForeColor.Equals( bar.BackColor ) )
				{
					PollView parent = this.NamingContainer as PollView;
					if ( parent != null && !parent.ForeColor.Equals( bar.BackColor ) )
					{
						bar.ForeColor = parent.ForeColor;
					}
					else
					{
						if ( bar.BackColor.Equals( System.Drawing.Color.Black ) )
						{
							bar.ForeColor = System.Drawing.Color.White;
						}
						else
						{
							bar.ForeColor = System.Drawing.Color.Black;
						}
					}

				}
				barHolder.Controls.Add( bar );

				TableCell numericDisplay = new TableCell();
				switch ( this.VoteCountDisplay )
				{
					case ResultDisplayType.Percentage:
						Double votePercentage = ( totalVotes != 0 ) ? ( Double.Parse( item.Value, System.Globalization.CultureInfo.InvariantCulture ) / totalVotes ) * 100 : 0;
						numericDisplay.Text = votePercentage.ToString( "##0.0", System.Globalization.CultureInfo.InvariantCulture ) + "%";
						break;
					case ResultDisplayType.Count:
						numericDisplay.Text = item.Value;
						break;
				}
				numericDisplay.VerticalAlign = VerticalAlign.Bottom;
				rRow.Cells.Add( numericDisplay );
			}

			outerTable.RenderControl( writer );
		}

		/// <summary>
		/// Overrides <see cref="WebControl.RenderEndTag"/>.
		/// </summary>
		public override void RenderEndTag( HtmlTextWriter writer )
		{
			// No End Tag
		}
		#endregion
	}
}
