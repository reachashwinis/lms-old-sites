<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AllCerts.aspx.cs" Inherits="Pages_AllCerts" Title="Licensing System | List all Certificates" %>

<%@ Register Src="../Controls/AllCerts.ascx" TagName="AllCerts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AllCerts ID="AllCerts1" runat="server" />
</asp:Content>

