<% @Page Language="C#" %>
<script language="C#" runat="server">
  
void Page_Load (Object Sender, EventArgs E)
{
     if (!IsPostBack) 
     {
       ArrayList arliTables = new ArrayList();

       arliTables.Add("--Tabelle wählen--");
       arliTables.Add("Produkte");
       arliTables.Add("Lieferanten");
       
       drpdTableSel.DataSource = arliTables;
       drpdTableSel.DataBind();
     }
     TableSelChange(null, null);
   
}

void TableSelChange(Object Source, EventArgs E)
{
    if (TableSel.Controls.Count > 0) return;

    string strTableSel;

    strTableSel = drpdTableSel.SelectedItem.Text;
      
    switch(strTableSel)
    {
  
    case "Produkte": TableSel.Controls.Add( Page.LoadControl("ProdChkBox.ascx") );
                     break;
    
    case "Lieferanten": TableSel.Controls.Add( Page.LoadControl("SupplChkBox.ascx") );
                     break;
    }
  
} 
</script>


<html>
<body>

<form runat="server">
<h1>Northwind Datenbank</h2>
Verf&uuml;gbare Tabellen:
 <asp:DropDownList id="drpdTableSel" runat="server"
   AutoPostBack="True"
   OnSelectedIndexChanged="TableSelChange"
 />

 <span id="TableSel" runat="server"/>
</form> 


 
     


</body>
</html>