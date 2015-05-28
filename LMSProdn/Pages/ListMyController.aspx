<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ListMyController.aspx.cs" 
Inherits="Pages_ListMyController" Title="Licensing System | My Controllers" %>

<%@ Register Src="../Controls/ListMyController.ascx" TagName="ListMyController" TagPrefix="uc1" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListMyController ID="ListMyController1" runat="server" />
    
</asp:Content>
