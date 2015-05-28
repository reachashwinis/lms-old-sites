<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BulkActivateCert.ascx.cs" Inherits="Controls_BulkActivateCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table style="width: 100%" border="0">
<tr>
<td style="width: 695px">
</td>
</tr>
<tr>
<td style="width: 695px">
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" 
Text="Bulk Activation of certificates" Width="272px"></asp:Label>
</td>
</tr>
<tr>
<td style="width: 695px">
</td>
</tr>
<tr>
<td style="width: 695px">
</td>
</tr>
<tr>
<td style="width: 695px">
    <asp:Label ID="LblURL" Font-Bold="True" runat="server" Text="Label"></asp:Label>
</td>
</tr>
<tr>
<td style="width: 695px"> 
 <input id="FileUpload" type="file" style="width: 594px" runat="server" accept="text/html" /> </td>
<td><asp:RequiredFieldValidator ID="ReqdFileValidate" runat="server" ErrorMessage="Please Enter the File name" ValidationGroup="UploadFile" Width="192px" ControlToValidate="FileUpload"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td style="width: 695px">    
    <asp:Label ID="LblError" runat="server" ForeColor="Red" Width="706px" Font-Bold="True"></asp:Label></td>
</tr>
<tr>
<td style="width: 695px">
    <asp:Button ID="BtnUpload" runat="server" Text="Submit" OnClick="BtnUpload_Click" ValidationGroup="UploadFile" />
</td>
</tr>
<tr>
<td style="width: 714px; height: 23px" colspan="2">
    <asp:Label ID="LblInvalidCert" runat="server" ForeColor="Red" Width="964px"></asp:Label></td>
</tr>
<tr>
<td style="width: 714px; height: 26px" colspan="2">
    <asp:Label ID="LblInvalidFru" runat="server" ForeColor="Red" Width="964px"></asp:Label></td>
</tr>
</table>