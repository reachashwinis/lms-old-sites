<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AddMissingSN.aspx.cs" Inherits="Pages_AddMissingSN" Title="Licensing System | Add missing Serial number" %>

<%@ Register Src="../Controls/AddMissingSN.ascx" TagName="AddMissingSN" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AddMissingSN ID="AddMissingSN1" runat="server" />
</asp:Content>

