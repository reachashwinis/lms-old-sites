<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ListMyEvalCerts.aspx.cs" Inherits="Pages_ListMyEvalCerts" Title="Licensing System | My Eval Certificates" %>

<%@ Register Src="../Controls/ListMyEvalCerts.ascx" TagName="ListMyEvalCerts" TagPrefix="uc1" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ListMyEvalCerts ID="ListMyEvalCerts1" runat="server" />
    
</asp:Content>

