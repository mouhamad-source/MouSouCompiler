<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="OnlineCompilerWebForms.pages.ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .reset-container {
            max-width: 450px;
            margin: 0 auto;
            margin-top: 10vh;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="reset-container glass-card animate__animated animate__zoomIn">
        <h2 class="text-center mb-4 text-mousou">Reset Password</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>
        
        <asp:Panel ID="pnlReset" runat="server">
            <div class="mb-3">
                <label class="form-label">New Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
            </div>
            <div class="mb-4">
                <label class="form-label">Confirm New Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Required="true"></asp:TextBox>
            </div>
            <div class="d-grid">
                <asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn btn-mousou btn-lg" OnClick="btnReset_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
