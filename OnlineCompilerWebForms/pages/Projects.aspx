<%@ Page Title="Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Projects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .project-card {
            background: rgba(255, 255, 255, 0.05);
            border: 1px solid rgba(255, 255, 255, 0.1);
            transition: transform 0.2s, box-shadow 0.2s;
        }
        .project-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.2);
            border-color: rgba(0, 210, 255, 0.5);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container animate__animated animate__fadeIn">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="text-mousou m-0">My Projects</h2>
            <a href="CreateProject.aspx" class="btn btn-mousou">
                + New Project
            </a>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <div class="row">
            <asp:Repeater ID="rptProjects" runat="server" OnItemCommand="rptProjects_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-4 mb-4">
                        <div class="card project-card text-white h-100">
                            <div class="card-body">
                                <div class="d-flex justify-content-between">
                                    <h5 class="card-title text-mousou text-truncate"><%# Eval("Name") %></h5>
                                    <span class="badge bg-secondary"><%# Eval("Language") %></span>
                                </div>
                                <p class="card-text small text-muted text-truncate"><%# string.IsNullOrEmpty(Eval("Description")?.ToString()) ? "No description" : Eval("Description") %></p>
                                <div class="mt-3">
                                    <small class="text-muted d-block mb-3">Updated: <%# Convert.ToDateTime(Eval("UpdatedAt")).ToString("MMM dd, yyyy") %></small>
                                    
                                    <div class="d-flex gap-2">
                                        <a href="Editor.aspx?id=<%# Eval("Id") %>" class="btn btn-sm btn-outline-info flex-grow-1">Open IDE</a>
                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteProject" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this project?');">
                                            Delete
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:PlaceHolder ID="phNoProjects" runat="server" Visible='<%# rptProjects.Items.Count == 0 %>'>
                        <div class="col-12 text-center text-muted mt-5">
                            <h4>No projects found.</h4>
                            <p>Click 'New Project' to get started!</p>
                        </div>
                    </asp:PlaceHolder>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>