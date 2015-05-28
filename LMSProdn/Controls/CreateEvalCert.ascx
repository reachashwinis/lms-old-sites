<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateEvalCert.ascx.cs" Inherits="Controls_CreateEvalCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border=0><tr><td>
<asp:Label ID="LblCaption1" runat="server" CssClass="lblHeader" Font-Bold="True" ForeColor="#0000C0" Text="Create eval certificate"
    Width="439px" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr></table>
<table>
<tr>
<td>
Version
</td>
 <td align="left" style="width: 79px">
<asp:DropDownList ID="ddlCertVersion" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertVersion_SelectedIndexChanged" Width="73px">
<asp:ListItem Value="PRE">Pre 5.0</asp:ListItem>
<asp:ListItem Selected="True" Value="POST">Post 5.0</asp:ListItem>
</asp:DropDownList>
</td>
</tr>
<tr>
<td>
Part Number:
</td>
<td style="width: 267px">
<asp:DropDownList ID="ddlEvalPart" runat="server"  CssClass="ddlist" Width="356px"></asp:DropDownList><br />
<asp:RequiredFieldValidator ID="rfvEvalPart" runat="server" ControlToValidate="ddlEvalPart" ErrorMessage="Eval Part Number is mandatory" Display="dynamic" ValidationGroup="CreateEval" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Name of recipient :
</td>
<td style="width: 267px">
<asp:TextBox ID="txtSoldTo" runat="server" MaxLength="50" CssClass="txt" Columns="50"></asp:TextBox><br />
<asp:RequiredFieldValidator ID="rfvSoldTo" runat="server" ControlToValidate="txtsoldTo" ErrorMessage="Name of recipient is mandatory" Display="dynamic" ValidationGroup="CreateEval" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Email address of recipient :
</td>
<td>
<asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="txt" Columns="50"></asp:TextBox><br />
<asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email address is mandatory" Display="dynamic" ValidationGroup="CreateEval"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnCreate" runat="server" ValidationGroup="CreateEval" OnClick="btnCreate_OnClick" Text="  Create Eval Cert  " CssClass="btn" />
</td>
</tr>
<tr>
<td colspan="2">
<asp:CustomValidator ID="cvEmail" runat="server" ErrorMessage="Invalid Email" Display="Dynamic" ValidationGroup="CreateEval" OnServerValidate="cvEmail_OnValidate" CssClass="lblError" Width="74px"></asp:CustomValidator>
<asp:Label ID="lblErr" runat="server" CssClass="lblError"></asp:Label></td></tr>
<tr>
  <td colspan="2">  <asp:Label ID="LblCaption" runat="server" ForeColor="Green" Visible="False" Width="154px" Font-Bold="True"></asp:Label>
   <asp:Label ID="LblCertId" runat="server" ForeColor="Green" Visible="False" Width="305px" Font-Bold="True"></asp:Label></td>
</tr>
<tr><td colspan = "2"> <asp:Label ID="LblMail" runat="server" ForeColor="Green" Visible="False" Width="444px"></asp:Label></td></tr>
</table>







