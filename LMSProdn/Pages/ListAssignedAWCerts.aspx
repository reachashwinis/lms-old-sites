<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ListAssignedAWCerts.aspx.cs" Inherits="Pages_ListAssignedAWCerts" Title="Licensing System | List Assigned AirWave Certificates"%>
<%@ Register Src="../Controls/ListAssignedAWCerts.ascx" TagName="ListAssignedAWCerts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
<uc1:ListAssignedAWCerts ID="ListAssignedAWCerts1" runat="server" />    
</asp:Content>
