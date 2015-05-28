<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/LoginTemplate.master" 
CodeFile="Error.aspx.cs" Inherits="Accounts_Error" %>

<%@ Register Src="../Controls/Error.ascx" TagName="Error" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
    <uc1:Error id="Error1" runat="server" />
</asp:Content>
