<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReassignAirwaveCert.ascx.cs" Inherits="Controls_ReassignAirwaveCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<%--<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
<table border="0">
    <tr>
        <td>
            <asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Reassign Airwave certificates"
                Width="472px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <!-- <td align="left" style="width: 75px">
            View:
            <br />
            <asp:DropDownList ID="ddlCertVersion" runat="server" AutoPostBack="True" CssClass="ddlist"
                OnSelectedIndexChanged="ddlCertVersion_SelectedIndexChanged" Width="73px">
                <asp:ListItem Value="PRE">Pre 5.0</asp:ListItem>
                <asp:ListItem Selected="True" Value="POST">Post 5.0</asp:ListItem>
            </asp:DropDownList>
        </td> -->
        <td align="left" style="width: 381px">
            Show Certs for Email:
            <br />
            <asp:TextBox ID="txtshowFor" runat="Server" CssClass="txt" Columns="30" Width="297px"></asp:TextBox>
            &nbsp;<asp:Button ID="btnGetCertsFor" runat="server" Text="Get Certs" OnClick="btnGetCertsFor_OnClick"
                CssClass="btn" />
        </td>
        <td align="right">
            <br />
            <asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
                <asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist">
                </asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false"
                    CssClass="ddlist">
                </asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtSearch" runat="Server" MaxLength="75" Columns="40" CssClass="txt"
                    Width="321px"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go"
                        ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo"
                        ImageAlign="AbsBottom" />
                <br />
                <asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch"
                    ErrorMessage="Search text is empty" Display="dynamic" ValidationGroup="btnGo"
                    Width="173px"></asp:RequiredFieldValidator></asp:Panel>
            <asp:Panel ID="pnlFilterQuery" runat="server">
                <asp:LinkButton CssClass="btn" ID="lnkRemFilter" runat="server" OnClick="lnkRemfilter_OnClick"
                    Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server"
                        CssClass="btn"></asp:Label></asp:Panel>
        </td>
    </tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr style="width: 100%">
        <td class="paging" align="left">
            Total Records:&nbsp;<asp:Label ID="totalRecords" runat="server"></asp:Label>&nbsp;|&nbsp;Page:&nbsp;<asp:Label
                ID="currentPage" runat="server"></asp:Label>&nbsp; of&nbsp;<asp:Label ID="totalPages"
                    runat="server"></asp:Label>&nbsp;&nbsp;<asp:LinkButton ID="lnkClearSort" runat="server"
                        Text="Clear Sort" OnClick="lnkClearSort_Click"></asp:LinkButton>
        </td>
        <td class="paging" align="right" style="width: 740px">
            <asp:ImageButton ID="btnFirst" OnClick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle"
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
                CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" />
        </td>
    </tr>
</table>

        <table width="100%" cellpadding="0" cellspacing="0" style="background-color: White">
            <tr style="width: 100%">
                <td colspan="2" align="left">
                    <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="False" GridLines="None"
                        CellSpacing="1" CellPadding="1" Width="100%" BorderWidth="0" HeaderStyle-CssClass="HeaderStyle"
                        HeaderStyle-HorizontalAlign="Center" FooterStyle-CssClass="FooterStyle" RowStyle-CssClass="RowStyle"
                        AlternatingRowStyle-CssClass="AlternatingRowStyle" AllowSorting="true" PageSize="30"
                        AllowPaging="false" PagerSettings-Visible="false" OnSorting="gvPR_Sort" OnRowDataBound="ItemCellsUpdate"
                        EmptyDataText="No Certificates found" EmptyDataRowStyle-CssClass="AlternatingRowStyle">
                        <Columns>
                            <asp:TemplateField HeaderText="Reassign?">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCert" runat="server" OnCheckedChanged="chkCert_CheckedChanged"
                                        AutoPostBack="true" /><input type="hidden" id="hdnCertId" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "pk_cert_id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PartId" HeaderText="Part Number" SortExpression="PartId">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartDesc" HeaderText="Description" SortExpression="PartDesc">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="certId" HeaderText="Certificate ID" SortExpression="certid">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IPAddress" HeaderText="IP Address" SortExpression="IPAddress">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="organization" HeaderText="Organization" SortExpression="organization">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActCode" HeaderText="Activation Key" SortExpression="ActCode"
                                HtmlEncode="False">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" SortExpression="ActivatedOn">
                                <HeaderStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="False" />
                        <RowStyle CssClass="RowStyle" />
                        <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
                        <FooterStyle CssClass="FooterStyle" />
                        <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2" background="../Images/MasterPage/images/dashborder_bg.gif">
                    <img src="../Images/MasterPage/images/dashborder_bg.gif" alt="line" border="0" /></td>
            </tr>
            <tr>
                <td align="left">
                    Reassign selected Certs to Email:
                    <asp:TextBox ID="txtEmailTo" runat="Server" Columns="50" CssClass="txt"></asp:TextBox>
                    &nbsp;<asp:Button ID="btnReassign" runat="server" Text="Reassign Certs" OnClick="btnReassign_OnClick"
                        ValidationGroup="ReassignCerts" CssClass="btn" />
                </td>
                <td align="right">
                    <asp:RequiredFieldValidator ControlToValidate="txtEmailTo" ID="rfvReassignTo" EnableClientScript="true"
                        runat="server" ValidationGroup="ReassignCerts" ErrorMessage="Email Address cannot be empty"
                        Display="dynamic"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvNoSelection" Display="dynamic" ValidationGroup="ReassignCerts"
                        ErrorMessage="No Certs were selected" runat="server" OnServerValidate="cvNoSelection_OnValidate"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErr" runat="server" CssClass="lblError"></asp:Label>
                </td>
            </tr>
        </table>
    <%--</ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>--%>

