<%@ Page Title="AI Project Generator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AiGenerator.aspx.cs" Inherits="OnlineCompilerWebForms.pages.AiGenerator" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .generator-container {
            max-width: 800px;
            margin: 40px auto;
            background: #1e1e1e;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.5);
            color: #ccc;
        }
        .generator-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .generator-header h2 {
            color: #00d2ff;
            font-weight: bold;
        }
        .generator-header p {
            color: #858585;
            font-size: 1.1em;
        }
        .prompt-box {
            background: #2d2d2d;
            border: 1px solid #3c3c3c;
            color: #fff;
            border-radius: 8px;
            padding: 15px;
            width: 100%;
            font-size: 1.1em;
            resize: vertical;
        }
        .prompt-box:focus {
            outline: none;
            border-color: #00d2ff;
            box-shadow: 0 0 10px rgba(0, 210, 255, 0.2);
        }
        .btn-generate {
            background: linear-gradient(135deg, #00d2ff, #3a7bd5);
            border: none;
            color: white;
            font-weight: bold;
            padding: 12px 25px;
            border-radius: 8px;
            font-size: 1.1em;
            transition: transform 0.2s, box-shadow 0.2s;
            width: 100%;
            margin-top: 20px;
        }
        .btn-generate:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(0, 210, 255, 0.4);
            color: white;
        }
        .loader-container {
            display: none;
            text-align: center;
            margin-top: 30px;
        }
        .loader-spinner {
            color: #00d2ff;
            font-size: 3em;
        }
        .loader-text {
            margin-top: 15px;
            font-size: 1.2em;
            color: #fff;
        }
        .result-panel {
            margin-top: 20px;
        }
        .lang-select {
            background: #2d2d2d;
            color: #fff;
            border: 1px solid #3c3c3c;
            padding: 10px;
            border-radius: 6px;
            width: 100%;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Add ScriptManager here – only for this page, does not affect others --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <div class="generator-container">
        <div class="generator-header">
            <h2><i class="fas fa-magic me-2"></i> AI Project Generator</h2>
            <p>Describe your idea, and AI will build the complete project structure for you.</p>
        </div>

        <asp:UpdatePanel ID="upGenerator" runat="server">
            <ContentTemplate>
                <div id="inputForm" runat="server">
                    <label class="form-label fw-bold">Project Name</label>
                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="prompt-box mb-3" placeholder="e.g. My Awesome App" Rows="1"></asp:TextBox>
                    
                    <label class="form-label fw-bold">Language</label>
                    <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="lang-select">
                        <asp:ListItem Value="csharp" Text="C# (.NET)" />
                        <asp:ListItem Value="python" Text="Python" />
                    </asp:DropDownList>

                    <label class="form-label fw-bold">Project Description</label>
                    <asp:TextBox ID="txtPrompt" runat="server" TextMode="MultiLine" Rows="5" CssClass="prompt-box" placeholder="e.g. Create a C# console application that manages a library system with Books and Members classes..."></asp:TextBox>

                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Project" CssClass="btn-generate" OnClick="btnGenerate_Click" OnClientClick="showLoader();" />
                </div>

                <div id="loaderUI" class="loader-container">
                    <i class="fas fa-circle-notch fa-spin loader-spinner"></i>
                    <div class="loader-text">AI is coding your project... This may take a minute.</div>
                </div>

                <div class="result-panel">
                    <asp:Literal ID="litResult" runat="server"></asp:Literal>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function showLoader() {
            var inputForm = document.getElementById('<%= inputForm.ClientID %>');
            var loaderUI = document.getElementById('loaderUI');
            if (inputForm) inputForm.style.display = 'none';
            if (loaderUI) loaderUI.style.display = 'block';
        }

        // Ensure loader hides if there's an async postback error
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, args) {
            if (args.get_error()) {
                var inputForm = document.getElementById('<%= inputForm.ClientID %>');
                var loaderUI = document.getElementById('loaderUI');
                if (inputForm) inputForm.style.display = 'block';
                if (loaderUI) loaderUI.style.display = 'none';
                args.set_errorHandled(true);
            }
        });
    </script>
</asp:Content>