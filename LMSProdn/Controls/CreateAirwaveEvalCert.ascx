<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateAirwaveEvalCert.ascx.cs" Inherits="Controls_CreateAirwaveEvalCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<!--<table border="0"><tr><td>
<asp:Label ID="LblCaption1" runat="server" CssClass="lblHeader" Font-Bold="True" ForeColor="#0000C0" Text="Create evaluation key"
    Width="439px" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr>
    <tr></tr>
    <tr></tr>
    <tr></tr>
    </table>
<asp:Wizard ID="wizAirwave" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="733px" DisplaySideBar="False" >
<WizardSteps>
<asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate Ids" runat="server" >
<table>
<tr>
<td>
Part Number:
</td>
<td style="width: 267px">
<asp:DropDownList ID="ddlEvalPart" runat="server"  CssClass="ddlist" Width="359px"></asp:DropDownList><br />
<asp:RequiredFieldValidator ID="rfvEvalPart" runat="server" ControlToValidate="ddlEvalPart" ErrorMessage="Eval Part is mandatory" Display="Dynamic" ValidationGroup="CreateEval" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Name of Organization :
</td>
<td style="width: 267px">
<asp:TextBox ID="TxtOrg" runat="server" CssClass="txt" Columns="70"></asp:TextBox><br />
<asp:RequiredFieldValidator ID="rfvEvalOrg" runat="server" ControlToValidate="TxtOrg" ErrorMessage="Name of organization is mandatory" Display="Dynamic" ValidationGroup="CreateEval" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Name of recipient :
</td>
<td style="width: 267px">
<asp:TextBox ID="txtSoldTo" runat="server" MaxLength="50" CssClass="txt" Columns="50"></asp:TextBox><br />
<asp:RequiredFieldValidator ID="rfvSoldTo" runat="server" ControlToValidate="txtsoldTo" ErrorMessage="Name of recipient is mandatory" Display="Dynamic" ValidationGroup="CreateEval" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Email address of recipient :
</td>
<td>
<asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="txt" Columns="50"></asp:TextBox><br />
<asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email address is mandatory" Display="Dynamic" ValidationGroup="CreateEval"></asp:RequiredFieldValidator>
</td>
</tr>
<tr></tr>
<tr>
<td colspan="2">
<asp:Button ID="btnCreate" runat="server" ValidationGroup="CreateEval" OnClick="btnCreate_OnClick" Text="  Create Eval Cert  " CssClass="btn" />
</td>
</tr>
<tr>
<td colspan="2">
<asp:CustomValidator ID="cvEmail" runat="server" ErrorMessage="Invalid Email" Display="Dynamic" ValidationGroup="CreateEval" OnServerValidate="cvEmail_OnValidate" CssClass="lblError" Width="74px"></asp:CustomValidator>
<asp:Label ID="lblErr" runat="server" CssClass="lblError"></asp:Label></td></tr>
</table>
</asp:WizardStep>
<asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Start" runat="server" >
    <asp:Literal ID="LiteralCert" runat="server"></asp:Literal>
</asp:WizardStep>
</WizardSteps>
<StartNavigationTemplate>
</StartNavigationTemplate>
<StepNavigationTemplate>
</StepNavigationTemplate>
<FinishNavigationTemplate>
</FinishNavigationTemplate>
</asp:Wizard>
    -->

