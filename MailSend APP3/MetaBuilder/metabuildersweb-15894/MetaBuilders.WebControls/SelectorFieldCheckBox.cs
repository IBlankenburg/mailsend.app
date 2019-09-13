using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls
{
	public partial class SelectorField
	{

		/// <summary>
		/// The checkboxes which appear in a <see cref="SelectorField"/> when <see cref="SelectorField.SelectionMode"/> is set to <see cref="ListSelectionMode.Multiple"/>.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class SelectorFieldCheckBox : System.Web.UI.HtmlControls.HtmlInputCheckBox, ISelectorFieldControl
		{
			/// <summary>
			/// Creates a new instance for the given field.
			/// </summary>
			public SelectorFieldCheckBox( SelectorField field )
				: base()
			{
				_field = field;
			}

			SelectorField _field;

			#region ISelectorFieldControl

			/// <summary>
			/// Gets or sets if the control will postback after being changed.
			/// </summary>
			public Boolean AutoPostBack
			{
				get
				{
					Object savedState = this.ViewState[ "AutoPostBack" ];
					if ( savedState != null )
					{
						return (Boolean)savedState;
					}
					return false;
				}
				set
				{
					this.ViewState[ "AutoPostBack" ] = value;
				}
			}

			/// <summary>
			/// Gets or sets if the control is selected.
			/// </summary>
			public bool Selected
			{
				get
				{
					return this.Checked;
				}
				set
				{
					this.Checked = value;
				}
			}

			/// <summary>
			/// Gets or sets the index of the control.
			/// </summary>
			public int Index
			{
				get
				{
					Object state = this.ViewState[ "SelectorIndex" ];
					if ( state != null )
					{
						return (Int32)state;
					}
					return -1;
				}
				set
				{
					this.ViewState[ "SelectorIndex" ] = value;
				}
			}

			#endregion

			/// <exclude />
			protected override void OnPreRender( EventArgs e )
			{
				base.OnPreRender( e );
				if ( Page != null )
				{
					this.Page.ClientScript.RegisterArrayDeclaration( "MetaBuilders_SelectorField_CheckBoxes", "{ Field:'" + this._field.FieldId + "', ID:'" + this.ClientID + "' }" );
				}
			}

			/// <exclude />
			protected override void RenderAttributes( HtmlTextWriter writer )
			{
				{
					String originalOnClick = this.Attributes[ "onclick" ];
					if ( originalOnClick != null )
					{
						this.Attributes.Remove( "onclick" );
					}
					else
					{
						originalOnClick = "";
					}
					this.Attributes[ "onclick" ] = "MetaBuilders_SelectorField_CheckChildren( '" + this._field.FieldId + "' ); " + originalOnClick;
				}

				if ( this.AutoPostBack )
				{
					String originalOnClick = this.Attributes[ "onclick" ];
					if ( originalOnClick != null )
					{
						this.Attributes.Remove( "onclick" );
					}
					else
					{
						originalOnClick = "";
					}
					this.Attributes[ "onclick" ] = originalOnClick + " ; " + Page.ClientScript.GetPostBackEventReference( this, "" );
				}

				base.RenderAttributes( writer );
			}

		}

	}
}
