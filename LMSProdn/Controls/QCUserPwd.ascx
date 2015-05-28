<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QCUserPwd.ascx.cs" Inherits="Controls_QCUserPwd" %>

<asp:Wizard ID="wizPwd" runat="Server" DisplayCancelButton="false" ActiveStepIndex="0" Width="823px" DisplaySideBar="False" >
<StartNavigationTemplate>
  </StartNavigationTemplate>  
  <StepNavigationTemplate>
  </StepNavigationTemplate>
<WizardSteps>
<asp:WizardStep StepType="Start" ID="wizLogin" AllowReturn="False" Title="Enter User Name" runat="server" >
<table width="100%">
<tr><td style="width:30%">
Enter your login User Name<br /></td>
<td style="width:30%">
<asp:TextBox ID="txtUserName" runat="server" CssClass="txt" Columns="50" ></asp:TextBox></td>
<td style="width:40%">
<asp:RequiredFieldValidator ID="rfvUserName" runat="server" CssClass="lblError" ControlToValidate="txtCertId"  ErrorMessage="You must enter your login user name." Display="Dynamic" ValidationGroup="VerifyUser"></asp:RequiredFieldValidator></td>
</tr>
<tr><td style="width:30%">
Enter your login Password<br /></td>
<td style="width:30%">
<asp:TextBox ID="txtPassword" runat="server" CssClass="txt" Columns="50" TextMode="Password" ></asp:TextBox></td>
<td style="width:40%">
<asp:RequiredFieldValidator ID="rfvPwd" runat="server" CssClass="lblError" ControlToValidate="txtCertId"  ErrorMessage="You must enter your login password" Display="Dynamic" ValidationGroup="VerifyUser"></asp:RequiredFieldValidator></td>
</tr>
<tr><td colspan="3">
<asp:Button ID="btnVerifyUser" CssClass="btn" runat="server" OnClick="btnVerifyUser_OnClick" Text="Next &gt;&gt;" ValidationGroup="VerifyUser" /> </td>
</tr>
<tr><td>
<asp:Label ID="lblError" runat="server" Text="" CssClass="lblError"></asp:Label></td></tr>
</table>
</asp:WizardStep>
<asp:WizardStep ID="wizQCPwd" runat="server" StepType="Complete" AllowReturn="False" Title="Quick Connect Password">
<table width="100%">
<tr>
<td style="width:30%">
User Name
</td>
<td style="width:50%">
    <asp:Literal ID="LiteralQCUser" runat="server"></asp:Literal>
</td>
</tr>
<tr>
<td style="width:30%"> 
Password
</td>
<td style="width:50%">
<asp:Literal ID="LiteralQCPwd" runat="server"></asp:Literal>
</td>
</tr>
</table>
</asp:WizardStep>
</WizardSteps>
</asp:Wizard>

