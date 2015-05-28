<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master"  AutoEventWireup="true" CodeFile="UpdateMyEmailId.aspx.cs" Inherits="UpdateMyEmailId" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<table>
<tr>
<td style="height: 39px" colspan="2">
<B>Your email Id is found in our Restricted domain list.Please Change it to your Corporate
    Mail Id.Note that New Email Id will be used for future transactions.</B>
</td></tr>
<tr>
<td style="width: 226px">
Email:
</td>
<td style="width: 573px">
<asp:TextBox ID="txtEmail" runat="server" Columns="75" MaxLength="75"  CssClass="txt" Width="368px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator id="rfvEmail" 
runat="server" ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="UpdateGroup" ErrorMessage="Email is mandatory"></asp:RequiredFieldValidator>
</td>
</tr>
<tr><td style="width: 226px" colspan="2">
    <asp:RegularExpressionValidator ID="RegExpEmail" runat="server" ControlToValidate="txtEmail"
        ErrorMessage="Email Id is not in proper format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        ValidationGroup="UpdateGroup" Width="257px"></asp:RegularExpressionValidator></td></tr>
<tr>
<td colspan="2">
<asp:Button ID="btnEnter" runat="server" OnClick="btnEnter_OnClick" Text="Submit" ValidationGroup="UpdateGroup" CssClass="btn" />
<asp:Label ID="LblSuccess" runat="server"></asp:Label></td>
</tr>
<tr>
<td colspan="2">
<asp:Label ID="lblError" runat="server" CssClass="lblError" ></asp:Label>
</td>
</tr>
</table>
</asp:Content>


