<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="CreateAccount.aspx.cs" Inherits="Accounts_CreateAccount" Title="Licensing System | Create a New Account" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<asp:Wizard ID="wizNewAcct" runat="Server" DisplayCancelButton="false" ActiveStepIndex="0" Width="761px" DisplaySideBar="False" >
<StartNavigationTemplate>
  </StartNavigationTemplate>
  
  <StepNavigationTemplate>
      </StepNavigationTemplate>
<WizardSteps>
<asp:WizardStep StepType="Start" ID="wizCert" AllowReturn="False" Title="Enter Valid Certificate ID" runat="server" >
Enter a valid Certificate ID<br />
<asp:TextBox ID="txtCertId" runat="server" CssClass="txt" Columns="50" ></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvCertId" runat="server" CssClass="lblError" ControlToValidate="txtCertId"  ErrorMessage="You must enter a license certificate ID" Display="Dynamic" ValidationGroup="VerifyCert"></asp:RequiredFieldValidator>
<asp:CustomValidator ID="cvVerifyCert" runat="server" OnServerValidate="cvVerifyCert_OnServerValidate" CssClass="lblError" ControlToValidate="txtCertId" ErrorMessage="You must enter a valid license certificate ID" Display="Dynamic" ValidationGroup="VerifyCert"></asp:CustomValidator><br />
<asp:Button ID="btnVerifyCert" CssClass="btn" runat="server" OnClick="btnVerifyCert_OnClick" Text="Next &gt;&gt;" ValidationGroup="VerifyCert" />
</asp:WizardStep>
<asp:WizardStep ID="wizEmail" runat="server" StepType="Step" AllowReturn="False" Title="Enter Account Information">
<table width="100%">
<tr>
<td style="width:40%">
Email Address:
</td>
<td style="width:20%">
<asp:TextBox ID="txtEmail" runat="server" MaxLength="255" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td style="width:40%">
<asp:RequiredFieldValidator ID="rfvEmail" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter an email address" ControlToValidate="txtEmail" ValidationGroup="UserInfo" Width="276px"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="revEmail" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please enter a valid email address" ControlToValidate="txtEmail"
ValidationExpression="^([a-zA-Z0-9])+([a-zA-Z0-9\.+_-])*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$" Width="277px" ></asp:RegularExpressionValidator>
<asp:CustomValidator id="cvEmail" runat="server" CssClass="lblError" Display="Dynamic" ControlToValidate="txtEmail" ValidationGroup="VerifyEmail"
 OnServerValidate="cvEmail_OnServerValidate" Width="278px"></asp:CustomValidator>
</td>
</tr>
<tr>
<td style="width:40%; height: 37px;">
Confirm Email Address;</td>
<td style="width:20%; height: 37px;">
<asp:TextBox ID="txtConfirmEmail" runat="server" MaxLength="255" Columns="50" CssClass="txt"></asp:TextBox>
</td>
<td style="width:40%; height: 37px;">
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
<asp:RegularExpressionValidator ID="RegPassChck" runat="server" Enabled = "true"
ControlToValidate="txtPassword" ValidationExpression="(?=.{8,})[a-zA-Z]+[^a-zA-Z]+|[^a-zA-Z]+[a-zA-Z]+"
Display="Dynamic" ErrorMessage="Password must be 8 characters and have both letters and numbers." Width="362px" />
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
<asp:RequiredFieldValidator ID="rfvFName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a First Name" ControlToValidate="txtFName" ValidationGroup="UserInfo" Width="170px"></asp:RequiredFieldValidator>
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
<asp:RequiredFieldValidator ID="rfvLName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a Last Name" ControlToValidate="txtLName" ValidationGroup="UserInfo" Width="172px"></asp:RequiredFieldValidator>
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
<asp:RequiredFieldValidator ID="rfvCName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a Company Name" ControlToValidate="txtCompany" ValidationGroup="UserInfo" Width="172px"></asp:RequiredFieldValidator>
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


<asp:Button ID="btnActivate" runat="server" Text="Create" CssClass="btn" OnClick="btnActivate_OnClick" ValidationGroup="UserInfo" />
</asp:WizardStep>
<asp:WizardStep AllowReturn="False" StepType="Complete" runat="server">
Your account is created.<br />
<span id="spanDuplicate" runat="server">
Thank you for creating your account. An email will be sent to you shortly.  Your response will be needed from that email to activate your account
</span>
</asp:WizardStep>
</WizardSteps>

</asp:Wizard>
<input type="hidden" id="hdnDuplicate" runat="server" />
</asp:Content>--%>

