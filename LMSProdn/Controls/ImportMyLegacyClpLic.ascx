<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImportMyLegacyClpLic.ascx.cs" Inherits="Controls_ImportMyLegacyClpLic" %>
<table width="100%">
<tr><td style="width: 239px">
    <asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Import ClearPass license"
        Width="239px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
</table>
<asp:Wizard ID="WizImportlegacySub" runat="server" Height="100px" Width="95%" ActiveStepIndex="0" Font-Bold="True" DisplaySideBar="False">
<StepStyle Wrap="True"></StepStyle>
<StartNavigationTemplate>    
</StartNavigationTemplate>
<WizardSteps>
<asp:WizardStep runat="server" AllowReturn="False" ID="SubStp1" StepType="Start" Title="Enter Subscription ID">
<table border="0" width="100%">
<tr>
<td><asp:Label runat="server" Text="Sales Order:" Width="175px" ID="LblSoId"></asp:Label></td>
<td style="color:Red; width:2%"><b> * </b> </td>
<td><asp:TextBox ID="TxtSoId1" runat="server"></asp:TextBox></td>
 <td style="width: 177px"><asp:RequiredFieldValidator runat="server" ControlToValidate="TxtSoId1" ErrorMessage="Please Enter Sales Order" ValidationGroup="WizStp" ID="ReqdValidSo1" CssClass="lblError"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td><asp:Label runat="server" Text="Subscription ID:" Width="175px" ID="LblSubKey"></asp:Label></td>
<td style="color:Red; width:2%"><b> * </b> </td>
<td><asp:TextBox runat="server" Width="448px" ID="TxtSubKey1" CssClass="txt"></asp:TextBox></td>
 <td style="width: 177px"><asp:RequiredFieldValidator runat="server" ControlToValidate="TxtSubKey1" ErrorMessage="Please Enter Subscription ID" ValidationGroup="WizStp" ID="ReqdValid1" CssClass="lblError"></asp:RequiredFieldValidator></td>
 </tr>
 <tr>
 <td style="width: 490px" colspan="4">
 <asp:Button runat="server" Text="Next &gt;&gt;" ValidationGroup="WizStp" ID="BtnNext1" CssClass="btn" OnClick="BtnNext1_Click"></asp:Button></td>
 </tr>
 <tr>
 <td style="width: 490px" colspan="4"><asp:Label runat="server" Font-Bold="True" ForeColor="Red" Width="448px" CssClass="lblError" ID="LblError1" Visible="False"></asp:Label></td>
 </tr>
</table>
 </asp:WizardStep>
<asp:WizardStep runat="server" ID="SubStp2" StepType="Step" Title="Display Subscription ID details">
 <asp:Repeater runat="server" ID="rptSub2">
 <HeaderTemplate>
     <table style="width: 100%">  
    <tr><td><br /></td></tr>    
    <tr><td>Following are the details of your Subscription ID. Please hit <i>Import</i> button to continue.</td></tr>   
    </HeaderTemplate> 
    <ItemTemplate>
    <table style="width: 100%"> 
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
</asp:Repeater>
<asp:Label ID="lblNoRecords1" runat="server" Text="No ClearPass licenses are associated to this Subscription ID." Visible="False" Font-Bold="True"></asp:Label>
 <asp:Repeater runat="server" ID="rptSub3" OnPreRender="rptSub3_PreRender" OnItemDataBound="rptSub3_ItemDataBound">
  <HeaderTemplate>
    <table style="width: 100%" border="1">  
            <td style="color:Red; font-style:italic" colspan="6">* The rows in Red indicates that key is already imported to LMS.</td>
    <tr style="width: 100%"> 
    <td align="left" style="width:15%"><b>Part No</b></td>
    <td align="left" style="width:30%"><b>Description </b></td> 
    <td align="left" style="width:30%"><b>Activation Key </b></td> 
    <td align="left" style="width:5%"><b>Version </b></td>     
    <td align="left" style="width:10%"><b>Activated On </b></td> 
    <td align="left" style="width:10%"><b>Expire On </b></td>     
    </tr>
    </table>
 </HeaderTemplate>
 <ItemTemplate>
      <table style="width: 100%" border="1" runat="server" id="T1">
      <tr style="width: 100%" runat="server" id="TR"> 
      <td style="width:15%" runat="server" id="TD1"><%# DataBinder.Eval(Container.DataItem, "part_id")%></td>
      <td style="width:30%" runat="server" id="TD2"> <%# DataBinder.Eval(Container.DataItem, "part_desc")%></td>
      <td style="width:30%" runat="server" id="TD3"> <%# DataBinder.Eval(Container.DataItem, "LicenseKey")%></td>
      <td style="width:5%" runat="server" id="TD4"> <%# DataBinder.Eval(Container.DataItem, "version")%></td>      
      <td style="width:10%" runat="server" id="TD5"> <%# DataBinder.Eval(Container.DataItem, "create_time")%></td>      
      <td style="width:10%" runat="server" id="TD6"> <%# DataBinder.Eval(Container.DataItem, "expire_time")%></td>      
      </tr>  
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
 </asp:WizardStep>
<asp:WizardStep runat="server" AllowReturn="False" ID="SubStp3" StepType="Finish" Title="Import result">
<table>
<tr><td>
Congratulations!.You have successfully Imported your Subscription ID to LMS. Following are the details. You will 
receive email notification shortly.
</td></tr>
</table>
 <asp:Repeater runat="server" ID="rptSub4" OnPreRender="rptSub5_PreRender">
 <HeaderTemplate>         
 </HeaderTemplate> 
    <ItemTemplate>
    <table style="width: 100%" border="1"> 
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
</asp:Repeater>
    <asp:Label ID="lblNoRecords2" runat="server" Text="No ClearPass licenses are associated to this Subscription ID." Visible="False"></asp:Label>
<asp:Repeater runat="server" ID="rptSub5">
  <HeaderTemplate>
    <table style="width: 100%" border="1">  
    <tr style="width: 100%"> 
    <td align="left" style="width:15%"><b>Part No:</b></td>
    <td align="left" style="width:30%"><b>Description: </b></td> 
    <td align="left" style="width:30%"><b>Activation Key: </b></td> 
    <td align="left" style="width:5%"><b>Version: </b></td>     
    <td align="left" style="width:10%"><b>Activated On: </b></td> 
    <td align="left" style="width:10%"><b>Expire On: </b></td>     
    </table>
 </HeaderTemplate>
 <ItemTemplate>
      <table border="1">
      <tr>     
      <td style="width:15%" ><%# DataBinder.Eval(Container.DataItem, "part_id")%></td>
      <td style="width:30%"> <%# DataBinder.Eval(Container.DataItem, "part_desc")%></td>
      <td style="width:30%"> <%# DataBinder.Eval(Container.DataItem, "LicenseKey")%></td>
      <td style="width:5%"> <%# DataBinder.Eval(Container.DataItem, "version")%></td>      
      <td style="width:10%"> <%# DataBinder.Eval(Container.DataItem, "create_time")%></td>      
      <td style="width:10%"> <%# DataBinder.Eval(Container.DataItem, "expire_time")%></td>       
      </tr>  
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

