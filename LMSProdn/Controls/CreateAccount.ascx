<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateAccount.ascx.cs" Inherits="Controls_CreateAccount" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<%--<table>
<tr>
<td colspan="3"><asp:Label ID="lblHead" CssClass="lblHeader" runat="server" Text="Account information" Width="266px" Font-Bold="True" ForeColor="#0000C0" Font-Italic="True" Font-Size="Small"></asp:Label></td>
</tr>
<tr>
</tr>
<tr>
<td>First Name</td>
<td><asp:TextBox ID="txtFName" runat="Server" MaxLength="100" Columns="75" CssClass="txt" ></asp:TextBox></td>
<td style="width:200px"><asp:RequiredFieldValidator ID="rfvFname" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name is mandatory" CssClass="lblError" ValidationGroup="ACCT"  Display="dynamic"></asp:RequiredFieldValidator> </td>
</tr>
<tr>
<td>Last Name</td>
<td><asp:TextBox ID="txtLName" runat="Server" MaxLength="100" Columns="75" CssClass="txt" ></asp:TextBox></td>
<td style="width:200px"><asp:RequiredFieldValidator ID="rfvLname" runat="server" ControlToValidate="txtLName" ErrorMessage="Last Name is mandatory" CssClass="lblError" ValidationGroup="ACCT"  Display="dynamic"></asp:RequiredFieldValidator> </td>
</tr>
<tr>
<td>Email</td>
<td><asp:TextBox ID="txtEmail" runat="Server" MaxLength="255" Columns="75" CssClass="txt" ></asp:TextBox></td>
<td style="width:200px"><asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is mandatory" CssClass="lblError" ValidationGroup="ACCT"  Display="dynamic"></asp:RequiredFieldValidator> </td>
</tr>
<tr>
<td>Company Name</td>
<td><asp:TextBox ID="txtCompany" runat="Server" MaxLength="75" Columns="75" CssClass="txt" ></asp:TextBox></td>
<td style="width:200px"><asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="txtFName" ErrorMessage="Company Name is mandatory" ValidationGroup="ACCT"  CssClass="lblError" Display="dynamic"></asp:RequiredFieldValidator> </td>
</tr>
<tr>
<td>Phone</td>
<td><asp:TextBox ID="txtPhone" runat="Server" MaxLength="25" Columns="25" CssClass="txt" ></asp:TextBox></td>
<td style="width:200px">&nbsp; </td>
</tr>
<asp:Panel ID="pnlArubaCa" runat="server">
<tr>
<td>
Account Type
</td>
<td>
<asp:DropDownList ID="ddlCustType" runat="server" CssClass="ddlist" AutoPostBack="false" 
OnSelectedIndexChanged="ddlCustType_SelectedIndexChanged"></asp:DropDownList>
</td>
<td>
&nbsp;
</td>
</tr>
<tr>
<td>
Is Company Admin
</td>
<td>
<asp:CheckBox ID="ChkIsAdmin" runat="server"/>
</td>
</tr>
<tr>
<td>
Status
</td>
<td>
<asp:DropDownList ID="ddlStatus" runat="server" CssClass="ddlist" AutoPostBack="false">
<asp:ListItem Text="Active" Value="Active"></asp:ListItem>
<asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
</asp:DropDownList>
</td>
<td>&nbsp;</td>
</tr>
<tr>
<td>Company</td>
<td colspan="2">
<asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>
</td>
</tr>
<tr><td>Comments</td>
<td>
<asp:TextBox ID="TxtComments" runat="Server" MaxLength="255" Columns="75" CssClass="txt" TextMode="MultiLine"></asp:TextBox>
</td></tr>
</asp:Panel>
<tr>
<td colspan="2"><asp:Button ID="btnUpdate" CssClass="btn" runat="server" Text="Update Account" ValidationGroup="ACCT" OnClick="btnUpdate_OnClick" />
<asp:Button ID="btnCreate" CssClass="btn" runat="server" Text="Add Account" ValidationGroup="ACCT" OnClick="btnAdd_OnClick" /> 
 </td>
</tr>
<tr>
<td colspan="2" style="height: 21px">
<asp:Label ID="lblError" CssClass="lblError" runat="server"></asp:Label></td></tr>
<tr><td>
    <asp:Label ID="LblSucc" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Green"
        Width="154px"></asp:Label></td>
</tr>
</table>

<br />

<asp:Panel id="pnlRstPwd" runat="server" Width="332px">
<table>
<tr>
<td colspan="2">
<asp:Label ID="lblHead2" runat="server" CssClass="lblHeader" Text="Reset Password"></asp:Label>
</td>
</tr>
<tr>
<td>
New Password
</td>
<td style="width: 211px">
<asp:TextBox ID="txtNewPass" runat="server" CssClass="txt" MaxLength="25" Columns="25" Width="199px"></asp:TextBox>
</td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnResetPwd" runat="server" Text="Reset Password" OnClick="btnResetPwd_OnClick" CssClass="btn" />
</td>
</tr>
<tr>
<td colspan="2">
<asp:Label ID="lblResetErr" runat="server" CssClass="lblError" Width="306px"></asp:Label>
</td>
</tr>
</table>
    <asp:Label ID="LblSuccess" runat="server" Font-Bold="True" ForeColor="Green" Width="306px"></asp:Label></asp:Panel>
&nbsp;

<input type="hidden" id="hdnAcctId" runat="server" />--%>