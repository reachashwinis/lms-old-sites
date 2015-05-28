<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpgradeCert.ascx.cs" Inherits="Controls_UpgradeCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0"><tr><td>
<asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Upgrade certificate"
    Width="489px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr>
   <tr> <td><asp:TextBox ID="TxtCtrl" runat="server" Width="265px"></asp:TextBox> </td></tr>
   <tr><td> 
       <asp:Button ID="BtnSubmit" runat="server" Text="Button" OnClick="BtnSubmit_Click" />
       <asp:Label ID="LblError" runat="server" ForeColor="Red" Width="335px"></asp:Label></td></tr>
    </table>

