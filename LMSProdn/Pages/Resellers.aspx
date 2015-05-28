<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="Resellers.aspx.cs" Inherits="Pages_Resellers" Title="Licensing System | Resellers" %>

<%@ Register Src="../Controls/Companies.ascx" TagName="Companies" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:Companies id="Companies1" runat="server">
    </uc1:Companies>
</asp:Content>

