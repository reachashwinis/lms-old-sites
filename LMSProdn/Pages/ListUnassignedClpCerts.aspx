<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListUnassignedClpCerts.aspx.cs" MasterPageFile="~/MasterPages/template.master" Inherits="Pages_ListUnassignedClpCerts"Title="Licensing System | List Unassigned Certificates" %>

<%@ Register Src="../Controls/ListUnassignedClpCerts.ascx" TagName="ListUnassignedClpCerts"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListUnassignedClpCerts ID="ListUnassignedClpCerts1" runat="server" />

</asp:Content>


