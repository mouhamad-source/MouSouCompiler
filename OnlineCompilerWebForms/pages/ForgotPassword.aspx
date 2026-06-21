<%@ Page Title="Forgot Password" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="OnlineCompilerWebForms.pages.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .forgot-container {
            max-width: 450px;
            margin: 0 auto;
            margin-top: 10vh;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="forgot-container glass-card animate__animated animate__fadeIn">
        <h2 class="text-center mb-4 text-mousou">Forgot Password</h2>
        <p class="text-center mb-4">Enter your email address and we'll send you a link to reset your password.</p>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>
        
        <div class="mb-4">
            <label class="form-label">Email address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" Required="true"></asp:TextBox>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnSubmit" runat="server" Text="Send Reset Link" CssClass="btn btn-mousou btn-lg" OnClick="btnSubmit_Click" />
        </div>
        <div class="mt-4 text-center">
            <small><a href="Login.aspx" class="text-mousou">Back to Login</a></small>
        </div>
    </div>
</asp:Content>
