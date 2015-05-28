<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="MyControllers.aspx.cs" 
Inherits="Pages_MyControllers" Title="Licensing System | My Controllers" %>

<%@ Register Src="../Controls/MyControllers.ascx" TagName="MyControllers" TagPrefix="uc1" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyControllers ID="MyControllers1" runat="server" />
    
</asp:Content>

