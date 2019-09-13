using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Resources;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A ListBox which can retrieve and persist any client-side changes to its Items collection.
	/// </summary>
	/// <remarks>
	/// <p>
	/// In order for your client-side changes to be persisted, 
	/// you must call new javascript methods on the ListBox.
	/// This Select element now has "Add" and "Remove" methods.
	/// The Add method takes the value, text, and optional index of the new ListItem.
	/// The Remove method takes the index of the ListItem to remove.
	/// </p>
	/// </remarks>
	/// <example>
	/// The following is an example of how to combine the DynamicListBox with client-script in your page.
	/// <code><![CDATA[
	/// <%@ Page Language="C#" %>
	/// <%@ Register TagPrefix="mb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DualList" %>
	/// <script runat="server">
	/// 
	/// 	protected void Page_Load( Object sender, EventArgs e ) {
	///			RegisterClientScriptBlock( MyList.ClientID, "<script>\r\nwindow.MyListId='" + MyList.ClientID + "';\r\n</" + "script>");
	///		}
	///     protected void MyList_ListChanged( Object sender, EventArgs e ) {
	///         if( MyList.SelectedIndex == -1 ) {
	///             Result.Text = "The SelectedIndex changed and now nothing on the list is selected";
	///         } else {
	///             Result.Text = "The SelectedIndex changed and is now '" + MyList.SelectedIndex + "'<br>The selected item is: " + MyList.SelectedItem.Value + "/" + MyList.SelectedItem.Text;
	///         }
	///     }
	/// 
	/// </script>
	/// <html><body><form runat="server">
	/// 	
	///     <mb:DynamicListBox id="MyList" runat="server" OnSelectedIndexChanged="MyList_ListChanged" SelectionMode="Multiple" >
	///         <asp:ListItem value="normalItem1" text="normalItem1"></asp:ListItem>
	///         <asp:ListItem value="normalItem2" text="normalItem2"></asp:ListItem>
	///     </mb:DynamicListBox>
	///     
	///     <br><br>
	///     <a href="javascript:remove();" >Remove</a>
	///     <a href="javascript:add();" >Add</a>
	///     <script>
	/// 		function remove() {
	/// 			var list = DynamicListBox_FindControl(window.MyListId);
	/// 			var keepLooking = true;
	/// 			while ( keepLooking ) {
	/// 				list.Remove( list.options.selectedIndex );
	/// 				if ( list.options.selectedIndex < 0 ) {
	/// 					keepLooking = false;
	/// 				}
	/// 			}
	/// 		}
	/// 		
	/// 		function add() {
	/// 			var list = DynamicListBox_FindControl(window.MyListId);
	/// 			var generatedName = "newItem" + ( list.options.length + 1 );
	/// 			list.Add(generatedName,generatedName);
	/// 		}
	///     </script>
	///     
	///     <br><br>
	///     <asp:Button runat="server" text="Smack"/>
	///     
	///     <br><br>
	///     <asp:Label runat="server" id="Result" />
	///     
	/// </form></body></html>
	/// ]]></code></example>
	public partial class DynamicListBox : System.Web.UI.WebControls.ListBox, INamingContainer
	{

		#region Render Overrides
		/// <summary>
		/// Overridden to call EnsureChildControls.
		/// </summary>
		/// <remarks>Supposedly should be done for all controls which override CreateChildControls</remarks>
		public override System.Web.UI.ControlCollection Controls
		{
			get
			{
				EnsureChildControls();
				return base.Controls;
			}
		}

		/// <summary>
		/// Overridden to include the hidden input which tracks the client side changes.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			this.itemTracker = new HtmlInputHidden();
			this.itemTracker.ID = "itemTracker";
			this.itemTracker.EnableViewState = false;
			this.Controls.Add( itemTracker );
		}

		/// <summary>
		/// Overridden to register client script.
		/// </summary>
		protected override void OnPreRender( System.EventArgs e )
		{
			base.OnPreRender( e );
			if ( this.Page != null )
			{
				this.Page.RegisterRequiresPostBack( this );
				this.ItemTracker.Value = "";
				this.RegisterClientScript();
			}
		}

		/// <summary>
		/// Overridden to render children after the end tag.
		/// </summary>
		public override void RenderEndTag( System.Web.UI.HtmlTextWriter writer )
		{
			base.RenderEndTag( writer );
			this.RenderChildren( writer );
		}
		#endregion

		#region Events

		/// <summary>
		/// The event that is raised when the user has changed the listbox's items collection..
		/// </summary>
		[
		Category( "Action" ),
		]
		public event EventHandler ItemsChanged
		{
			add
			{
				Events.AddHandler( EventItemsChanged, value );
			}
			remove
			{
				Events.RemoveHandler( EventItemsChanged, value );
			}
		}

		/// <summary>
		/// Raises the <see cref="ItemsChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnItemsChanged( EventArgs e )
		{
			EventHandler handler = Events[ EventItemsChanged ] as EventHandler;
			if ( handler != null )
			{
				handler( this, e );
			}
		}

		/// <summary>
		/// The object which is the key into the Events dictionary for the ItemsChanged event.
		/// </summary>
		protected static readonly Object EventItemsChanged = new Object();

		#endregion

		#region Implementation of IPostBackDataHandler

		/// <summary>
		/// Raises the <see cref="ListControl.SelectedIndexChanged"/> and <see cref="ItemsChanged"/> events when appropriate.
		/// </summary>
		protected override void RaisePostDataChangedEvent()
		{
			if ( this.raiseItemsChanged )
			{
				OnItemsChanged( EventArgs.Empty );
			}
			if ( this.selectedItemsChanged || this.newSelectionPosted )
			{
				this.OnSelectedIndexChanged( EventArgs.Empty );
			}
		}

		/// <summary>
		/// Loads any changes in items or selected index from the post data.
		/// </summary>
		protected override Boolean LoadPostData( string postDataKey, System.Collections.Specialized.NameValueCollection postCollection )
		{
			if ( !dataLoaded )
			{
				dataLoaded = true;
				selectedItemsChanged = loadNewItems( postCollection );

				base.LoadPostData( postDataKey, postCollection );

				this.raiseItemsChanged = postCollection[ this.ItemTracker.UniqueID ] != null && postCollection[ this.ItemTracker.UniqueID ].Length > 0;

				return this.raiseItemsChanged || selectedItemsChanged || newSelectionPosted;
			}
			else
			{
				return false;
			}
		}

		private Boolean dataLoaded = false;
		private Boolean raiseItemsChanged = false;
		private Boolean selectedItemsChanged = false;
		private Boolean newSelectionPosted = false;

		/// <summary>
		/// Edits the ListItems that were added or removed on the clientside
		/// </summary>
		/// <returns>True if the list of items has changed.</returns>
		private Boolean loadNewItems( System.Collections.Specialized.NameValueCollection postCollection )
		{
			this.EnsureChildControls();
			String postedValue = postCollection[ this.ItemTracker.UniqueID ];

			Boolean result = false;

			ArrayList currentSelection = this.SelectedIndicesInternal;

			if ( postedValue != null && postedValue.Length != 0 )
			{
				ListCommand[] commands = ListCommand.Split( postedValue.Trim() );
				foreach ( ListCommand command in commands )
				{
					if ( command.Operator == "+" )
					{
						ListItem newItem = new ListItem( command.Text, command.Value );
						if ( command.Index >= 0 && command.Index <= this.Items.Count - 1 )
						{
							this.Items.Insert( command.Index, newItem );
						}
						else
						{
							this.Items.Add( newItem );
						}
					}
					else if ( command.Operator == "-" )
					{
						if ( command.Index >= 0 && command.Index <= this.Items.Count - 1 )
						{
							if ( this.Items[ command.Index ].Selected )
							{
								result = true;
							}
							this.Items.RemoveAt( command.Index );
						}
					}
				}
			}

			if ( !result )
			{
				ArrayList newSelection = this.SelectedIndicesInternal;
				if ( newSelection == null )
				{
					result = ( currentSelection != null );
				}
				else
				{
					if ( newSelection.Count != currentSelection.Count )
					{
						result = true;
					}
					else
					{
						for ( Int32 i = 0; i < currentSelection.Count; i++ )
						{
							if ( (Int32)newSelection[ i ] != (Int32)currentSelection[ i ] )
							{
								result = true;
								break;
							}
						}
					}
				}
			}

			return result;
		}

		private ArrayList SelectedIndicesInternal
		{
			get
			{
				ArrayList selections;

				selections = null;
				for ( Int32 i = 0; i < this.Items.Count; i++ )
				{
					if ( this.Items[ i ].Selected )
					{
						if ( selections == null )
						{
							selections = new ArrayList( 3 );
						}
						selections.Add( i );
					}
				}
				return selections;
			}
		}

		#endregion

		#region Client Script

		/// <summary>
		/// Registers all the client script for the DynamicListBox
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			ClientScriptManager script = this.Page.ClientScript;
            script.RegisterClientScriptResource(typeof(DynamicListBox), "MetaBuilders.WebControls.Embedded.DynamicListBoxScript.js");
			script.RegisterStartupScript( typeof( DynamicListBox ), "DynamicListBox Script", "MetaBuilders_DynamicListBox_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_DynamicListBox_Init" ), true );
			script.RegisterArrayDeclaration( "MetaBuilders_DynamicListBoxes", "{ ID:'" + this.ClientID + "', TrackerID:'" + this.ItemTracker.ClientID + "' }" );
		}

		#endregion

		private HtmlInputHidden itemTracker;
		private HtmlInputHidden ItemTracker
		{
			get
			{
				this.EnsureChildControls();
				return itemTracker;
			}
		}

	}
}
