<% @Control Language="C#" %>
<script language="C#" runat="server">

 void btnSubmit_Click(Object Src, EventArgs E)
 {
    txtHW.Text = "Hello World";
 }
</script>
<form runat=server>
<asp:Button id="btnSubmit" text="Click" onclick="btnSubmit_Click" runat="server"/>
<asp:Label id="txtHW" runat="server" />
</form>