<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyAmigopod.aspx.cs" 
MasterPageFile="~/MasterPages/template.master" Inherits="Pages_MyAmigopod" Title="Licensing System | My Amigopod SKUs"%>
<%@ Register Src="../Controls/MyAmigopod.ascx" TagName="MyAmigopod"  TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyAmigopod ID="MyAmigopod1" runat="server" />
</asp:Content>
