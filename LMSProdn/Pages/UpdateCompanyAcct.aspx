<%@ Page Language="C#" MasterPageFile="~/MasterPages/template.master" AutoEventWireup="true" 
CodeFile="UpdateCompanyAcct.aspx.cs" Inherits="Pages_UpdateCompanyAcct" Title="Licensing System | Update My Company Accounts" %>

<%@ Register Src="../Controls/UpdateCompanyAcct.ascx" TagName="UpdateCompanyAcct" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phBody" Runat="Server">
    <uc1:UpdateCompanyAcct id="UpdateCompanyAcct1" runat="server" />

</asp:Content>
