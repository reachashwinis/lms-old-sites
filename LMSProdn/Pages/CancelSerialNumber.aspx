<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="CancelSerialNumber.aspx.cs" Inherits="Pages_CancelSerialNumber" Title="Licensing System | RMA/Cancel a Serial Number or Certificate" %>

<%@ Register Src="../Controls/CancelSerialNumber.ascx" TagName="CancelSerialNumber"   TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:CancelSerialNumber id="CancelSerialNumber1" runat="server">
    </uc1:CancelSerialNumber>
</asp:Content>

