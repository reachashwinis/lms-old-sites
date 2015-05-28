<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/template.master" CodeFile="MyAirwaveCert.aspx.cs" Inherits="Pages_MyAirwaveCert" 
Title="Licensing System | My Airwave Certificate" %>
<%@ Register Src="../Controls/MyAirwaveCert.ascx" TagName="MyAirwaveCert"  TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyAirwaveCert ID="MyAirwaveCert1" runat="server" />
</asp:Content>
