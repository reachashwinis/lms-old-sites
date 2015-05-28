<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DecodeCert.ascx.cs" Inherits="Controls_DecodeCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border=0><tr><td>
<asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Decode certificate/Activation key"
    Width="382px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr></table>
<asp:Panel ID="PnlDecodeCert" runat="server" Height="144px" Width="582px">
    <table>
    <tr><td>
        &nbsp;<asp:RadioButtonList ID="RdBtnLst1" runat="server" Width="284px" AutoPostBack="True">
            <asp:ListItem Value="Certificate ID" Selected="True">Certificate ID</asp:ListItem>
            <asp:ListItem Value="LSerialNo">Certificate Serial Number</asp:ListItem>
            <asp:ListItem Value="ActivationKey">Activation Key</asp:ListItem>
        </asp:RadioButtonList></td></tr>
    <tr></tr>
    <tr><td style="width: 578px; height: 8px">
    <asp:Label ID="lblCert" runat="server" Height="25px" Text="Certificate ID:" Width="113px"></asp:Label></td></tr>
    <tr><td style="width: 578px">
    <asp:TextBox ID="TxtBoxCertId" runat="server" Width="550px" MaxLength="150"></asp:TextBox></td>
    <td style="width: 1116px">
    <asp:RequiredFieldValidator ID="ReqdValid1" runat="server" 
     ErrorMessage="Please Enter Certificate ID/Activation Code" ControlToValidate="TxtBoxCertId" ValidationGroup="WizStp" Width="303px" ></asp:RequiredFieldValidator></td></tr>
    <tr></tr><td><asp:Label ID="LblError" runat="server" ForeColor="Red" Visible="False" Height="21px"></asp:Label></td>
    <tr><td style="width: 578px; height: 31px;"><asp:Button ID="BtnDecode" runat="server" Text="Decode Certificate" Width="145px" Height="27px" ValidationGroup="WizStp" OnClick="Button1_Click" /></td></tr>
    <tr><td style="width: 578px"><asp:Label ID="LblPartId" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Green" Visible="False" Height="22px"></asp:Label></td></tr>
    <tr><td style="width: 578px"><asp:Label ID="Lbloutput" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Navy" Height="22px">Output from Generation Site:</asp:Label></td></tr>
    <tr><td style="width: 578px"><asp:Label ID="LblPartNo" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Green" Visible="False" Height="22px"></asp:Label></td></tr>
    <tr><td style="height: 20px">
        <asp:Label ID="LblLNo" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Green" Visible="False"></asp:Label></td></tr>
    <tr><td colspan="2"><asp:Label ID="LblCreator" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Green" Visible="False"></asp:Label></td></tr>
    <tr><td>
        <asp:Label ID="LblCreatedOn" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Green" Visible="False"></asp:Label></td></tr>
    </table>
    
</asp:Panel>
