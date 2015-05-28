<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddMembers.ascx.cs" Inherits="Controls_AddMembers" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<asp:Label ID="lblCompanyInfo" runat="server" CssClass="lblBold"></asp:Label><br />
<asp:Label id="lblErr" runat="server" CssClass="lblError"></asp:Label><br />
<asp:Wizard ID="wizAddMem" runat="server" DisplaySideBar="False" DisplayCancelButton="false"  Width="100%" ActiveStepIndex="0" >
<WizardSteps>
<asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start" runat="server">
<table>
<tr>
<td style="width:30%">
<asp:Label ID="lblhead" CssClass="lbl" runat="server" Text="I would like to add :"></asp:Label>
</td>
<td style="width: 313px" >
<asp:RadioButtonList ID="rblAccType" runat="server"  CssClass="ddlist" RepeatDirection="Horizontal" >
                      <asp:ListItem Text="New Accounts" Value="NEW"></asp:ListItem>  
                      <asp:ListItem Text="Existing Accounts" Value="EXIST"></asp:ListItem>  
                      </asp:RadioButtonList> 
</td>
</tr>
<tr>
<td colspan="2"><asp:RequiredFieldValidator ID="rfvAccType" runat="server" ControlToValidate="rblAccType" CssClass="lblError" ErrorMessage="Account Type is mandatory" Display="Dynamic" ValidationGroup="Step1"></asp:RequiredFieldValidator></td>
</tr>
<tr>
<td colspan="2" style="height: 26px">
<asp:Button ID="btnStep1" runat="server" Text="Next" OnClick="btnStep1_OnClick" ValidationGroup="Step1" CssClass="btn" />
</td>
</tr>
</table>
</asp:WizardStep>
<asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Finish" runat="server">
<asp:Panel ID="pnlNew" runat="server">
<table>
<tr>
<td>
Enter multiple email addresses seperated by comma(,) 
</td>
</tr>
<tr>
<td>
Email Address for New Accounts:
</td>
</tr>
<tr>
<td>
<asp:TextBox ID="txtNew" runat="server" TextMode="MultiLine" Rows="7"  Columns="55" CssClass="txt"></asp:TextBox>
</td>
</tr>
<tr>
<td>
<asp:Button ID="btnAccNew" runat="server" Text="Group Accounts" OnClick="btnAccNew_OnClick" ValidationGroup="Step2New" CssClass="btn" />
    <asp:Label ID="LblDisplay" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Green"></asp:Label>
</td>
</tr>
<tr>
<td>
<asp:CustomValidator ID="cvNew" runat="server" OnServerValidate="cvNew_OnServerValidate" Display="Dynamic" ValidationGroup="Step2New" CssClass="lblError"></asp:CustomValidator>
</td>
</tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlExist" runat="server">
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
<td align="left">
<asp:CustomValidator ID="cvExist" runat="server" OnServerValidate="cvExist_OnServerValidate" Display="Dynamic" ValidationGroup="Step2Exist" CssClass="lblError"></asp:CustomValidator>
</td>
<td align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label>
</asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="server" MaxLength="75" Columns="30" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="Dynamic" ValidationGroup="btnGo"></asp:RequiredFieldValidator>
</asp:Panel>

</td>
</tr>
<tr style="width:100%">
<td class="paging" align="left">Total Records:&nbsp;<asp:label id="totalRecords" runat="server"></asp:label>&nbsp;|&nbsp;Page:&nbsp;<asp:label id="currentPage" runat="server"></asp:label>&nbsp;
of&nbsp;<asp:label id="totalPages" runat="server"></asp:label>&nbsp;&nbsp;<asp:LinkButton CssClass="btn"  ID="lnkClearSort" runat="server" Text="Clear Sort"  OnClick="lnkClearSort_Click"></asp:LinkButton>
</td>
<td class="paging" align="right">
<asp:imagebutton id="btnFirst" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page" EnableViewState="False"></asp:imagebutton>
<asp:LinkButton CssClass="btn"  ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0" Text="first"></asp:LinkButton> |
<asp:imagebutton id="btnPrev" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page" EnableViewState="False"></asp:imagebutton>
<asp:LinkButton CssClass="btn"  ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev" Text="previous"></asp:LinkButton> | <asp:LinkButton CssClass="btn"  ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next" Text="next"></asp:LinkButton>
<asp:imagebutton id="btnNext" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navNext.gif" CommandArgument="next" AlternateText="Next page" EnableViewState="False"></asp:imagebutton>
| <asp:LinkButton CssClass="btn"  ID="lnlLast" runat="server" OnClick="linkButton_Click" CommandArgument="last" Text="last"></asp:LinkButton>
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="False" />
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
<tr style="width:100%">
<td colspan="2" align="left">
        <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0px"
        AllowSorting ="True"  
        PageSize="50" 
        OnSorting="gvPR_Sort" OnRowDataBound="ItemCellsUpdate" 
        EmptyDataText="No ungrouped accounts found"
         >
            <Columns>
             <asp:TemplateField HeaderText="Group ?">
           <ItemTemplate>
           <asp:CheckBox ID="chkAcct" runat="server" /><input type="hidden" id="hdnAcctId" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "AcctId") %>' />
           </ItemTemplate>
           </asp:TemplateField>
                <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="FullName" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="Company" HeaderText="User-entered Company" SortExpression="Company" />
                <asp:BoundField DataField="AccountType" HeaderText="Account Type" SortExpression="AccountType"  />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"  />
                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" SortExpression="CreatedOn" />
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
        </asp:GridView>
        </td>
        </tr>
        </table>
<table>
<tr>
<td>
<asp:Button ID="btnAccExist" runat="server" Text="Group Accounts" OnClick="btnAccExist_OnClick" ValidationGroup="Step2Exist" CssClass="btn" />
</td>
</tr>

</table>
</asp:Panel>
</asp:WizardStep>
</WizardSteps>
<StartNavigationTemplate>
</StartNavigationTemplate>
<StepNavigationTemplate>
</StepNavigationTemplate>
<FinishNavigationTemplate>
</FinishNavigationTemplate>
</asp:Wizard>
