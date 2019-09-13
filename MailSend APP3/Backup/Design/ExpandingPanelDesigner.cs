using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.Web.UI.Design.WebControls;

namespace MetaBuilders.WebControls.Design
{

	/// <summary>
	/// The designer for the ExpandingPanel control.
	/// </summary>
	public class ExpandingPanelDesigner : CompositeControlDesigner
	{

		/// <exclude />
		public override void Initialize( IComponent component )
		{
			base.Initialize( component );
			SetViewFlags( ViewFlags.TemplateEditing, true );
		}

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public override string GetDesignTimeHtml()
		{
			ExpandingPanel panel = this.Panel;

			if ( panel.ExpandedTemplate == null && panel.ContractedTemplate == null )
			{
				return GetEmptyDesignTimeHtml();
			}

			string designTimeHtml = String.Empty;
			try
			{
				panel.DataBind();
				Boolean originalClientScript = panel.EnableClientScript;
				panel.EnableClientScript = false;
				designTimeHtml = base.GetDesignTimeHtml();
				panel.EnableClientScript = originalClientScript;
			}
			catch ( Exception e )
			{
				designTimeHtml = GetErrorDesignTimeHtml( e );
			}

			return designTimeHtml;
		}

		/// <exclude />
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{

				if ( col == null )
				{
					TemplateGroup tempGroup = new TemplateGroup( "Panel Templates" );

					TemplateDefinition expandedTemplate = new TemplateDefinition( this, "Expanded View Template", this.Panel, "ExpandedTemplate", false );
					tempGroup.AddTemplateDefinition( expandedTemplate );

					TemplateDefinition contractedTemplate = new TemplateDefinition( this, "Contracted View Template", this.Panel, "ContractedTemplate", false );
					tempGroup.AddTemplateDefinition( contractedTemplate );

					col = base.TemplateGroups;
					col.Add( tempGroup );

				}

				return col;
			}
		}

		/// <exclude />
		public override bool AllowResize
		{
			get
			{
				bool templateExists = ( Panel.ContractedTemplate != null ) && ( Panel.ExpandedTemplate != null );
				if ( templateExists || this.InTemplateMode )
					return true;
				else
					return false;
			}
		}

		/// <exclude />
		protected ExpandingPanel Panel
		{
			get
			{
				return (ExpandingPanel)this.Component;
			}
		}

		TemplateGroupCollection col = null;
	}
}
