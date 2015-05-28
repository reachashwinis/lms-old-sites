<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="UnAssignedUsers.aspx.cs" Inherits="Pages_UnAssignedUsers" Title="Licensing System | List UnAssigned Users" %>

<%@ Register Src="../Controls/UnAssignedUsers.ascx" TagName="UnAssignedUsers" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:UnAssignedUsers ID="UnAssignedUsers1" runat="server" />

</asp:Content>

