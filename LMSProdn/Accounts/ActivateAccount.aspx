<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="ActivateAccount.aspx.cs" Inherits="Accounts_ActivateAccount" Title="Licensing System | Activate Account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<span id="spanActCode" runat="server">
Please enter the code provided in the email in the box below:
<asp:TextBox ID="txtActCode" runat="server" CssClass="txt" ValidationGroup="ACTIVATE"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvActCode" runat="server" ControlToValidate="txtActCode" ErrorMessage="You must enter the code" ValidationGroup="ACTIVATE"></asp:RequiredFieldValidator><br />
<asp:Button ID="btnActivate" runat="server" ValidationGroup="ACTIVATE" OnClick="btnActivate_OnClick" Text="Activate" />
</span>
<span id="spanActive" runat="server">
Your Licensing Account is active &nbsp; &nbsp;<a href="../Login.aspx">Login here</a>
</span>
<asp:Label ID="lblError" runat="server" CssClass="lblError"></asp:Label>
</asp:Content>

