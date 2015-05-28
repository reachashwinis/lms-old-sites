<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivateALELicense.ascx.cs" Inherits="Controls_ActivateALELicense" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border = "0">
<tr><td style="width: 311px; height: 21px">
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Activate ALE certificates"
    Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
    </table>
    
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="733px" DisplaySideBar="False" >
<WizardSteps>
<asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate ID" runat="server" >
<table width="100%">
<tr></tr>
<tr></tr>
<tr>
 <td style="width:25%">
     IP Address</td>
     <td style="color:Red; width:2%"><b> * </b> </td>
 <td style="width:48%">
  <asp:TextBox ID="txtSNum" runat="server" Columns="50" MaxLength="50" CssClass="txt" Width="266px"></asp:TextBox></td>
  <td style="width:25%">
  <asp:RequiredFieldValidator ID="rfvSerialNumber" ValidationGroup="ValidAirwave" runat="server" ControlToValidate="txtSNum" ErrorMessage="IP Address is mandatory" Display="Dynamic" Width="185px"></asp:RequiredFieldValidator>
  </td>
</tr>
<tr>
<td style="width: 25%">
    Certificate ID</td>
   <td style="color:Red; width:2%"><b> * </b> </td>
<td style="width:48%">
    <asp:TextBox ID="TxtEvalKey" runat="server" Width="349px"></asp:TextBox></td>
  <td style="width:25%">
  <asp:RequiredFieldValidator ID="rfvTempKey" runat="server" ValidationGroup="ValidAirwave" ControlToValidate="TxtEvalKey" ErrorMessage="Certificate ID is mandatory" Display="Dynamic"></asp:RequiredFieldValidator>
  </td>
</tr>
 <tr>
<td style="width: 25%">
    Organization</td> <td style="color:Red; width:2%"><b> * </b> </td>
<td style="width:48%">
 <asp:TextBox ID="txtSoId" runat="server" MaxLength="2000" Columns="10" CssClass="txt" Width="350px"></asp:TextBox></td>
  <td style="width: 25%">
  <asp:RequiredFieldValidator ID="rfvSoId" runat="server" ValidationGroup="ValidAirwave" ControlToValidate="txtSoId" ErrorMessage="Organization is mandatory" Display="Dynamic"></asp:RequiredFieldValidator>
  </td> 
 </tr>
<tr>
   <td style="width: 100%" colspan="4">
Do you Acknowledge that you have read and are willing to abide by the 
<asp:LinkButton ID="lnkEULA" runat="server" OnClick="lnkEULA_Click">End-User Software License Agreement?</asp:LinkButton>
</td>
</tr>
<tr><td style="height: 43px; width: 10%">
<asp:RadioButtonList ID="RdBtnLst2" runat="server" Height="34px" Width="74px">
 <asp:ListItem>Yes</asp:ListItem>
 <asp:ListItem>No</asp:ListItem>
 </asp:RadioButtonList>
 </td>
 <td style="width: 75%" colspan="4">
 <asp:RequiredFieldValidator ID="ReqdValidYes2" ValidationGroup="ValidAirwave" runat="server" ErrorMessage="Acceptance of EULA is mandatory"
 Width="460px" ControlToValidate="RdBtnLst2" Display="Dynamic"></asp:RequiredFieldValidator>
 <asp:CompareValidator ID="CvAckyes2" runat="server" ValidationGroup="ValidAirwave" Display="Dynamic" ErrorMessage="Please Acknoledge that You will abide by the rules and Regulations of EULA"
ValueToCompare="Yes" Width="460px" ControlToValidate="RdBtnLst2"></asp:CompareValidator>
</td></tr>
 <tr></tr>
<tr>
<td colspan="1" style="width: 293px">
<asp:Button ID="btnALEActivate" runat="server" CssClass="btn" Text ="Activate" ValidationGroup="ValidAirwave" OnClick="btnALEActivate_Click" />
</td>
<td style="width: 724px">
&nbsp;
</td>
</tr>
</table>
<table>
<tr>
<td style="height: 21px; width: 417px;">
 <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="False" Width="418px"></asp:Label>
</td>
</tr> 
</table>
</asp:WizardStep>
<asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Finish" Title="View Activation Info" runat="server" >
<table>
<tr><td style="color: #0000cc; font-style: italic; height: 60px">Congratulations, you have successfully activated the certificate. 
Following is your Activation key.<br /></td></tr>
</table>
<table><tr>
<td style="width: 615px">
<asp:Literal ID="LiteralAct" runat="server"></asp:Literal>
</td>
</tr>
</table>
<table>
<tr><td style="color: #0000cc; font-style: italic; height: 39px; width: 615px;">Please print this page for your records.<br />
This information has been emailed to you.<br /></td></tr>
</table>
</asp:WizardStep>
</WizardSteps>
<StartNavigationTemplate>
</StartNavigationTemplate>
<StepNavigationTemplate>
</StepNavigationTemplate>
<FinishNavigationTemplate>
</FinishNavigationTemplate>
</asp:Wizard>