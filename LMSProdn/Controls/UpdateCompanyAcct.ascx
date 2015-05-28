<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpdateCompanyAcct.ascx.cs" Inherits="Controls_UpdateCompanyAcct" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table>
<tr>
<td colspan="3"><asp:Label ID="lblHead" CssClass="lblHeader" runat="server" Text="Account information" Width="266px" Font-Bold="True" ForeColor="#0000C0" Font-Italic="True" Font-Size="Small"></asp:Label></td>
</tr>
<tr>
</tr>
<tr>
<td style="width: 151px">First Name</td>
<td><asp:TextBox ID="txtFName" runat="Server" MaxLength="100" Columns="75" CssClass="txt"></asp:TextBox></td>
</tr>
<tr>
<td style="width: 151px">Last Name</td>
<td><asp:TextBox ID="txtLName" runat="Server" MaxLength="100" Columns="75" CssClass="txt"></asp:TextBox></td>
</tr>
<tr>
<td style="width: 151px">Email</td>
<td><asp:TextBox ID="txtEmail" runat="Server" MaxLength="255" Columns="75" CssClass="txt"></asp:TextBox></td>
</tr>
<tr>
<td style="width: 151px">Company Name</td>
<td><asp:TextBox ID="txtCompany" runat="Server" MaxLength="75" Columns="75" CssClass="txt" ></asp:TextBox></td>
</tr>
<tr>
<td style="width: 151px">Company</td>
<td>
<asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="false" CssClass="ddlist" Width="370px"></asp:DropDownList>
</td>
<tr><td style="width: 151px">Comments</td>
<td>
<asp:TextBox ID="TxtComments" runat="Server" MaxLength="255" Columns="75" CssClass="txt" TextMode="MultiLine"></asp:TextBox>
</td></tr>
<tr>
<td colspan="3"><asp:Button ID="btnUpdate" CssClass="btn" runat="server" Text="Update Account" ValidationGroup="ACCT" OnClick="btnUpdate_OnClick" />
 </td>
</tr>
<tr>
<td colspan="2" style="height: 21px">
<asp:Label ID="lblError" CssClass="lblError" runat="server" Width="546px"></asp:Label></td></tr>
<tr><td style="width: 151px" colspan="2">
    <asp:Label ID="LblSucc" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Green"
        Width="544px"></asp:Label></td>
</tr>
<asp:Label ID="LblSuccess" runat="server" Font-Bold="True" ForeColor="Green" Width="225px"></asp:Label><input type="hidden" id="hdnAcctId" runat="server" />
</table>
