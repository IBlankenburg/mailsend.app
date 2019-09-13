using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// A control that displays a link to another web page, which opens in a new, minimally adorned window.
	/// </summary>
	/// <remarks>
	/// Only browsers which have javascript will open the window with the properties set on the control.
	/// Other browsers will open a new window with default adornments and attributes.
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to create a simple remote window.
	/// <code>
	/// <![CDATA[
	/// <html>
	///		<body>
	///		<form runat="server">
	///			<h3>RemoteWindow Example</h3>
	///
	///			Click on the link.<br><br>
	///
	///			<mb:RemoteWindow id="RemoteWindow1"
	///				NavigateUrl="http://www.google.com"
	///				Text="Quick Search"
	///				runat="server"/>
	///       
	///		</form>
	///		</body>
	/// </html>
	/// ]]>
	/// </code>
	/// </example>
	public class RemoteWindow : HyperLink {

		/// <summary>
		/// Initializes a new instance of the <see cref="RemoteWindow"/> class.
		/// </summary>
		public RemoteWindow(): base() {
			base.Target = "_blank";
		}


	
		/// <summary>
		/// Gets or sets the <see cref="WindowName"/> of the <see cref="RemoteWindow"/>.
		/// </summary>
		/// <remarks>
		/// The WindowName is largly irrelevant unless client-side script is going to access it.
		/// The control will make up its own name, which is likely to be unique, if no name is given.
		/// </remarks>
		[
			Bindable(true),
			MbwcCategory( "Window Attributes" ),
			Description("Gets or sets the WindowName of the RemoteWindow."),
			DefaultValue("")
		]
		public String WindowName {
			get {
				object savedState;
                
				savedState = this.ViewState["WindowName"];
				if (savedState != null) {
					return (String) savedState;
				}
				return "";
			}
            set { 
				ViewState["WindowName"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets if the user can resize the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		MbwcCategory("WindowAttributes"),
		Description("Gets or sets if the user can resize the RemoteWindow."),
		DefaultValue(true)
		]
		public Boolean Resizable {
			get { 
				object savedState;
                
				savedState = this.ViewState["Resizable"];
				if (savedState != null) {
					return (Boolean) savedState;
				}
				return true;
			}
			set { ViewState["Resizable"] = value; }
		}

		/// <summary>
		/// Gets or sets the distance, in pixels, from the top of the screen to the top of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		MbwcCategory( "WindowAttributes" ),
		Description("Gets or sets the distance, in pixels, from the top of the screen to the top of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit Top {
			get {
				object savedState;
                
				savedState = this.ViewState["Top"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["Top"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets the distance, in pixels, from the left of the screen to the left of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		MbwcCategory( "WindowAttributes" ),
		Description("Gets or sets the distance, in pixels, from the left of the screen to the left of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit Left {
			get {
				object savedState;
                
				savedState = this.ViewState["Left"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["Left"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="WindowHeight"/>, in pixels, of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		MbwcCategory( "WindowAttributes" ),
		Description("Gets or sets the WindowHeight, in pixels, of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit WindowHeight {
			get {
				object savedState;
                
				savedState = this.ViewState["WindowHeight"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["WindowHeight"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="WindowWidth"/>, in pixels, of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		MbwcCategory( "WindowAttributes" ),
		Description("Gets or sets the WindowWidth, in pixels, of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit WindowWidth {
			get {
				object savedState;
                
				savedState = this.ViewState["WindowWidth"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["WindowWidth"] = value; 
			}
		}

		/// <summary>
		/// Adds to the specified writer those HTML attributes and styles that need to be rendered. This method is primarily used by control developers.
		/// </summary>
		/// <param name="writer">The output stream that renders HTML content to the client.</param>
		/// <remarks>
		/// Overridden to set additional attributes.
		/// </remarks>
		protected override void AddAttributesToRender( HtmlTextWriter writer ) {
			base.AddAttributesToRender( writer );
			writer.AddAttribute("onclick", this.OpeningScript, true );
		}

		/// <summary>
		/// Gets the clientscript used in the onclick event of the link used to open the new window.
		/// </summary>
		[Browsable(false)]
		protected virtual String OpeningScript {
			get {
				StringBuilder script = new StringBuilder();

				script.Append ("javascript:window.open( '");
				script.Append (this.ResolveUrl(NavigateUrl));
				script.Append ("', '");
				if ( !String.IsNullOrEmpty( WindowName ) ) {
					script.Append (WindowName);
				} else {
					Random ran = new Random(this.GetHashCode());
					script.Append( "newWindow" + ran.Next().ToString( CultureInfo.InvariantCulture ) );
				}
				script.Append ("', '");
				script.Append( OptionsString );
				script.Append( "'); return false;" );
				return script.ToString();
			}
		}

		/// <summary>
		/// Gets he string of options passed in the third parameter of the window.open call
		/// </summary>
		[Browsable(false)]
		protected virtual String OptionsString {
			get {
				this.WindowOptions.Clear();
				BuildOptionsToRender();

				StringBuilder options = new StringBuilder();
				for( Int32 i=0; i<this.WindowOptions.AllKeys.Length; i++ ) {
					String key = this.WindowOptions.AllKeys[i];
					options.Append(key);
					options.Append("=");
					options.Append(this.WindowOptions[key]);
					if ( i != this.WindowOptions.AllKeys.Length - 1 ) {
						options.Append(",");
					}
				}
				return options.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void BuildOptionsToRender() {
			this.WindowOptions["menubar"] = "0";
			this.WindowOptions["toolbar"] = "0";

			if ( this.Resizable ) {
				this.WindowOptions["resizable"] = "1";
			} else {
				this.WindowOptions["resizable"] = "0";
			}

			if ( this.Top != Unit.Empty ) {
				this.WindowOptions["top"] = this.Top.Value.ToString( CultureInfo.InvariantCulture);
				this.WindowOptions["screenY"] = this.Top.Value.ToString( CultureInfo.InvariantCulture );
			}

			if ( this.Left != Unit.Empty ) {
				this.WindowOptions["left"] = this.Left.Value.ToString( CultureInfo.InvariantCulture );
				this.WindowOptions["screenX"] = this.Left.Value.ToString( CultureInfo.InvariantCulture );
			}


			if ( this.WindowHeight != Unit.Empty ) {
				this.WindowOptions["height"] = this.WindowHeight.Value.ToString( CultureInfo.InvariantCulture );
				this.WindowOptions["innerHeight"] = this.WindowHeight.Value.ToString( CultureInfo.InvariantCulture );
			}

			if ( this.WindowWidth != Unit.Empty ) {
				this.WindowOptions["width"] = this.WindowWidth.Value.ToString( CultureInfo.InvariantCulture );
				this.WindowOptions["innerWidth"] = this.WindowWidth.Value.ToString( CultureInfo.InvariantCulture );
			}

		}

		/// <summary>
		/// Gets the collection of window options to be used on the remote window.
		/// </summary>
		protected NameValueCollection WindowOptions {
			get {
				if ( windowOptions == null ) {
					windowOptions = new NameValueCollection();
				}
				return windowOptions;
			}
		}

		private NameValueCollection windowOptions;

	}

}