<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivateCertificate.ascx.cs"
    Inherits="Controls_ActivateCertificate" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0"
                Text="Activate certificates" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label></td>
    </tr>
</table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0"
    Width="733px" DisplaySideBar="False">
    <WizardSteps>
        <asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate ID"
            runat="server">
            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                <tr>
                    <td colspan="2">
                        Certificate ID:
                    </td>
                </tr>
                <tr>
                    <td style="width: 458px">
                        <asp:TextBox ID="txtCertificate1" runat="server" Width="500px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 55px">
                        <asp:Label ID="lblErr1" runat="server" CssClass="lblError" Width="190px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 458px">
                        <asp:TextBox ID="txtCertificate2" runat="server" Width="500px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 55px">
                        <asp:Label ID="lblErr2" runat="server" CssClass="lblError" Width="190px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 458px">
                        <asp:TextBox ID="txtCertificate3" runat="server" Width="500px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 55px">
                        <asp:Label ID="lblErr3" runat="server" CssClass="lblError" Width="190px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 458px">
                        <asp:TextBox ID="txtCertificate4" runat="server" Width="500px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 55px">
                        <asp:Label ID="lblErr4" runat="server" CssClass="lblError" Width="190px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 458px">
                        <asp:TextBox ID="txtCertificate5" runat="server" Width="500px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 55px">
                        <asp:Label ID="lblErr5" runat="server" CssClass="lblError" Width="190px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 34px; width: 458px;">
                        <asp:TextBox ID="txtCertificate6" runat="server" Width="500px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 55px; height: 34px;">
                        <asp:Label ID="lblErr6" runat="server" CssClass="lblError" Width="190px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 29px">
                        <asp:CustomValidator ID="cvCertBoxes" runat="server" Display="Dynamic" ErrorMessage="Please enter atleast one certficate"
                            ValidationGroup="Step1" OnServerValidate="cvCertBoxes_OnServerValidate" CssClass="lblError"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td colspan="2" style="height: 34px">
                        <asp:Button ID="btnNext" runat="server" ValidationGroup="Step1" OnClick="btnNext_OnClick"
                            Text="Next &gt;&gt;" CssClass="btn" />
                    </td>
                </tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Step" Title="Enter System Info"
            runat="server">
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:Repeater ID="rptCert" runat="server">
                            <ItemTemplate>
                                <table width="100%">
                                    <!--<%# DataBinder.Eval(Container.DataItem, "RowNo")%><br /> -->
                                    <tr>
                                        <td>
                                            Certificate ID:
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "LicSN")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Part Number:
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "LicPartId")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Part Description:
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "LicPartDesc")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "Comments")%>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSNMac" runat="server" CssClass="txt" Columns="50"></asp:TextBox>
                                        </td>
                                        <tr>
                                            <td>
                                                Location:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="txt" Columns="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RequiredFieldValidator ID="rfvSNMac" runat="server" ControlToValidate="txtSNMac"
                                                    Display="Dynamic" ErrorMessage="Serial Number/Mac is mandatory" ValidationGroup="Step2"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr style="color: Red">
                                            <td colspan="2">
                                                <b>
                                                    <%# DataBinder.Eval(Container.DataItem, "SysError")%>
                                                </b>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" background="../images/masterpage/images/dashborder_bg.gif">
                                                <img src="../images/masterpage/images/dashborder_bg.gif" alt="----------" border="0" /></td>
                                        </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td style="width: 192px">
                        <asp:HyperLink ID="HLinkSlNo" runat="server" NavigateUrl="~/Pages/HowToFindSlNo.aspx"
                            Target="_blank" Width="227px">How do I find my serial number?</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Do you Acknowledge that you have read and are willing to abide by the <a href="http://www.arubanetworks.com/pdf/EULA_Aruba.pdf">
                            End-User Software License Agreement? </a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 192px">
                        <asp:RadioButtonList ID="rblyesNo" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="no"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="width: 261px">
                        <asp:RequiredFieldValidator ControlToValidate="rblyesNo" ID="rfvEula" runat="server"
                            ErrorMessage="Acceptance of EULA is mandatory" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ValueToCompare="yes" ControlToValidate="rblyesNo" ErrorMessage="You must acknowledge that you are willing to abide by EULA"
                            Display="Dynamic" ID="cvEula" runat="server"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 192px">
                        <asp:Button ID="btnActivate" runat="server" OnClick="btnActivate_OnClick" Text="Activate"
                            Width="121px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 21px">
                        <asp:Label ID="lblDbError" runat="server" Visible="False" CssClass="lblError" ForeColor="Red"></asp:Label>
                        <asp:CheckBox ID="CheckBox1" Visible="false" Text="Yes" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep3" StepType="Finish" Title="View Activation Info"
            runat="server">
            <table>
                <tr>
                    <td style="color: #0000cc; font-style: italic; height: 60px">
                        Congratulations, you have successfully activated the license(s). Following are your
                        license key(s).These key are required to be applied to the system(s) you generated
                        these license for.<br />
                    </td>
                </tr>
            </table>
            <asp:Repeater ID="rptrActInfo" runat="server">
                <ItemTemplate>
                    <table width="100%">
                        <tr>
                            <td>
                                Certificate ID:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "LicSN")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Part Number:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "LicPartId")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Part Description:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "LicPartDesc")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                System Serial Number/MAC:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "SysSN")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                System Part Number:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "SysPartId")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                System Part Description:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "SysPartDesc")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Location:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Location")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Activation Key:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "ActivationKey")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" background="../images/masterpage/images/dashborder_bg.gif">
                                <img src="../images/masterpage/images/dashborder_bg.gif" alt="----------" border="0" /></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
            <table>
                <tr>
                    <td style="color: #0000cc; font-style: italic; height: 39px">
                        Please print this page for your records.<br />
                        This information has been emailed to you.<br />
                    </td>
                </tr>
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

