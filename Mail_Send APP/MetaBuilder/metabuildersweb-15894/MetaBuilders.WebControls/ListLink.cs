using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetaBuilders.WebControls.Design;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// The <see cref="ListLink"/> control is a non-visual control which creates a parent-child link
	/// between two <see cref="ListControl"/>s which render to the html <b>select</b> element.
	/// </summary>
	/// <remarks>
	/// This control requires that both lists are DataBound to the same <see cref="DataSet"/>,
	/// and that that <see cref="DataSet"/> contains the <see cref="System.Data.DataRelation"/> specified in the <see cref="ListLink.DataRelation"/> property.
	/// </remarks>
	/// <example>
	/// The following is an example page using two <see cref="ListLink"/> controls to bind three lists together, using the Northwind database supplied with MS Sql Server.
	/// <code><![CDATA[
	/// <%@ Page language="C#" %>
	/// <%@ Register tagprefix="mbll" namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.ListLink" %>
	/// <%@ Import namespace="System.Data" %>
	/// <%@ Import namespace="System.Data.SqlClient" %>
	/// <script runat="server">
	/// protected void Page_Load( Object sender, EventArgs e ) {
	///		if (!Page.IsPostBack) {
	///			BindData();
	///		}
	/// }
	/// private void BindData() {
	///		string ConnectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Northwind;Data Source=localhost";
	///		string RegionsSelectCommand = "Select RegionId, RTrim(RegionDescription) As Description From Region";
	///		string TerritoriesSelectCommand = "SELECT TerritoryId, RTrim(TerritoryDescription) As Description, RegionId from Territories";
	///		string PeopleSelectCommand = "Select EmployeeId, TerritoryId, FirstName + ' ' + LastName As Name From EmployeesInTerritories";
	///		SqlConnection myConnection = new SqlConnection(ConnectionString);
	///		SqlDataAdapter ad;
	///		DataSet ds = new DataSet();
	/// 
	///		ad = new SqlDataAdapter(RegionsSelectCommand,myConnection);
	///		ad.Fill(ds, "Regions");
	/// 
	///		ad = new SqlDataAdapter(TerritoriesSelectCommand,myConnection);
	///		ad.Fill(ds, "Territories");
	/// 
	///		ad = new SqlDataAdapter(PeopleSelectCommand,myConnection);
	///		ad.Fill(ds, "People");
	/// 
	///		ds.Relations.Add( new DataRelation( "RegionsTerritories", ds.Tables["Regions"].Columns["RegionId"], ds.Tables["Territories"].Columns["RegionId"] ) );
	///		ds.Relations.Add( new DataRelation( "TerritoriesPeople", ds.Tables["Territories"].Columns["TerritoryId"], ds.Tables["People"].Columns["TerritoryId"] ) );
	///		    
	///		ParentListBox.DataSource = ds;
	///		ParentListBox.DataMember = "Regions";
	///		ParentListBox.DataValueField = "RegionId";
	///		ParentListBox.DataTextField = "Description";
	///		ParentListBox.DataBind();
	/// 
	///		ChildListBox.DataSource = ds;
	///		ChildListBox.DataMember = "Territories";
	///		ChildListBox.DataValueField = "TerritoryId";
	///		ChildListBox.DataTextField = "Description";
	///		ChildListBox.DataBind();
	///	
	///		GrandChildListBox.DataSource = ds;
	///		GrandChildListBox.DataMember = "People";
	///		GrandChildListBox.DataValueField = "EmployeeId";
	///		GrandChildListBox.DataTextField = "Name";
	///		GrandChildListBox.DataBind();
	///         
	///		ListLink1.DataBind();
	///		ListLink2.DataBind();
	/// 
	/// }
	/// </script>
	/// <html><body><form runat="server">
	///		<asp:ListBox runat="server" Id="ParentListBox" />
	///		<asp:DropDownList runat="server" Id="ChildListBox" />
	///		<asp:ListBox runat="server" Id="GrandChildListBox" />
	/// 
	///		<mbll:ListLink runat="server" Id="ListLink1" ParentList="ParentListBox" ChildList="ChildListBox" DataRelation="RegionsTerritories" EnableViewState="True" />
	///		<mbll:ListLink runat="server" Id="ListLink2" ParentList="ChildListBox" ChildList="GrandChildListBox" DataRelation="TerritoriesPeople" EnableViewState="True" />
	/// 
	///		<asp:Button runat="server" Text="Smack" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	Designer( typeof( ListLinkDesigner ) ),
	]
	public class ListLink : Control
	{

		#region Properties

		/// <summary>
		/// Gets or sets the ID of the parent <see cref="ListControl"/>.
		/// </summary>
		[
		Description( "Gets or sets the ID of the parent ListControl." ),
		Category( "Data" ),
		Bindable( true ),
		DefaultValue( "" ),
		TypeConverter( typeof( ListControlConverter ) ),
		]
		public virtual String ParentList
		{
			get
			{
				Object savedState = this.ViewState["ParentList"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState["ParentList"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ID of the child <see cref="ListControl"/>.
		/// </summary>
		[
		Description( "Gets or sets the ID of the child ListControl." ),
		Category( "Data" ),
		Bindable( true ),
		DefaultValue( "" ),
		TypeConverter( typeof( ListControlConverter ) ),
		]
		public virtual String ChildList
		{
			get
			{
				Object savedState = this.ViewState["ChildList"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState["ChildList"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the <see cref="System.Data.DataRelation"/> that links the items in the <see cref="ParentList"/> and <see cref="ChildList"/>.
		/// </summary>
		[
		Description( "Gets or sets the name of the DataRelation that links the items in the ParentList and ChildList." ),
		Category( "Data" ),
		Bindable( true ),
		DefaultValue( "" ),
		]
		public virtual String DataRelation
		{
			get
			{
				Object savedState = this.ViewState["DataRelation"];
				if ( savedState != null )
				{
					return (String)savedState;
				}
				return "";
			}
			set
			{
				this.ViewState["DataRelation"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the <see cref="ListLink"/> is active or not.
		/// </summary>
		[
		Description( "Gets or sets whether the ListLink is active or not." ),
		Category( "Behavior" ),
		Bindable( true ),
		DefaultValue( true ),
		]
		public virtual Boolean Enabled
		{
			get
			{
				Object savedState = this.ViewState["Enabled"];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return true;
			}
			set
			{
				this.ViewState["Enabled"] = value;
			}
		}

		/// <summary>
		/// Gets or sets if the first item in the <see cref="ChildList"/> will exist for all selections of the <see cref="ParentList"/>
		/// </summary>
		/// <remarks>
		/// Set this to true if you want your "Any" or "Please Select An Item" ListItem to show no matter what is selected in the parent list.
		/// </remarks>
		[
		Description( "Gets or sets if the first item in the ChildList will exist for all selections of the ParentList" ),
		Category( "Behavior" ),
		Bindable( true ),
		DefaultValue( false ),
		]
		public virtual Boolean LockFirstItem
		{
			get
			{
				Object savedState = this.ViewState["LockFirstItem"];
				if ( savedState != null )
				{
					return (Boolean)savedState;
				}
				return false;
			}
			set
			{
				this.ViewState["LockFirstItem"] = value;
			}
		}

		#endregion

		#region Render Path

		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>.
		/// </summary>
		protected override void LoadViewState( object savedState )
		{
			Pair combo = savedState as Pair;
			if ( combo == null )
			{
				base.LoadViewState( null );
				return;
			}
			base.LoadViewState( combo.First );
			this.LoadLinkViewState( combo.Second );
		}

		///// <summary>
		///// Overrides <see cref="Control.CreateControlCollection"/> to disable adding controls to this control.
		///// </summary>
		///// <returns>Returns an <see cref="EmptyControlCollection"/>.</returns>
		//protected override System.Web.UI.ControlCollection CreateControlCollection()
		//{
		//    return new EmptyControlCollection( this );
		//}

		/// <summary>
		/// Overrides DataBind to create the link between the <see cref="ParentList"/> and the <see cref="ChildList"/>.
		/// </summary>
		public override void DataBind()
		{
			base.OnDataBinding( EventArgs.Empty );

			this.ValidateProperties();
			this.CreateLink();
		}

		/// <summary>
		/// Creates the link between the <see cref="ParentList"/> and the <see cref="ChildList"/>.
		/// </summary>
		protected virtual void CreateLink()
		{
			this.parentKeys = new Hashtable();
			foreach ( DataRow parentRow in relation.ParentTable.Rows )
			{
				Object parentValue = parentRow[parentListControl.DataValueField];
				NameValueCollection childrenItems = new NameValueCollection();

				foreach ( DataRow childRow in parentRow.GetChildRows( relation ) )
				{
					childrenItems.Add( childRow[this.childListControl.DataValueField].ToString(), childRow[this.childListControl.DataTextField].ToString() );
				}

				this.parentKeys.Add( parentValue.ToString(), childrenItems );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/> to register the client script for the <see cref="ListLink"/>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender( System.EventArgs e )
		{
			base.OnPreRender( e );
			if ( this.Enabled )
			{
				ValidateControlProperties();
				this.RegisterClientScript();
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>.
		/// </summary>
		protected override object SaveViewState()
		{
			Pair combo = new Pair();
			combo.First = base.SaveViewState();
			combo.Second = this.SaveLinkViewState();
			if ( combo.First == null && combo.Second == null )
			{
				return null;
			}
			return combo;
		}

		#endregion

		#region Property Validation

		/// <summary>
		/// Ensures that all public properties are valid.
		/// </summary>
		protected virtual void ValidateProperties()
		{
			this.ValidateControlProperties();
			this.ValidateDataProperties();
		}

		/// <summary>
		/// Ensures that <see cref="ParentList"/> and <see cref="ChildList"/> are valid <see cref="ListControl"/> IDs.
		/// </summary>
		protected virtual void ValidateControlProperties()
		{
			if ( !String.IsNullOrEmpty( this.ParentList ) )
			{
				ListControl parentList = this.NamingContainer.FindControl( this.ParentList ) as ListControl;
				if ( parentList != null )
				{
					this.parentListControl = parentList;
				}
				else
				{
					throw new InvalidOperationException( String.Format( System.Globalization.CultureInfo.CurrentCulture, Resources.ListLink_ListCannotBeFound, this.ParentList ) );
				}
			}
			else
			{
				throw new InvalidOperationException( Resources.ListLink_ParentListSet );
			}

			if ( !String.IsNullOrEmpty( this.ChildList ) )
			{
				ListControl childList = this.NamingContainer.FindControl( this.ChildList ) as ListControl;
				if ( childList != null )
				{
					this.childListControl = childList;
				}
				else
				{
					throw new InvalidOperationException( String.Format( System.Globalization.CultureInfo.CurrentCulture, Resources.ListLink_ListCannotBeFound, this.ChildList ) );
				}
			}
			else
			{
				throw new InvalidOperationException( Resources.ListLink_ChildListSet );
			}
		}

		/// <summary>
		/// Ensures that <see cref="ListLink.DataRelation"/> is a valid <see cref="System.Data.DataRelation"/> DataSource of the participating controls.
		/// </summary>
		protected virtual void ValidateDataProperties()
		{
			if ( parentListControl.DataSource != childListControl.DataSource )
			{
				throw new InvalidOperationException( Resources.ListLink_SameDataSource );
			}

			DataSet dataSource = parentListControl.DataSource as DataSet;
			if ( dataSource == null )
			{
				throw new InvalidOperationException( Resources.ListLink_DataSetOnly );
			}
			if ( dataSource.Relations.Count == 0 )
			{
				throw new InvalidOperationException( Resources.ListLink_NoDataRelation );
			}


			relation = null;
			if ( String.IsNullOrEmpty( this.DataRelation ) )
			{
				relation = dataSource.Relations[0];
			}
			else
			{
				if ( dataSource.Relations.Contains( this.DataRelation ) )
				{
					relation = dataSource.Relations[this.DataRelation];
				}
				else
				{
					throw new InvalidOperationException( Resources.ListLink_NoDataRelation );
				}
			}

		}

		#endregion

		#region Client Script

		/// <summary>
		/// Registers all the client script for the ListLink
		/// </summary>
		protected virtual void RegisterClientScript()
		{
			ClientScriptManager script = this.Page.ClientScript;
			script.RegisterClientScriptResource( typeof( ListLink ), "MetaBuilders.WebControls.Embedded.ListLinkScript.js" );
			this.RegisterRelationScript();
			script.RegisterArrayDeclaration( scriptArrayName, "{ ParentID:'" + this.parentListControl.UniqueID + "', ChildID:'" + this.childListControl.UniqueID + "', LockFirst:'" + this.LockFirstItem.ToString() + "' }" );
			script.RegisterStartupScript( typeof( ListLink ), "Startup", "MetaBuilders_ListLink_Init(); " + String.Format( Resources.AjaxWorkaroundScript, "MetaBuilders_ListLink_Init" ), true );
		}

		/// <summary>
		/// Creates and registers the script which creates the relations between the parent and the child.
		/// </summary>
		protected virtual void RegisterRelationScript()
		{
			System.Text.StringBuilder relationScript = new System.Text.StringBuilder();
			relationScript.Append( @"
if ( typeof(window.ListLinkManager) == ""undefined"" ) {
	window.ListLinkManager = new Object();
}
" );
			foreach ( String parentValue in this.parentKeys.Keys )
			{
				relationScript.Append( @"window.ListLinkManager[""" );
				relationScript.Append( this.parentListControl.UniqueID );
				relationScript.Append( "," );
				relationScript.Append( this.childListControl.UniqueID );
				relationScript.Append( "=" );
				relationScript.Append( parentValue );
				relationScript.Append( "\"] = new Array( " );
				NameValueCollection childrenItems = (NameValueCollection)this.parentKeys[parentValue];
				if ( childrenItems.Count > 0 )
				{
					for ( Int32 i = 0; i < childrenItems.Count - 1; i++ )
					{
						relationScript.Append( "\"" );
						relationScript.Append( childrenItems.Keys[i] );
						relationScript.Append( "\", \"" );
						relationScript.Append( childrenItems[i].Replace( @"\", @"\\" ).Replace( @"""", @"\""" ) );
						relationScript.Append( "\"," );
					}
					relationScript.Append( "\"" );
					relationScript.Append( childrenItems.Keys[childrenItems.Count - 1] );
					relationScript.Append( "\", \"" );
					relationScript.Append( childrenItems[childrenItems.Count - 1].Replace( @"\", @"\\" ).Replace( @"""", @"\""" ) );
					relationScript.Append( "\"" );
				}
				relationScript.Append( ");" );
			}
			this.Page.ClientScript.RegisterClientScriptBlock( typeof( ListLink ), scriptArrayName + this.UniqueID, relationScript.ToString(), true );
		}

		private const String scriptArrayName = "ListLinks";

		#endregion

		#region Private Members

		private ListControl parentListControl;
		private ListControl childListControl;
		private System.Data.DataRelation relation;

		private System.Collections.Hashtable parentKeys;

		#endregion

		#region Link ViewState

		/// <summary>
		/// Loads the ViewState for the link between the two lists.
		/// </summary>
		protected virtual void LoadLinkViewState( object savedState )
		{
			Triplet topLevel = savedState as Triplet;
			if ( topLevel != null )
			{
				if ( topLevel.First is Int32 )
				{
					Int32 itemCount = (Int32)topLevel.First;
					Object[] parentValues = topLevel.Second as Object[];
					Object[] childItems = topLevel.Third as Object[];

					if ( parentValues == null || childItems == null || parentValues.Length != itemCount )
					{
						return;
					}

					this.parentKeys = new Hashtable( itemCount );
					for ( Int32 i = 0; i < parentValues.Length; i++ )
					{
						Object parentValue = parentValues[i];
						Triplet childrenContainer = childItems[i] as Triplet;
						if ( parentValue != null && childrenContainer != null )
						{
							NameValueCollection children = loadChildItemsViewState( childrenContainer );
							this.parentKeys.Add( parentValue, children );
						}
					}
				}
			}
		}

		/// <summary>
		/// Saves the ViewState for the link between the two lists.
		/// </summary>
		protected virtual object SaveLinkViewState()
		{
			if ( this.parentKeys == null )
			{
				return null;
			}

			Int32 itemCount = this.parentKeys.Count;
			if ( itemCount == 0 )
			{
				return null;
			}

			Object[] parentValues = new Object[itemCount];
			Object[] childItems = new Object[itemCount];

			Int32 index = 0;
			foreach ( String parentKey in this.parentKeys.Keys )
			{
				parentValues[index] = parentKey;
				childItems[index] = saveChildItemsViewState( parentKey );
				index++;
			}

			Triplet state = new Triplet();
			state.First = itemCount;
			state.Second = parentValues;
			state.Third = childItems;
			return state;
		}

		/// <summary>
		/// saves the ViewState for the child items in the hierarchy, for the given parent.
		/// </summary>
		private Triplet saveChildItemsViewState( Object parentKey )
		{
			NameValueCollection children = (NameValueCollection)this.parentKeys[parentKey];
			object[] textValues;
			object[] valueValues;

			Int32 itemCount = children.Count;
			textValues = new String[checked( (uint)itemCount )];
			valueValues = new String[checked( (uint)itemCount )];
			for ( Int32 i = 0; i < itemCount; i++ )
			{
				textValues[i] = children[i];
				valueValues[i] = children.Keys[i];
			}
			return new Triplet( itemCount, valueValues, textValues );
		}

		/// <summary>
		/// loads the ViewState for the child items in the hierarchy
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		private NameValueCollection loadChildItemsViewState( Triplet savedState )
		{
			Int32 itemCount = (Int32)savedState.First;
			Object[] valueValues = (Object[])savedState.Second;
			Object[] textValues = (Object[])savedState.Third;

			NameValueCollection result = new NameValueCollection( itemCount );
			for ( Int32 i = 0; i < textValues.Length; i++ )
			{
				result.Add( (String)valueValues[i], (String)textValues[i] );
			}
			return result;
		}

		#endregion

	}
}
