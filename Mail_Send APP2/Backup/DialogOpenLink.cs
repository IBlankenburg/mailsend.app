using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Opens the specified dialog without a postback to call <see cref="DialogWindowBase.Open"/>
	/// </summary>
	[
	DefaultProperty("DialogToOpen"),
	Designer(typeof(MetaBuilders.WebControls.Design.OpenerDesigner)),
	]
	public class DialogOpenLink : WebControl {
		
		/// <exclude/>
		protected override System.Web.UI.HtmlTextWriterTag TagKey {
			get {
				return HtmlTextWriterTag.A;
			}
		}

		/// <summary>
		/// Gets or sets the ID of the <see cref="DialogWindowBase"/> to open.
		/// </summary>
		[
		Description("Gets or sets the ID of the DialogWindowBase to open."),
		MbwcCategory( "Dialog" ),
		DefaultValue(""),
		Bindable(true),
		TypeConverter(typeof(MetaBuilders.WebControls.Design.DialogControlConverter)),
		]
		public virtual String DialogToOpen {
			get {
				Object savedState = this.ViewState["DialogToOpen"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				if ( value == null ) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["DialogToOpen"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Text of the link.
		/// </summary>
		/// <remarks>
		/// If both <see cref="ImageUrl"/> and <see cref="Text"/> are set,
		/// the <see cref="Text"/> value will be used as the alternate text of the image.
		/// </remarks>
		[
		Description("Gets or sets the Text of the link."),
		Category("Appearance"),
		DefaultValue(""),
		Bindable(true),
		]
		public virtual String Text {
			get {
				Object savedState = this.ViewState["Text"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				if ( value == null ) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the url of an image to display for the link.
		/// </summary>
		/// <remarks>
		/// If both <see cref="ImageUrl"/> and <see cref="Text"/> are set,
		/// the <see cref="Text"/> value will be used as the alternate text of the image.
		/// </remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), Description("Gets or sets the url of an image to display for the link."),
		Category("Appearance"),
		DefaultValue(""),
		Bindable(true),
		EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
		]
		public virtual String ImageUrl {
			get {
				Object savedState = this.ViewState["ImageUrl"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				if ( value == null ) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["ImageUrl"] = value;
			}
		}

		/// <exclude/>
		protected override void RenderChildren(HtmlTextWriter writer) {
			if ( !String.IsNullOrEmpty( this.ImageUrl ) ) {
				System.Web.UI.WebControls.Image image = new Image();
				image.ImageUrl = this.ImageUrl;
				image.AlternateText = this.Text;
				image.RenderControl(writer);
			} else {
				writer.Write(this.Text);
			}
		}

		/// <exclude/>
		protected override void AddAttributesToRender(HtmlTextWriter writer) {

			if ( System.Web.HttpContext.Current != null && this.Page != null && this.Enabled && this.DialogToOpen.Length != 0 ) {

				DialogWindowBase dialog = this.NamingContainer.FindControl(this.DialogToOpen) as DialogWindowBase;
				if ( dialog == null ) {
					dialog = this.Page.FindControl(this.DialogToOpen) as DialogWindowBase;
				}
				if ( dialog == null ) {
					throw new InvalidOperationException( String.Format( CultureInfo.InvariantCulture, Resources.DialogWindow_CannotFindDialogWindow, this.DialogToOpen ) );
				}

				writer.AddAttribute(HtmlTextWriterAttribute.Href,"javascript:void(" + dialog.GetDialogOpenScript() + ");");
			} else {
				writer.AddAttribute(HtmlTextWriterAttribute.Href,"#");
			}

			base.AddAttributesToRender (writer);
		}



	}
}
