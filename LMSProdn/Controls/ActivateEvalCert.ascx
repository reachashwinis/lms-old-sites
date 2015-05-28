<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivateEvalCert.ascx.cs" Inherits="Controls_ActivateEvalCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0"><tr><td>
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Activate an eval certificate"
    Width="365px" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
    </table>
    <!-- comment -->
<asp:Wizard ID="WizActivateEval" runat="server" Height="99px"
    Width="714px" ActiveStepIndex="0" DisplaySideBar="False">
    <StepStyle Wrap="True" />
    <StartNavigationTemplate>
        
    </StartNavigationTemplate>
    <WizardSteps>
        <asp:WizardStep ID="CertStp1" runat="server" AllowReturn="False" StepType="Start"
            Title="Enter Certificate ID">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 99%">
            <tr><td style="width: 438px">
            <asp:Label ID="LblCertId1" runat="server" Text="Certificate ID:" Width="95px"></asp:Label></td></tr>
            <tr><td style="width: 438px"><asp:TextBox ID="TxtCertId1" ValidationGroup="Step1" runat="server" Width="448px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ReqdValid1" runat="server" 
                ErrorMessage="Please Enter Certificate ID" ControlToValidate="TxtCertId1" ValidationGroup="Step1"></asp:RequiredFieldValidator>
            </td></tr>
            <tr><td style="width: 438px"><asp:Button ID="Btnnext1" runat="server" Text="Next &gt;&gt;" OnClick="Btnnext1_Click" /></td></tr>
            <tr><td style="width: 438px; height: 29px;">
            <asp:Label ID="LblError1" runat="server" ForeColor="Red" Visible="False" Width="448px" Font-Bold="True"></asp:Label>
            </td></tr>
            <tr><td style="width: 438px; height: 28px;"></td></tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep ID="CertStp2" runat="server" AllowReturn="False" StepType="Step"
            Title="Enter Serial Number">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 100%">
            <tr>
            <td  colspan="2"style="height: 29px; width: 495px;">
                &nbsp;<asp:FormView ID="FrmVwSNo2" runat="server" Width="533px">
                    <ItemTemplate>
                        <table>
                                 <tr><td align="left"><b>Certificate ID:</b></td><td><%# DataBinder.Eval(Container.DataItem, "LicSN")%></td></tr>
                                 <tr><td align="left"><b>Part Number:</b></td><td><%# DataBinder.Eval(Container.DataItem, "LicPartId")%></td></tr>
                                 <tr><td align="left"><b>Part Description: </b></td><td><%# DataBinder.Eval(Container.DataItem, "LicPartDesc")%></td></tr>
                                 <tr><td align="left"><b><%# DataBinder.Eval(Container.DataItem, "Comments")%></b></td>
                                 <td><asp:TextBox ID="TxtSnNo2" runat="server"></asp:TextBox></td></tr>
                                 <tr><td></td></tr>
                <tr><td colspan="2">             
                    <asp:RequiredFieldValidator ID="ReqdValidSl2" runat="server" 
                    ErrorMessage="Serial Number/Mac address is mandatory"
                   Width="300px" ControlToValidate="TxtSnNo2" ValidationGroup="ReqdValidSl2"></asp:RequiredFieldValidator>
                </td></tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>      
            </td></tr> 
                    <tr>
                <td colspan="2">
                   Do you acknowledge that you have read and are willing
                   to abide by the <a href="https://www.licensing.arubanetworks.com/eula.php">End-User Software License Agreement? </a></td>
                </tr>
                <tr><td style="height: 43px">
                    <asp:RadioButtonList ID="RdBtnLst2" runat="server" Height="34px" Width="74px">
                         <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                    <td>
                       <asp:RequiredFieldValidator ID="ReqdValidYes2" runat="server" ErrorMessage="Acceptance of EULA is mandatory"
                            Width="460px" ControlToValidate="RdBtnLst2" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CvAckyes2" runat="server" Display="Dynamic" ErrorMessage="Please Acknoledge that You will abide by the rules and Regulations of EULA"
                            ValueToCompare="Yes" Width="460px" ControlToValidate="RdBtnLst2"></asp:CompareValidator>
                </td></tr>
                <tr><td colspan="2">
                    &nbsp;<asp:Label ID="LblError2" runat="server" Width="567px" ForeColor="Red"></asp:Label>
                </td></tr>
                <tr><td colspan="3">
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnActivate2" runat="server" Text="Activate" Width="82px" OnClick="BtnActivate2_Click" />
                    <asp:Button ID="BtnContinue" runat="server" Text="Continue" Width="78px" OnClick="BtnContinue_Click" Visible="False" />
                    <asp:Button ID="BtnCancel2" runat="server" Text="Cancel" Width="79px" OnClick="BtnCancel2_Click" Visible="False" />
                </td>
                </tr>
            <tr>
            </tr>
            </table> 
            
        </asp:WizardStep>
        <asp:WizardStep ID="CertStp3" runat="server" StepType="Step"
            Title="Enter Part ID">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 100%">
            <tr>
            <td  colspan="2"style="height: 29px; width: 495px;">
                &nbsp;<asp:FormView ID="FrmViewSl3" runat="server" Width="533px">
                    <ItemTemplate>
                        <table>
                                 <tr><td align="left"><b>Certificate ID:</b></td><td><%# DataBinder.Eval(Container.DataItem, "LicSN")%></td></tr>
                                 <tr><td align="left"><b>Part Number:</b></td><td><%# DataBinder.Eval(Container.DataItem, "LicPartId")%></td></tr>
                                 <tr><td align="left"><b>Part Description: </b></td><td><%# DataBinder.Eval(Container.DataItem, "LicPartDesc")%></td></tr>
                                 <tr><td align="left"><b>Serial Number: </b></td><td><%# DataBinder.Eval(Container.DataItem, "SysSN")%></td></tr>
                                 <tr><td align="left"><b>Please enter the Part Number of controller</b></td>
                                 <td><asp:TextBox ID="TxtpartId" runat="server" ValidationGroup="rfvValidPart3"></asp:TextBox></td></tr>
                                 <tr><td></td></tr>
                <tr><td colspan="2">             
                    <asp:RequiredFieldValidator ID="ReqdValidPart3" runat="server" 
                    ErrorMessage="Part ID of Controller is Mandatory"
                   Width="300px" ControlToValidate="TxtpartId" ValidationGroup="rfvValidPart3"></asp:RequiredFieldValidator>
                </td></tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>      
            </td></tr> 
                <tr><td colspan="2">
                    &nbsp;<asp:Label ID="LblError3" runat="server" Width="567px" ForeColor="Red"></asp:Label>
                </td></tr>
                <tr><td colspan="1">
                    <asp:Button ID="BtnContinue3" runat="server" Text="Activate" Width="83px" ValidationGroup="rfvValidPart3" OnClick="BtnContinue3_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="BtnCancel3" runat="server" Text="Cancel" Width="90px" OnClick="BtnCancel3_Click" />
                </td>
                </tr>
            <tr>
            </tr>
            </table> 
        </asp:WizardStep>
        <asp:WizardStep ID="CertStp4" runat="server" AllowReturn="False" StepType="Finish"
            Title="Activation Key">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 100%">
            <tr><td style="color: #0000cc; font-style: italic; height: 94px">Congratulations!!.You have successfully Activated License</td></tr>
            <tr><td>
                
                <asp:FormView ID="FrmVwActivation3" runat="server" Width="575px">
                 <ItemTemplate>
    <table>
      <tr><td align="left"><b>Certificate ID:</b></td> <td><%# DataBinder.Eval(Container.DataItem, "LicSN")%></td></tr>
      <tr><td align="left"><b>Part Number:</b></td><td><%# DataBinder.Eval(Container.DataItem, "LicPartId")%></td></tr>
      <tr><td align="left"><b>Part Description: </b></td><td><%# DataBinder.Eval(Container.DataItem, "LicPartDesc")%></td></tr>
      <tr><td align="left"><b>System Serial Number/MAC:</b></td><td><%# DataBinder.Eval(Container.DataItem, "SysSN")%></td></tr>
      <tr><td align="left"><b>System Part Number: </b></td><td><%# DataBinder.Eval(Container.DataItem, "SysPartId")%></td></tr>
      <tr><td align="left"><b>System Part Description: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "SysPartDesc")%></td></tr>
      <tr><td align="left"><b>Activation Key: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "ActivationKey")%> </td></tr>
    </table>                 
  </ItemTemplate>     
                </asp:FormView>
                </td></tr>
                <tr><td style="color: #0000cc; font-style: italic; height: 94px">Please print this page for your records.</td></tr>
           </table>
        </asp:WizardStep>
    </WizardSteps>
    <StepNavigationTemplate>
    </StepNavigationTemplate>
    <FinishNavigationTemplate>
    </FinishNavigationTemplate>
</asp:Wizard>
&nbsp;&nbsp;&nbsp;
