<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreateClpEval.ascx.cs" Inherits="Controls_CreateClpEval" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />

<table border="0" width="100%">
    <tr>
    <td style="width: 311px; height: 21px">
        <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0"
        Text="Generate ClearPass evaluation license" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label></td>
</tr>
<tr><td><br /></td></tr>
</table>
<asp:Wizard ID="WizGenerate" runat="server" ActiveStepIndex="0" Width="990px" DisplaySideBar="False">
    <WizardSteps>
        <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
        <table width="100%">
<tr>
<td style="width: 220px">
Company Name:
</td>
<td style="color:Red; width:2%"><b> * </b> </td>
<td style="width: 280px">
<asp:TextBox ID="TxtCompany" runat="server" Width="270px"></asp:TextBox>
</td>
<td>
 <asp:RequiredFieldValidator ID="reqdCompany" runat="server" ValidationGroup="CreateClpEvals" ControlToValidate="TxtCompany" ErrorMessage="Recipient's Company Name is mandatory." CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td style="width: 220px">
Name of Recipient :</td>
<td style="color:Red; width:2%"><b> * </b> </td>
<td style="width: 280px">
    <asp:TextBox ID="TxtName" runat="server" Width="270px"></asp:TextBox>
</td>
<td>
 <asp:RequiredFieldValidator ID="reqdName" runat="server" ValidationGroup="CreateClpEvals" ControlToValidate="TxtName" ErrorMessage="Recipient's Name is mandatory." CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td style="width: 220px">
Email Address of Recipient:</td>
<td style="color:Red; width:2%"><b> * </b> </td>
<td style="width: 280px">
    <asp:TextBox ID="TxtEmail" runat="server" Width="270px"></asp:TextBox>
</td>
<td>
 <asp:RequiredFieldValidator ID="reqdEmail" runat="server" ValidationGroup="CreateClpEvals" ControlToValidate="TxtEmail" ErrorMessage="Recipient's Email Address is mandatory." CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td colspan="4">
<asp:CustomValidator ID="CustmValidateEmail" runat="server" ErrorMessage="Invalid Email" Display="Dynamic" ValidationGroup="CreateClpEvals" OnServerValidate="CustmValidateEmail_OnValidate" CssClass="lblError" Width="387px"></asp:CustomValidator>
</td>
</tr>
<tr>
<td colspan="4">
    <asp:Button ID="btnSubmit" runat="server" ValidationGroup="CreateClpEvals" Text="Submit" OnClick="btnSubmit_Click" />
</td>
</tr>
<tr>
<td colspan="4">
<asp:Label ID="LblError" runat="server" CssClass="lblError"></asp:Label>
</td>
</tr>
</table>
    </asp:WizardStep>
    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
        <table width="100%">
        <tr><td colspan="4" style="color: #0000cc; font-style: italic;"><i>Congratulations!. You have successfully generated the evaluation license keys and Subscription ID.</i></td></tr>
        <tr>
        <td style="width:234px;">Subscription ID:</td>
        <td style="width:75%;">
        <span style="font-family: monospace;">
            <asp:Literal ID="LiteralSubKey" runat="server"></asp:Literal>
        </span></td>
        </tr>
       <tr>
    <td style="color: #0000cc; font-style: italic;" colspan="4"> 
        A ClearPass Subscription ID is used to automatically download software updates to your Clearpass appliances. <br /> You should use the same Subscription ID for each ClearPass server in a cluster. <br />
        Enter your Subscription ID in Policy Manager by navigating to Administration >> Agents and Software Updates >> Software Updates 
    </td>
    </tr>
        <tr>
        <td style="width:234px;">Policy Manager License Key:</td>
        <td style="width:75%;">
        <span style="font-family: monospace;">
            <asp:Literal ID="LiteralPolicyLic" runat="server"></asp:Literal>
        </span></td>
        </tr>
       <tr>
        <td style="color: #0000cc; font-style: italic;" colspan="4"> This License Key is required when you first login to ClearPass Policy Manager<br /> You can also add a new license in Policy Manager by navigating to Administration >> Server manager >> Licensing </td>
        </tr>
        <tr>
        <td style="width:234px;">Enterprise License key:</td>
        <td style="width:75%;">
        <span style="font-family: monospace;">
            <asp:Literal ID="LiteralEnterLic" runat="server"></asp:Literal>
         </span>   </td>
        </tr>
        <tr>
       <td style="color: #0000cc; font-style: italic;" colspan="4"><i>Please print this page for your record.<br /></i></td></tr>
       <tr><td style="width: 234px" colspan="4">
     <asp:Label ID="LblInfo" ForeColor="#0000CC" Font-Italic="True" runat="server" Text="This information has been emailed to you." Width="909px"></asp:Label></td>
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


