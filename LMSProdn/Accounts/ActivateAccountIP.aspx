<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="ActivateAccountIP.aspx.cs" 
Inherits="Accounts_ActivateAccountIP" Title="Licensing System | Activate IP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<span id="spanActCode" runat="server">
    <asp:Label ID="LbluserId" runat="server" Text="Please Enter IP Activation Code" Width="170px"></asp:Label>
    <asp:TextBox ID="TxtUserId" runat="server" Width="290px"></asp:TextBox><br />
<asp:Button ID="btnActivate" runat="server" ValidationGroup="ACTIVATE" OnClick="btnActivate_OnClick" Text="Submit" />
    &nbsp;<asp:Label ID="LblDisplay" runat="server" Text="New IP is Configured to LMS" Visible="False"
        Width="151px"></asp:Label></span><span id="spanActive" runat="server">
&nbsp;<a href="../Login.aspx">Login here</a>
</span>
<asp:Label ID="lblError" runat="server" CssClass="lblError"></asp:Label>
</asp:Content>