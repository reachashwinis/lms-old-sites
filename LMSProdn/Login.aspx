<%@ Page Language="C#" MasterPageFile="~/MasterPages/LoginTemplate.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="LoginPage"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
   <table style="width:100%">
   <!--<tr>
   <td style="color:Red"><b>
   Please note that licensing site will go offline on April/18/2014 from 11.30PM to 12midnight for maintenence. Apologies for the inconvenience caused.
   </b>   
   </td>
   </tr> -->
   <tr >
   <td align="center">
        <asp:login ID="lgnLogin"  RememberMeSet="false" DisplayRememberMe="false" UserNameLabelText="Email Address: "  Width="100%" 
         DestinationPageUrl="~/Pages/Default.aspx?tabId=Home" runat="server" OnAuthenticate="lgnLogin_Authenticate" TextBoxStyle-CssClass="txtLogin" LabelStyle-CssClass="lbl" LoginButtonStyle-CssClass="btn"
         UserNameRequiredErrorMessage="Email Address is mandatory" PasswordLabelText="Password:" PasswordRequiredErrorMessage="Password is mandatory"  Orientation="Vertical" LoginButtonType="Button" 
         ></asp:login>  <br />
         <a href="Accounts/ForgotPassword.aspx">Forgot Password ?</a>
    </td>
   <td>
   </td>
   </tr>
      <tr><br /><br /></tr>
   <tr><td align="center"> <b><a href="Accounts/RegisterPromoAcct.aspx">Free 90-day AirWave Trial for Aruba Instant users</a></b></td></tr>

   </table>  
   <%--<script src="http://static.getclicky.com/js" type="text/javascript"></script>
<script type="text/javascript">try{ clicky.init(66377314); }catch(err){}</script>--%>
</asp:Content>


