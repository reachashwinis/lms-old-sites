<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateQALic.ascx.cs" Inherits="Controls_GenerateQALic" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="scrptMan1" runat="Server" >
</asp:ScriptManager>
<asp:Panel ID="pnlQALic" runat="server" DefaultButton="btnGenQALic" Width="1222px">
<table width="100%">
<tr>
<td colspan="3">
 <asp:Label ID="lbl1" runat="server" CssClass="lblHeader" Text="Generate a QA license" Font-Bold="True" ForeColor="#0000C0" Font-Italic="True" Font-Size="Small"></asp:Label>
</td>
</tr>

 <!-- <asp:UpdatePanel ID="upPartSN" runat="server" EnableViewState="true" UpdateMode="Conditional" >
<ContentTemplate> -->

<tr>
<td>Version</td>
<td>
<asp:DropDownList ID="ddlVersion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVersion_SelectedIndexChanged" CssClass="ddlist">
    <asp:ListItem Selected="True" Value="3">3.0</asp:ListItem>
    <asp:ListItem Value="5">5.0</asp:ListItem>
</asp:DropDownList>
</td></tr>

<tr>
<td>System Part Number</td>
<td>
 <asp:DropDownList ID="ddlPartId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPartId_SelectedIndexChanged" CssClass="ddlist"></asp:DropDownList>
</td>
  <td>
  <asp:RequiredFieldValidator ID="rfvSysPartId" runat="server" ControlToValidate="ddlPartId" ErrorMessage="System Part Number is mandatory" Display="dynamic" ValidationGroup="GenerateQALIC"></asp:RequiredFieldValidator>
  </td>
</tr>
<tr>
 <td style="width:10%">
 Serial Number</td>
 <td style="width:45%">
  <asp:TextBox ID="txtSNum" runat="server" Columns="50" MaxLength="50" CssClass="txt"></asp:TextBox></td>
  <td style="width:45%">
  <asp:RequiredFieldValidator ID="rfvSerialNumber" runat="server" ControlToValidate="txtSNum" ErrorMessage="Serial Number is mandatory" Display="dynamic" ValidationGroup="GenerateQALIC"></asp:RequiredFieldValidator>
  <asp:CustomValidator ID="cvSerialNumber" runat="server" ControlToValidate="txtSNum" Display="dynamic" ValidationGroup="GenerateQALIC" OnServerValidate="cvSerialNumber_OnServerValidate"></asp:CustomValidator>
  </td>
</tr>
<tr>
<td style="height: 40px">License Part Number</td>
  <td style="height: 40px">
      <asp:GridView ID="GrdVwCertParts" runat="server">
      <Columns>
      <asp:TemplateField>
               <ItemTemplate>
                <asp:CheckBox CssClass="btn"  ID="chkCert" runat="server"/>
                </ItemTemplate>
      </asp:TemplateField>
      <asp:BoundField DataField="CertPartId" />
      </Columns>
      </asp:GridView>
</td>
<td style="height: 40px">
  <asp:RequiredFieldValidator ID="rfvCertPart" runat="server" ControlToValidate="ddlCertPart" ErrorMessage="License Part Number is mandatory" Display="dynamic" ValidationGroup="GenerateQALIC"></asp:RequiredFieldValidator>
  </td>
</tr>

<!-- </ContentTemplate>
</asp:UpdatePanel> -->

<tr>
<td>Customer Code</td>
<td>
 <asp:TextBox ID="txtCustCode" runat="server" ReadOnly="True" CssClass="txt" Text="ARU0QA"></asp:TextBox></td>
<td>&nbsp;</td> 
 </tr>
<tr>
<td colspan="2" style="height: 40px">
<asp:Button ID="btnGenQALic" runat="server" CssClass="btn" Text ="Generate QA License" ValidationGroup="GenerateQALIC" OnClick="btnGenQALic_OnClick" />
</td>
<td style="height: 40px">
<asp:CheckBox ID="chkOverride" runat="server" AutoPostBack="false" Text="Override Errors" Visible="False" Width="128px" />
</td>
</tr>
<tr>
<td colspan="3">
 <asp:Label ID="lblSuccess" runat="server" CssClass="lblSuccess" Visible="false"></asp:Label>
</td>
</tr> 
<tr>
<td colspan="3" style="height: 21px">
 <asp:Label ID="lblErr" runat="server" CssClass="lblError" Visible="false"></asp:Label>
</td>
</tr> 
</table>

</asp:Panel>
