<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="MyALELicense.aspx.cs" Inherits="Pages_MyALELicense" Title="Licensing System | My ALE Certificate" %>
<%@ Register Src="../Controls/MyALELicense.ascx" TagName="MyALECert"  TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyALECert ID="MyALECert1" runat="server" />
</asp:Content>

