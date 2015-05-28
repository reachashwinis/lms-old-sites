<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="Impersonate.aspx.cs" Inherits="Pages_Impersonate" Title="Licensing System | Impersonate User" %>

<%@ Register Src="../Controls/Impersonate.ascx" TagName="Impersonate" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:Impersonate ID="Impersonate1" runat="server" />

</asp:Content>

