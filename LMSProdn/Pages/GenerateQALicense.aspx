<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="GenerateQALicense.aspx.cs" 
Inherits="Pages_GenerateQALicense" Title="Licensing System | Generate QA License" %>

<%@ Register Src="../Controls/GenerateQALicense.ascx" TagName="GenerateQALicense" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:GenerateQALicense ID="GenerateQALicense1" runat="server" />
</asp:Content>
