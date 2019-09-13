using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Globalization;

namespace MetaBuilders.WebControls.Design {

	/// <summary>
	/// The designer for the MultiViewBar
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiView" )]
	public class MultiViewBarDesigner : ControlDesigner
	{

		private MultiViewBar owner;

		/// <exclude />
		public override void Initialize( System.ComponentModel.IComponent component )
		{
			this.owner = component as MultiViewBar;
			if ( this.owner == null ) {
				throw new ArgumentException( Resources.MultiViewBarDesigner_Only_MultiViewBar, "component" );
			}
			base.Initialize( component );
			this.SetViewFlags( ViewFlags.TemplateEditing, true );
		}

		#region HTML

		/// <exclude />
		public override string GetDesignTimeHtml()
		{
			if ( owner.Items.Count == 0 ) {
				return this.CreatePlaceHolderDesignTimeHtml( Resources.MultiViewBarDesigner_Add_Items );
			} else {
				
				MultiViewItem currentItem = null;
				foreach( MultiViewItem item in owner.Items ) {
					if ( item.Title == owner.CurrentItem ) {
						currentItem = item;
						break;
					}
				}
				if ( currentItem == null ) {
					currentItem = owner.Items[ 0 ];
				}

				Boolean emptyTemplateFound = false;
				if ( currentItem.ContentTemplate == null || ( currentItem.Controls[ 0 ].Controls.Count == 0 ) ) {
					currentItem.Controls[ 0 ].Controls.Add( new LiteralControl( this.CreatePlaceHolderDesignTimeHtml( String.Format(CultureInfo.InvariantCulture, Resources.MultiViewBarDesigner_Edit_Item_Template, currentItem.Title ) ) ) );
					emptyTemplateFound = true;
				}


				String result = base.GetDesignTimeHtml();

				if ( emptyTemplateFound ) {
					currentItem.Controls[ 0 ].Controls.Clear();
				}

				return result;
			}
		}

		#endregion

		#region Templating

		/// <exclude />
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection groupCollection = new TemplateGroupCollection();
				TemplateGroup group = new TemplateGroup( "MultiViewBar Item Templates" );
				for ( int i = 0; i < owner.Items.Count; i++ )
				{
					MultiViewItem item = owner.Items[i];
					TemplateDefinition def = new TemplateDefinition( this, String.Format(CultureInfo.InvariantCulture, Resources.MultiViewBarDesigner_Template_Edit_Verb_Text, item.Title ), item, "ContentTemplate", false );
					group.AddTemplateDefinition( def );
				}
				groupCollection.Add( group );
				return groupCollection;
			}

		}

		#endregion

	}
}
