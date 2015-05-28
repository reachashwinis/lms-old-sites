<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ListUnassignedCerts.aspx.cs" Inherits="Pages_ListUnassignedCerts" Title="Licensing System | List Unassigned Certificates" %>

<%@ Register Src="../Controls/ListUnassignedCerts.ascx" TagName="ListUnassignedCerts"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListUnassignedCerts ID="ListUnassignedCerts1" runat="server" />

</asp:Content>

