using System;
using System.ComponentModel;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The data for the Call event.
	/// </summary>
	public class QueryCallEventArgs : System.ComponentModel.CancelEventArgs {
		/// <summary>
		/// Creates a new instance of the QueryCallEventArgs with the given method name.
		/// </summary>
		public QueryCallEventArgs(String methodName) : base()
		{
			this.methodName = methodName;
		}

		/// <summary>
		/// Creates a new instance of the QueryCallEventArgs with the given method name and initial cancel value.
		/// </summary>
		public QueryCallEventArgs( String methodName, Boolean cancel ) : base(cancel) {
			this.methodName = methodName;
		}

		/// <summary>
		/// The name of the method which will be called.
		/// </summary>
		[
		Description("The name of the method which will be called."),
		]
		public virtual String MethodName {
			get {
				return methodName;
			}
		}

		private String methodName = "";
	}

	/// <summary>
	/// Represents a function which can handle a QueryCall.Call event.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
	public delegate void QueryCallEventHandler( Object sender, QueryCallEventArgs e );

}
