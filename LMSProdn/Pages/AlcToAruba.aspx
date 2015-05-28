<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AlcToAruba.aspx.cs" Inherits="Pages_AlcToAruba" %>

<%@ Register Src="../Controls/AlcToAruba.ascx" TagName="AlcToAruba" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AlcToAruba ID="AlcToAruba1" runat="server" />
</asp:Content>
