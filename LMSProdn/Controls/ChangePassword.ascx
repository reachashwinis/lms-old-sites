<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.ascx.cs" Inherits="Controls_ChangePassword" %>
<%--<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border=0><tr><td>
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Width="325px" Font-Italic="True" Font-Size="Small">Change Password</asp:Label>
</td></tr></table>
<asp:Panel ID="pnlNonAruba" runat="server" DefaultButton="btnChgPwd">
<table>
<tr>
<td>
Current Password
</td>
<td>
<asp:TextBox TextMode="Password" ID="txtCurrPass" runat="server" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td>
    <br />
<asp:RequiredFieldValidator ID="rfvCurrPass" runat="server" ControlToValidate="txtCurrPass" ErrorMessage="Current Password is mandatory" ValidationGroup="CHGPWD" Display="Dynamic" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
New Password
</td>
<td>
<asp:TextBox  runat="server" TextMode="Password" ID="txtNewPass" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td>
<asp:RequiredFieldValidator ID="rfvNewPass" runat="server" ControlToValidate="txtNewPass" ErrorMessage="New Password is mandatory" ValidationGroup="CHGPWD" Display="Dynamic" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>Confirm New Password</td>
<td>
<asp:TextBox  runat="server" TextMode="Password" ID="txtConfPass" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td>
<asp:RequiredFieldValidator ID="rfvConfPass" runat="server" ControlToValidate="txtConfPass" ErrorMessage="Confirm New Password is mandatory" ValidationGroup="CHGPWD" Display="Dynamic" CssClass="lblError"></asp:RequiredFieldValidator>
<asp:CompareValidator ID="cmpPass" runat="server" ControlToValidate="txtConfPass" ControlToCompare="txtNewPass" Type="String" ErrorMessage="Passwords do not match" ValidationGroup="CHGPWD"  Display="dynamic" CssClass="lblError"></asp:CompareValidator>
</td>
</tr>
<tr>
<td colspan="3">
<asp:Button ID="btnChgPwd" runat="server" CssClass="btn" Text="Change Password" OnClick="btnChgPwd_OnClick" ValidationGroup="CHGPWD" />
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblSuccess" runat="server" CssClass="lblSuccess" Visible="false"></asp:Label>
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblError" runat="server" CssClass="lblError" Visible="false"></asp:Label>
</td>
</tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlAruba" runat="server">
You are using your windows credential. Please use Intranet to change your password.</asp:Panel>--%>
