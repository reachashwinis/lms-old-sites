<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="Accounts_ForgotPassword" Title="Licensing System | Forgot Password" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<table width="100%">
<tr>
<td>
Email Address:
</td>
<td>
<asp:TextBox ID="txtEmail" runat="server" Columns="50" ></asp:TextBox>
</td>
<td style="width:200px">
<asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter your email address" Display="dynamic" ValidationGroup="ForgotPwd"></asp:RequiredFieldValidator>
</td>
</tr>
<tr><td>
<asp:RegularExpressionValidator ID="revEmail" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please enter a valid email address" ControlToValidate="txtEmail" Enabled="True"
ValidationExpression="^([a-zA-Z0-9])+([a-zA-Z0-9\.+_-])*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$" Width="277px" ></asp:RegularExpressionValidator></td>
</tr>
<tr>
<td colspan="3">
<asp:Button ID="btnForgotPwd" runat="server" Text="Send me my Password" ValidationGroup="ForgotPwd" OnClick="btnForgotPwd_OnClick" />
</td>
</tr>
<tr>
<td colspan="3" style="height: 18px">
<asp:Label ID="lblError" runat="server" CssClass="lblError" Width="124px" Visible="False"></asp:Label>
    <asp:Label ID="LblSuccess" runat="server" Font-Bold="True" ForeColor="Green" Width="149px" Visible="False"></asp:Label></td>
</tr>
</table>
</asp:Content>--%>

