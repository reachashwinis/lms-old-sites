<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConfigData.ascx.cs" Inherits="Controls_ConfigData" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table width="100%">
<tr>
<td colspan="3">
<asp:Label ID="lbl1" runat="server" CssClass="lblHeader" Text="Config data table" Font-Bold="True" ForeColor="#0000C0" Font-Italic="True" Font-Size="Small"></asp:Label>
</td>
</tr>
<tr></tr>
<tr>
<td colspan="3">
Please fill out the form below to add the MAC address mapping &nbsp;to the LMS database. 
</td>
</tr>
<tr>
<td>
  Serial Number of MMS Appliance
</td>
<td>
<asp:TextBox ID="txtSNum" runat="server" CssClass="txt" MaxLength="15" Columns="15"></asp:TextBox>
</td>
<td>
<asp:RequiredFieldValidator ID="rfvSNum" runat="server" ControlToValidate="txtSNum" ErrorMessage="Serial Number is Mandatory" Display="dynamic" CssClass="lblError" ValidationGroup="AddMac"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td> MAC Address</td>
<td><asp:TextBox ID="txtMacAddr" runat="server" CssClass="txt" Columns="18" MaxLength="18"></asp:TextBox></td>
<td>
<asp:RequiredFieldValidator ID="rfvMaccAddr" runat="server" ControlToValidate="txtMacAddr" ErrorMessage="MAC Address is Mandatory" Display="dynamic" CssClass="lblError" ValidationGroup="AddMac"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="revMacAddr" runat="server" ControlToValidate="txtMacAddr" ValidationExpression="^[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]:[0-9a-fA-F][0-9a-fA-F]$" ValidationGroup="AddMac"  ErrorMessage="MAC Address must follow the format xx:xx:xx:xx:xx:xx where x is a hexadecimal character" Display="dynamic" CssClass="lblError"></asp:RegularExpressionValidator>
</td>
</tr>
<tr>
<td colspan="3">
<asp:Button ID="btnAdd" runat="server" Text="Add it" CssClass="btn" ValidationGroup="AddMac" OnClick="btnAdd_OnClick" />
</td>
</tr>
<tr>
<td colspan="3">
<asp:Label ID="lblErr" CssClass="lblError" runat="server"></asp:Label>
</td>
</tr>
</table>
<br />
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
<td align="right" colspan="2">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label></asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="Server" MaxLength="75" Columns="30" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="dynamic" ValidationGroup="btnGo" CssClass="lblError"></asp:RequiredFieldValidator></asp:Panel>

</td>
</tr>
<tr style="width:100%">
<td class="paging" align="left">Total Records:&nbsp;<asp:label id="totalRecords" runat="server"></asp:label>&nbsp;|&nbsp;Page:&nbsp;<asp:label id="currentPage" runat="server"></asp:label>&nbsp;
of&nbsp;<asp:label id="totalPages" runat="server"></asp:label>&nbsp;&nbsp;<asp:LinkButton  ID="lnkClearSort" runat="server" Text="Clear Sort"  OnClick="lnkClearSort_Click"></asp:LinkButton>
</td>
<td class="paging" align="right">
<asp:imagebutton id="btnFirst" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton   ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0" Text="first"></asp:LinkButton> |
<asp:imagebutton id="btnPrev" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton   ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev" Text="previous"></asp:LinkButton> | <asp:LinkButton  ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next" Text="next"></asp:LinkButton>
<asp:imagebutton id="btnNext" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navNext.gif" CommandArgument="next" AlternateText="Next page" EnableViewState="false"></asp:imagebutton>
| <asp:LinkButton  ID="lnlLast" runat="server" OnClick="linkButton_Click" CommandArgument="last" Text="last"></asp:LinkButton>
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" />
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
<tr style="width:100%">
<td colspan="2" align="left">
        <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0"
         HeaderStyle-CssClass="HeaderStyle"
         HeaderStyle-HorizontalAlign="Center"
        FooterStyle-CssClass="FooterStyle"
        RowStyle-CssClass="RowStyle"
        AlternatingRowStyle-CssClass="AlternatingRowStyle"
        AllowSorting ="true"  
        PageSize="30" AllowPaging="false" 
        PagerSettings-Visible="false" 
        OnSorting="gvPR_Sort" OnRowDataBound="ItemCellsUpdate" 
        EmptyDataText="No data records to display"
        EmptyDataRowStyle-CssClass="AlternatingRowStyle"
         >
       
           <Columns>
           <asp:BoundField DataField="add_ts" HeaderText="Added" ReadOnly="True" SortExpression="add_ts" ItemStyle-HorizontalAlign="Center" />
           <asp:BoundField DataField="exp_serial" HeaderText="Serial Number" SortExpression="exp_serial" ItemStyle-HorizontalAlign="Center" />
          <asp:BoundField DataField="value" HeaderText="MAC Address" SortExpression="value" ItemStyle-HorizontalAlign="Center" />
            </Columns>

        </asp:GridView>
        </td>
        </tr>
        </table>