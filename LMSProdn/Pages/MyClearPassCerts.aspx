<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="MyClearPassCerts.aspx.cs" Inherits="Pages_MyClearPassCerts" Title="Licensing System | My ClearPass Certificates"%>

<%@ Register Src="../Controls/MyClearPassCerts.ascx" TagName="MyClearPassCerts" TagPrefix="uc1" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyClearPassCerts ID="MyClearPassCerts1" runat="server" />
    
</asp:Content>


