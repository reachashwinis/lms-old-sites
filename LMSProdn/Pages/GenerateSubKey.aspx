<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="GenerateSubKey.aspx.cs" Inherits="Pages_GenerateSubKey" Title="Licensing System | Generate Subscriptions" %>

<%@ Register Src="../Controls/GenerateSubKey.ascx" TagName="GenerateSubKey" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:GenerateSubKey id="GenerateSubKey1" runat="server" />
</asp:Content>