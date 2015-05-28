<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AllCerts.ascx.cs" Inherits="Controls_AllCerts" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0"><tr><td>
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="All certificates"
    Width="344px" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr></table>

<table width="100%" cellpadding="0" cellspacing="0">
<tr>

<td align="left" style="width: 194px">
View&nbsp;:&nbsp;<asp:DropDownList ID="ddlCertType" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertType_SelectedIndexChanged">
<asp:ListItem Value="ALL_CERTS">All Certifcates</asp:ListItem>
<asp:ListItem Value="EVAL_CERTS">Eval Certificates</asp:ListItem>
<asp:ListItem Selected="True" Value="PERM_CERTS">Permanent Certificates</asp:ListItem>
</asp:DropDownList>
</td>
 <td align="left" style="width: 79px">
<asp:DropDownList ID="ddlCertVersion" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertVersion_SelectedIndexChanged" Width="73px">
<asp:ListItem Value="PRE">Pre 5.0</asp:ListItem>
<asp:ListItem Selected="True" Value="POST">Post 5.0</asp:ListItem>
</asp:DropDownList>
</td>
<td colspan="2" align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label></asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="Server" MaxLength="75" Columns="100" CssClass="txt" Width="401px"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="dynamic" ValidationGroup="btnGo"></asp:RequiredFieldValidator></asp:Panel>
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
<tr style="width:100%">
<td class="paging" align="left" style="width: 520px">Total Records:&nbsp;<asp:label id="totalRecords" runat="server"></asp:label>&nbsp;|&nbsp;Page:&nbsp;<asp:label id="currentPage" runat="server"></asp:label>&nbsp;
of&nbsp;<asp:label id="totalPages" runat="server"></asp:label>&nbsp;&nbsp;<asp:LinkButton  ID="lnkClearSort" runat="server" Text="Clear Sort"  OnClick="lnkClearSort_Click"></asp:LinkButton>
</td>
<td class="paging" align="right" style="width: 535px">
<asp:imagebutton id="btnFirst" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton    ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0" Text="first"></asp:LinkButton> |
<asp:imagebutton id="btnPrev" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton    ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev" Text="previous"></asp:LinkButton> | <asp:LinkButton   ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next" Text="next"></asp:LinkButton>
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
        EmptyDataText="No Certificates found"
        EmptyDataRowStyle-CssClass="AlternatingRowStyle">
            <Columns>
            <asp:BoundField DataField="PartNo" HeaderText="Part Number" SortExpression="PartNo" HeaderStyle-Wrap="false" />
                <asp:BoundField DataField="PartDesc" HeaderText="Description" SortExpression="PartDesc" HeaderStyle-Wrap="false" />
                <asp:BoundField DataField="SerialNum" HeaderText="Certificate ID" SortExpression="SerialNum" HeaderStyle-Wrap="false" ItemStyle-Width="20%" />
              <asp:BoundField DataField="LSerialNum" HeaderText="Certificate Serial Number" SortExpression="LSerialNum" HeaderStyle-Wrap="false" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="SONo" HeaderText="SO No" SortExpression="SONo" HeaderStyle-Wrap="false" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="CustPO" HeaderText="Cust PO No" SortExpression="CustPO" HeaderStyle-Wrap="false" ItemStyle-Width="20%" />      
                            <asp:BoundField DataField="EndUserPO" HeaderText="Reseller/End User PO" SortExpression="EndUserPO" HeaderStyle-Wrap="false" ItemStyle-Width="20%" />                                                    
                <asp:BoundField DataField="OwnedBy" HeaderText="Owned By" SortExpression="OwnedBy" HeaderStyle-Wrap="false" />
                <asp:BoundField DataField="ActCode" HeaderText="Activation Key" SortExpression="ActCode" HeaderStyle-Wrap="false"  ItemStyle-HorizontalAlign="Right" HtmlEncode="false"  />
                <asp:BoundField DataField="Fru" HeaderText="System Serial Number"  SortExpression="Fru" HeaderStyle-Wrap="false" />
                <asp:BoundField DataField="location" HeaderText="System Location"  SortExpression="location" HeaderStyle-Wrap="false" />
                <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" ReadOnly="True" SortExpression="ActivatedOn" HeaderStyle-Wrap="false"  />
                <asp:BoundField DataField="ActivatedBy" HeaderText="Activated By" ReadOnly="True" SortExpression="ActivatedBy" HeaderStyle-Wrap="false"  />                              
            </Columns>
        </asp:GridView>
        </td>
        </tr>
        </table>