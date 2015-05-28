<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateSubKey.ascx.cs" Inherits="Controls_GenerateSubKey" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Generate Subscription" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
    </tr>
</table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="969px" DisplaySideBar="False">
    <WizardSteps>
        <asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate Ids" runat="server" >
            <table width ="100%">
                <tr>
                    <td style="width:20%">Certificate ID</td>
                    <td style="color:Red; width:2%"><b> * </b> </td>
                    <td style="width:30%">
                        <asp:TextBox ID="txtCertId" runat="server" Width="349px"></asp:TextBox>
                    </td>
                    <td style="width:48%">
                        <asp:RequiredFieldValidator ID="rfvCertId" runat="server" ErrorMessage="Certificate ID cannot be empty" ControlToValidate="txtCertId" ValidationGroup="validaAmigopod"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="info">(Please enter the certificate ID of a base ClearPass SKU)</td>
                </tr>
                <tr>
                    <td style="width: 257px" colspan="4"> 
                        <asp:Button ID="btnSubmit" CssClass="btn" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="validaAmigopod"/></td>
                </tr>
                <tr>
                    <td style="height: 21px; width: 417px;" colspan="4">
                        <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="False" Width="734px"></asp:Label>
                    </td>
                </tr> 
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Finish" Title="View Activation Info" runat="server" >
            <table>
                <tr><td style="color: #0000cc; font-style: italic; height: 60px">Congratulations, You have successfully generated the Subscription ID. 
                Following is your Subscription ID.<br /></td></tr>
                <tr>
                    <td>
                        <span style="font-family: monospace;">
                        <asp:Literal ID="lblOnBoardMessage" runat="server" />
                        </span> 
                    </td>
                </tr>
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
                    <td style="color: #0000cc; font-style: italic; height: 39px; width: 615px;">Please print this page for your records.<br />
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


