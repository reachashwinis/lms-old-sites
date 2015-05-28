<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="UpgradeCert.aspx.cs" Inherits="Pages_UpgradeCert" Title="Licensing System | Upgrade Certificates" %>

<%@ Register Src="../Controls/UpgradeCert.ascx" TagName="UpgradeCert" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:UpgradeCert ID="UpgradeCert1" runat="server" />
</asp:Content>

