<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master"  AutoEventWireup="true" CodeFile="ListMyRMAedContrl.aspx.cs" 
Inherits="Pages_ListMyRMAedContrl" Title="Licensing System | List RMAed Certificates" %>
<%@ Register Src="../Controls/ListMyRMAedContrl.ascx" TagName="ListMyRMAedContrl"
    TagPrefix="uc1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListMyRMAedContrl ID="ListMyRMAedContrl1" runat="server" />

</asp:Content>

