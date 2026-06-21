<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .login-container {
            max-width: 450px;
            margin: 0 auto;
            margin-top: 5vh;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-container glass-card animate__animated animate__fadeInUp">
        <h2 class="text-center mb-4 text-mousou">Welcome Back</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>
        
        <div class="mb-3">
            <label class="form-label">Username or Email</label>
            <asp:TextBox ID="txtUsernameOrEmail" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
        </div>
        <div class="mb-4">
            <label class="form-label">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        <div class="d-flex justify-content-between mb-4">
            <div class="form-check">
                <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                <label class="form-check-label text-light">Remember me</label>
            </div>
            <a href="ForgotPassword.aspx" class="text-mousou text-decoration-none">Forgot Password?</a>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-mousou btn-lg" OnClick="btnLogin_Click" />
        </div>
        <div class="mt-4 text-center">
            <small>Don't have an account? <a href="Register.aspx" class="text-mousou fw-bold">Sign up</a></small>
        </div>
    </div>
</asp:Content>
