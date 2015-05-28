<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="CompanyAccounts.aspx.cs" Inherits="Pages_CompanyAccounts" Title="Licensing System | List Accounts" %>

<%@ Register Src="../Controls/CompanyAccounts.ascx" TagName="CompanyAccounts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:CompanyAccounts ID="CompanyAccounts1" runat="server" />

</asp:Content>