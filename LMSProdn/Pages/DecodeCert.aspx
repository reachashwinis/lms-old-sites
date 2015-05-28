<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="DecodeCert.aspx.cs" 
Inherits="Pages_DecodeCert" %>
<%@ Register Src="../Controls/DecodeCert.ascx" TagName="DecodeCert" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:DecodeCert ID="DecodeCert1" runat="server" />
</asp:Content>

