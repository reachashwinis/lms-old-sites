<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyAirwaveCert.ascx.cs" Inherits="Controls_MyAirwaveCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border = "0">
<tr><td style="width: 311px; height: 21px">
<asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="My AirWave certificates"
 Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label></td></tr>
 </table>
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
<td align="left">
<asp:DropDownList ID="ddlCertType" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertType_SelectedIndexChanged" Visible="false">
<asp:ListItem Value="ALL_CERTS">All Certifcates</asp:ListItem>
<asp:ListItem Value="EVAL_CERTS">Eval Certificates</asp:ListItem>
<asp:ListItem Selected="True" Value="PERM_CERTS">Permanent Certificates</asp:ListItem>
</asp:DropDownList>
</td>
<td align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label></asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="Server" MaxLength="250" Columns="100" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" /></asp:Panel>

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
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" />|
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
            <asp:BoundField DataField="PartId" HeaderText="Part Number" SortExpression="PartId" HeaderStyle-Wrap="false" />
            <asp:BoundField DataField="PartDesc" HeaderText="Description" SortExpression="PartDesc" HeaderStyle-Wrap="false" />
            <asp:BoundField DataField="certId" HeaderText="Certificate ID" SortExpression="certid" HeaderStyle-Wrap="false" />
            <asp:BoundField DataField="IPAddress" HeaderText="IP Address" SortExpression="IPAddress" HeaderStyle-Wrap="false" />
            <asp:BoundField DataField="organization" HeaderText="Organization" SortExpression="organization" HeaderStyle-Wrap="false" />
            <asp:BoundField DataField="ActCode" HeaderText="Activation Key" SortExpression="ActCode" HtmlEncode="false" HeaderStyle-Wrap="false" />     
            <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" SortExpression="ActivatedOn" HeaderStyle-Wrap="false" />                              </Columns>
        </asp:GridView>
        </td>
        </tr>
        </table>
        
    
    
