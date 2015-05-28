<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="AssignClpCertificates.aspx.cs" Inherits="Pages_AssignClpCertificates" Title="License Management System | Assign ClearPass Certificates" %>
<%@ Register Src="../Controls/AssignClpCertificates.ascx" TagName="AssignClpCertificates"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AssignClpCertificates id="AssignClpCertificates1" runat="server" />
</asp:Content>
