<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Pages_Default" MasterPageFile="~/MasterPages/template.master" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="phBody">
<table width="100%" border="0" cellpadding="1" cellspacing="1">
				<tr>
					<td align="left" valign="top" bgcolor="#ffffff" >
						<asp:PlaceHolder ID="plTab" Runat="server"></asp:PlaceHolder>
					</td>
				</tr>
				<tr><td>
			    <!-- other controls -->
                <asp:Literal ID="LitrModDesc" runat="server"></asp:Literal>
				</td></tr>
			</table>
</asp:Content>


