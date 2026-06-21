<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="OnlineCompilerWebForms.pages.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="glass-card animate__animated animate__fadeIn">
        <h2 class="text-mousou mb-4">About MouSou Compiler</h2>
        <p class="lead">Welcome to MouSou Compiler, a robust, cloud-driven IDE built using ASP.NET Web Forms and Microservices.</p>
        
        <h4 class="mt-4 text-mousou">Features</h4>
        <ul>
            <li>Secure Authentication (Hashing, Tokens, Sessions)</li>
            <li>Multi-language support (C#, Python)</li>
            <li>Real-time Compilation microservice architecture</li>
            <li>Responsive UI powered by Bootstrap 5 & Animate.css</li>
        </ul>

        <h4 class="mt-4 text-mousou">Technology Stack</h4>
        <p>This project utilizes an N-Tier monolithic web application front-end connected to a highly scalable ASP.NET Web API backend execution service.</p>
    </div>
</asp:Content>
