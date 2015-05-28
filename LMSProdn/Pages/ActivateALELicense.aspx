<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ActivateALELicense.aspx.cs" Inherits="Pages_ActivateALELicense" Title="Licensing System | Activate ALE Certificate" %>
<%@ Register Src="../Controls/ActivateALELicense.ascx" TagName="ActivateALECert"  TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ActivateALECert ID="ActivateALECert1" runat="server" />
</asp:Content>

