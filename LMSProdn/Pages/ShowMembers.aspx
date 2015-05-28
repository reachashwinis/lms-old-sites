<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ShowMembers.aspx.cs" Inherits="Pages_ShowMembers" Title="Licensing System | List Members" %>

<%@ Register Src="../Controls/ShowMembers.ascx" TagName="ShowMembers" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ShowMembers ID="ShowMembers1" runat="server" />
</asp:Content>

