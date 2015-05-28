<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="ActivateAcct.aspx.cs" Inherits="Accounts_ActivateAcct" Title="Licensing System | Activate Account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<span id="spanActCode" runat="server">
Please enter the code provided in the email in the box below:
<asp:TextBox ID="txtActivationCode" runat="server" CssClass="txt" ValidationGroup="ACTIVATE" Width="235px"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvActCode" runat="server" ControlToValidate="txtActivationCode" ErrorMessage="You must enter the code" ValidationGroup="ACTIVATE"></asp:RequiredFieldValidator><br />
<asp:Button ID="btnActivate" runat="server" ValidationGroup="ACTIVATE" OnClick="btnActivate_OnClick" Text="Activate" />
</span>
<span id="spanActive" runat="server">
Your Licensing Account is active &nbsp; &nbsp;<a href="../Login.aspx">Login here</a>
</span>
<asp:Label ID="lblError" runat="server" CssClass="lblError"></asp:Label>
</asp:Content>

