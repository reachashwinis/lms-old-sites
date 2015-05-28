<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="Accounts.aspx.cs" Inherits="Pages_Accounts" Title="Licensing System | List Accounts" %>

<%@ Register Src="../Controls/ListAllAccounts.ascx" TagName="ListAllAccounts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListAllAccounts ID="ListAllAccounts1" runat="server" />

</asp:Content>

