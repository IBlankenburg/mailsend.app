using System;
using System.Web.UI;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Holds the data for the <see cref="RuntimeTemplate.CreateTemplate"/> event.
	/// </summary>
	public class RuntimeTemplateEventArgs : System.EventArgs {
		
		/// <summary>
		/// Creates a new instance of the RuntimeTemplateEventArgs class.
		/// </summary>
		/// <param name="container"></param>
		public RuntimeTemplateEventArgs(Control container) {
			this.container = container;	
		}

		/// <summary>
		/// The Control into which the template should be instantiated.
		/// </summary>
		public Control Container {
			get {
				return container;
			}
		}

		private Control container;
	}

	/// <summary>
	/// The signature of a handler of the <see cref="RuntimeTemplate.CreateTemplate"/> event.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
	public delegate void RuntimeTemplateEventHandler( Object sender, RuntimeTemplateEventArgs e );
}
