<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyAmigopod.ascx.cs" Inherits="Controls_MyAmigopod" %>
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
                Text="My ClearPass Subscriptions" Width="308px" Font-Italic="True" Font-Size="Small"></asp:Label>
               
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
        <td class="paging" align="left" style="height: 15px">
            Total Records:&nbsp;<asp:Label ID="totalRecords" runat="server"></asp:Label>&nbsp;|&nbsp;Page:&nbsp;<asp:Label
                ID="currentPage" runat="server"></asp:Label>&nbsp; of&nbsp;<asp:Label ID="totalPages"
                    runat="server"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkClearSort" runat="server"
                        Text="Clear Sort" OnClick="lnkClearSort_Click"></asp:LinkButton>
        </td>
        <td class="paging" align="right" style="height: 15px">
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
                AutoGenerateColumns="False" CellSpacing="1" CellPadding="1" BorderWidth="0px" PageIndex="1" AllowPaging="false" OnRowDataBound="grdSubscription_RowDataBound" BorderStyle="None" GridLines="None" AllowSorting="true" OnSorting="grdSubscription_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Upgrade Details">
                        <ItemTemplate>
                            <a href="javascript:switchViews('div<%# Eval("subscription") %>', 'one','img<%# Eval("subscription") %>');"><img id="img<%#Eval("subscription") %>" src="../Images/expansion.png" alt="Details" /> </a>
                            <%--<asp:LinkButton ID="lnkBtnDetails" runat="server" Text="+" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "subscription") %>' OnCommand="GetDetails_OnCommand"/>--%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Part ID" SortExpression="part_id">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnDetails" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "part_id") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "subscription") %>' OnCommand="GetDetails_OnCommand" ToolTip="Click here for details"/>
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="part_desc" HeaderText="Part Description" ItemStyle-Width="20%" SortExpression="part_desc"/>
                    <asp:BoundField DataField="CertId" HeaderText="Certificate ID" ItemStyle-Width="10%" SortExpression="CertId"/>
                    <asp:BoundField DataField="subscription" HeaderText="Subscription ID" ItemStyle-Width="20%" SortExpression="subscription"/>
                    <asp:BoundField DataField="ActivatedBy" HeaderText="Activated By" ItemStyle-Width="15%" SortExpression="ActivatedBy"/>
                    <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" ItemStyle-Width="10%" SortExpression="ActivatedOn"/>
                    <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" ItemStyle-Width="10%" SortExpression="ExpiryDate"/>
                    <asp:TemplateField >
                        <ItemTemplate>
                                <tr>
                                    <td colspan="8" align="left">
                                        <div id="div<%# Eval("subscription") %>" style="display: none; position: relative;
                                            left: 0px;">
                                            <asp:GridView ID="grdSubscriptionDetails" runat="server" EmptyDataText="No details found" AutoGenerateColumns="False" Width="100%"
                                            CellSpacing="1" CellPadding="1" BorderWidth="0px" BorderStyle="None" GridLines="None" RowStyle-BackColor="lightgray">
                                                <Columns>
                                                    <asp:BoundField DataField="" ItemStyle-Width="4.9%" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="Upgrade_Part" ItemStyle-Width="9.9%"/>
                                                    <asp:BoundField DataField="part_desc" ItemStyle-Width="20%"/>
                                                    <asp:BoundField DataField="Certificate_Id" ItemStyle-Width="10%"/>
                                                    <asp:BoundField DataField="subscription_key" ItemStyle-Width="20%"/>
                                                    <asp:BoundField DataField="Activated_By" ItemStyle-Width="15%"/>
                                                    <asp:BoundField DataField="Activated_On" ItemStyle-Width="10%"/>
                                                    <asp:BoundField DataField="" ItemStyle-Width="10%"/>
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
    <tr style="height:50px">
        <td></td>
    </tr>
    <tr >
        <td></td>
    </tr>
    <tr>
        <td>* Indicates HA</td>
    </tr>
</table>

