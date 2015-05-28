<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="CreateAccount.aspx.cs" Inherits="Pages_CreateAccount" Title="Licensing System | Create Account" %>

<%@ Register Src="../Controls/CreateAccount.ascx" TagName="CreateAccount" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:CreateAccount ID="CreateAccount1" runat="server" />

</asp:Content>

