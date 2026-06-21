<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Async="true" Inherits="OnlineCompilerWebForms.pages.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .register-container {
            max-width: 500px;
            margin: 0 auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="register-container glass-card animate__animated animate__zoomIn">
        <h2 class="text-center mb-4 text-mousou">Create an Account</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>
        
        <div class="mb-3">
            <label class="form-label">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">Email address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" Required="true"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        <div class="mb-4">
            <label class="form-label">Confirm Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnRegister" runat="server" Text="Sign Up" CssClass="btn btn-mousou btn-lg" OnClick="btnRegister_Click" />
        </div>
        <div class="mt-3 text-center">
            <small>Already have an account? <a href="Login.aspx" class="text-mousou">Login here</a></small>
        </div>
    </div>
</asp:Content>
