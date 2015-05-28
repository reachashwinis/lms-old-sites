<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddMissingSN.ascx.cs" Inherits="Controls_AddMissingSN" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="pnlAddMissSn" runat="server" DefaultButton="btnAddMissingSn">
<table width="100%">
<tr>
<td colspan="3">
 <asp:Label ID="lbl1" runat="server" CssClass="lblHeader" Text="Add missing serial number" Font-Bold="True" ForeColor="MediumBlue" Font-Italic="True" Font-Size="Small"></asp:Label>
</td>
</tr>
<tr>
 <td style="width:15%">
 Serial Number</td>
 <td style="width:40%">
  <asp:TextBox ID="txtSNum" runat="server" Columns="50" MaxLength="50" CssClass="txt"></asp:TextBox></td>
  <td style="width:45%">
  <asp:RequiredFieldValidator ID="rfvSerialNumber" runat="server" ControlToValidate="txtSNum" ErrorMessage="Serial Number is mandatory" Display="dynamic" ValidationGroup="AddSN"></asp:RequiredFieldValidator>
  <asp:CustomValidator ID="cvSerialNumber" runat="server" ControlToValidate="txtSNum" Display="dynamic" ValidationGroup="AddSN" OnServerValidate="cvSerialNumber_OnServerValidate"></asp:CustomValidator>
  </td>
</tr>
<tr>
<td>Part Number</td>
<td>
 <asp:DropDownList ID="ddlPartId" runat="server" AutoPostBack="false" CssClass="ddlist" ></asp:DropDownList>
</td>
  <td>
  <asp:RequiredFieldValidator ID="rfvSysPartId" runat="server" ControlToValidate="ddlPartId" ErrorMessage="Part Number is mandatory" Display="dynamic" ValidationGroup="AddSN"></asp:RequiredFieldValidator>
  </td>
</tr>
 <tr>
<td>Sold to Customer Code</td>
<td>
 <asp:TextBox ID="txtCustCode" runat="server" MaxLength="10" Columns="10" CssClass="txt"></asp:TextBox></td>
  <td>
  <asp:RequiredFieldValidator ID="rfvCustCode" runat="server" ControlToValidate="txtCustCode" ErrorMessage="Customer Code is mandatory" Display="dynamic" ValidationGroup="AddSN"></asp:RequiredFieldValidator>
  </td> 
 </tr>
<tr>
<td colspan="2">
<asp:Button ID="btnAddMissingSn" runat="server" CssClass="btn" Text ="Add Serial Number" ValidationGroup="AddSN" OnClick="btnAddMissingSn_OnClick" />
</td>
<td>
&nbsp;<asp:CheckBox ID="chkOverride" runat="server" AutoPostBack="false" Text="Override Errors" Visible="False" Width="142px" />
</td>
</tr>
<tr>
<td colspan="3">
 <asp:Label ID="lblSuccess" runat="server" CssClass="lblSuccess" Visible="false"></asp:Label>
</td>
</tr> 
<tr>
<td colspan="3">
 <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="false"></asp:Label>
</td>
</tr> 
</table>

</asp:Panel>