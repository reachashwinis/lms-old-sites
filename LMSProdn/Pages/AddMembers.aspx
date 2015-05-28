<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AddMembers.aspx.cs" Inherits="Pages_AddMembers" Title="Licensing System | Add Members" %>

<%@ Register Src="../Controls/AddMembers.ascx" TagName="AddMembers" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AddMembers ID="AddMembers1" runat="server" />
</asp:Content>

