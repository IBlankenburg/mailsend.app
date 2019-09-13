using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>The Designer for the ComboBox</summary>
	[
	SupportsPreviewControl( true ),
	]
	public class ComboBoxDesigner : ListControlDesigner
	{

		/// <exclude />
		protected virtual void CreateChildControls()
		{
			ICompositeControlDesignerAccessor accessor1 = (ICompositeControlDesignerAccessor)base.ViewControl;
			accessor1.RecreateChildControls();
		}

		/// <summary>
		/// Overrides <see cref="ListControlDesigner.GetDesignTimeHtml"/>.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public override String GetDesignTimeHtml()
		{
			this.CreateChildControls();

			try
			{
				ComboBox combo = this.ViewControl as ComboBox;
				combo.SelectControl.Visible = false;

				String originalText = combo.Text;
				if ( String.IsNullOrEmpty( combo.Text ) && ( this.IsDataBound || combo.Items.Count == 0 ) )
				{
					if ( this.IsDataBound )
					{
						combo.Text = Resources.DataBound;
					}
					else
					{
						combo.Text = Resources.UnDataBound;
					}
				}

				String result = base.GetDesignTimeHtml();

				combo.Text = originalText;
				combo.SelectControl.Visible = true;

				return result;
			}
			catch ( Exception ex )
			{
				return this.GetErrorDesignTimeHtml( ex );
			}
		}

		private bool IsDataBound
		{
			get
			{
				if ( base.DataBindings[ "DataSource" ] == null )
				{
					return ( base.DataSourceID.Length > 0 );
				}
				return true;
			}
		}




	}
}
