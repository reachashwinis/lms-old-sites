<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="PendingAccountInfo.aspx.cs" Inherits="Accounts_PendingAccountInfo" Title="Licensing System | Pending Account Info" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<asp:Wizard ID="wizNewAcct" runat="Server" DisplayCancelButton="false" ActiveStepIndex="1" Height="52px" DisplaySideBar="False" >
<StartNavigationTemplate>
  </StartNavigationTemplate>
  
  <StepNavigationTemplate>
      </StepNavigationTemplate>
<WizardSteps>
<asp:WizardStep StepType="Start" ID="wizCert" AllowReturn="False" Title="Enter Valid Certificate ID" runat="server" >
Please fill up this form<br />
<input type="hidden" id="hdnEmail" runat="server" />
<input type="hidden" id="hdnCompanyId" runat="server" />
<input type="hidden" id="hdnCustType" runat="server" />
<input type="hidden" id="hdnBrand" runat="server" />
<input type="hidden" id="hdnCompanyName" runat="server" />
<input type="hidden" id="hdnDuplicate" runat="server" />
<input type="hidden" id="hdnActivationCode" runat="server" />

<div id="divPwd" runat="server"  style="width:100%">
<table width="100%">
<tr>
<td style="width:10%">
Password
</td>
<td style="width:10%">
<asp:TextBox ID="txtPassword" runat="server" MaxLength="32" Columns="32" CssClass="txt" TextMode="Password"></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvPwd" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please enter your password" ControlToValidate="txtPassword" ValidationGroup="UserInfo"></asp:RequiredFieldValidator>
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
<asp:RequiredFieldValidator ID="rfvConfirm" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="Please confirm your password" ControlToValidate="txtPassword" ValidationGroup="UserInfo"></asp:RequiredFieldValidator>
<asp:CompareValidator ID="cmpPassword" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirm" CssClass="lblError"  ErrorMessage="Passwords do not match" Display="Dynamic" ValidationGroup="UserInfo"></asp:CompareValidator>
</td>
</tr>
</table>
</div>
<table width="100%">
<tr>
<td style="width:10%">
Firstname
</td>
<td style="width:10%">
<asp:TextBox ID="txtFName" runat="server" MaxLength="255" Columns="50" CssClass="txt" ></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvFName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a first name" ControlToValidate="txtFName" ValidationGroup="UserInfo"></asp:RequiredFieldValidator>
</td>
</tr>


<tr>
<td style="width:10%">
Lastname
</td>
<td style="width:10%">
<asp:TextBox ID="txtLName" runat="server" MaxLength="255" Columns="50" CssClass="txt" ></asp:TextBox>
</td>
<td style="width:100px">
<asp:RequiredFieldValidator ID="rfvLName" runat="server" CssClass="lblError" Display="Dynamic" ErrorMessage="You did not enter a last name" ControlToValidate="txtLName" ValidationGroup="UserInfo"></asp:RequiredFieldValidator>
</td>
</tr>

<tr>
<td style="width:10%">
Company
</td>
<td style="width:10%">
<asp:TextBox ID="txtCompany" runat="server" MaxLength="255" Columns="50" CssClass="txt" ReadOnly="True" ></asp:TextBox>
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
<asp:WizardStep AllowReturn="False" StepType="Complete" runat="server">
<span id="spanNew" runat="server">
    <asp:Label ID="LblDisplay" runat="server" Font-Bold="True" ForeColor="#00C000" Height="26px"
        Width="467px"></asp:Label>
</span>
</asp:WizardStep>
</WizardSteps>

</asp:Wizard>
</asp:Content>

