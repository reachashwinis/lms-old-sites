<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="Companies.aspx.cs" Inherits="Pages_Companies" Title="License Management System | Manage Customer Companies" %>

<%@ Register Src="../Controls/Companies.ascx" TagName="Companies" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:Companies ID="Companies1" runat="server" />
</asp:Content>

