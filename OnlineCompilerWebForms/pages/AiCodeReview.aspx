<%@ Page Title="AI Code Review" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AiCodeReview.aspx.cs" Inherits="OnlineCompilerWebForms.pages.AiCodeReview" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .review-container {
            max-width: 900px;
            margin: 40px auto;
            background: #1e1e1e;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.5);
            color: #ccc;
        }
        .review-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .review-header h2 {
            color: #00d2ff;
            font-weight: bold;
        }
        .project-select {
            background: #2d2d2d;
            color: #fff;
            border: 1px solid #3c3c3c;
            padding: 12px;
            border-radius: 6px;
            width: 100%;
            margin-bottom: 20px;
        }
        .btn-analyze {
            background: linear-gradient(135deg, #f5af19, #f12711);
            border: none;
            color: white;
            font-weight: bold;
            padding: 12px 25px;
            border-radius: 8px;
            font-size: 1.1em;
            transition: transform 0.2s, box-shadow 0.2s;
            width: 100%;
        }
        .btn-analyze:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(241, 39, 17, 0.4);
            color: white;
        }
        
        .scores-section {
            display: flex;
            justify-content: space-around;
            margin-top: 40px;
            margin-bottom: 40px;
            text-align: center;
        }
        .score-card {
            background: #2a2a2a;
            padding: 20px;
            border-radius: 10px;
            width: 30%;
            border: 1px solid #333;
        }
        .score-circle {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            margin: 0 auto 15px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.8em;
            font-weight: bold;
            color: #fff;
            border: 5px solid #444;
        }
        .score-title {
            font-weight: bold;
            color: #00d2ff;
            margin-bottom: 5px;
        }
        .score-desc {
            font-size: 0.85em;
            color: #aaa;
        }

        .report-content {
            background: #2d2d2d;
            padding: 20px;
            border-radius: 8px;
            border-left: 4px solid #00d2ff;
            margin-top: 20px;
            white-space: pre-wrap;
            font-family: 'Consolas', monospace;
        }
        
        .loader-container {
            display: none;
            text-align: center;
            margin-top: 30px;
        }
        .loader-spinner {
            color: #f5af19;
            font-size: 3em;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Add ScriptManager for this page only – does not affect other pages --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <div class="review-container">
        <div class="review-header">
            <h2><i class="fas fa-search-code me-2"></i> AI Code Review</h2>
            <p>Select a project. Our AI will analyze all files to generate a comprehensive Health Report.</p>
        </div>

        <asp:UpdatePanel ID="upReview" runat="server">
            <ContentTemplate>
                <div id="selectionUI" runat="server">
                    <label class="form-label fw-bold">Select Project to Review:</label>
                    <asp:DropDownList ID="ddlProjects" runat="server" CssClass="project-select" DataTextField="Name" DataValueField="Id"></asp:DropDownList>

                    <asp:Button ID="btnAnalyze" runat="server" Text="Analyze Project" CssClass="btn-analyze" OnClick="btnAnalyze_Click" OnClientClick="showLoader();" />
                </div>

                <div id="loaderUI" class="loader-container">
                    <i class="fas fa-cog fa-spin loader-spinner"></i>
                    <div class="mt-3 text-light">Scanning codebase...</div>
                </div>

                <asp:Panel ID="pnlResults" runat="server" Visible="false">
                    <div class="scores-section">
                        <div class="score-card">
                            <div class="score-circle" id="circleSecurity" runat="server">0</div>
                            <div class="score-title"><i class="fas fa-shield-alt"></i> Security</div>
                            <div class="score-desc">Vulnerabilities & best practices</div>
                        </div>
                        <div class="score-card">
                            <div class="score-circle" id="circlePerformance" runat="server">0</div>
                            <div class="score-title"><i class="fas fa-tachometer-alt"></i> Performance</div>
                            <div class="score-desc">Optimization & speed</div>
                        </div>
                        <div class="score-card">
                            <div class="score-circle" id="circleReadability" runat="server">0</div>
                            <div class="score-title"><i class="fas fa-book-open"></i> Readability</div>
                            <div class="score-desc">Clean code & structure</div>
                        </div>
                    </div>

                    <h4 class="text-info mt-4"><i class="fas fa-clipboard-list"></i> Detailed Feedback</h4>
                    <div class="report-content">
                        <asp:Literal ID="litFeedback" runat="server"></asp:Literal>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function showLoader() {
            var selectionUI = document.getElementById('<%= selectionUI.ClientID %>');
            var pnlResults = document.getElementById('<%= pnlResults.ClientID %>');
            var loaderUI = document.getElementById('loaderUI');
            
            if (selectionUI) selectionUI.style.display = 'none';
            if (pnlResults) pnlResults.style.display = 'none';
            if (loaderUI) loaderUI.style.display = 'block';
        }

        // Ensure UI resets if an error occurs during async postback
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, args) {
            if (args.get_error()) {
                var selectionUI = document.getElementById('<%= selectionUI.ClientID %>');
                var loaderUI = document.getElementById('loaderUI');
                if (selectionUI) selectionUI.style.display = 'block';
                if (loaderUI) loaderUI.style.display = 'none';
                args.set_errorHandled(true);
            }
        });
    </script>
</asp:Content>