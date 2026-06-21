<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .error-container {
            max-width: 600px;
            margin: 10vh auto 0;
            text-align: center;
        }
        .error-code {
            font-size: 6rem;
            font-weight: 700;
            color: #f14c4c;
            line-height: 1;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="error-container glass-card animate__animated animate__shakeX">
        <div class="error-code"><asp:Literal ID="litErrorCode" runat="server" Text="Error"></asp:Literal></div>
        <h3 class="mt-4 mb-3 text-mousou"><asp:Literal ID="litErrorTitle" runat="server" Text="Something went wrong"></asp:Literal></h3>
        <p class="text-muted mb-4"><asp:Literal ID="litErrorMessage" runat="server" Text="We encountered an unexpected issue while processing your request."></asp:Literal></p>
        <a href="/pages/About.aspx" class="btn btn-mousou btn-lg">Return Home</a>
    </div>
</asp:Content>
