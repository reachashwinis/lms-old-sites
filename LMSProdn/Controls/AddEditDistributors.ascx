<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditDistributors.ascx.cs" Inherits="Controls_AddEditDistributors" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table  width="100%">
<tr>
<td style="width:30%">
Company Name:
</td>
<td>
<asp:TextBox ID="txtCompany" runat="server" MaxLength="60" Columns="50"  CssClass="txt"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfvCompany" runat="server" CssClass="lblError" ControlToValidate="txtCompany"  Display="dynamic" ErrorMessage="Company Name is mandatory" ValidationGroup="Company"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td colspan="2">
Distributor IDs:
</td>
</tr>
<tr>
<td colspan="2">
<div style="height:300px;overflow:auto">
<asp:CheckBoxList ID="cblDistIds" runat="server" AutoPostBack="false" RepeatDirection="horizontal"  RepeatColumns="3" CssClass="ddlist" ></asp:CheckBoxList>
</div>

</td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnAdd" runat="server"  OnClick="btnAdd_OnClick" Text="Add Company" ValidationGroup="Company" CssClass="btn"/>
<asp:Button ID="btnEdit" runat="server"  OnClick="btnEdit_OnClick" Text="Edit Company" ValidationGroup="Company"  CssClass="btn"/>
</td>
</tr>
<tr>
<td colspan="2">
<asp:CustomValidator ID="cvCompany" runat="server" Display="Dynamic" ValidationGroup="Company" OnServerValidate="cvCompany_OnServerValidate" CssClass="lblError"></asp:CustomValidator>
<asp:Label ID="lblErr" runat="server" CssClass="lblError"></asp:Label>
</td>
</tr>
</table>
<input type="hidden" runat="server" id="hdnOldName" />