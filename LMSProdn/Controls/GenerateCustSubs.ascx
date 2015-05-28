<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateCustSubs.ascx.cs" Inherits="Controls_GenerateCustSubs" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border = "0">
<tr><td style="width: 311px; height: 21px">
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Generate Customer Subscription ID"
Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
</table>

    <asp:Wizard ID="wizGenerate" runat="server" DisplayCancelButton="false" ActiveStepIndex="0" Width="981px" Height="239px" DisplaySideBar="False">
        <WizardSteps>
            <asp:WizardStep ID="wStep1" runat="server" AllowReturn="False" StepType="Start" Title="Enter Certificate ID" >
  <table width="100%" border="0"> <tr><td style="color:#0000cc;"  colspan="4"><i>
      <asp:Literal ID="LitrInstruction" runat="server" Text="A ClearPass Subscription ID is used to automatically download software updates to your ClearPass appliances. &lt;BR&gt; You should use the same Subscription ID for each Clearpass server in a cluster. &lt;BR&gt; Use this form to create new Subscription ID for a ClearPass deployment."></asp:Literal> </i></td></tr>

<tr style="height:30px"><td style="width: 180px">Certificate ID :</td> 
<td style="color:Red; width:1%"><b> * </b> </td>
<td style="width: 405px"><asp:TextBox ID="TxtCertId" runat="server" Width="294px" CssClass="txt"></asp:TextBox></td>
<td>
<asp:RequiredFieldValidator ID="rfvCertId" runat="server" ErrorMessage="Certificate ID cannot be empty." ControlToValidate="TxtCertId" CssClass="lblError"  ValidationGroup="validateClearPass"></asp:RequiredFieldValidator></td></tr>
<tr><td colspan="4"> 
    <asp:Label ID="LblHelp" runat="server" ForeColor="#0000CC" Text="This certificate ID must be for a ClearPass appliance." Font-Italic="True" Width="492px"></asp:Label></td></tr>
 <tr style="height:30px">
 <td style="width: 180px"> Customer Account Name: </td>
 <td style="color:Red; width:1%"><b> * </b> </td>
 <td style="width: 405px">
     <asp:TextBox ID="TxtCustName" runat="server" Width="401px" CssClass="txt"></asp:TextBox></td>
 <td>
     <asp:RequiredFieldValidator ID="rfvCompanyId" runat="server" ErrorMessage="Customer Name cannot be empty."
     ControlToValidate="TxtCustName" CssClass="lblError" ValidationGroup="validateClearPass"></asp:RequiredFieldValidator></td>     
 </tr>
 <!-- <tr><td><br /></td></tr>-->
 <tr style="height:30px">
 <td style="width: 180px; height: 30px;">Customer Email Address: </td>
 <td style="width:1%; height: 30px;"><b>  </b> </td>
    <td style="width: 405px; height: 30px;"> <asp:TextBox ID="TxtEmail" runat="server" Width="401px" CssClass="txt"></asp:TextBox></td>
    <td>
        <asp:CustomValidator ID="CustmValidateEmail" runat="server" OnServerValidate="CustmValidateEmail_ServerValidate" Width="284px" ControlToValidate="TxtEmail" CssClass="lblError" ValidationGroup="validateClearPass"></asp:CustomValidator> </td>
 </tr>
  <tr><td> Version:</td>
 <td style="color:Red; width:1%"><b> * </b> </td>
<td>
<asp:DropDownList ID="DrpListVersion" runat="server">
<asp:ListItem Text = "6.X.X.X" value = "6.X.X.X" Selected="True"></asp:ListItem>
<asp:ListItem Text = "5.0.X.X" value = "5.0.X.X"></asp:ListItem>
</asp:DropDownList></td>
</tr>
    <tr style="height:30px"><td colspan="4">  
        <asp:Button ID="BtnSubmit" CssClass= "btn" runat="server" Text="Submit" ValidationGroup="validateClearPass" OnClick="BtnSubmit_Click" />
    </td></tr>
  <tr><td colspan="4"> 
      <asp:Label ID="LblError" runat="server" CssClass="lblError" Visible="False"></asp:Label></td></tr>
   </table>
            </asp:WizardStep>
            <asp:WizardStep ID="wStep2" runat="server" AllowReturn="False" Title="Display Subscription ID" StepType="Finish">
               <table border="0">
                <tr><td style="color: #0000cc; font-style: italic;">Congratulations, You have successfully generated the Subscription ID and policy manager license key.<br /></td></tr>
                <tr>                
                    <td>Policy Manager license key :
                    <span style="font-family: monospace;">
                    <asp:Literal ID="LiteralLickey" runat="server"></asp:Literal>  
                </span>                                      
                    </td>
                </tr>
                <tr>
                <td style="color: #0000cc; font-style: italic;"> This License Key is required when you first login to ClearPass Policy Manager<br /> You can also add a new license in Policy Manager by navigating to Administration >> Server manager >> Licensing </td>
                </tr>
            </table>
            <table border="0">
                <tr>
                    <td>Subscription Key :    
                    <span style="font-family: monospace;">                  
                     <asp:Literal ID="LiteralAct" runat="server"></asp:Literal>  
                     </span> 
                    </td>
                </tr>
                <tr>
                <td style="color: #0000cc; font-style: italic;"> 
                    A ClearPass Subscription ID is used to automatically download software updates to your Clearpass appliances. <br /> You should use the same Subscription ID for each ClearPass server in a cluster. <br />
                    Enter your Subscription ID in Policy Manager by navigating to Administration >> Agents and Software Updates >> Software Updates 
                </td>
                </tr>
            </table>
            <table border="0">
            <tr><td></td></tr>
                <tr>
                    <td style="color: #0000cc; font-style: italic; height: 39px; width: 615px;">Please print this page for your records.<br />
                        <asp:Label ID="LblInfo" ForeColor="#0000CC" Font-Italic="True" runat="server" Text="This information has been emailed to you."></asp:Label>
                    <br />
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

