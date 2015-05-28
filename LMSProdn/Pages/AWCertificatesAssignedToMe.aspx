<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AWCertificatesAssignedToMe.aspx.cs" Inherits="Pages_AWCertificatesAssignedToMe" Title="Licensing System | List AirWave Certficates Assigned to me" %>

<%@ Register Src="../Controls/ListAWCertificatesAssignedToMe.ascx" TagName="ListAWCertificatesAssignedToMe"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListAWCertificatesAssignedToMe ID="ListAWCertificatesAssignedToMe1" runat="server" />
</asp:Content>
