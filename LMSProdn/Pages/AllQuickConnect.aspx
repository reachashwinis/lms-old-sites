<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master"  AutoEventWireup="true" CodeFile="AllQuickConnect.aspx.cs" Inherits="Pages_AllQuickConnect" Title="Licensing System | List all QuickConnect Certificates"%>

<%@ Register Src="../Controls/AllQuickConnect.ascx" TagName="AllQuickConnect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AllQuickConnect ID="AllQuickConnect1" runat="server" />
</asp:Content>