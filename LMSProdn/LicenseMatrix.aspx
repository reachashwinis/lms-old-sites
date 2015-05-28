<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="LicenseMatrix.aspx.cs" Inherits="Pages_LicenseMatrix" Title="License System | License Matrix" %>

<%@ Register Src="~/Controls/LicenseMatrix.ascx" TagName="LicenseMatrix" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:LicenseMatrix id="matrix1" runat="server" />
</asp:Content>

