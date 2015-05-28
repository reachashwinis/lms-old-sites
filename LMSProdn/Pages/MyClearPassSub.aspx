<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="MyClearPassSub.aspx.cs" Inherits="Pages_MyClearPassSub" Title="Licensing System | My ClearPass Subscription Keys"%>

<%@ Register Src="../Controls/MyClearPassSub.ascx" TagName="MyClearPassSub" TagPrefix="uc1" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:MyClearPassSub ID="MyClearPassSub1" runat="server" />
    
</asp:Content>



