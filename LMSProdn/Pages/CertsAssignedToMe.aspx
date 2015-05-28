<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="CertsAssignedToMe.aspx.cs" Inherits="Pages_CertsAssignedToMe" Title="Licensing System | List Certficates Assigned to me" %>

<%@ Register Src="../Controls/ListCertificatesAssignedToMe.ascx" TagName="ListCertificatesAssignedToMe"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListCertificatesAssignedToMe ID="ListCertificatesAssignedToMe1" runat="server" />
</asp:Content>

