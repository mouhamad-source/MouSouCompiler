<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .dashboard-container { margin: 0 auto; }
        .table-dark {
            background-color: rgba(255, 255, 255, 0.05);
            color: #fff;
        }
        .table-dark th {
            color: #00d2ff;
            border-bottom: 2px solid rgba(255, 255, 255, 0.1);
        }
        .table-dark td {
            border-bottom: 1px solid rgba(255, 255, 255, 0.05);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container glass-card animate__animated animate__fadeIn">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="text-warning">Admin Dashboard</h2>
            <a href="AddUser.aspx" class="btn btn-primary">
                <i class="fas fa-user-plus"></i> Add New User
            </a>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>

        <div class="table-responsive">
            <asp:GridView ID="gvUsers" runat="server" CssClass="table table-dark table-hover" AutoGenerateColumns="False"
                OnRowCommand="gvUsers_RowCommand" DataKeyNames="Id" OnRowDataBound="gvUsers_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Role" HeaderText="Role" />
                    <asp:CheckBoxField DataField="IsEmailConfirmed" HeaderText="Confirmed" />
                    <asp:CheckBoxField DataField="IsLocked" HeaderText="Locked" />
                    <asp:BoundField DataField="FailedLoginAttempts" HeaderText="Fails" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnUnlock" runat="server" CommandName="UnlockUser" CommandArgument='<%# Eval("Id") %>'
                                Text="Unlock" CssClass="btn btn-sm btn-success me-1" Visible='<%# Convert.ToBoolean(Eval("IsLocked")) %>' />
                            <asp:Button ID="btnLock" runat="server" CommandName="LockUser" CommandArgument='<%# Eval("Id") %>'
                                Text="Lock" CssClass="btn btn-sm btn-danger me-1" Visible='<%# !Convert.ToBoolean(Eval("IsLocked")) %>' />
                            <asp:Button ID="btnDelete" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>'
                                Text="Delete" CssClass="btn btn-sm btn-warning" OnClientClick="return confirm('Delete this user permanently?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>