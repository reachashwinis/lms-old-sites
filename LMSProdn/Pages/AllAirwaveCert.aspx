<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="AllAirwaveCert.aspx.cs" Inherits="Pages_AllAirwaveCert" Title="Licensing System | List all Airwave Certificates" %>

<%@ Register Src="~/Controls/AllAirwaveCert.ascx" TagName="AllAirwaveCerts" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:AllAirwaveCerts ID="allAirwaveCerts1" runat="server" />
</asp:Content>

