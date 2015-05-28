<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="QuickSearch.aspx.cs" Inherits="Pages_QuickSearch" Title="Licensing System | Search" %>

<%@ Register Src="../Controls/QuickSearch.ascx" TagName="QuickSearch" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:QuickSearch ID="QuickSearch1" runat="server" />
</asp:Content>

