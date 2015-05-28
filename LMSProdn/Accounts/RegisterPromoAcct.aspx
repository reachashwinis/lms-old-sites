<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/LoginTemplate.master" CodeFile="RegisterPromoAcct.aspx.cs" Inherits="Accounts_RegisterPromoAcct" Title="Licensing System | Register me for Promotional offer"%>

<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<asp:Wizard ID="wizNewAcct" runat="Server" ActiveStepIndex="0" Width="730px" DisplaySideBar="False" >
  <StepNavigationTemplate>
      </StepNavigationTemplate>
<WizardSteps>
<asp:WizardStep StepType="Start" ID="wizCert" AllowReturn="False" Title="Enter Serial Number of Instant Mesh" runat="server">
<table width="100%">
<tr><td>Enter Serial Number of Instant AP</td>
<td style="color:Red"> * </td>
<td>
<asp:TextBox ID="txtCertId" runat="server" CssClass="txt" Columns="50" ></asp:TextBox> </td>
<td style="width: 157px">
<asp:RequiredFieldValidator ID="rfvCertId" runat="server" CssClass="lblError" ControlToValidate="txtCertId"  ErrorMessage="You must enter a valid serial number" Display="Dynamic" ValidationGroup="VerifyCert" Width="217px"></asp:RequiredFieldValidator>
</td>
</tr>
<tr> <td style="width: 287px">Enter Promotion code </td>
<td style="color:Red"> * </td>
<td> <asp:TextBox ID="txtPromoCode" runat="server" CssClass="txt" Columns="50" ></asp:TextBox> </td>
<td style="width: 157px">
<asp:RequiredFieldValidator ID="rfvPrmCode" runat="server" CssClass="lblError" ControlToValidate="txtPromoCode"  ErrorMessage="You must enter a valid Promotion code" Display="Dynamic" 
ValidationGroup="VerifyCert" Width="224px"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td style="width: 287px">
<asp:CustomValidator ID="cvVerifyCert" runat="server" OnServerValidate="cvVerifyCert_OnServerValidate" CssClass="lblError" ControlToValidate="txtCertId" ErrorMessage="You must enter a valid Serial Number" Display="Dynamic" ValidationGroup="VerifyCert"></asp:CustomValidator>
</td>
<td></td>
<td>
<asp:CustomValidator ID="cvVerifyPromoCode" runat="server" OnServerValidate="cvVerifyPromoCode_OnServerValidate" CssClass="lblError" ControlToValidate="txtPromoCode" ErrorMessage="You must enter a valid Promo Code" Display="Dynamic" ValidationGroup="VerifyCert"></asp:CustomValidator>
</td>
</tr>
<tr> <td style="width: 287px">
<asp:Button ID="btnVerifyCert" CssClass="btn" runat="server" OnClick="btnVerifyCert_OnClick" Text="Next &gt;&gt;" ValidationGroup="VerifyCert"/>
</td>
</tr>
</table>
</asp:WizardStep>
<asp:WizardStep ID="wizEmail" runat="server" StepType="Step" AllowReturn="False" Title="Enter Account Information">
<table width="100%">
<tr>
<td style="width:84%">
Email Address
</td>
<td style="width:10%">
<asp:TextBox ID="txtEmail" runat="server" MaxLength="255" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td style="width:94px">
<asp:RequiredFieldValidator ID="rfvEmail" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter an email address" ControlToValidate="txtEmail" ValidationGroup="UserInfo" Width="276px"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="revEmail" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please enter a valid email address" ControlToValidate="txtEmail" Enabled="True"
ValidationExpression="^([a-zA-Z0-9])+([a-zA-Z0-9\.+_-])*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$" Width="277px" ></asp:RegularExpressionValidator>
<asp:CustomValidator id="cvEmail" runat="server" CssClass="lblError" Display="Dynamic" ControlToValidate="txtEmail" ValidationGroup="VerifyEmail"
 OnServerValidate="cvEmail_OnServerValidate" Width="278px"></asp:CustomValidator>
</td>
</tr>
<tr>
<td style="width:84%; height: 37px;">
Confirm Email Address: </td>
<td style="width:10%; height: 37px;">
<asp:TextBox ID="txtConfirmEmail" runat="server" MaxLength="255" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td style="width:94px; height: 37px;">
<asp:RequiredFieldValidator ID="rfvConfirmEmail" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please confirm your email address" ControlToValidate="txtConfirmEmail" ValidationGroup="VerifyEmail" Width="281px"></asp:RequiredFieldValidator>
<asp:CompareValidator ID="cmpConfirmEmail" runat="server" ControlToCompare="txtEmail" ControlToValidate="txtConfirmEmail" CssClass="lblError"  ErrorMessage="Email addresses do not match" Display="Dynamic" ValidationGroup="VerifyEmail" Width="279px"></asp:CompareValidator>
</td>
</tr>
</table>
<asp:Button ID="btnNext" CssClass="btn" runat="server" OnClick="btnNext_OnClick" Text="Next &gt;&gt;" ValidationGroup="VerifyEmail" />
</asp:WizardStep>
<asp:WizardStep ID="wizuserInfo" runat="server" StepType="Step">

<div id="divPwd" runat="server">
<table width="100%">
<tr>
<td style="width:10%">
Password
</td>
<td style="width:10%">
<asp:TextBox ID="txtPassword" runat="server" MaxLength="32" Columns="32" CssClass="txt" TextMode="Password"></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvPwd" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please enter your password" ControlToValidate="txtPassword" ValidationGroup="UserInfo" Width="182px"></asp:RequiredFieldValidator>
</td>
</tr>

<tr>
<td style="width:10%">
Confirm Password
</td>
<td style="width:10%">
<asp:TextBox ID="txtConfirm" runat="server" MaxLength="32" Columns="32" CssClass="txt" TextMode="Password"></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvConfirm" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please confirm your password" ControlToValidate="txtPassword" ValidationGroup="UserInfo" Width="183px"></asp:RequiredFieldValidator>
<asp:CompareValidator ID="cmpPassword" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirm" CssClass="lblError"  ErrorMessage="Passwords do not match" Display="Dynamic" ValidationGroup="UserInfo" Width="184px"></asp:CompareValidator>
</td>
</tr>
</table>
</div>
<table width="100%">
<tr>
<td style="width:10%">
First Name
</td>
<td style="width:10%">
<asp:TextBox ID="txtFName" runat="server" MaxLength="255" Columns="50" CssClass="txt" ></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvFName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a first name" ControlToValidate="txtFName" ValidationGroup="UserInfo" Width="170px"></asp:RequiredFieldValidator>
</td>
</tr>


<tr>
<td style="width:10%">
Last Name
</td>
<td style="width:10%">
<asp:TextBox ID="txtLName" runat="server" MaxLength="255" Columns="50" CssClass="txt" ></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvLName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a last name" ControlToValidate="txtLName" ValidationGroup="UserInfo" Width="172px"></asp:RequiredFieldValidator>
</td>
</tr>

<tr>
<td style="width:10%">
Company
</td>
<td style="width:10%">
<asp:TextBox ID="txtCompany" runat="server" MaxLength="255" Columns="50" CssClass="txt" ></asp:TextBox>
</td>
<td style="width:100px">
&nbsp;
</td>
</tr>



<tr>
<td style="width:10%">
Phone
</td>
<td style="width:10%">
<asp:TextBox ID="txtPhone" runat="server" MaxLength="50" Columns="50" CssClass="txt" ></asp:TextBox>
</td>
<td style="width:100px">
&nbsp;
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblError" CssClass="lblError" runat="server"></asp:Label>
</td>
</tr>
</table>


<asp:Button ID="btnActivate" runat="server" Text="Activate" CssClass="btn" OnClick="btnActivate_OnClick" ValidationGroup="UserInfo" />
</asp:WizardStep>
<asp:WizardStep ID="WizardStep1" AllowReturn="False" StepType="Complete" runat="server">
Your account is created.<br />
<span id="spanDuplicate" runat="server">
Thank you for creating your account. An email will be sent to you shortly.  Your response will be needed from that email to activate your account
</span>
</asp:WizardStep>
</WizardSteps>
    <StartNavigationTemplate>
        <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Next"
            Visible="False" />
    </StartNavigationTemplate>

</asp:Wizard>
<input type="hidden" id="hdnDuplicate" runat="server" />
</asp:Content>


