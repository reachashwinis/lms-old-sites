<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="SubscriptionDetails.aspx.cs" Inherits="Pages_SubscriptionDetails" Title="Licensing System | My Amigopod Subscription" %>
<%@ Register Src="../Controls/SubscriptionDetails.ascx" TagName="Subscription" TagPrefix="uc1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
   <uc1:Subscription runat="server" />
</asp:Content>

