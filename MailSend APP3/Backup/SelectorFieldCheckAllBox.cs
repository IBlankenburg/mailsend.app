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
		public class SelectorFieldCheckAllBox : System.Web.UI.HtmlControls.HtmlInputCheckBox, ISelectorFieldControl
		{

			/// <summary>Creates a new instance of the SelectorFieldCheckAllBox for the given field</summary>
			public SelectorFieldCheckAllBox( SelectorField field )
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
					return false;
				}
				set
				{
					
				}
			}

			/// <summary>
			/// Gets the index of the control. Always returns -1.
			/// </summary>
			public int Index
			{
				get
				{
					return -1;
				}
				set
				{
					// noop
				}
			}

			#endregion

			/// <exclude />
			protected override void OnPreRender( EventArgs e )
			{
				base.OnPreRender( e );

				if ( Page != null )
				{
					this.Page.ClientScript.RegisterArrayDeclaration( "MetaBuilders_SelectorField_CheckAllBoxes", "{ Field:'" + this._field.FieldId + "', ID:'" + this.ClientID + "' }" );
				}
			}

			/// <exclude />
			protected override void RenderAttributes( HtmlTextWriter writer )
			{
				String finalOnClick = this.Attributes[ "onclick" ];
				if ( finalOnClick == null )
				{
					finalOnClick = "";
				}

				finalOnClick += " MetaBuilders_SelectorField_SelectAll( this );";
				if ( this.AutoPostBack )
				{
					finalOnClick += Page.ClientScript.GetPostBackEventReference( this, "" );
				}
				this.Attributes[ "onclick" ] = finalOnClick;

				base.RenderAttributes( writer );
			}

		}

	}
}
