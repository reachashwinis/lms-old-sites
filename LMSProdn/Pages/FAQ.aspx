<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="FAQ.aspx.cs" Inherits="Pages_FAQ" 
Title="Licensing System | Frequently Asked Questions" %>

<%@ Register Src="../Controls/FAQ.ascx" TagName="FAQ" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:FAQ ID="FAQ1" runat="server" />
</asp:Content>
