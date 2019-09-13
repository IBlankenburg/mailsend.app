using System;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Globalization;

namespace MetaBuilders.WebControls
{
	public partial class SelectorField
	{

		/// <summary>
		/// The radiobutton which appears a <see cref="SelectorField"/> when <see cref="SelectorField.SelectionMode"/> is set to <see cref="ListSelectionMode.Single"/>.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public sealed class SelectorFieldRadioButton : System.Web.UI.HtmlControls.HtmlInputRadioButton, ISelectorFieldControl, IPostBackDataHandler
		{
			/// <summary>
			/// Creates a new instance for the given field.
			/// </summary>
			/// <param name="field"></param>
			public SelectorFieldRadioButton( SelectorField field )
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
			/// Gets or sets if this control is selected.
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
			/// Gets or sets the Index of this control.
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

			#region IPostBackDataHandler Implementation

			/// <summary>
			/// This doesn't differ from the original implementaion... 
			/// except now i'm using my own RenderednameAttribute instead of the InputControl implementation.
			/// </summary>
			bool IPostBackDataHandler.LoadPostData( string postDataKey, NameValueCollection postCollection )
			{
				bool result = false;

				string postedValue = postCollection[ this.RenderedNameAttribute ];
				if ( postedValue != null && postedValue == this.Value )
				{
					if ( this.Checked )
					{
						return result;
					}
					this.Checked = true;
					result = true;
				}
				else if ( this.Checked )
				{
					this.Checked = false;
				}
				return result;
			}

			/// <summary>
			/// No change from the InputControl implementation
			/// </summary>
			void IPostBackDataHandler.RaisePostDataChangedEvent()
			{
				this.OnServerChange( EventArgs.Empty );
			}

			#endregion

			/// <exclude />
			protected override void RenderAttributes( HtmlTextWriter writer )
			{
				writer.WriteAttribute( "value", this.Value );
				this.Attributes.Remove( "value" );
				writer.WriteAttribute( "name", this.RenderedNameAttribute );
				this.Attributes.Remove( "name" );
				if ( this.ID != null )
				{
					writer.WriteAttribute( "id", this.ClientID );
				}
				if ( this.AutoPostBack )
				{
					String finalOnClick = this.Attributes[ "onclick" ];
					if ( finalOnClick != null )
					{
						finalOnClick += " " + Page.ClientScript.GetPostBackEventReference( this, "" );
						this.Attributes.Remove( "onclick" );
					}
					else
					{
						finalOnClick = Page.ClientScript.GetPostBackEventReference( this, "" );
					}
					writer.WriteAttribute( "onclick", finalOnClick );
				}

				this.Attributes.Render( writer );
				writer.Write( " /" );

			}

			/// <summary>
			/// Gets the final rendering of the Name attribute.
			/// </summary>
			/// <remarks>
			/// Differs from the standard RenderedNameAttribute to use the column as the logical naming container instead of the row.
			/// </remarks>
			private String RenderedNameAttribute
			{
				get
				{
					GridView _containerGrid = this._field.Control as GridView;
					if ( _containerGrid == null )
					{
						return this.Name;
					}
					else
					{
						System.Text.StringBuilder groupContainer = new System.Text.StringBuilder();
						groupContainer.Append( _containerGrid.UniqueID );
						groupContainer.Append( this.IdSeparator );
						groupContainer.Append( "SelectorField" );
						groupContainer.Append( _containerGrid.Columns.IndexOf( this._field ).ToString( CultureInfo.InvariantCulture ) );
						groupContainer.Append( this.IdSeparator );
						groupContainer.Append( this.Name );

						return groupContainer.ToString();
					}
				}
			}


		}

	}
}
