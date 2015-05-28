<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="AssignAWCertificates.aspx.cs" Inherits="Pages_AssignAWCertificates" Title="License Management System | Assign AirWave Certificates" %>
<%@ Register Src="../Controls/AssignAWCertificates.ascx" TagName="AssignAWCertificates"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AssignAWCertificates id="AssignAWCertificates1" runat="server" />
</asp:Content>
