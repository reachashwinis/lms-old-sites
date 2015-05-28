<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" CodeFile="ReassignAirwaveCert.aspx.cs" Inherits="Pages_ReassignAirwaveCert" Title="Licensing System | Re-Assign Airwave Certificates" %>
<%@ Register Src="~/Controls/ReassignAirwaveCert.ascx" TagName="ReAssign" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:ReAssign ID="reAssign1" runat="server" />
</asp:Content>

