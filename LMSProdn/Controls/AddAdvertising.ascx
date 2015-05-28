<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddAdvertising.ascx.cs" Inherits="Controls_AddAdvertising" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Add Advertising" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
    </tr>
</table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="883px" DisplaySideBar="False">
    <WizardSteps>
        <asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate ID" runat="server" >
            <table width ="100%">
                <tr>
                    <td style="width:20%">Certificate ID</td>
                    <td style="color:Red; width:2%"><b> * </b> </td>
                    <td style="width:50%">
                        <asp:TextBox ID="txtCertId" runat="server" Width="349px"></asp:TextBox>
                    </td>
                    <td style="width:28%">
                        <asp:RequiredFieldValidator ID="rfvCertId" runat="server" ErrorMessage="Please enter Certificate ID" ControlToValidate="txtCertId" ValidationGroup="Advertising"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="info" colspan="4">(Please enter the certificate ID of an advertising license)</td>
                    
                </tr>
                <tr>
                    <td class="info" colspan="4"></td>
                    
                </tr>
                <tr>
                    <td style="width:20%">Subscription ID</td>
                    <td style="color:Red; width:2%"><b> * </b> </td>
                    <td style="width:50%">
                        <asp:TextBox ID="txtSubscriptionId" runat="server" Width="349px"></asp:TextBox>
                    </td>
                    <td style="width:28%">
                        <asp:RequiredFieldValidator ID="rfvSubKey" runat="server" ErrorMessage="Please enter Subscription ID " ControlToValidate="txtSubscriptionId" ValidationGroup="Advertising"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"> 
                        <asp:Button ID="btnAddAdvertising" CssClass="btn" runat="server" Text="Submit" OnClick="btnAddAdvertising_Click" ValidationGroup="Advertising"/></td>
                </tr>
                <tr>
                    <td style="height: 21px; width: 417px;" colspan="4">
                        <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="False" Width="475px"></asp:Label>
                    </td>
                </tr> 
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Finish" Title="View Activation Info" runat="server" >
            <table>
                <tr><td style="color: #0000cc; font-style: italic; height: 60px">Congratulations, You have successfully added advertising feature to your subscription below. 
                </td></tr>
            </table>
            <table>
                <tr>
                    <td style="width: 660px">
                    <span style="font-family: monospace;">
                        <asp:Literal ID="LiteralAct" runat="server"></asp:Literal>
                     </span>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="color: #0000cc; font-style: italic; height: 39px; width: 663px;"><br />
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