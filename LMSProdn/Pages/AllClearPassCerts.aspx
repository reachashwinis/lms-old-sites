<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AllClearPassCerts.aspx.cs" Inherits="Pages_AllClearPassCerts" Title="Licensing System | List all ClearPass Certificates" %>

<%@ Register Src="../Controls/AllClearPassCerts.ascx" TagName="AllClearPassCerts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AllClearPassCerts ID="AllClearPassCerts1" runat="server" />
</asp:Content>
