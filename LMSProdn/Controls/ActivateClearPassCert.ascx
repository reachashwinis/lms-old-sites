<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivateClearPassCert.ascx.cs" Inherits="Controls_ActivateClearPassCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0" width="100%">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0"
                Text="Activate ClearPass certificates" Width="404px" Font-Italic="True" Font-Size="Small"></asp:Label></td>
    </tr>   
</table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0"
    Width="100%" DisplaySideBar="False">
    <WizardSteps>        
        <asp:WizardStep ID="wStep2" runat="server" StepType="Step" Title="SubscriptionDetails">
         <table style="width: 100%">
             <tr><td style="color:#0000cc; height: 40px; width: 640px;"> <i>Following are the details of your Subscription ID. To continue, Please hit <b>Next</b> button or hit <b>Cancel</b> button to cancel the Activation process.</i></td></tr>
 <tr><td style="height: 150px; width: 640px;">
     <asp:Repeater runat="server" ID="rptSub2"><ItemTemplate>
    <table>    
      <tr><td align="left"><b>Subscription ID:</b></td> <td><%# DataBinder.Eval(Container.DataItem, "SubscriptionKey")%></td></tr>
    <tr><td align="left"><b>Organization: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "Organization")%></td></tr>
      <tr><td align="left"><b>Purchase Order:</b></td><td><%# DataBinder.Eval(Container.DataItem, "po_id")%></td></tr>
      <tr><td align="left"><b>Sales Order: </b></td><td><%# DataBinder.Eval(Container.DataItem, "so_id")%></td></tr>
      <tr><td align="left"><b>Expire Time: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "expire_time")%></td></tr>
     <tr><td align="left"><b>User Name: </b></td><td><%# DataBinder.Eval(Container.DataItem, "user_name")%></td></tr>
    <tr><td align="left"><b>Password: </b></td><td><%# DataBinder.Eval(Container.DataItem, "password")%></td></tr>
    <tr><td align="left"><b>Email: </b></td><td><%# DataBinder.Eval(Container.DataItem, "email")%></td></tr>
    <tr><td align="left"><b>Created Time: </b></td><td><%# DataBinder.Eval(Container.DataItem, "create_time")%></td></tr>
    <tr><td style="color:Red" colspan="2"><%# DataBinder.Eval(Container.DataItem, "Error")%></td></tr>  
    </table>                   
</ItemTemplate>
</asp:Repeater> </td></tr>
<tr><td style="height: 26px; width: 640px;">
    <asp:Button ID="BtnImport" runat="server" CssClass="btn" Text="Next" OnClick="BtnImport_Click" />
    <asp:Button ID="BtnCancel" runat="server" CssClass="btn" Text="Cancel" OnClick="BtnCancel_Click" />
    </td></tr>
    <tr><td colspan="2" style="height: 21px">
        <asp:Label ID="LblImportError" runat="server" CssClass="lblError" ForeColor="Red"></asp:Label> </td></tr>
    </table>
        </asp:WizardStep>            
            <asp:WizardStep AllowReturn="False" ID="wStep3" StepType="Step" Title="Enter Certificate ID"
            runat="server">
            <table border="0" cellpadding="5" cellspacing="5" width="100%">
            <tr><td style="color:#0000cc; width: 646px;" colspan="2"> <i>Please enter Certificate ID(s) for Subscription ID : </i>
                <span style="font-family: monospace;">
                <asp:Label ID="LblSubkey3" runat="server" ForeColor="#0000CC" Font-Italic="True" Font-Bold="True"></asp:Label> </span>
      </td></tr>                
                <tr>
                    <td colspan="2">
                        Certificate ID:
                    </td>
                </tr>
                <tr>
                    <td style="width: 350px">
                        <asp:TextBox ID="txtCertificate1" runat="server" Width="350px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width: 100px">
                        <asp:Label ID="lblErr1" runat="server" CssClass="lblError" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:350px">
                        <asp:TextBox ID="txtCertificate2" runat="server" Width="350px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width:100px">
                        <asp:Label ID="lblErr2" runat="server" CssClass="lblError" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:350px">
                        <asp:TextBox ID="txtCertificate3" runat="server" Width="350px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width:100px">
                        <asp:Label ID="lblErr3" runat="server" CssClass="lblError" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:350px">
                        <asp:TextBox ID="txtCertificate4" runat="server" Width="350px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width:100px">
                        <asp:Label ID="lblErr4" runat="server" CssClass="lblError" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:350px">
                        <asp:TextBox ID="txtCertificate5" runat="server" Width="350px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width:50%">
                        <asp:Label ID="lblErr5" runat="server" CssClass="lblError" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:350px">
                        <asp:TextBox ID="txtCertificate6" runat="server" Width="350px" CssClass="txt"></asp:TextBox>
                    </td>
                    <td style="width:100px">
                        <asp:Label ID="lblErr6" runat="server" CssClass="lblError" Width="300px"></asp:Label>
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
        <asp:WizardStep AllowReturn="False" ID="wStep4" StepType="Step" Title="Enter version Info"
            runat="server">
            <table width="100%">
            <tr><td style="color:#0000cc;"> <i>Please enter Certificate(s) details for Subscription ID: </i>
            <span style="font-family: monospace;">
    <asp:Label ID="Lblsubkey4" runat="server" ForeColor="#0000CC" Font-Italic="True" Font-Bold="True"></asp:Label>
    </span>
    </td>
    </tr>
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
                                            Version:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DrpListVersion" runat="server">
                                            <asp:ListItem Text = "6.X.X.X" value = "6.X.X.X" Selected="true"> </asp:ListItem>
                                            <asp:ListItem Text = "5.0.X.X" value = "5.0.X.X" Selected="false"> </asp:ListItem>
                                             </asp:DropDownList>
         
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Location:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="txt" Columns="100"></asp:TextBox>
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
                        <asp:Button ID="btnActivate" runat="server" CssClass="btn" OnClick="btnActivate_OnClick" Text="Activate"
                            Width="121px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 21px">
                        <asp:Label ID="lblDbError" runat="server" Visible="False" CssClass="lblError" ForeColor="Red"></asp:Label>                        
                    </td>
                </tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep5" StepType="Finish" Title="View Activation Info"
            runat="server">
            <table>
                <tr>
                    <td style="color: #0000cc; font-style: italic; height: 60px">
                        Congratulations, you have successfully activated the certificate(s). Following are your
                        license key(s).These keys are required to be applied to the system(s) you generated
                        these license for Subscription ID 
                        <span style="font-family: monospace;">
                        <asp:Label ID="Lblsubkey5" runat="server" ForeColor="#0000CC" Font-Italic="True" Font-Bold="True"></asp:Label> </span> 
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
                                Version:
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Version")%>
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

