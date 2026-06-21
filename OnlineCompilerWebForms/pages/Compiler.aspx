<%@ Page Title="Compiler" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compiler.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Compiler" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .compiler-container {
            margin: 0 auto;
        }
        .code-editor {
            font-family: 'Consolas', 'Courier New', monospace;
            background-color: rgba(0, 0, 0, 0.5);
            color: #d4d4d4;
            border: 1px solid rgba(255, 255, 255, 0.2);
            min-height: 400px;
        }
        .code-editor:focus {
            background-color: rgba(0, 0, 0, 0.7);
            border-color: #00d2ff;
            color: #fff;
            box-shadow: 0 0 0 0.25rem rgba(0, 210, 255, 0.25);
        }
        .result-box {
            background-color: rgba(0, 0, 0, 0.8);
            border: 1px solid rgba(255, 255, 255, 0.1);
            color: #fff;
            padding: 15px;
            min-height: 200px;
            border-radius: 8px;
            overflow-y: auto;
            font-family: 'Consolas', 'Courier New', monospace;
        }
        .success-message { color: #4ec9b0; }
        .error-message { color: #f14c4c; }
        .exec-time-badge {
            background: rgba(0, 210, 255, 0.1);
            color: #00d2ff;
            padding: 5px 10px;
            border-radius: 15px;
            font-size: 0.85em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="compiler-container glass-card animate__animated animate__fadeIn">
        <h2 class="mb-4 text-mousou">Online Compiler</h2>
        
        <div class="row">
            <div class="col-md-8">
                <div class="mb-3 d-flex justify-content-between align-items-center">
                    <div class="w-50">
                        <label class="form-label">Language</label>
                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-select bg-dark text-white border-secondary">
                            <asp:ListItem Value="csharp" Text="C#" />
                            <asp:ListItem Value="python" Text="Python" />
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary me-2" OnClick="btnClear_Click" />
                        <asp:Button ID="btnRun" runat="server" Text="Run Code" CssClass="btn btn-mousou" OnClick="btnRun_Click" />
                    </div>
                </div>
                
                <div class="mb-3">
                    <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" CssClass="form-control code-editor p-3" placeholder="Write your code here..."></asp:TextBox>
                </div>
            </div>
            
            <div class="col-md-4">
                <div class="mb-3">
                    <label class="form-label">Output</label>
                    <div class="result-box">
                        <asp:Literal ID="litResult" runat="server" Text="<span style='color: #858585;'>✨ Output will appear here.</span>"></asp:Literal>
                    </div>
                </div>
                <div class="text-end">
                    <asp:Literal ID="litExecutionTime" runat="server" Visible="false"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
