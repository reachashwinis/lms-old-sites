<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AddEditDistributor.aspx.cs" Inherits="Pages_AddEditDistributor" Title="Licensing System | Add/Edit Distributor" %>

<%@ Register Src="../Controls/AddEditDistributors.ascx" TagName="AddEditDistributors"
    TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AddEditDistributors ID="AddEditDistributors1" runat="server" />
    
</asp:Content>

