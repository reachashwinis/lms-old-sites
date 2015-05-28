<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="MyCertList.aspx.cs" Inherits="Pages_MyCertList" Title="Licensing System | My Certificates" %>

<%@ Register Src="../Controls/MyCerts.ascx" TagName="MyCerts" TagPrefix="uc1" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyCerts ID="MyCerts1" runat="server" />
    
</asp:Content>

