<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddQuickConnect.ascx.cs" Inherits="Controls_AddQuickConnect" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Activate QuickConnect" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
    </tr>
    <tr><td></td></tr>
</table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="953px" DisplaySideBar="False">
    <WizardSteps>
        <asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" Title="Enter Certificate ID" runat="server" >
            <table width ="100%">
                <tr><td colspan="4"> If you already have QuickConnect credentials and don't see them in LMS. Please import them first to LMS and then activate to renew your credentials here.</td></tr>
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
                    <td colspan="4" class="info"0005002-1612-190011
>(Please enter the certificate ID of QuickConnect license)</td>
                </tr>               
                <tr>
                    <td style="width: 137px" colspan="4"> 
                        <asp:Button ID="btnActivate" CssClass="btn" runat="server" Text="Next" ValidationGroup="Upgrade" OnClick="btnActivate_Click"/></td>
                </tr>
                <tr>
                    <td style="height: 21px; width: 417px;" colspan="4">
                        <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="False" Width="418px"></asp:Label>
                    </td>
                </tr> 
            </table>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Step" Title="Renew existing QuickConnect" runat="server" >
            <table>
            <tr><td><br /></td></tr>
            <tr><td style="color: #0000cc; font-style: italic">Following are the details of Certificate Id 
                <asp:Literal ID="LitrCert1" runat="server"></asp:Literal></td></tr>
                <tr><td>Part Number:<asp:Literal ID="LitrPartId1" runat="server"></asp:Literal></td></tr>                
              <tr><td>User Count:<asp:Literal ID="LitrUserCount1" runat="server"></asp:Literal></td>
            </tr>
            <tr><td>Expiry Date:<asp:Literal ID="LitrExpiry1" runat="server"></asp:Literal></td></tr>             
            <tr> <td style="color: #0000cc; font-style: italic">
            <asp:Literal ID="LitrText" runat="server" Text=" You have already activated a QuickConnect instance."></asp:Literal>
            </td></tr>
            <tr><td>
                <asp:RadioButtonList ID="RdBtnPrompt" runat="server" Width="555px">
                <asp:ListItem Text = "I want to extend my existing QuickConnect subscription." Value="RENEW" />         
                <asp:ListItem Text = "I want a new login for a separate QuickConnect instance." Value="NEW" />
                </asp:RadioButtonList> </td>
                </tr>  
                <tr>
                <td>
                    <asp:Button ID="btnNext" runat="server" CssClass="btn" Text="Next" OnClick="btnNext_Click" /></td>
                </tr>
                <tr><td>
                    <asp:Label ID="LblErr1" runat="server" CssClass="lblError" Visible="False"></asp:Label></td></tr>
            </table>
        </asp:WizardStep>
                <asp:WizardStep AllowReturn="False" ID="WizardStep1" StepType="Step" Title="Add New QuickConnect" runat="server" >
            <table>
            <tr><td style="width: 635px"><br /></td></tr>
           <tr><td style="color: #0000cc; font-style: italic">Following are the details of Certificate Id 
           <asp:Literal ID="LitrCert2" runat="server"></asp:Literal></td></tr>
               <tr><td>Part Number:<asp:Literal ID="LitrPartId2" runat="server"></asp:Literal></td></tr>              
              <tr><td style="width: 635px">User Count:<asp:Literal ID="LitrUserCount2" runat="server"></asp:Literal></td></tr>
             <tr><td style="width: 635px">Expiry Date:<asp:Literal ID="LitrExpiry2" runat="server"></asp:Literal></td></tr>
             <tr>   
             <td style="width: 635px">
            <asp:Panel ID="PanelAdd" runat="server" Visible="False">                                                     
            Company Name<b> * </b> <asp:TextBox ID="txtCompany" runat="server" Width="349px"></asp:TextBox>  
            </asp:Panel>
           <asp:Panel ID="PanelSelect" runat="server" Visible="False">                                                               Company Name <b> * </b> <asp:DropDownList ID="LstCompany" runat="server" Width="257px" CssClass="ddlist">
            </asp:DropDownList>                 
            </asp:Panel></td> </tr>
            <tr><td style="width: 635px"><asp:Label ID="LblErr2" runat="server" CssClass="lblError" Visible="False"></asp:Label></td></tr>
                <tr>                
                <td style="width: 635px">
                    <asp:Button ID="BtnSubmit" runat="server" CssClass="btn" Text="Submit" ValidationGroup="Submit" OnClick="BtnSubmit_Click" /></td>
                </tr>
            </table>
            </asp:WizardStep>
        <asp:WizardStep AllowReturn="False" ID="wStep3" StepType="Finish" Title="View Activation Info" runat="server" >
            <table>
                <tr><td style="color: #0000cc; font-style: italic; height: 60px">Congratulations, You have successfully added QuickConnect license credentials. 
                </td></tr>
            </table>
            <table>
                <tr><td>User Name:</td>                
                <td>                
                <asp:Literal ID="LiteralUserName" runat="server"></asp:Literal></td></tr>
                <tr><td>Password:</td>
                <td><asp:Literal ID="LiteralPwd" runat="server"></asp:Literal></td>
                </tr>
                <tr><td>User Count:</td>
                <td><asp:Literal ID="LiteralCount" runat="server"></asp:Literal></td>
                </tr>
                <tr><td>Expiry Date:</td><td><asp:Literal ID="LiteralExpry" runat="server"></asp:Literal></td></tr>
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

