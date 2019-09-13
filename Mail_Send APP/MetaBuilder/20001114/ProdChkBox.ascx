<% @Control Language="C#" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script language="C#" runat="server">


void btnSubmit_Click (Object Sender, EventArgs E)
{
     
     char[] chTrim ={','};
     string strSQLCol;
     string strConn ="server=(local)\\NetSDK;database=northwind;Trusted_Connection=yes";
     string strSQL="";

     StringBuilder strColumns = new StringBuilder();
   
     for (int i=0; i < ColumnChkBox.Items.Count; i++)
     {
       if (true == ColumnChkBox.Items[i].Selected)
       {
         strColumns.Append(ColumnChkBox.Items[i].Text);
         strColumns.Append(",");       
         
       }   
     }
   
    strSQLCol = strColumns.ToString();
    strSQLCol = strSQLCol.TrimEnd(chTrim);
    strSQL = "SELECT " + strSQLCol + " FROM Products";

    SqlConnection MyNWConn = new SqlConnection(strConn);

    SqlDataAdapter oCommand = new SqlDataAdapter(strSQL,MyNWConn);
    DataSet MyDataSet = new DataSet();
    
    oCommand.Fill(MyDataSet,"Produkte");

    MyDG.DataSource = MyDataSet.Tables["Produkte"].DefaultView;
    MyDG.DataBind();    

 
}     
</script>

<html>
<body>
<br><br>Folgende Spalten anzeigen:
    <asp:CheckBoxList id="ColumnChkBox" runat=server 
    RepeatDirection="Horizontal">
     <asp:ListItem>ProductName</asp:ListItem>
     <asp:ListItem>UnitPrice</asp:ListItem>
     <asp:ListItem>UnitsInStock</asp:ListItem>
    </asp:CheckBoxList>

<asp:Button id="btnSubmit" text="Abschicken" onclick="btnSubmit_Click" runat="server"/>
<br><br>
<asp:DataGrid id="MyDG" runat="server"
    Width="350"
    BorderColor="black"
    CellPadding="2" 
    CellSpacing="2"
    HeaderStyle-BackColor="#f8d07a"
/>
</body>
</html>