using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The QueryCall component maps QueryString commands to method calls on the Page handling the request.
	/// </summary>
	/// <example>
	/// The following is an example for using the QueryCall component.
	/// <code><![CDATA[
	/// <%@ Page language="C#" %>
	/// <%@ Import namespace="MetaBuilders.WebControls" %>
	/// <script runat="server">
	/// private QueryCall queryCall1;
	///	
	/// protected void Page_Init(Object sender, EventArgs e ) {
	///   queryCall1 = new QueryCall();
	/// }
	///
	/// [QueryCallVisible()]
	/// protected void ShowCall() {
	///   Result.Text = queryCall1.CurrentMethod + "!";
	/// }
	/// </script>
	/// <html><body><form runat="server">
	///   <a href="/QueryCall.aspx?__action=ShowCall" >Run ShowCall()</a>
	///   <asp:Label runat="server" id="Result" EnableViewState="false" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	System.ComponentModel.DesignerCategory("Code"),
	]
	public class QueryCall : Component
	{

		#region Constructors
		/// <summary>
		/// Creates a new instance of the QueryCall component
		/// </summary>
		public QueryCall()
		{
			HttpContext context = HttpContext.Current;
			if (context != null)
			{
				page = context.Handler as Page;
				if (page != null && !page.IsPostBack)
				{
					page.Load += new EventHandler(this.examineQuery);
				}
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The name of the QueryString key to examine for methods to call.
		/// </summary>
		[
		DefaultValue("__action"),
		Description("The name of the QueryString key to examine for methods to call."),
		]
		public virtual String QueryStringKey
		{
			get
			{
				return queryStringKey;
			}
			set
			{
				queryStringKey = value;
			}
		}

		/// <summary>
		/// Gets the name of the method currently being called.
		/// </summary>
		/// <remarks>
		/// During a call, returns the name of the method, otherwise returns String.Empty
		/// </remarks>
		[
		Browsable(false),
		DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),
		Description("Gets the name of the method currently being called."),
		]
		public virtual String CurrentMethod
		{
			get
			{
				return currentMethod;
			}
		}

		/// <summary>
		/// Gets whether a call has been requested on the querystring.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),
		Description("Gets whether a call has been requested."),
		]
		public virtual Boolean IsCallRequested
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return false;
				}
				else
				{
					return HttpContext.Current.Request.QueryString[this.QueryStringKey] != null;
				}
			}
		}

		#endregion

		#region Methods

		/// <exclude />
		public virtual String CreateCallReference( params String[] functionNames )
		{
			if (functionNames == null)
			{
				throw new ArgumentNullException("functionNames", Resources.QueryCall_FunctionNamesNull);
			}
			if (functionNames.Length == 0)
			{
				throw new ArgumentException(Resources.QueryCall_FunctionNamesEmpty, "functionNames");
			}

			System.Text.StringBuilder call = new System.Text.StringBuilder();
			call.Append(this.QueryStringKey);
			call.Append("=");
			call.Append(functionNames[0]);
			for (Int32 i = 1; i < functionNames.Length - 1; i++)
			{
				call.Append(",");
				call.Append(functionNames[i]);
			}
			return call.ToString();
		}
		#endregion

		#region Events
		/// <summary>
		/// The cancelable event that occurs directly before a method call
		/// </summary>
		public event QueryCallEventHandler Call;

		/// <summary>
		/// raises the Call event
		/// </summary>
		protected virtual void OnCall(QueryCallEventArgs e)
		{
			if (this.Call != null)
			{
				this.Call(this, e);
			}
		}
		#endregion

		#region private implementation
		/// <summary>
		/// Parses the QueryString for methods to call
		/// </summary>
		private void examineQuery(Object sender, EventArgs e)
		{
			String[] actions = page.Request.QueryString.GetValues(QueryStringKey);
			if (actions != null)
			{
				foreach (String action in actions)
				{
					this.TryCallMethod(action);
				}
			}
		}

		/// <summary>
		/// Attempts to call the page method by name
		/// </summary>
		/// <param name="name">The name of the method to call</param>
		/// <remarks>
		/// In order to call the method a few things have to be true.
		/// 1) The method must be non-private and non-static
		/// 2) The method must be adornded with the QueryCallVisible attribute
		/// 3) The Call event must not return with the Cancel property true
		/// </remarks>
		private void TryCallMethod(String name)
		{
			MethodInfo method = page.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				Attribute attr = Attribute.GetCustomAttribute(method, typeof(QueryCallVisibleAttribute), true);
				if (attr != null)
				{
					QueryCallEventArgs e = new QueryCallEventArgs(name, false);
					OnCall(e);
					if (!e.Cancel)
					{
						currentMethod = name;
						method.Invoke(page, null);
						currentMethod = String.Empty;
					}
				}
				else
				{
					page.Trace.Write("QueryCall", "Attempted call to method '" + name + "', but that method does not have the QueryCallVisibleAttribute on it.");
				}
			}
			else
			{
				page.Trace.Write("QueryCall", "Attempted call to to method '" + name + "', but no method found.");
			}
		}


		private String queryStringKey = "__action";
		private Page page = null;
		private String currentMethod = String.Empty;
		#endregion
	}
}
