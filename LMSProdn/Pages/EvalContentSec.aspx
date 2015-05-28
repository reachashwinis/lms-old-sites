<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="EvalContentSec.aspx.cs" 
Inherits="Pages_EvalContentSec" Title="Licensing System | Content Security Subscription (Evaluation)" %>

<%@ Register Src="../Controls/EvalContentSec.ascx" TagName="EvalContentSec" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:EvalContentSec ID="EvalContentSec1" runat="server" />
</asp:Content>

