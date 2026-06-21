<%@ Page Title="Confirm Email" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmEmail.aspx.cs" Inherits="OnlineCompilerWebForms.pages.ConfirmEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .confirm-container {
            max-width: 500px;
            margin: 0 auto;
            margin-top: 10vh;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="confirm-container glass-card animate__animated animate__zoomIn">
        <h2 class="mb-4 text-mousou">Email Confirmation</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-4 fw-bold fs-5"></asp:Label>
        <a href="/pages/Login.aspx" class="btn btn-mousou">Go to Login</a>
    </div>
</asp:Content>
