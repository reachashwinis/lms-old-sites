<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImportCTOCerts.ascx.cs" Inherits="Controls_ImportCTOCerts" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table width="100%">
<tr><td style="width: 239px">
    <asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Import preloaded certificate(s)"
        Width="239px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
<tr>
<td colspan="2" style="color: #0000cc; font-style: italic; height: 94px">
 This utility allows you to transfer the licenses that were preloaded onto your systems<br /> 
 into this login account.With the systems, you should have received a number of paper certificates. <br/>
 The certificate id is the 35 character string in the middle of the page with the long barcode.<br/>
 Entering one certificate id will import all the rest of the certificates that were used on that sales order.<br/>
</td>
 </tr>
<tr>
<td style="width: 15%; height: 26px;">
    Certificate ID: 
</td>
<td style="width: 85%; "height: 26px">
<asp:TextBox ID="txtCertId" runat="server" MaxLength="75" Columns="50" CssClass="txt" ></asp:TextBox>
<asp:CustomValidator ID="cvCertId" runat="Server" OnServerValidate="cvCertId_OnServerValidate" Display="dynamic" ValidationGroup="ImportCTO" CssClass="lblError" ></asp:CustomValidator></td>
</tr>
<tr><td colspan="2" style="color: #0000cc; font-style: italic">
Please Enter Controller Serial Number 
<asp:CheckBox ID="chkyes" runat="server" AutoPostBack="True" Font-Bold="True" Font-Italic="False" OnCheckedChanged="chkyes_CheckedChanged" Width="94px" />
</td></tr>
<tr>
<td style="width: 15%; height: 26px;">
    Controller Serial Number: 
</td>
<td style="width: 85%; "height: 26px">
<asp:TextBox ID="txtFru" runat="server" MaxLength="75" Columns="50" CssClass="txt" Enabled="False" ></asp:TextBox>
<asp:CustomValidator ID="cvFruId" runat="Server" OnServerValidate="cvFruId_OnServerValidate" Display="dynamic" ValidationGroup="ImportCTO" CssClass="lblError" ></asp:CustomValidator></td>
</tr>
<tr>
<td colspan="2">
    &nbsp;
</td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnImport" runat="server" ValidationGroup="ImportCTO" OnClick="btnImport_OnClick" Text="Import Certificates" CssClass="btn" />
</td>
</tr>
<tr>
<td colspan="2">
<asp:Label ID="lblErr" runat="server" CssClass="lblError"></asp:Label>
</td>
</tr>
</table>




