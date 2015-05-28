<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CancelOPS.ascx.cs" Inherits="Controls_CancelOPS" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table>
<tr>
<td colspan="3">
<asp:Label ID="lbl1" runat="server" CssClass="lblHeader" Text="Remove serial number from OPS Activation" Font-Bold="True" 
ForeColor="#0000C0" Font-Italic="True" Font-Size="Small"></asp:Label>
</td>
</tr>
<tr>
<td colspan="3">&nbsp;</td>
</tr>
<tr>
<td colspan="3" style="color: blue; font-style: italic; height: 40px"><i>
Cancelling a unit means that no one will be able to use it in LMS. We are effectively removing the certificate or 
system from the database. </i></td>
</tr>
<tr><td colspan="3">
    Enter multiple Serial Number seperated by comma(,)
</td></tr>
<tr>
<td style="width:20%; height: 55px;">
    Serial Number
</td>
<td style="width:42%; height: 55px;">
<asp:TextBox ID="txtSNum" runat="server" MaxLength="1400" Columns="40" CssClass="txt" Height="95px" TextMode="MultiLine" Width="439px" ></asp:TextBox>
</td>
<td style="width: 243px; height: 55px">
<asp:RequiredFieldValidator ID="rfvSNum" runat="server" ControlToValidate="txtSNum" ErrorMessage="Serial Number is mandatory" CssClass="lblError" Display="dynamic" ValidationGroup="WHACK" Width="425px"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td style="height: 40px">
<asp:CheckBox ID="chkOverride" runat="server" AutoPostBack="false" Visible="False" Text="Override Errors" />
</td>
<td colspan="2" style="height: 40px">
<asp:Button CssClass="btn" ID="btnCancel" OnClick="btnCancel_OnClick" runat="server" Text="Cancel" />&nbsp;
<asp:Button CssClass="btn" ID="btnConfirm" OnClick="btnConfirm_OnClick" runat="server" Text="Confirm" />
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblSuccess" runat="server" CssClass="lblSuccess" Visible="False" Width="69px"></asp:Label>
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblError" runat="server" CssClass="lblError" Visible="False" Width="70px"></asp:Label>
</td>
</tr>
</table>
<input id="hdnCertId" runat="server" enableviewstate="true" type="hidden" />