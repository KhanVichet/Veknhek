<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Form</title>
     <link rel="stylesheet" href="../Style/Register.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.6.0/css/all.min.css" integrity="sha512-Kc323vGBEqzTmouAECnVceyQqyqdsSiqLQISBL29aUW4U/M7pSPA/gEUZQqv1cwx4OnYxTxve5UMg5GT6L4JJg==" crossorigin="anonymous" referrerpolicy="no-referrer" /></head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="image">
                <asp:Image ID="Image1" runat="server" src="../Image/VEKNHEK.png" />
            </div>
            <div class="title"><h2>បង្កើតគណនីថ្មី</h2></div>
            <div class="input">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" placeholder="សូមបញ្ចូលអ៊ីមែលរបស់អ្នក"></asp:TextBox>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-field" placeholder="សូមបង្កើតកូដសម្ងាត់"></asp:TextBox>
                <asp:TextBox ID="txtComPassword" runat="server" TextMode="Password" CssClass="input-field" placeholder="សូមបញ្ចូលកូដសម្ងាត់ម្ដងទៀត"></asp:TextBox>
            </div>
            <div class="btn"> 
                <asp:Button ID="btnRegister" runat="server" Text="បង្កើត" CssClass="submit-button" OnClick="btnRegister_Click" />
            </div>
          
        </div>
        <div>
             <a href="Login.aspx">
                    <i class="fa-solid fa-reply"></i>
                 <h3>
                     ត្រឡប់ទៅទំព័រដើម
                 </h3>
             </a>
        </div>
    </form>
</body>
</html>
