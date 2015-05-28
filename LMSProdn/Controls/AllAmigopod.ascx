<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AllAmigopod.ascx.cs" Inherits="Controls_AllAmigopod" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
function switchViews(obj, row,imgObj) {
    var div = document.getElementById(obj);
    var img = document.getElementById(imgObj);
    if (div.style.display == "none") {
            div.style.display = "inline";
            img.src="../Images/collation.png";
        } 
    else {
            div.style.display = "none";
            img.src="../Images/expansion.png";
        }
}
</script>

<table border="0">
    <tr>
        <td style="width: 311px; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0"
                Text="All Subscriptions" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
    </tr>
</table>

<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td align="left">
    <%--        <asp:DropDownList ID="ddlCertType" runat="server" AutoPostBack="True" CssClass="ddlist" 
                OnSelectedIndexChanged="ddlCertType_SelectedIndexChanged" Visible="true">
                <asp:ListItem Value="ALL_CERTS">All Certifcates</asp:ListItem>
                <asp:ListItem Value="EVAL_CERTS">Eval Certificates</asp:ListItem>
                <asp:ListItem Selected="True" Value="PERM_CERTS">Permanent Certificates</asp:ListItem>
            </asp:DropDownList>--%>
            
        </td>
        <td align="right">
            <asp:Panel ID="pnlFilterQuery" runat="server">
                <asp:LinkButton CssClass="btn" ID="lnkRemFilter" runat="server" OnClick="lnkRemfilter_OnClick"
                    Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server"
                        CssClass="btn"></asp:Label></asp:Panel>
            <asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
                <asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;&nbsp;
                <asp:TextBox ID="txtSearch" runat="Server" MaxLength="250" Columns="100" CssClass="txt"></asp:TextBox>
                <asp:ImageButton  ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />&nbsp;
            </asp:Panel>
            &nbsp;
        </td>
    </tr>
    <tr style="width: 100%">
        <td class="paging" align="left">
            Total Records:&nbsp;<asp:Label ID="totalRecords" runat="server"></asp:Label>&nbsp;|&nbsp;Page:&nbsp;<asp:Label
                ID="currentPage" runat="server"></asp:Label>&nbsp; of&nbsp;<asp:Label ID="totalPages"
                    runat="server"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkClearSort" runat="server"
                        Text="Clear Sort" OnClick="lnkClearSort_Click"></asp:LinkButton>
        </td>
        <td class="paging" align="right">
            <asp:ImageButton ID="btnFirst" OnClick="linkButton_Click" runat="server" ImageAlign="AbsMiddle"
                ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page"
                EnableViewState="false"></asp:ImageButton>
            <asp:LinkButton ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0"
                Text="first"></asp:LinkButton>
            |
            <asp:ImageButton ID="btnPrev" OnClick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle"
                ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page"
                EnableViewState="false"></asp:ImageButton>
            <asp:LinkButton ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev"
                Text="previous"></asp:LinkButton>
            |
            <asp:LinkButton ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next"
                Text="next"></asp:LinkButton>
            <asp:ImageButton ID="btnNext" OnClick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle"
                ImageUrl="~/Images/navNext.gif" CommandArgument="next" AlternateText="Next page"
                EnableViewState="false"></asp:ImageButton>
            |
            <asp:LinkButton ID="lnlLast" runat="server" OnClick="linkButton_Click" CommandArgument="last"
                Text="last"></asp:LinkButton>
            <asp:ImageButton ID="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page"
                CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" />|
            <asp:LinkButton  ID="lnkDload" runat="server" OnClick="lnkDload_OnClick" CommandArgument="all" Text="Download Text"></asp:LinkButton> | 
            <asp:LinkButton  ID="lnkDloadEx" runat="server" OnClick="lnkDloadEx_OnClick" CommandArgument="allEx" Text="Download Excel"></asp:LinkButton>                
        </td>
    </tr>
</table>

<table style="width:100%" border="0">
    <tr>
        <td style="width:100%">
            <asp:GridView ID="grdSubscription" runat="server" Width="100%" EmptyDataText="No Records found"
                AutoGenerateColumns="False" CellSpacing="1" CellPadding="1" BorderWidth="0px"
                PageSize="100" PageIndex="1" AllowPaging="false" OnRowDataBound="grdSubscription_RowDataBound" BorderStyle="None" GridLines="None" AllowSorting="true" OnSorting="grdSubscription_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Upgrade Details" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <a href="javascript:switchViews('div<%# Eval("subscription") %>', 'one','img<%# Eval("subscription") %>');"><img id="img<%#Eval("subscription") %>" src="../Images/expansion.png" alt="Details" /> </a>
                            <%--<asp:LinkButton ID="lnkBtnDetails" runat="server" Text="+" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "subscription") %>' OnCommand="GetDetails_OnCommand"/>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Part Number" ItemStyle-Width="10.4%" SortExpression ="part_id">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnDetails" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "part_id") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "subscription") %>' OnCommand="GetDetails_OnCommand" ToolTip="Click here for details"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="part_desc" HeaderText="Part Description" ItemStyle-Width="19.8%" SortExpression="part_desc"/>
                    <asp:BoundField DataField="CertId" HeaderText="Certificate ID" ItemStyle-Width="10.1%" SortExpression="CertId"/>
                    <asp:BoundField DataField="subscription" HeaderText="Subscription" ItemStyle-Width="16%" SortExpression="subscription"/>
                    <asp:BoundField DataField="ActivatedBy" HeaderText="Activated By" ItemStyle-Width="12%" SortExpression="ActivatedBy"/>
                    <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" ItemStyle-Width="10%" SortExpression="ActivatedOn"/>
                    <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" ItemStyle-Width="9%" SortExpression="ExpiryDate"/> 
                    <asp:BoundField DataField="companyName" HeaderText="Organization" ItemStyle-Width="9%" SortExpression="companyName"/> 
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                        </tr>
                                <tr>
                                    <td colspan="9" align="left" style="width:100%">
                                        <div id="div<%# Eval("subscription") %>" style="display: none; position: relative;
                                            left: 0px;">
                                            <asp:GridView ID="grdSubscriptionDetails" runat="server" EmptyDataText="No Upgrade details found" AutoGenerateColumns="False" Width="100%"
                                            CellSpacing="1" CellPadding="1" BorderWidth="0px" BorderStyle="None" GridLines="None" RowStyle-BackColor="lightgray">
                                                <Columns>
                                                    <asp:BoundField DataField="" ItemStyle-Width="3.9%" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true"/>
                                                    <asp:BoundField DataField="Upgrade_Part" ItemStyle-Width="10.2%"/>
                                                    <asp:BoundField DataField="part_desc" ItemStyle-Width="19.1%"/>
                                                    <asp:BoundField DataField="Certificate_Id" ItemStyle-Width="9.8%"/>
                                                    <asp:BoundField DataField="subscription_key" ItemStyle-Width="16%"/>
                                                    <asp:BoundField DataField="Activated_By" ItemStyle-Width="14%"/>
                                                    <asp:BoundField DataField="Activated_On" ItemStyle-Width="10%"/>
                                                    <asp:BoundField DataField="" ItemStyle-Width="9%"/>
                                                    <asp:BoundField DataField="" ItemStyle-Width="9%"/>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
                <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                <RowStyle CssClass="RowStyle" />
            </asp:GridView>
        </td>
    </tr>
</table>
<table>
    <tr style="height:120px">
        <td></td>
    </tr>
    <tr >
        <td></td>
    </tr>
    <tr>
        <td>* Indicates HA</td>
    </tr>
</table>