<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FAQ.ascx.cs" Inherits="Controls_FAQ" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border=0><tr><td>
<asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Frequently Asked Questions"
    Width="476px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label>
 </td></tr></table>
     <asp:Repeater ID="RptFAQ" runat="server">
    <ItemTemplate >
<table width="100%">
<tr style="color:Blue">
<td style="font-weight:bold">
<%# DataBinder.Eval(Container.DataItem, "FAQ")%>
</td>
</tr>
<tr>
<td style="font-weight:bold">
<%# DataBinder.Eval(Container.DataItem, "FAA")%>
</td>
</tr>
<tr>
<td>
<%# DataBinder.Eval(Container.DataItem, "ImageURL")%>
</td>
</tr>
</table>
</ItemTemplate>
     </asp:Repeater>
