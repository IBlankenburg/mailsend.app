using System;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Represents filter options for poll selection.
	/// </summary>
	/// <remarks>
	/// The <see cref="PollSelectionFilter"/> class is used by the <see cref="Polling"/> class to filter
	/// the selection of poll data. Selection can be filtered on <see cref="Category"/>, <see cref="Text"/>, <see cref="BeginDate">Begin Date</see>, and <see cref="EndDate">End Date</see>.
	/// </remarks>
	/// <example>
	/// <code>
	/// &lt;script language="c#" runat="server"&gt;
	///   protected void Page_Load( Object sender, EventArgs e ) {
	///    MetaBuilders.WebControls.PollSelectionFilter filter = new MetaBuilders.WebControls.PollSelectionFilter();
	///    filter.Text = "Poll";
	/// 
	///    MetaBuilders.WebControls.PollDataSet filteredData = MetaBuilders.WebControls.Polling.GetPollsByFilter( filter );
	///    Repeater1.DataSource = filteredData.Polls;
	///    Repeater1.DataBind();
	///  }
	/// &lt;/script&gt;
	/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
	///   &lt;asp:Repeater runat="server" ID="Repeater1"&gt;
	///     &lt;ItemTemplate&gt;
	///       &lt;%# DataBinder.Eval( Container.DataItem, "Text" ) %&gt;
	///       &lt;hr&gt;
	///     &lt;/ItemTemplate&gt;
	///   &lt;/asp:Repeater&gt;
	/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
	/// </code>
	/// </example>
	public class PollSelectionFilter
	{

		/// <summary>
		/// Gets or sets the category to filter on.
		/// </summary>
		public String Category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
			}
		}
		private String _category;

		/// <summary>
		/// Gets or sets the poll text to filter on.
		/// </summary>
		public String Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}
		private String _text;

		/// <summary>
		/// Gets or sets the earliest date on which a filter poll could start
		/// </summary>
		public System.Data.SqlTypes.SqlDateTime BeginDate
		{
			get
			{
				return _beginDate;
			}
			set
			{
				_beginDate = value;
			}
		}
		System.Data.SqlTypes.SqlDateTime _beginDate;

		/// <summary>
		/// Gets or sets the latest date on which a filter poll could start
		/// </summary>
		public System.Data.SqlTypes.SqlDateTime EndDate
		{
			get
			{
				return _endDate;
			}
			set
			{
				_endDate = value;
			}
		}
		System.Data.SqlTypes.SqlDateTime _endDate;

		/// <summary>
		/// Gets if any of the filter properties have been set.
		/// </summary>
		public Boolean IsFilterSet
		{
			get
			{
				return ( _category != null || _text != null || !_beginDate.IsNull || !_endDate.IsNull );
			}
		}

	}
}
