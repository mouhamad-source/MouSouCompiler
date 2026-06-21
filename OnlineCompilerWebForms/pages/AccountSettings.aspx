<%@ Page Title="Account Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountSettings.aspx.cs" Inherits="OnlineCompilerWebForms.pages.AccountSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .settings-container {
            max-width: 600px;
            margin: 0 auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="settings-container glass-card animate__animated animate__fadeIn">
        <h2 class="mb-4 text-mousou">Account Settings</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>
        
        <div class="mb-3">
            <label class="form-label">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            <small class="text-muted">Username cannot be changed.</small>
        </div>
        <div class="mb-4">
            <label class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>

        <hr style="border-color: rgba(255,255,255,0.2)" />
        <h4 class="mt-4 mb-3">Change Password</h4>
        
        <div class="mb-3">
            <label class="form-label">Current Password</label>
            <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">New Password</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
        <div class="mb-4">
            <label class="form-label">Confirm New Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnUpdate" runat="server" Text="Update Password" CssClass="btn btn-mousou btn-lg" OnClick="btnUpdate_Click" />
        </div>
    </div>
</asp:Content>
