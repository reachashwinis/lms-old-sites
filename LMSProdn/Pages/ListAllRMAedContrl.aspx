<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master"  AutoEventWireup="true" CodeFile="ListAllRMAedContrl.aspx.cs" 
Inherits="Pages_ListAllRMAedContrl" Title="Licensing System | List RMAed Certificates" %>
<%@ Register Src="../Controls/ListAllRMAedContrl.ascx" TagName="ListAllRMAedContrl"
    TagPrefix="uc1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListAllRMAedContrl ID="ListAllRMAedContrl1" runat="server" />

</asp:Content>

