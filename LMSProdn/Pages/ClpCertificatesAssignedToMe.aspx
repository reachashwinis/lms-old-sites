<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ClpCertificatesAssignedToMe.aspx.cs" Inherits="Pages_ClpCertificatesAssignedToMe" Title="Licensing System | List ClearPass Certficates Assigned to me"%>

<%@ Register Src="../Controls/ListClpCertificatesAssignedToMe.ascx" TagName="ListClpCertificatesAssignedToMe"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListClpCertificatesAssignedToMe ID="ListClpCertificatesAssignedToMe1" runat="server" />
</asp:Content>