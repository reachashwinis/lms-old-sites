<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CancelSerialNumber.ascx.cs" Inherits="Controls_CancelSerialNumber" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table>
<tr>
<td colspan="3">
<asp:Label ID="lbl1" runat="server" CssClass="lblHeader" Text="Remove serial number/certificate from LMS" Font-Bold="True" ForeColor="#0000C0" Font-Italic="True" Font-Size="Small"></asp:Label>
</td>
</tr>
<tr>
<td colspan="3">&nbsp;</td>
</tr>
<tr>
<td colspan="3">Cancelling a unit means that no one will be able to use it in LMS. We are effectively removing the certificate or system from the database.</td>
</tr>
<tr><td colspan="3">&nbsp;</td></tr>
<tr>
<td style="width:20%">
 Certificate ID/Serial Number
</td>
<td style="width:40%">
<asp:TextBox ID="txtSNum" runat="server" MaxLength="40" Columns="40" CssClass="txt" ></asp:TextBox>
</td>
<td>
<asp:RequiredFieldValidator ID="rfvSNum" runat="server" ControlToValidate="txtSNum" ErrorMessage="Certificate ID/Serial Number is mandatory" CssClass="lblError" Display="dynamic" ValidationGroup="WHACK"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Reason for removal from LMS
</td>
<td>
<asp:TextBox ID="txtReason" runat="server" Rows="4" TextMode="MultiLine" Columns="40" CssClass="txt" ></asp:TextBox>
</td>
<td>
<asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" ErrorMessage="Reason for removal is mandatory" CssClass="lblError" Display="dynamic" ValidationGroup="WHACK"></asp:RequiredFieldValidator>
<asp:CustomValidator ID="cvReason" runat="server" ControlToValidate="txtReason" CssClass="lblError" Display="dynamic" ValidationGroup="WHACK" OnServerValidate="cvReason_OnServerValidate"></asp:CustomValidator>
</td>
</tr>
<tr>
<td>
&nbsp;<asp:CheckBox ID="chkOverride" runat="server" AutoPostBack="false" Text="Override Errors" Visible="False" />
</td>
<td colspan="2">
<asp:Button CssClass="btn" ID="btnCancel" OnClick="btnCancel_OnClick" runat="server" Text="Cancel" />
<asp:Button CssClass="btn" ID="btnConfirm" OnClick="btnConfirm_OnClick" runat="server" Text="Confirm" />
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblSuccess" runat="server" CssClass="lblSuccess" Visible="false"></asp:Label>
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblError" runat="server" CssClass="lblError" Visible="false"></asp:Label>
</td>
</tr>
</table>
<input id="hdnCertId" runat="server" enableviewstate="true" type="hidden" />