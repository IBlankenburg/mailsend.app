using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The container control for the <see cref="PollView.HeaderTemplate"/> and <see cref="PollView.FooterTemplate"/> templates.
	/// </summary>
	/// <remarks>
	/// This control holds the data when databinding content within the <see cref="PollView.HeaderTemplate"/> or <see cref="PollView.FooterTemplate"/>
	/// templates. To access the poll data, use <c>Container.PollData</c> in your databinding expression.
	/// </remarks>
	/// <example>
	/// <para>The following is an example using the <see cref="PollView"/> control with templates
	/// that databind to the poll data through the <see cref="PollViewTemplateContainer"/> control.</para>
	/// <code>
	/// &lt;%@ Register TagPrefix="mb" Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls" %&gt;
	/// &lt;script runat="server" language="c#" &gt;
	///   protected String GetTotalVotes( PollDataSet pollData ) {
	///     return pollData.Votes.Count.ToString();
	///   }
	/// &lt;/script&gt;
	/// &lt;html&gt;&lt;body&gt;&lt;form runat="server"&gt;
	///   &lt;mb:PollView id="PollView1" runat="server" BorderWidth="2px" BorderColor="Black"&gt;
	///     &lt;FooterTemplate&gt;
	///       &lt;div&gt;Total Votes: &lt;%# GetTotalVotes( Container.PollData ) %&gt;&lt;/div&gt;
	///     &lt;/FooterTemplate&gt;
	///   &lt;/mb:PollView&gt;
	/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
	/// </code>
	/// </example>
	[
	ToolboxItem( false ),
	]
	public class PollViewTemplateContainer : PlaceHolder, INamingContainer
	{

		/// <summary>
		/// Creates a new instance of the <see cref="PollViewTemplateContainer"/> class.
		/// </summary>
		public PollViewTemplateContainer( PollView parent )
		{
			this.parentPollView = parent;
		}

		/// <summary>
		/// The <see cref="PollDataSet"/> which holds the data for the currently loaded poll.
		/// </summary>
		public PollDataSet PollData
		{
			get
			{
				return _pollData;
			}
		}

		/// <summary>
		/// Gets the <see cref="PollView"/> control which is the parent for this control.
		/// </summary>
		public PollView ParentPollView
		{
			get
			{
				return parentPollView;
			}
		}
		private PollView parentPollView;

		private PollDataSet _pollData;

		internal void SetPollData( PollDataSet pollData )
		{
			_pollData = pollData;
		}

	}
}
