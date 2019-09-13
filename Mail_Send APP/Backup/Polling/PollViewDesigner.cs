using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The designer for the PollView control.
	/// </summary>
	/// <exclude/>
	[
	SupportsPreviewControl(false),
	]
	public class PollViewDesigner : ControlDesigner
	{

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		public override void Initialize( System.ComponentModel.IComponent component )
		{
			base.Initialize( component );
			this.SetViewFlags( ViewFlags.TemplateEditing, true );

			try
			{
				IWebApplication webApp = (IWebApplication)component.Site.GetService( typeof( IWebApplication ) );
				Configuration config = webApp.OpenWebConfiguration( false );

				ConfigHelper.WriteConfig( config );

			}
			catch ( Exception ex )
			{
				// Gulp
				Debug.Write( ex.ToString() );
			}

		}


		#region Design-time HTML

		/// <exclude/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		public override string GetDesignTimeHtml()
		{
			String designTimeHtml = String.Empty;
			try
			{
				( (Control)this.Component ).DataBind();
				designTimeHtml = base.GetDesignTimeHtml();
			}
			catch ( Exception ex )
			{
				designTimeHtml = this.GetErrorDesignTimeHtml( ex );
			}
			return designTimeHtml;
		}

		/// <exclude/>
		protected override string GetEmptyDesignTimeHtml()
		{
			return this.CreatePlaceHolderDesignTimeHtml( "Right-click to edit the templates of this control." );
		}

		/// <exclude/>
		protected override string GetErrorDesignTimeHtml( Exception e )
		{
			return this.CreatePlaceHolderDesignTimeHtml( "There was an error rendering the control." );
		}

		#endregion


		#region Templating

		/// <exclude />
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection groupCollection = new TemplateGroupCollection();
				TemplateGroup group = new TemplateGroup( "PollView Templates" );

				List<String> templateProperties = new List<string>(){
					"Header",
					"Footer",
					"NoPoll",
					"WriteInHeader",
					"WriteInItem",
					"WriteInFooter"
				};

				foreach( String templateProp in templateProperties ) {
					TemplateDefinition def = new TemplateDefinition( this, templateProp, this.Component, templateProp + "Template", true );
					group.AddTemplateDefinition( def );
				}

				groupCollection.Add( group );
				return groupCollection;
			}

		}

		#endregion

	}
}