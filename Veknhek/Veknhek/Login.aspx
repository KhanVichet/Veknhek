<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Default" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form</title>
    <link rel="stylesheet" href="../Style/Login.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="image">
                <asp:Image ID="Image1" runat="server" src="../Image/VEKNHEK.png" />
            </div>
            <div class="input">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" placeholder="សូមបញ្ចូលអ៊ីមែលរបស់អ្នក"></asp:TextBox>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-field" placeholder="សូមបញ្ចូលលេខសម្ងាត់របស់អ្នក"></asp:TextBox>
            </div>
            <div class="btn"> 
                <asp:Button ID="btnLogin" runat="server" Text="ចូល" CssClass="submit-button" OnClick="btnLogin_Click" />
            </div>
            <h3>មិនទាន់មានគណនី? <a href="Register.aspx" OnClick="btnRegister ">បង្កើតគណនីថ្មី</a></h3>
           
        </div>
    </form>
</body>

</html>
