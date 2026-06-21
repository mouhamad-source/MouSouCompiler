<%@ Page Title="Add New User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Admin.AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .form-container {
            max-width: 600px;
            margin: 2rem auto;
            padding: 2rem;
            background: rgba(0,0,0,0.6);
            border-radius: 16px;
        }
        .form-label {
            color: #00d2ff;
            font-weight: 500;
        }
        .btn-back {
            margin-right: 1rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container glass-card animate__animated animate__fadeIn">
        <h2 class="text-warning mb-4">➕ Add New User</h2>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger mb-3 d-block" Visible="false"></asp:Label>

        <div class="mb-3">
            <label class="form-label">Username *</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control bg-dark text-white" Required="true" MaxLength="50" />
        </div>
        <div class="mb-3">
            <label class="form-label">Email *</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control bg-dark text-white" TextMode="Email" Required="true" MaxLength="100" />
        </div>
        <div class="mb-3">
            <label class="form-label">Password *</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control bg-dark text-white" TextMode="Password" Required="true" />
        </div>
        <div class="mb-3">
            <label class="form-label">Confirm Password *</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control bg-dark text-white" TextMode="Password" Required="true" />
        </div>
        <div class="mb-3">
            <label class="form-label">Role</label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select bg-dark text-white">
                <asp:ListItem Text="User" Value="User" />
                <asp:ListItem Text="Admin" Value="Admin" />
            </asp:DropDownList>
        </div>
        <div class="form-check mb-3">
            <asp:CheckBox ID="chkEmailConfirmed" runat="server" CssClass="form-check-input" />
            <label class="form-check-label">Email Confirmed</label>
        </div>
        <div class="form-check mb-4">
            <asp:CheckBox ID="chkLocked" runat="server" CssClass="form-check-input" />
            <label class="form-check-label">Locked (cannot log in)</label>
        </div>

        <div class="d-flex">
            <asp:Button ID="btnCreate" runat="server" Text="Create User" CssClass="btn btn-success me-2" OnClick="btnCreate_Click" />
            <a href="Dashboard.aspx" class="btn btn-secondary">Cancel</a>
        </div>
    </div>
</asp:Content>