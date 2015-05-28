<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImportMyLegacySubscription.ascx.cs" Inherits="Controls_ImportMyLegacySubscription" %>
<table width="100%">
<tr><td style="width: 239px">
    <asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Import Leagacy Subscription ID"
        Width="239px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
        <tr><td><br /></td></tr>
</table>
<asp:Wizard ID="WizImportlegacySub" runat="server" Height="152px" Width="100%" ActiveStepIndex="0" DisplaySideBar="False">
<StepStyle Wrap="True"></StepStyle>
<StartNavigationTemplate>    
</StartNavigationTemplate>
<WizardSteps>
<asp:WizardStep runat="server" AllowReturn="False" ID="SubStp1" StepType="Start" Title="Enter Subscription ID">
<table width="100%">
<tr><td>
<asp:Label runat="server" Text="Subscription ID:" Width="175px" ID="LblSubKey"></asp:Label>
</td></tr>
<tr><td style="width: 490px"><asp:TextBox runat="server" Width="448px" ID="TxtSubKey1" CssClass="txt"></asp:TextBox>
 <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtSubKey1" ErrorMessage="Please Enter Subscription ID" ValidationGroup="WizStp" ID="ReqdValid1"></asp:RequiredFieldValidator>
 </td></tr>
 <tr><td style="width: 490px">
 <asp:Button runat="server" Text="Next &gt;&gt;" ValidationGroup="WizStp" ID="BtnNext1" CssClass="btn" OnClick="BtnNext1_Click"></asp:Button>
 </td></tr>
 <tr><td style="width: 490px"><asp:Label runat="server" Font-Bold="True" ForeColor="Red" Width="448px" CssClass="lblError" ID="LblError1" Visible="False"></asp:Label>
 </td></tr>
 <tr><td style="width: 490px; HEIGHT: 28px"></td></tr></table>
 </asp:WizardStep>
<asp:WizardStep runat="server" ID="SubStp2" StepType="Step" Title="Display Subscription ID details">
 <table style="width: 100%">
 <tr><td><asp:Repeater runat="server" ID="rptSub2"><ItemTemplate>
    <table>
    <tr><td>Following are the details of your Subscription ID. Please hit <i>Import</i> button to continue.</td></tr>
    <tr><td><br /></td></tr>
      <tr><td align="left"><b>Subscription ID:</b></td> <td><%# DataBinder.Eval(Container.DataItem, "SubscriptionKey")%></td></tr>
    <tr><td align="left"><b>Organization: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "Organization")%></td></tr>
      <tr><td align="left"><b>Purchase Order:</b></td><td><%# DataBinder.Eval(Container.DataItem, "po_id")%></td></tr>
      <tr><td align="left"><b>Sales Order: </b></td><td><%# DataBinder.Eval(Container.DataItem, "so_id")%></td></tr>
      <tr><td align="left"><b>Expire Time: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "expire_time")%></td></tr>
      <tr><td align="left"><b>License Count:</b></td><td><%# DataBinder.Eval(Container.DataItem, "license_count")%></td></tr>
      <tr><td align="left"><b>OnBoard Count: </b></td><td><%# DataBinder.Eval(Container.DataItem, "onBoard_count")%></td></tr>
     <tr><td align="left"><b>User Name: </b></td><td><%# DataBinder.Eval(Container.DataItem, "user_name")%></td></tr>
    <tr><td align="left"><b>Password: </b></td><td><%# DataBinder.Eval(Container.DataItem, "password")%></td></tr>
    <tr><td align="left"><b>SMS Credit: </b></td><td><%# DataBinder.Eval(Container.DataItem, "sms_credit")%></td></tr>
    <tr><td align="left"><b>SMS handler: </b></td><td><%# DataBinder.Eval(Container.DataItem, "sms_handler")%></td></tr>
    <tr><td align="left"><b>Email: </b></td><td><%# DataBinder.Eval(Container.DataItem, "email")%></td></tr>
    <tr><td align="left"><b>Created Time: </b></td><td><%# DataBinder.Eval(Container.DataItem, "create_time")%></td></tr>
    <tr><td align="left"><b>High Availability Key: </b></td><td><%# DataBinder.Eval(Container.DataItem, "HASubscriptionKey")%></td></tr>
    <tr><td align="left"><b>Adversting: </b></td><td><%# DataBinder.Eval(Container.DataItem, "advertising")%></td></tr>
    <tr><td style="color:Red" colspan="2"><%# DataBinder.Eval(Container.DataItem, "Error")%></td></tr>  
    </table>                   
</ItemTemplate>
</asp:Repeater>
    <table>
    <tr><td><br /></td></tr>
    <tr><td colspan="2">
        <asp:Label ID="LblError2" runat="server" Font-Bold="True" ForeColor="Red" visible = "False" CssClass="lblError"></asp:Label>
    </td></tr>  
    <tr><td colspan="2">
    <asp:Button runat="server" Text="Import" Width="117px" ID="BtnImport2" CssClass="btn" OnClick="BtnImport2_Click"></asp:Button>
    </td></tr>    
    </table>
 </td></tr></table>

 </asp:WizardStep>
<asp:WizardStep runat="server" AllowReturn="False" ID="SubStp3" StepType="Finish" Title="Import result">
<table style="width: 90%"><tr><td>Congratulations!!.You have successfully Imported your Subscription ID to LMS. Following are the details. You will reicieve email notification shortly!.</td></tr><tr><td></td></tr>
<tr><td><br /></td></tr>
</table>
<asp:Repeater runat="server" ID="rptSub3"><ItemTemplate>
    <table>
      <tr><td align="left"><b>Subscription ID:</b></td> <td><%# DataBinder.Eval(Container.DataItem, "SubscriptionKey")%></td></tr>
      <tr><td align="left"><b>Part Number:</b></td><td><%# DataBinder.Eval(Container.DataItem, "part_id")%></td></tr>
      <tr><td align="left"><b>Certificate ID: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "serial_number")%>
      </td></tr> 
      <tr><td align="left"><b>Certificate Serial Number: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "Lserial_number")%>
      </td></tr>           
      <tr><td align="left"><b> High Availability Key: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "HASubscriptionKey")%>
      </td></tr>
      <tr><td align="left"><b>HA Part Number: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "HApart_id")%>
      </td></tr>      
            <tr><td align="left"><b> HA Certificate ID: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "HAserial_number")%>
      </td></tr>
            <tr><td align="left"><b> HA Certificate Serial Number: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "HALserial_number")%>
      </td></tr>      
   <tr><td align="left"><b>Certificate ID for Guest License: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "LicCertId")%>
      </td></tr>
            <tr><td align="left"><b>Certificate Serial Number for Guest License: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "LicSerialNo")%>
      </td></tr>
         <tr><td align="left"><b>Certificate ID for Onboard License: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "OnBoardCertId")%>
      </td></tr>
            <tr><td align="left"><b>Certificate Serial Number for Onboard License: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "OnBoardSerialNo")%>
      </td></tr>
         <tr><td align="left"><b>Certificate ID for Advertising: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "AdvCertId")%>
      </td></tr>
            <tr><td align="left"><b>Certificate Serial Number for Advertising: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "AdvSerialNo")%>
      </td></tr>      
      <tr><td align="left"><b>Purchase Order:</b></td><td><%# DataBinder.Eval(Container.DataItem, "po_id")%></td></tr>
      <tr><td align="left"><b>Sales Order: </b></td><td><%# DataBinder.Eval(Container.DataItem, "so_id")%></td></tr>
      <tr><td align="left"><b>Organization: </b></td><td><%# DataBinder.Eval(Container.DataItem, "Organization")%></td>
     <!-- <tr><td align="left"><b>Shipped TO Email: </b></td><td><%# DataBinder.Eval(Container.DataItem, "email")%></td>
      </tr> -->
      <tr><td><%# DataBinder.Eval(Container.DataItem, "Error")%></td></tr>
    </table>                   
</ItemTemplate>
</asp:Repeater>
</asp:WizardStep>
</WizardSteps>
<FinishNavigationTemplate>
    
</FinishNavigationTemplate>
<StepNavigationTemplate>
    
</StepNavigationTemplate>
</asp:Wizard>
