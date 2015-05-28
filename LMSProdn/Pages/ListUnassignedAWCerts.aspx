<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ListUnassignedAWCerts.aspx.cs" Inherits="Pages_ListUnassignedAWCerts" Title="Licensing System | List Unassigned Certificates" %>

<%@ Register Src="../Controls/ListUnassignedAWCerts.ascx" TagName="ListUnassignedAWCerts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListUnassignedAWCerts ID="ListUnassignedAWCerts1" runat="server" />

</asp:Content>


