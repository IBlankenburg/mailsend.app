using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Represents the style used for the buttons of a <see cref="MultiViewItem"/> in a <see cref="MultiViewBar"/> control.
	/// </summary>
	[
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), 
	System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" ), 
	System.ComponentModel.DesignerCategory( "Code" )
	]
	public class MultiViewItemButtonStyle : Style {
		
		#region Constructors

		/// <summary>
		/// Creates a new instance of the MultiViewItemButtonStyle class.
		/// </summary>
		public MultiViewItemButtonStyle() {
			
		}

		/// <summary>
		/// Creates a new instance of the MultiViewItemButtonStyle class.
		/// </summary>
		public MultiViewItemButtonStyle( StateBag bag ) :
			base( bag ) {
		}

		#endregion

		#region Properties

		private const String key_BackImageUrl = "BackImageUrl";
		private const String key_HorizontalAlign = "HorizontalAlign";
		private const String key_Wrap = "Wrap";


		/// <summary>
		/// Gets or sets the background image of the Button
		/// </summary>
		[
		System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings" ), 
		Bindable( true ),
		Category( "Appearance" ),
		DefaultValue( "" ),
		Description( "Gets or sets the background image of the Button" ),
		NotifyParentProperty( true ),
		]
		public virtual String BackImageUrl {
			get {
				if ( IsBackImageUrlSet ) {
					return (String)ViewState[ key_BackImageUrl ];
				}
				return String.Empty;
			}
			set {
				ViewState[ key_BackImageUrl ] = value;
			}
		}

		/// <summary>
		/// Gets or sets the horizontal alignment of the Button's contents
		/// </summary>
		[
		Bindable( true ),
		Category( "Layout" ),
		DefaultValue( HorizontalAlign.NotSet ),
		Description( "Gets or sets the horizontal alignment of the Button's contents" ),
		NotifyParentProperty( true ),
		]
		public virtual HorizontalAlign HorizontalAlign {
			get {
				if ( IsHorizontalAlignSet ) {
					return (HorizontalAlign)ViewState[ key_HorizontalAlign ];
				}
				return HorizontalAlign.NotSet;
			}
			set {
				ViewState[ key_HorizontalAlign ] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the Button's contents can wrap around
		/// </summary>
		[
		Bindable( true ),
		Category( "Layout" ),
		DefaultValue( false ),
		Description( "Gets or sets whether the Button's contents can wrap around" ),
		NotifyParentProperty( true ),
		]
		public virtual Boolean Wrap {
			get {
				if ( IsWrapSet ) {
					return (Boolean)ViewState[ key_Wrap ];
				}
				return false;
			}
			set {
				ViewState[ key_Wrap ] = value;
			}
		}

		#endregion

		#region Emptyness

		/// <exclude />
		protected new internal Boolean IsEmpty
		{
			get {
				return base.IsEmpty && !IsBackImageUrlSet && !IsHorizontalAlignSet && !IsWrapSet;
			}
		}

		private Boolean IsBackImageUrlSet {
			get {
				return ViewState[ key_BackImageUrl ] != null;
			}
		}

		private Boolean IsHorizontalAlignSet {
			get {
				return ViewState[ key_HorizontalAlign ] != null;
			}
		}

		private Boolean IsWrapSet {
			get {
				return ViewState[ key_Wrap ] != null;
			}
		}

		/// <exclude />
		public override void Reset() {
			base.Reset();

			if ( IsBackImageUrlSet ) {
				ViewState.Remove( key_BackImageUrl );
			}

			if ( IsHorizontalAlignSet ) {
				ViewState.Remove( key_HorizontalAlign );
			}

			if ( IsWrapSet ) {
				ViewState.Remove( key_Wrap );
			}
		}

		#endregion

		#region Render

		/// <exclude />
		public override void AddAttributesToRender( HtmlTextWriter writer, WebControl owner ) {
			base.AddAttributesToRender( writer, owner );

			if ( IsBackImageUrlSet ) {
				String s = BackImageUrl;
				if ( s.Length > 0 ) {
					if ( owner != null ) {
						s = owner.ResolveUrl( s );
					}
					writer.AddStyleAttribute( HtmlTextWriterStyle.BackgroundImage, "url(" + s + ")" );
				}
			}

			if ( IsHorizontalAlignSet && this.HorizontalAlign != HorizontalAlign.NotSet ) {
				TypeConverter converter = TypeDescriptor.GetConverter( typeof( HorizontalAlign ) );
				writer.AddAttribute( HtmlTextWriterAttribute.Align, converter.ConvertToInvariantString( this.HorizontalAlign ) );
			}
			

			if ( !this.Wrap ) {
				writer.AddAttribute( HtmlTextWriterAttribute.Nowrap, "nowrap" );
			}

		}

		#endregion

		#region Copy/Merge

		/// <exclude />
		public override void CopyFrom( Style s ) {
			if ( s != null ) {
				base.CopyFrom( s );

				MultiViewItemButtonStyle buttonStyle = s as MultiViewItemButtonStyle;
				if ( buttonStyle != null && !buttonStyle.IsEmpty ) {
					if ( buttonStyle.IsBackImageUrlSet ) {
						this.BackImageUrl = buttonStyle.BackImageUrl;
					}
					if ( buttonStyle.IsHorizontalAlignSet ) {
						this.HorizontalAlign = buttonStyle.HorizontalAlign;
					}
					if ( buttonStyle.IsWrapSet ) {
						this.Wrap = buttonStyle.Wrap;
					}
				}
			}
		}

		/// <exclude />
		public override void MergeWith(Style s) {
			if ( s != null && IsEmpty ) {
				CopyFrom( s );
				return;
			}

			base.MergeWith( s );

			MultiViewItemButtonStyle buttonStyle = s as MultiViewItemButtonStyle;
			if ( buttonStyle != null && !buttonStyle.IsEmpty ) {
				if ( buttonStyle.IsBackImageUrlSet && !this.IsBackImageUrlSet ) {
					this.BackImageUrl = buttonStyle.BackImageUrl;
				}
				if ( buttonStyle.IsHorizontalAlignSet && !this.IsHorizontalAlignSet ) {
					this.HorizontalAlign = buttonStyle.HorizontalAlign;
				}
				if ( buttonStyle.IsWrapSet && !this.IsWrapSet ) {
					this.Wrap = buttonStyle.Wrap;
				}
			}
		}

		#endregion

	}
}
