using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Resources;


namespace MetaBuilders.WebControls
{

	/// <summary>
	/// Displays an image link on the page.
	/// When the mouse is over it, the images changes to a different one.
	/// </summary>
	/// <remarks>
	/// This behaves exactly like a <see cref="System.Web.UI.WebControls.HyperLink"/> unless both <see cref="System.Web.UI.WebControls.HyperLink.ImageUrl"/> and <see cref="RollOverLink.RollOverImageUrl"/> are specified.
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to create a image which does a roll-over.
	/// <code>
	/// <![CDATA[
	/// <html>
	///		<body>
	///		<form runat="server">
	///			<h3>RollOver Example</h3>
	///
	///			Put your mouse over the image.<br><br>
	///
	///			<mb:RollOverLink id="myLink"
	///				Text="google"
	///				ImageUrl="/normalImage.png"
	///				RollOverImageUrl="/activeImage.png"
	///				NavigateUrl="http://www.google.com"
	///				runat="server"/>
	///       
	///		</form>
	///		</body>
	/// </html>
	/// ]]>
	/// </code>
	/// </example>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "RollOver" )]
	public class RollOverLink : HyperLink
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="RollOverLink"/> class.
		/// </summary>
		/// <remarks>
		/// Use this constructor to create and initialize a new instance of the <see cref="RollOverLink"/> class.
		/// </remarks>
		public RollOverLink()
			: base()
		{
		}

		/// <summary>
		/// Gets or sets the path to an image to display for the <see cref="RollOverLink"/> control during a mouse-over.
		/// </summary>
		/// <value>
		/// The path to the image to display for the <see cref="RollOverLink"/> control. The default value is <see cref="String.Empty"/>.
		/// </value>
		/// <remarks>
		/// The <see cref="RollOverImageUrl"/> is only used if <see cref="HyperLink.ImageUrl"/> is also set.
		/// The image will be shown when the user puts the mouse arrow over the image.
		/// </remarks>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "RollOver" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Bindable( true ),
		Category( "Appearance" ),
		Editor( typeof( System.Web.UI.Design.ImageUrlEditor ), typeof( System.Drawing.Design.UITypeEditor ) ),
		DefaultValue( "" ),
		]
		public virtual String RollOverImageUrl
		{
			get
			{
				object savedState = this.ViewState["RollOverImageUrl"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				ViewState["RollOverImageUrl"] = value;
			}
		}

		/// <exclude />
		protected override void OnPreRender( EventArgs e )
		{
			this.RegisterClientScript();
			base.OnPreRender( e );
		}

		/// <summary>
		/// Emits the client scripts which support the rollover behavior.
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			if ( !String.IsNullOrEmpty( RollOverImageUrl ) && !String.IsNullOrEmpty( this.ImageUrl ) )
			{
				Page.ClientScript.RegisterClientScriptResource( typeof( RollOverLink ), "MetaBuilders.WebControls.Embedded.RollOverLinkScript.js" );
				Page.ClientScript.RegisterStartupScript( typeof( RollOverLink ), "Startup", "RollOverLink_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "RollOverLink_Init" ), true );
				Page.ClientScript.RegisterArrayDeclaration( "RollOverLink_Images", "'" + this.ResolveUrl( RollOverImageUrl ) + "'" );
			}
		}

		/// <exclude />
		protected override void AddAttributesToRender( HtmlTextWriter writer )
		{
			base.AddAttributesToRender( writer );
			if ( !String.IsNullOrEmpty( RollOverImageUrl ) && !String.IsNullOrEmpty( this.ImageUrl ) )
			{
				writer.AddAttribute( "OnMouseOver", "RollOverLink_swapImage('" + this.ClientID + "_img" + "','','" + this.ResolveUrl( RollOverImageUrl ) + "',1);" );
				writer.AddAttribute( "OnMouseOut", "RollOverLink_swapImgRestore();" );
			}
		}

		/// <exclude />
		protected override void RenderContents( HtmlTextWriter writer )
		{

			if ( !String.IsNullOrEmpty( this.ImageUrl ) )
			{
				System.Web.UI.WebControls.Image innerImage = new System.Web.UI.WebControls.Image();
				innerImage.ImageUrl = this.ResolveClientUrl( this.ImageUrl );
				innerImage.Attributes["name"] = this.ClientID + "_img";
				innerImage.ToolTip = this.ToolTip;
				innerImage.AlternateText = this.Text;
				innerImage.Height = this.Height;
				innerImage.Width = this.Width;
				innerImage.RenderControl( writer );
				return;
			}
			if ( this.HasControls() )
			{
				base.RenderContents( writer );
				return;
			}
			writer.Write( this.Text );

		}
	}
}