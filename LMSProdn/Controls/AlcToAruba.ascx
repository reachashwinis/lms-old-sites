<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AlcToAruba.ascx.cs" Inherits="Controls_AlcToAruba" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0"><tr><td>
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Alcatel to Aruba parts"
    Width="365px" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
    </table>
<asp:Wizard ID="WizAlcToAruba" runat="server" Height="252px"
    Width="714px" ActiveStepIndex="0" DisplaySideBar="False">
    <StepStyle Wrap="True" />
    <StartNavigationTemplate>
        
    </StartNavigationTemplate>
    <WizardSteps>
        <asp:WizardStep ID="CertStp1" runat="server" AllowReturn="False" StepType="Start"
            Title="Enter Certificate ID">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 106%">
             <tr><td>
        &nbsp;<asp:RadioButtonList ID="RdBtnLst1" runat="server" Width="271px" AutoPostBack="True">
            <asp:ListItem Selected="True" Value="CertificateId">Certificate ID</asp:ListItem>
            <asp:ListItem Value="serialno">Controller Serial Number</asp:ListItem>
        </asp:RadioButtonList></td></tr>
        <tr><td style="width: 490px">
            <asp:Label ID="LblCertId1" runat="server" Text="Certificate ID:" Width="95px"></asp:Label></td></tr>
            <tr><td style="width: 490px"><asp:TextBox ID="TxtCertId1" runat="server" Width="448px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ReqdValid1" runat="server" 
                ErrorMessage="Please Enter Certificate ID" ControlToValidate="TxtCertId1" ValidationGroup="WizStp" ></asp:RequiredFieldValidator>
            </td></tr>
            <tr><td style="width: 490px"><asp:Button ID="Btnnext1" runat="server" Text="Next &gt;&gt;" ValidationGroup="WizStp" OnClick="Btnnext1_Click" /></td></tr>
            <tr><td style="width: 490px">
            <asp:Label ID="LblError1" runat="server" ForeColor="Red" Visible="False" Width="448px" Font-Bold="True"></asp:Label>
            </td></tr>
            <tr><td style="width: 490px; height: 28px;"></td></tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep ID="CertStp2" runat="server" AllowReturn="False" StepType="Step"
            Title="Display equivalent Alcatel Parts">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 100%">
            <tr>
            <td  colspan="2"style="height: 29px; width: 495px;">
                &nbsp;<asp:Repeater ID="rptCertSNo2" runat="server">
                    <ItemTemplate>
                        <table>
                                 <tr><td align="left"><b>Alcatel Cerificate Part Number:</b></td><td><%# DataBinder.Eval(Container.DataItem, "part_id")%></td></tr>
                                 <tr><td align="left"><b>Alcatel Certificate Part Desc:</b></td><td><%# DataBinder.Eval(Container.DataItem, "part_desc")%></td></tr>
                                 <tr><td align="left"><b>Alcatel Certificate ID: </b></td><td><%# DataBinder.Eval(Container.DataItem, "serial_number")%></td></tr>
                                 <tr><td align="left"><b>Aruba Certificate Part Number: </b></td><td><%# DataBinder.Eval(Container.DataItem, "ArubaPartId")%></td></tr>
                                 <tr><td align="left"><b>Aruba Certificate Part Desc: </b></td><td><%# DataBinder.Eval(Container.DataItem, "ArubaPartDesc")%></td></tr>
                                 <tr><td><%# DataBinder.Eval(Container.DataItem, "Error")%></td></tr>
                                 <tr><td></td></tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>      
            </td></tr> 
                    <tr>
                <td colspan="2">
                   Do you acknowledge that you have read and are willing
                   to abide by the <a href="https://www.licensing.arubanetworks.com/eula.php">End-User Software License Aggrement? </a></td>
                </tr>
                <tr><td style="height: 43px">
                    <asp:RadioButtonList ID="RdBtnLst2" runat="server" Height="34px" Width="74px">
                         <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                    <td>
                       <asp:RequiredFieldValidator ID="ReqdValidYes2" runat="server" ErrorMessage="Acceptance of EULA is mandatory"
                            Width="460px" ControlToValidate="RdBtnLst2" Display="Dynamic" ValidationGroup="ReqdValidYes2"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CvAckyes2" runat="server" Display="Dynamic" ErrorMessage="Please Acknoledge that You will abide by the rules and Regulations of EULA"
                            ValueToCompare="Yes" Width="460px" ControlToValidate="RdBtnLst2"></asp:CompareValidator>
                </td></tr>
                <tr><td colspan="2">
                    <asp:Button ID="BtnActivate2" runat="server" Text="Convert" Width="117px" OnClick="BtnActivate2_Click" />
                </td></tr>
                <tr><td colspan="2">
                    <asp:Label ID="LblError2" runat="server" Width="550px" ForeColor="Red"></asp:Label>
                </td>
                </tr>
            <tr>
            </tr>
            </table> 
            
        </asp:WizardStep>
        <asp:WizardStep ID="CertStp3" runat="server" AllowReturn="False" StepType="Finish"
            Title="Generate Alcatel Certificate">
            <table border="0" cellpadding="5" cellspacing="5" style="width: 106%">
            <tr><td>Congratulations!!.You have successfully Converted to Aruba License.</td></tr>
            <tr><td>
                
                <asp:Repeater ID="rptCertConvert3" runat="server">
                 <ItemTemplate>
    <table>
      <tr><td align="left"><b>Alcatel Certificate ID:</b></td> <td><%# DataBinder.Eval(Container.DataItem, "serial_number")%></td></tr>
      <tr><td align="left"><b>Alcatel Certificate Part Id:</b></td><td><%# DataBinder.Eval(Container.DataItem, "part_id")%></td></tr>
      <tr><td align="left"><b>Alcatel Certificate Part Description: </b></td><td><%# DataBinder.Eval(Container.DataItem, "part_desc")%></td></tr>
      <tr><td align="left"><b>Aruba Certificate ID: </b></td> <td><%# DataBinder.Eval(Container.DataItem, "ArubaSerialNumber")%></td></tr>
      <tr><td align="left"><b>Aruba Part Number:</b></td><td><%# DataBinder.Eval(Container.DataItem, "ArubaPartId")%></td></tr>
      <tr><td align="left"><b>Aruba Part Description: </b></td><td><%# DataBinder.Eval(Container.DataItem, "ArubaPartDesc")%></td></tr>
      <tr><td><%# DataBinder.Eval(Container.DataItem, "Error")%></td></tr>
    </table>                 
  </ItemTemplate>     
                </asp:Repeater>
                <asp:Label ID="LblEmail" runat="server" Width="313px"></asp:Label>
                </td></tr>
           </table>
        </asp:WizardStep>
    </WizardSteps>
    <StepNavigationTemplate>
    </StepNavigationTemplate>
    <FinishNavigationTemplate>
    </FinishNavigationTemplate>
</asp:Wizard>
&nbsp;&nbsp;&nbsp;

