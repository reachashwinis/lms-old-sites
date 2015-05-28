<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConsolidateAirwaveCert.ascx.cs" Inherits="Controls_ConsolidateAirwaveCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border = "0">
<tr><td style="width: 311px; height: 21px">
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Consolidate AirWave certificates"
    Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
    </table>
        <asp:Wizard ID="WizConsolidate" runat="server" ActiveStepIndex="0" Width="100%" DisplayCancelButton="false" DisplaySideBar="False" Height="138px" StartNextButtonText="" StartNextButtonType="Link" FinishCompleteButtonText="" FinishCompleteButtonType="Link" FinishPreviousButtonText="" FinishPreviousButtonType="Link" >
            <WizardSteps>
                <asp:WizardStep ID="WizardStep1" StepType="Start" Title="Enter Certificate ID" runat="server" AllowReturn="False">
                            <table width="100%">
                            <tr>
                            <td colspan="2" style="height: 29px; width:5%;"> Certificate ID:
                            </td>
                            <td style="color:Red; width:1%; height: 29px;">*</td>
                            <td style="height: 29px; width: 84%;"> <asp:TextBox ID="TxtCertId1" runat="server" Width="222px" CssClass="txt"></asp:TextBox></td>
                            </tr>
                            <tr>
                             <td colspan="2" style="height: 29px; width: 5%;"> Certificate ID:
                            </td>
                            <td style="color:Red; width:1%; height: 29px;">*</td>
                            <td style="height: 29px; width: 84%;"> <asp:TextBox ID="TxtCertId2" runat="server" Width="225px" CssClass="txt"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>  <td colspan="2" style="height: 30px; width: 5%;"> Organization:
                            </td>
                        <td style="color:Red; width:1%; height: 30px;">*</td>
                        <td style="height: 30px; width: 84%;">
                            <asp:TextBox ID="Txtorg" runat="server" Width="314px"></asp:TextBox></td></tr>
                        <tr><td colspan="4" style="width: 5%">
                            <asp:CustomValidator ID="CustomValid" runat="server" Display="Dynamic" CssClass="lblError" ValidationGroup="Validgrp" ErrorMessage="CustomValidator" OnServerValidate="CustomValid_ServerValidate" Width="367px"></asp:CustomValidator>
                        </td></tr>
                        <tr><td colspan="4" style="width: 5%">
                            <asp:Label ID="LblErr" runat="server" CssClass="lblError" Width="370px"></asp:Label>
                        </td></tr>
                            <tr><td style="height: 40px; width: 23px">
                            <asp:Button ID="BtnNext" runat="server" ValidationGroup="Validgrp" Text="Next&gt;&gt;" CssClass="btn" OnClick="BtnNext_Click" />
                            </td></tr></table>
                            
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep2" runat="server" StepType="Finish" Title="Consolidate License key" AllowReturn="False">
                <table width="100%"><tr><td style="height: 148px">
                    <asp:Literal ID="LiteralLickey" runat="server"></asp:Literal></td></tr></table>
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>

 
