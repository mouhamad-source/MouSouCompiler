<%@ Page Title="Create New Project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="OnlineCompilerWebForms.pages.CreateProject" %>

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
        <h2 class="text-mousou mb-4">➕ Create New Project</h2>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger mb-3 d-block" Visible="false"></asp:Label>

        <div class="mb-3">
            <label class="form-label">Project Name *</label>
            <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control bg-dark text-white" Required="true" MaxLength="100" />
        </div>
        <div class="mb-3">
            <label class="form-label">Language</label>
            <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-select bg-dark text-white">
                <asp:ListItem Value="csharp" Text="C#" />
                <asp:ListItem Value="python" Text="Python" />
            </asp:DropDownList>
        </div>
        <div class="mb-4">
            <label class="form-label">Description (Optional)</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control bg-dark text-white" />
        </div>

        <div class="d-flex">
            <asp:Button ID="btnCreate" runat="server" Text="Create Project" CssClass="btn btn-mousou me-2" OnClick="btnCreate_Click" />
            <a href="Projects.aspx" class="btn btn-secondary">Cancel</a>
        </div>
    </div>
</asp:Content>