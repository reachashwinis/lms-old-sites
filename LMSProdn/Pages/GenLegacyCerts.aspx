<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="GenLegacyCerts.aspx.cs" Inherits="Pages_GenLegacyCerts"%>

<%@ Register Src="../Controls/GenLegacyCerts.ascx" TagName="GenLegacyCerts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:GenLegacyCerts ID="GenLegacyCerts1" runat="server" />
</asp:Content>
