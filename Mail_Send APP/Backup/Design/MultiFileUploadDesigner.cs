using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design.WebControls;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls.Design
{
	internal class MultiFileUploadDesigner : ControlDesigner
	{

		MultiFileUpload parent;

		public override void Initialize( System.ComponentModel.IComponent component )
		{
			base.Initialize( component );
			parent = component as MultiFileUpload;
		}

		protected virtual void CreateChildControls()
		{
			ICompositeControlDesignerAccessor accessor = (ICompositeControlDesignerAccessor)base.ViewControl;
			accessor.RecreateChildControls();
		}


		public override string GetDesignTimeHtml()
		{
			this.CreateChildControls();

			for ( int i = 1; i < parent.uploaders.Length; i++ )
			{
				parent.uploaders[ i ].Visible = false;
			}

			String result = base.GetDesignTimeHtml();

			for ( int i = 1; i < parent.uploaders.Length; i++ )
			{
				parent.uploaders[ i ].Visible = true;
			}

			return result;
		}
	}
}
