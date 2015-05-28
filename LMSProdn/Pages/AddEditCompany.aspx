<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AddEditCompany.aspx.cs" Inherits="Pages_AddEditCompany" Title="Licensing System | Add/Edit Company" %>

<%@ Register Src="../Controls/AddEditCompany.ascx" TagName="AddEditCompany" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AddEditCompany ID="AddEditCompany1" runat="server" />
</asp:Content>

