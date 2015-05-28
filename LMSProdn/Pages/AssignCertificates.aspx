<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AssignCertificates.aspx.cs" Inherits="Pages_AssignCertificates" Title="License Management System | Assign Certificates" %>

<%@ Register Src="../Controls/AssignCertificates.ascx" TagName="AssignCertificates"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AssignCertificates ID="AssignCertificates1" runat="server" />
</asp:Content>

