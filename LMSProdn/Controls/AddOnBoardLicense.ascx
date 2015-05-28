<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddOnBoardLicense.ascx.cs" Inherits="Controls_AddOnBoardLicense" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Add Onboard License" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
    </tr>
</table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="733px" DisplaySideBar="False">
    <WizardSteps>
        <asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate ID" runat="server" >
            <table width ="100%">
                <tr>
                    <td style="width:20%">Certificate ID</td>
                    <td style="color:Red; width:2%"><b> * </b> </td>
                    <td style="width:30%">
                        <asp:TextBox ID="txtCertId" runat="server" Width="349px"></asp:TextBox>
                    </td>
                    <td style="width:48%">
                        <asp:RequiredFieldValidator ID="rfvCertId" runat="server" ErrorMessage="Please enter Certificate ID" ControlToValidate="txtCertId" ValidationGroup="Upgrade"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td ></td>
                    <td></td>
                    <td colspan="2" class="info">(Please enter the certificate id of Onboard license)</td>
                </tr>
               
                <tr>
                    <td style="width:20%">Subscription ID</td>
                    <td style="color:Red; width:2%"><b> * </b> </td>
                    <td style="width:48%">
                        <asp:TextBox ID="txtSubscriptionId" runat="server" Width="349px"></asp:TextBox>
                    </td>
                    <td style="width:28%">
                        <asp:RequiredFieldValidator ID="rfvSubKey" runat="server" ErrorMessage="Please enter Subscription ID " ControlToValidate="txtSubscriptionId" ValidationGroup="Upgrade"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 257px" colspan="4"> 
                        <asp:Button ID="btnUpgrade" CssClass="btn" runat="server" Text="Submit" OnClick="btnUpgrade_Click" ValidationGroup="Upgrade"/></td>                    
                </tr>
                <tr>
                    <td style="height: 21px; width: 417px;" colspan="4">
                        <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="False" Width="418px"></asp:Label>
                    </td>
                </tr> 
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Finish" Title="View Activation Info" runat="server" >
            <table>
                <tr><td style="color: #0000cc; font-style: italic; height: 60px">Congratulations, You have successfully added On board license to your subscription below. 
                </td></tr>
            </table>
            <table>
                <tr>
                    <td style="width: 615px">
                    <span style="font-family: monospace;">
                        <asp:Literal ID="LiteralAct" runat="server"></asp:Literal>
                    </span> 
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="color: #0000cc; font-style: italic; height: 39px; width: 615px;"><br />
                    This information has been emailed to you.<br />
                    </td>
                </tr>
            </table>
        </asp:WizardStep>
    </WizardSteps>
<StartNavigationTemplate></StartNavigationTemplate>
<StepNavigationTemplate></StepNavigationTemplate>
<FinishNavigationTemplate></FinishNavigationTemplate>
</asp:Wizard>
