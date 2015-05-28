<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AllAmigopod.aspx.cs" Inherits="Pages_AllAmigopod" Title="Licensing System | List all Amigopod subscriptions" %>

<%@ Register Src="../Controls/AllAmigopod.ascx" TagName="AllAmigopod" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AllAmigopod ID="AllAmigopod1" runat="server" />
</asp:Content>