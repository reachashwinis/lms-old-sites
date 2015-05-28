<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Controls_Header" %>
 <link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table width="90%" border="0" cellpadding="0" cellspacing="0">
<tr style="width:100%">    
<td align="right">
<asp:label ID="lblLabel" runat="server" Text="Hello" CssClass="lblHeader"></asp:label>&nbsp;<asp:label ID="lblUserName" runat="server" CssClass="lblHeader"></asp:label>&nbsp;
<asp:ImageButton ID="imgLogout" runat="server" OnClick="imgLogout_Click" ImageUrl="~/Images/logout.gif" ImageAlign="Top" ToolTip="Logout" />
</td>
</tr>
</table>
