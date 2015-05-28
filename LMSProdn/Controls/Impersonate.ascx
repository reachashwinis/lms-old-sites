<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Impersonate.ascx.cs" Inherits="Controls_Impersonate" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="pnlDefault" runat="server" DefaultButton="btnImpersonate">
<table>
<tr>
<td colspan="2">
<b>
    <asp:Label ID="lblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Impersonate user"
        Width="409px"></asp:Label></b></td>
</tr>
<tr>
<td>
Email:
</td>
<td>
<asp:TextBox ID="txtImpEmail" runat="server" Columns="75" MaxLength="75"  CssClass="txt"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator id="rfvImpEmail" runat="server" ControlToValidate="txtImpEmail" Display="Dynamic" ValidationGroup="Impersonate" ErrorMessage="Impersonate Email is mandatory"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnImpersonate" runat="server" OnClick="btnImpersonate_OnClick" Text="Impersonate" ValidationGroup="Impersonate" CssClass="btn" />
</td>
</tr>
<tr>
<td colspan="2">
<asp:Label ID="lblError" runat="server" CssClass="lblError" ></asp:Label>
</td>
</tr>
</table>
</asp:Panel>