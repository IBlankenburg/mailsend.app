<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailsPanel.ascx.cs" Inherits="MailSendWeb.UserControls.MailsPanel" %>

<asp:Table ID="Table1" Width="340" runat="server">
    
    <asp:TableRow>
        <asp:TableCell ColumnSpan="3">
            <asp:ListBox ID="lstMails" runat="server" Width="340" SelectionMode="Multiple"></asp:ListBox>
        </asp:TableCell>   
        
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell >
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/page_add.png" />
                      </asp:TableCell>
                      <asp:TableCell HorizontalAlign="Left">R2C2</asp:TableCell>
                        <asp:TableCell>R2C3</asp:TableCell>
                        

                    
                </asp:TableRow>
            </asp:Table>
        </asp:TableCell>
        <asp:TableCell>
        <asp:TableCell runat="server" ColumnSpan="1" HorizontalAlign="Right">R2C4</asp:TableCell>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
