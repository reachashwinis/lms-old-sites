<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Controls_Menu" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table width="100%" style="background-color:white;color:Gray">
<tr>
<td>
<asp:TreeView ID="tvMenu" runat="server" ShowExpandCollapse="false" NodeStyle-VerticalPadding="2" ExpandDepth="FullyExpand" ShowCheckBoxes="None"  ShowLines="false" >

  </asp:TreeView>
  </td>
</tr>
<tr>
<td>
<asp:Panel ID="pnlSearchbasic" Visible="false" DefaultButton="lnkbtnBasicSearch" runat="server">
<asp:TextBox ID="txtSearch" runat="server" CssClass="txtSearch" Width="80%">Search</asp:TextBox>
<asp:ImageButton Visible="true" ID="lnkbtnBasicSearch" runat="server" ToolTip="Search" OnClick="lnkbtnFetch_Click" ImageUrl="~/Images/searchLens.gif" CausesValidation="false" style="vertical-align:bottom;height:19px;width:19px" ValidationGroup="btnGo" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="Server" ControlToValidate="txtSearch" Display="Dynamic" ErrorMessage="Search criteria is empty!" EnableClientScript="true" ValidationGroup="btnGo"></asp:RequiredFieldValidator>
</asp:Panel>
</td>
</tr>
<tr>
<td>
<asp:Panel ID="pnlImpUser" runat="server">
<asp:LinkButton  ID="lnkImpLogOff" runat="server" CssClass="txtSearch" OnClick="lnkImpLogOff_OnClick"></asp:LinkButton>
</asp:Panel>
</td>
</tr>
 </table>
 
 
 <!--
<asp:Menu ID="nmMenu" runat="server" Orientation="Horizontal" DynamicEnableDefaultPopOutImage="false" StaticEnableDefaultPopOutImage="false" StaticBottomSeparatorImageUrl="" PathSeparator="" 
DynamicHoverStyle-CssClass="ActiveMenuItem-Hover" DynamicMenuItemStyle-CssClass="ActiveMenuItem-Hover" StaticMenuItemStyle-CssClass="ActiveMenuItem" StaticHoverStyle-CssClass="ActiveMenuItem-Hover"
StaticHoverStyle-ForeColor="White" StaticMenuItemStyle-ForeColor="white" DynamicMenuItemStyle-ForeColor="white"
StaticMenuItemStyle-ItemSpacing="1px" StaticMenuItemStyle-HorizontalPadding="10px" DynamicMenuItemStyle-HorizontalPadding="10px"

>
</asp:Menu>-->

<!--
<body link="#ffffff" vlink="#ffffff">
</body> -->

