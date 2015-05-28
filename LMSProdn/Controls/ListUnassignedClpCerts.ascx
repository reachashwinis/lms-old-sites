<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListUnassignedClpCerts.ascx.cs" Inherits="Controls_ListUnassignedClpCerts" %>

<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />

<table>
<tr><td style="width: 375px">
    <asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Unassigned ClearPass certificates"
        Width="447px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
 <td align="left" style="width: 119px">
View: <asp:DropDownList ID="ddlCertVersion" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertVersion_SelectedIndexChanged" Width="73px">
<asp:ListItem Value="5.0.X.X">5.0.X.X</asp:ListItem>
<asp:ListItem Selected="True" Value="6.X.X.X">6.X.X.X</asp:ListItem>
</asp:DropDownList>
</td>
<td align="left" style="width: 194px">
View&nbsp;:&nbsp;<asp:DropDownList ID="ddlCertType" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertType_SelectedIndexChanged">
<asp:ListItem Selected="True" Value="ALL_CERTS">All Certifcates</asp:ListItem>
<asp:ListItem Value="SUB_CERTS">Subscription Certificates</asp:ListItem>
<asp:ListItem Value="PERM_CERTS">Permanent Certificates</asp:ListItem>
</asp:DropDownList>
</td>
<td colspan="2" align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label></asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="Server" MaxLength="75" Columns="100" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="dynamic" ValidationGroup="btnGo"></asp:RequiredFieldValidator></asp:Panel>
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
<tr style="width:100%">
<td class="paging" align="left" style="width: 449px">Total Records:&nbsp;<asp:label id="totalRecords" runat="server"></asp:label>&nbsp;|&nbsp;Page:&nbsp;<asp:label id="currentPage" runat="server"></asp:label>&nbsp;
of&nbsp;<asp:label id="totalPages" runat="server"></asp:label>&nbsp;&nbsp;<asp:LinkButton  ID="lnkClearSort" runat="server" Text="Clear Sort"  OnClick="lnkClearSort_Click"></asp:LinkButton>
</td>
<td class="paging" align="right">
<asp:imagebutton id="btnFirst" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton   ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0" Text="first"></asp:LinkButton> |
<asp:imagebutton id="btnPrev" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton   ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev" Text="previous"></asp:LinkButton> | <asp:LinkButton  ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next" Text="next"></asp:LinkButton>
<asp:imagebutton id="btnNext" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navNext.gif" CommandArgument="next" AlternateText="Next page" EnableViewState="false"></asp:imagebutton>
| <asp:LinkButton  ID="lnlLast" runat="server" OnClick="linkButton_Click" CommandArgument="last" Text="last"></asp:LinkButton>
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" /> | 
<asp:LinkButton  ID="lnkDload" runat="server" OnClick="lnkDload_OnClick" CommandArgument="all" Text="Download Text"></asp:LinkButton> | 
<asp:LinkButton  ID="lnkDloadEx" runat="server" OnClick="lnkDloadEx_OnClick" CommandArgument="allEx" Text="Download Excel"></asp:LinkButton>
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
<tr style="width:100%">
<td colspan="2" align="left" style="height: 133px">
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
        EmptyDataText="No unassigned certificates found"
        EmptyDataRowStyle-CssClass="AlternatingRowStyle"
         >
            <Columns>
                <asp:BoundField DataField="Part_Id" HeaderText="License Part Number" SortExpression="Part_Id"  />
                <asp:BoundField DataField="Part_Desc" HeaderText="Description" SortExpression="Part_Desc"  />
                <asp:BoundField DataField="Serial_Number" HeaderText="Certificate ID" SortExpression="Serial_Number"  />
                             
            </Columns>
        </asp:GridView>
        </td>
        </tr>
        </table>
