<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ListAssignedCertificates.aspx.cs" Inherits="Pages_ListAssignedCertificates" Title="Untitled Page" %>

<%@ Register Src="../Controls/ListAssignedCerts.ascx" TagName="ListAssignedCerts"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListAssignedCerts ID="ListAssignedCerts1" runat="server" />
</asp:Content>

