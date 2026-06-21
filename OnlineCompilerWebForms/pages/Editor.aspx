<%@ Page Title="Editor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Editor.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Editor" Async="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.38.0/min/vs/loader.min.js"></script>
    <style>
        /* (same as before – keep your existing styles) */
        :root {
            --editor-bg: #1e1e1e;
            --sidebar-bg: #252526;
            --border-color: #3c3c3c;
            --text-color: #cccccc;
            --hover-bg: #2a2d2e;
            --active-bg: #37373d;
            --active-color: #ffffff;
            --console-bg: #1e1e1e;
        }
        body.light-theme {
            --editor-bg: #ffffff;
            --sidebar-bg: #f3f3f3;
            --border-color: #cccccc;
            --text-color: #333333;
            --hover-bg: #e8e8e8;
            --active-bg: #007acc;
            --active-color: #ffffff;
            --console-bg: #f8f9fa;
        }
        * { box-sizing: border-box; user-select: none; }
        body { 
            overflow: hidden; 
            overflow-y: auto !important;
        }
        .resize-handle-x {
    width: 5px;
    background: var(--border-color);
    cursor: col-resize;
    flex-shrink: 0;
}
        .resize-handle-x:hover {
    background: #00d2ff;
}
        .editor-container {
    /* keep the base height but allow it to grow if needed */
    min-height: calc(100vh - 140px);
    height: auto;
}
        .editor-container {
            display: flex;
            height: calc(100vh - 140px);
            background: var(--editor-bg);
            border-radius: 8px;
            overflow: hidden;
            border: 1px solid var(--border-color);
        }
        .sidebar {
            width: 260px;
            background: var(--sidebar-bg);
            border-right: 1px solid var(--border-color);
            display: flex;
            flex-direction: column;
            flex-shrink: 0;
        }
        .sidebar-header {
            padding: 12px 15px;
            border-bottom: 1px solid var(--border-color);
            display: flex;
            justify-content: space-between;
            align-items: center;
            color: var(--text-color);
            font-weight: 600;
        }
        .file-list {
            flex: 1;
            overflow-y: auto;
            list-style: none;
            padding: 0;
            margin: 0;
        }
        .file-item {
            padding: 8px 12px;
            cursor: pointer;
            color: var(--text-color);
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-left: 3px solid transparent;
        }
        .file-item:hover { background: var(--hover-bg); }
        .file-item.active {
            background: var(--active-bg);
            color: var(--active-color);
            border-left-color: #00d2ff;
        }
        .file-name {
            flex: 1;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .file-actions {
            display: flex;
            gap: 6px;
            opacity: 0;
        }
        .file-item:hover .file-actions { opacity: 1; }
        .file-actions .btn-icon {
            background: none;
            border: none;
            color: #aaa;
            cursor: pointer;
            padding: 2px 4px;
            font-size: 12px;
        }
        .file-actions .btn-icon:hover { color: #fff; }
        .resize-handle-x {
            width: 5px;
            background: var(--border-color);
            cursor: col-resize;
            flex-shrink: 0;
        }
        .resize-handle-x:hover { background: #00d2ff; }
        .main-pane {
            flex: 1;
            display: flex;
            flex-direction: column;
            min-width: 0;
        }
        .tabs-header {
            height: 40px;
            background: var(--sidebar-bg);
            display: flex;
            align-items: center;
            padding-left: 15px;
            border-bottom: 1px solid var(--border-color);
            color: var(--text-color);
            font-weight: 600;
        }
        .editor-area {
            flex: 1;
            min-height: 0;
            position: relative;
        }
        #monaco-container {
            width: 100%;
            height: 100%;
        }
        .resize-handle-y {
            height: 5px;
            background: var(--border-color);
            cursor: row-resize;
            flex-shrink: 0;
        }
        
        .resize-handle-y:hover { background: #00d2ff; }
        .bottom-pane {
            height: 220px;
            background: var(--console-bg);
            border-top: 1px solid var(--border-color);
            display: flex;
            flex-direction: column;
            flex-shrink: 0;
        }
        .tab-bar {
            display: flex;
            background: var(--sidebar-bg);
            border-bottom: 1px solid var(--border-color);
        }
        .tab-btn {
            background: none;
            border: none;
            padding: 8px 16px;
            color: var(--text-color);
            cursor: pointer;
            font-size: 13px;
        }
        .tab-btn.active {
            background: var(--active-bg);
            color: #fff;
            border-bottom: 2px solid #00d2ff;
        }
        .tab-content {
            flex: 1;
            overflow-y: auto;
            padding: 12px;
            font-family: monospace;
            font-size: 13px;
        }
        .input-group-custom {
            display: flex;
            gap: 16px;
            height: 100%;
        }
        .input-group-custom > div {
            flex: 1;
            display: flex;
            flex-direction: column;
        }
        .input-group-custom label {
            font-size: 12px;
            margin-bottom: 4px;
            color: #aaa;
        }
        .input-group-custom textarea {
            flex: 1;
            background: #2d2d2d;
            color: #fff;
            border: 1px solid #3c3c3c;
            padding: 8px;
            border-radius: 4px;
            font-family: monospace;
            resize: vertical;
        }
        .toolbar {
            display: flex;
            gap: 8px;
            align-items: center;
        }
        .entry-point-badge {
            font-size: 0.7em;
            background: #00d2ff;
            color: #000;
            padding: 2px 6px;
            border-radius: 12px;
            margin-left: 8px;
        }
        .metadata-table {
            width: 100%;
            font-size: 0.85em;
            border-collapse: collapse;
        }
        .metadata-table td, .metadata-table th {
            padding: 6px 8px;
            border: 1px solid #3c3c3c;
            vertical-align: top;
        }
        .metadata-table th {
            background: #2d2d2d;
            width: 30%;
        }
        .btn-mousou {
            background: linear-gradient(135deg, #00d2ff, #3a7bd5);
            border: none;
            color: white;
        }
        .btn-mousou:hover {
            background: linear-gradient(135deg, #3a7bd5, #00d2ff);
        }
        .add-file-btn {
            background: none;
            border: none;
            color: #00d2ff;
            font-size: 1.2rem;
            cursor: pointer;
        }
        .add-file-btn:hover { color: #fff; }
        .new-file-panel {
            margin: 8px;
            display: flex;
            gap: 4px;
        }
        .new-file-panel input { flex: 1; }
        .ai-sidebar {
            width: 300px;
            background: var(--sidebar-bg);
            border-left: 1px solid var(--border-color);
            display: flex;
            flex-direction: column;
            flex-shrink: 0;
        }
        .ai-chat-messages {
            flex: 1;
            overflow-y: auto;
            padding: 12px;
            display: flex;
            flex-direction: column;
            gap: 8px;
            font-size: 13px;
            color: var(--text-color);
        }
        .ai-chat-input-container {
            padding: 10px;
            border-top: 1px solid var(--border-color);
            background: var(--console-bg);
            position: relative;
        }
        .ai-chat-input {
            width: 100%;
            background: #2d2d2d;
            color: #fff;
            border: 1px solid #3c3c3c;
            border-radius: 4px;
            padding: 8px;
            resize: none;
            font-family: inherit;
            font-size: 13px;
        }
        .mention-dropdown {
            position: absolute;
            bottom: 100%;
            left: 10px;
            right: 10px;
            background: var(--hover-bg);
            border: 1px solid var(--border-color);
            border-radius: 4px;
            max-height: 150px;
            overflow-y: auto;
            display: none;
            z-index: 100;
        }
        .mention-item {
            padding: 6px 10px;
            cursor: pointer;
            color: var(--text-color);
        }
        .mention-item:hover, .mention-item.selected {
            background: var(--active-bg);
            color: var(--active-color);
        }
        .chat-msg {
            padding: 8px;
            border-radius: 6px;
            background: #2d2d2d;
            word-wrap: break-word;
            white-space: pre-wrap;
        }
        .chat-msg.user {
            background: #004466;
            align-self: flex-end;
        }
        .chat-msg.ai {
            background: #333;
            align-self: flex-start;
            border: 1px solid #444;
        }
        .ai-diff-banner {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            background: rgba(0, 122, 204, 0.9);
            color: white;
            padding: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            z-index: 10;
            font-size: 13px;
            display: none;
        }
        .ai-code-block {
            background: #1e1e1e;
            padding: 8px;
            border-radius: 4px;
            font-family: monospace;
            overflow-x: auto;
            margin-top: 5px;
        }
        .ai-code-block-header {
            display: flex;
            justify-content: space-between;
            background: #2d2d2d;
            padding: 4px 8px;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            font-size: 11px;
            color: #aaa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h3 class="m-0">
            <i class="fas fa-code me-2"></i>
            <asp:Literal ID="litProjectName" runat="server" />
        </h3>
        <div class="toolbar">
            <button type="button" class="btn btn-sm btn-outline-secondary" onclick="toggleTheme()"><i class="fas fa-moon"></i> Theme</button>
            <button type="button" class="btn btn-sm btn-outline-secondary" onclick="formatCode()"><i class="fas fa-magic"></i> Format</button>
            <button type="button" class="btn btn-sm btn-outline-secondary" onclick="copyCode()"><i class="fas fa-copy"></i> Copy</button>
            <button type="button" class="btn btn-sm btn-outline-info" onclick="downloadCurrentFile()"><i class="fas fa-download"></i> Download</button>
            <button type="button" class="btn btn-sm btn-outline-warning" onclick="toggleAiSidebar()"><i class="fas fa-robot"></i> AI</button>
            <button type="button" class="btn btn-sm btn-outline-danger" onclick="triggerAutoFix()"><i class="fas fa-wrench"></i> Auto-Fix</button>
            <asp:UpdatePanel ID="upRunButton" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnRun" runat="server" Text="Run" CssClass="btn btn-sm btn-mousou fw-bold px-4 ms-2 shadow"
                        OnClientClick="syncCodeBeforePostback(); return true;" OnClick="btnRun_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <div class="editor-container">
        <!-- Sidebar (contains all hidden fields for proper refresh) -->
        <asp:UpdatePanel ID="upSidebar" runat="server" UpdateMode="Conditional" CssClass="sidebar" ClientIDMode="Static">
            <ContentTemplate>
                <!-- All hidden fields are inside the UpdatePanel so they get refreshed on partial postback -->
                <asp:HiddenField ID="hfProjectId" runat="server" />
                <asp:HiddenField ID="hfProjectLanguage" runat="server" />
                <asp:HiddenField ID="hfActiveFileId" runat="server" />
                <asp:HiddenField ID="hfActiveFileContent" runat="server" />
                <asp:HiddenField ID="hfFileToLoad" runat="server" />
                <asp:HiddenField ID="hfRenameFileId" runat="server" />
                <asp:HiddenField ID="hfNewFileName" runat="server" />

                <div class="sidebar-header">
                    <span><i class="fas fa-folder-open me-2"></i>Explorer</span>
                    <button type="button" class="add-file-btn" onclick="showAddFilePanel()" title="Add new file">
                        <i class="fas fa-plus-circle"></i>
                    </button>
                </div>
                <div id="newFilePanel" class="new-file-panel" style="display:none;">
                    <asp:TextBox ID="txtNewFileName" runat="server" CssClass="form-control form-control-sm" placeholder="filename.cs" />
                    <asp:Button ID="btnAddFile" runat="server" Text="Add" CssClass="btn btn-sm btn-mousou" OnClick="btnAddFile_Click" OnClientClick="syncCodeBeforePostback(); return validateFileName();" />
                    <button type="button" class="btn btn-sm btn-secondary" onclick="hideAddFilePanel()">Cancel</button>
                </div>
                <ul class="file-list">
                    <asp:Repeater ID="rptFiles" runat="server" OnItemCommand="rptFiles_ItemCommand" OnItemDataBound="rptFiles_ItemDataBound">
                        <ItemTemplate>
                            <li class="file-item <%# Eval("Id").ToString() == hfActiveFileId.Value ? "active" : "" %>">
                                <div class="file-name">
                                    <asp:LinkButton ID="lnkOpenFile" runat="server" CommandName="OpenFile" CommandArgument='<%# Eval("Id") %>'
                                        OnClientClick="syncCodeBeforePostback();" CssClass="text-decoration-none" style="color: inherit;">
                                        <i class='<%# GetFileIcon(Eval("FileName").ToString()) %> me-2'></i>
                                        <%# Eval("FileName") %>
                                    </asp:LinkButton>
                                    <asp:PlaceHolder ID="phEntryPoint" runat="server" Visible='<%# Convert.ToBoolean(Eval("IsEntryPoint")) %>'>
                                        <span class="entry-point-badge">MAIN</span>
                                    </asp:PlaceHolder>
                                </div>
                                <div class="file-actions">
                                    <asp:PlaceHolder ID="phMenu" runat="server" Visible='<%# !Convert.ToBoolean(Eval("IsEntryPoint")) %>'>
                                        <asp:LinkButton ID="lnkSetEntryPoint" runat="server" CommandName="SetEntryPoint" CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn-icon" ToolTip="Set as entry point" OnClientClick="syncCodeBeforePostback();">
                                            <i class="fas fa-play-circle text-success"></i>
                                        </asp:LinkButton>
                                    </asp:PlaceHolder>
                                    <a href="#" class="btn-icon" onclick="promptRenameFile('<%# Eval("Id") %>', '<%# Eval("FileName") %>'); return false;" title="Rename">
                                        <i class="fas fa-edit"></i>
                                    </a>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteFile" CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn-icon text-danger" OnClientClick="syncCodeBeforePostback(); return confirm('Delete this file permanently?');" title="Delete">
                                        <i class="fas fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Horizontal resize handle -->
        <div id="resizeHandleX" class="resize-handle-x"></div>

        <div class="main-pane">
            <div class="tabs-header">
    <i class="fas fa-file-code me-2"></i> <span id="activeFileNameDisplay">Welcome</span>
</div>
            <div class="editor-area">
                <div id="aiDiffBanner" class="ai-diff-banner">
                    <span><i class="fas fa-code-branch me-2"></i> AI suggests code changes</span>
                    <div>
                        <button type="button" class="btn btn-sm btn-success me-2" onclick="acceptAiFix()">Accept</button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="rejectAiFix()">Reject</button>
                    </div>
                </div>
                <div id="monaco-container"></div>
            </div>

            <!-- Vertical resize handle -->
            <div id="resizeHandleY" class="resize-handle-y"></div>

            <div class="bottom-pane" id="bottomPane">
                <div class="tab-bar">
                    <button type="button" class="tab-btn active" onclick="switchBottomTab('terminal', event)"><i class="fas fa-terminal"></i> Terminal</button>
<button type="button" class="tab-btn" onclick="switchBottomTab('metadata', event)"><i class="fas fa-chart-line"></i> Metadata</button>
<button type="button" class="tab-btn" onclick="switchBottomTab('input', event)"><i class="fas fa-keyboard"></i> Input / Args</button>
                </div>
                <div id="terminalTab" class="tab-content">
                    <asp:UpdatePanel ID="upConsole" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfLastErrors" runat="server" ClientIDMode="Static" />
                            <div class="d-flex justify-content-between mb-2">
                                <span><i class="fas fa-terminal"></i> Output</span>
                                <asp:LinkButton ID="btnClearConsole" runat="server" OnClick="btnClearConsole_Click" CssClass="btn btn-link p-0 text-secondary">Clear</asp:LinkButton>
                            </div>
                            <asp:Literal ID="litTerminalOutput" runat="server" Text="<span style='color:#858585;'><i class='fas fa-sparkles'></i> Ready</span>" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="metadataTab" class="tab-content" style="display:none;">
                    <asp:UpdatePanel ID="upMetadata" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Literal ID="litMetadataOutput" runat="server" Text="<span style='color:#858585;'>No execution yet.</span>" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="inputTab" class="tab-content" style="display:none;">
                    <div class="input-group-custom">
                        <div>
                            <label><i class="fas fa-keyboard"></i> Standard Input (stdin)</label>
                            <asp:TextBox ID="txtStdin" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control bg-dark text-light" placeholder="Enter input for your program..."></asp:TextBox>
                        </div>
                        <div>
                            <label><i class="fas fa-list"></i> Command line arguments (one per line)</label>
                            <asp:TextBox ID="txtArgs" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control bg-dark text-light" placeholder="arg1&#10;arg2"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- AI Sidebar Resize Handle -->
        <div id="resizeHandleAi" class="resize-handle-x" style="display:none;"></div>

        <div class="ai-sidebar" id="aiSidebar" style="display:none;">
            <div class="sidebar-header">
                <span><i class="fas fa-robot me-2"></i>AI Assistant</span>
                <button type="button" class="btn-icon" onclick="toggleAiSidebar()" title="Close"><i class="fas fa-times"></i></button>
            </div>
            <div class="ai-chat-messages" id="aiChatMessages">
                <div class="chat-msg ai">Hello! I'm your AI coding assistant. Ask me anything, or type <strong>@</strong> to mention a file in this project.</div>
            </div>
            <div class="ai-chat-input-container">
                <div id="mentionDropdown" class="mention-dropdown"></div>
                <textarea id="aiChatInput" class="ai-chat-input" rows="2" placeholder="Ask AI or type @ to mention a file... (Enter to send)"></textarea>
                <div class="mt-2 d-flex gap-2">
                    <button type="button" class="btn btn-sm btn-outline-info flex-fill" onclick="aiActionSelected('explain')"><i class="fas fa-question-circle"></i> Explain Selected</button>
                    <button type="button" class="btn btn-sm btn-outline-success flex-fill" onclick="aiActionSelected('optimize')"><i class="fas fa-magic"></i> Optimize Selected</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden button outside UpdatePanel (still works for postback) -->
    <asp:Button ID="btnRenameFileHidden" runat="server" OnClick="btnRenameFileHidden_Click" Style="display:none;" />

    <script>
        // Replace the existing script block with this:

        let editor, editorReady = false;
        let isResizingX = false, isResizingY = false;
        let startX, startWidth, startY, startHeight;

        // ---------- Horizontal resize (sidebar) ----------
        const sidebar = document.querySelector('.sidebar');
        const handleX = document.getElementById('resizeHandleX');
        if (handleX && sidebar) {
            handleX.addEventListener('mousedown', (e) => {
                isResizingX = true;
                startX = e.clientX;
                startWidth = sidebar.offsetWidth;
                document.body.style.cursor = 'col-resize';
                e.preventDefault();
            });
            document.addEventListener('mousemove', (e) => {
                if (!isResizingX) return;
                let newWidth = startWidth + (e.clientX - startX);
                newWidth = Math.min(500, Math.max(180, newWidth));
                sidebar.style.width = newWidth + 'px';
                localStorage.setItem('sidebarWidth', newWidth);
            });
            document.addEventListener('mouseup', () => {
                isResizingX = false;
                document.body.style.cursor = '';
            });
        }
        const savedWidth = localStorage.getItem('sidebarWidth');
        if (savedWidth && sidebar) sidebar.style.width = savedWidth + 'px';

        // ---------- Vertical resize (bottom pane) ----------
        const bottomPane = document.getElementById('bottomPane');
        const handleY = document.getElementById('resizeHandleY');
        if (handleY && bottomPane) {
            handleY.addEventListener('mousedown', (e) => {
                isResizingY = true;
                startY = e.clientY;
                startHeight = bottomPane.offsetHeight;
                document.body.style.cursor = 'row-resize';
                e.preventDefault();
            });
            document.addEventListener('mousemove', (e) => {
                if (!isResizingY) return;
                let containerRect = document.querySelector('.main-pane').getBoundingClientRect();
                let newHeight = startHeight + (startY - e.clientY);
                newHeight = Math.min(containerRect.height - 100, Math.max(120, newHeight));
                bottomPane.style.height = newHeight + 'px';
                localStorage.setItem('bottomHeight', newHeight);
                if (editor) editor.layout();
            });
            document.addEventListener('mouseup', () => {
                isResizingY = false;
                document.body.style.cursor = '';
            });
        }
        const savedHeight = localStorage.getItem('bottomHeight');
        if (savedHeight && bottomPane) bottomPane.style.height = savedHeight + 'px';

        // ---------- Add file panel ----------
        function showAddFilePanel() {
            document.getElementById('newFilePanel').style.display = 'flex';
        }
        function hideAddFilePanel() {
            document.getElementById('newFilePanel').style.display = 'none';
            document.getElementById('<%= txtNewFileName.ClientID %>').value = '';
        }
        function validateFileName() {
            let fileName = document.getElementById('<%= txtNewFileName.ClientID %>').value.trim();
            if (fileName === "") {
                alert("Please enter a file name.");
                return false;
            }
            return true;
        }

        // ---------- Monaco & sync ----------
        function syncCodeBeforePostback() {
            if (editor && editorReady) {
                document.getElementById('<%= hfActiveFileContent.ClientID %>').value = editor.getValue();
            }
        }

        require.config({ paths: { 'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.38.0/min/vs' } });
        require(['vs/editor/editor.main'], function () {
            const initial = document.getElementById('<%= hfFileToLoad.ClientID %>').value || '// Select a file';
    const lang = document.getElementById('<%= hfProjectLanguage.ClientID %>').value === 'csharp' ? 'csharp' : 'python';
    editor = monaco.editor.create(document.getElementById('monaco-container'), {
        value: initial, language: lang, theme: 'vs-dark', automaticLayout: true,
        fontSize: 14, fontFamily: "'Fira Code', 'Consolas', monospace", minimap: { enabled: false }
    });
    editorReady = true;
});

// Updated to also set the active file name in the header
window.updateMonacoContent = function (content, filename) {
    if (!editorReady) { setTimeout(() => updateMonacoContent(content, filename), 50); return; }
    editor.setValue(content);
    let lang = document.getElementById('<%= hfProjectLanguage.ClientID %>').value === 'csharp' ? 'csharp' : 'python';
    if (filename.endsWith('.json')) lang = 'json';
    else if (filename.endsWith('.txt')) lang = 'plaintext';
    monaco.editor.setModelLanguage(editor.getModel(), lang);
    // Update the header
    const headerSpan = document.getElementById('activeFileNameDisplay');
    if (headerSpan) headerSpan.textContent = filename;
};

function toggleTheme() {
    let isLight = document.body.classList.toggle('light-theme');
    monaco.editor.setTheme(isLight ? 'vs' : 'vs-dark');
}
function formatCode() { if (editor && editorReady) editor.getAction('editor.action.formatDocument').run(); }
function copyCode() { if (editor && editorReady) navigator.clipboard.writeText(editor.getValue()).then(() => alert('Copied!')); }
function downloadCurrentFile() {
    if (!editor || !editorReady) return;
    const content = editor.getValue(), filename = document.getElementById('activeFileNameDisplay').textContent.trim();
    const blob = new Blob([content], { type: 'text/plain' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = filename;
    a.click();
    URL.revokeObjectURL(a.href);
}

// Fixed to accept event parameter
function switchBottomTab(tab, e) {
    if (e) e.preventDefault();
    document.getElementById('terminalTab').style.display = tab === 'terminal' ? 'block' : 'none';
    document.getElementById('metadataTab').style.display = tab === 'metadata' ? 'block' : 'none';
    document.getElementById('inputTab').style.display = tab === 'input' ? 'block' : 'none';
    document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
    // set active class on the clicked button
    if (e && e.target) e.target.classList.add('active');
}
function promptRenameFile(fileId, oldName) {
    const newName = prompt("Enter new name for file:", oldName);
    if (newName && newName.trim() !== "" && newName.trim() !== oldName) {
        document.getElementById('<%= hfRenameFileId.ClientID %>').value = fileId;
        document.getElementById('<%= hfNewFileName.ClientID %>').value = newName.trim();
        syncCodeBeforePostback();
        document.getElementById('<%= btnRenameFileHidden.ClientID %>').click();
    }
}

// ========== AI Assistant Logic ==========
let aiMentionedFileIds = [];
let aiMentionOptions = [];
let currentMentionIndex = -1;
let aiSuggestedCode = null;
let aiOriginalCode = null;

function toggleAiSidebar() {
    const aiSidebar = document.getElementById('aiSidebar');
    const resizeHandleAi = document.getElementById('resizeHandleAi');
    if (aiSidebar.style.display === 'none') {
        aiSidebar.style.display = 'flex';
        resizeHandleAi.style.display = 'block';
    } else {
        aiSidebar.style.display = 'none';
        resizeHandleAi.style.display = 'none';
    }
    if (editor) editor.layout();
}

// AI Resizing
const handleAi = document.getElementById('resizeHandleAi');
const aiSidebar = document.getElementById('aiSidebar');
let isResizingAi = false, startAiX, startAiWidth;
if (handleAi && aiSidebar) {
    handleAi.addEventListener('mousedown', (e) => {
        isResizingAi = true;
        startAiX = e.clientX;
        startAiWidth = aiSidebar.offsetWidth;
        document.body.style.cursor = 'col-resize';
        e.preventDefault();
    });
    document.addEventListener('mousemove', (e) => {
        if (!isResizingAi) return;
        let newWidth = startAiWidth - (e.clientX - startAiX); // left direction
        newWidth = Math.min(600, Math.max(200, newWidth));
        aiSidebar.style.width = newWidth + 'px';
        if (editor) editor.layout();
    });
    document.addEventListener('mouseup', () => {
        isResizingAi = false;
        document.body.style.cursor = '';
    });
}

const aiChatInput = document.getElementById('aiChatInput');
const mentionDropdown = document.getElementById('mentionDropdown');

function getAllFilesFromSidebar() {
    const files = [];
    document.querySelectorAll('.file-item .file-name a').forEach(link => {
        const parts = link.id.split('_'); // Usually ctl00_MainContent_rptFiles_ctl00_lnkOpenFile
        // Get the file id from CommandArgument, we can't directly read CommandArgument from client side easily unless we render it.
        // Let's use the href which has doPostBack string or we can read a custom attribute if we added it.
        // Wait, we didn't add data-fileid. We can use the text content.
        const fileName = link.innerText.trim();
        files.push(fileName);
    });
    return files;
}

// We need file mapping. Let's just pass filenames to backend and backend will match them.
aiChatInput.addEventListener('input', function(e) {
    const val = this.value;
    const cursorPos = this.selectionStart;
    const lastAtPos = val.lastIndexOf('@', cursorPos - 1);
    
    if (lastAtPos !== -1 && !/\s/.test(val.substring(lastAtPos + 1, cursorPos))) {
        const query = val.substring(lastAtPos + 1, cursorPos).toLowerCase();
        showMentionDropdown(query);
    } else {
        hideMentionDropdown();
    }
});

aiChatInput.addEventListener('keydown', function(e) {
    if (mentionDropdown.style.display === 'block') {
        const items = mentionDropdown.querySelectorAll('.mention-item');
        if (e.key === 'ArrowDown') {
            currentMentionIndex = (currentMentionIndex + 1) % items.length;
            highlightMentionItem(items);
            e.preventDefault();
        } else if (e.key === 'ArrowUp') {
            currentMentionIndex = (currentMentionIndex - 1 + items.length) % items.length;
            highlightMentionItem(items);
            e.preventDefault();
        } else if (e.key === 'Enter') {
            if (currentMentionIndex >= 0) {
                items[currentMentionIndex].click();
                e.preventDefault();
                return;
            }
        } else if (e.key === 'Escape') {
            hideMentionDropdown();
            e.preventDefault();
        }
    } else {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            sendAiMessage();
        }
    }
});

function showMentionDropdown(query) {
    const files = getAllFilesFromSidebar();
    const filtered = files.filter(f => f.toLowerCase().includes(query));
    if (filtered.length === 0) {
        hideMentionDropdown();
        return;
    }
    mentionDropdown.innerHTML = '';
    filtered.forEach((file, index) => {
        const div = document.createElement('div');
        div.className = 'mention-item';
        div.textContent = file;
        div.onclick = () => selectMention(file);
        mentionDropdown.appendChild(div);
    });
    mentionDropdown.style.display = 'block';
    currentMentionIndex = 0;
    highlightMentionItem(mentionDropdown.querySelectorAll('.mention-item'));
}

function hideMentionDropdown() {
    mentionDropdown.style.display = 'none';
    currentMentionIndex = -1;
}

function highlightMentionItem(items) {
    items.forEach(i => i.classList.remove('selected'));
    if (items[currentMentionIndex]) {
        items[currentMentionIndex].classList.add('selected');
        items[currentMentionIndex].scrollIntoView({block: 'nearest'});
    }
}

function selectMention(fileName) {
    const val = aiChatInput.value;
    const cursorPos = aiChatInput.selectionStart;
    const lastAtPos = val.lastIndexOf('@', cursorPos - 1);
    const newVal = val.substring(0, lastAtPos) + '@' + fileName + ' ' + val.substring(cursorPos);
    aiChatInput.value = newVal;
    hideMentionDropdown();
    aiChatInput.focus();
}

        function appendChatMessage(sender, text, allowHtml = false) {
            const container = document.getElementById('aiChatMessages');
            const div = document.createElement('div');
            div.className = `chat-msg ${sender}`;

            let html;
            if (allowHtml) {
                // Trust the input and render as HTML
                html = text;
            } else {
                // Escape user text to prevent injection
                html = text.replace(/</g, '&lt;').replace(/>/g, '&gt;');
            }

            // Parse markdown code blocks loosely
            html = html.replace(/```(\w*)\n([\s\S]*?)```/g, function (match, lang, code) {
                if (sender === 'ai' && (lang.toLowerCase() === 'csharp' || lang.toLowerCase() === 'python' || lang === '')) {
                    proposeAiFix(code.trim());
                }
                return `<div class="ai-code-block-header"><span>${lang}</span><button class="btn-icon" onclick="navigator.clipboard.writeText(decodeURIComponent('${encodeURIComponent(code.trim())}'))"><i class="fas fa-copy"></i></button></div><div class="ai-code-block">${code.trim()}</div>`;
            });

            div.innerHTML = html;
            container.appendChild(div);
            container.scrollTop = container.scrollHeight;
            return div;
        }


function sendAiMessage(isAutoFix = false, autoFixPrompt = null) {
    const prompt = isAutoFix ? autoFixPrompt : aiChatInput.value.trim();
    if (!prompt) return;
    
    if (!isAutoFix) {
        appendChatMessage('user', prompt);
        aiChatInput.value = '';
    }
    
    const loadingDiv = appendChatMessage('ai', '<i class="fas fa-spinner fa-spin"></i> Thinking...' , true );
    
    const projectId = document.getElementById('<%= hfProjectId.ClientID %>').value;
    const activeFileName = document.getElementById('activeFileNameDisplay').textContent;
    const activeFileContent = editor ? editor.getValue() : '';
    
    // Extract mentioned files from prompt
    const mentionedFiles = [];
    const allFiles = getAllFilesFromSidebar();
    allFiles.forEach(f => {
        if (prompt.includes('@' + f)) {
            // Note: Since we don't have file GUIDs easily accessible on the client in the sidebar,
            // we will need to change AiChatHandler to resolve by filename if it's not a guid.
            // For now, let's pass the filename string list to the handler. We'll modify the handler to accept filenames.
            mentionedFiles.push(f);
        }
    });

    fetch('/Handlers/AiChatHandler.ashx', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            ProjectId: projectId,
            Prompt: prompt,
            MentionedFileNames: mentionedFiles, // Use names instead of Ids to be easier
            ActiveFileName: activeFileName,
            ActiveFileContent: activeFileContent,
            IsAutoFix: isAutoFix
        })
    })
    .then(res => res.json())
    .then(data => {
        loadingDiv.remove();
        if (data.error) {
            appendChatMessage('ai', 'Error: ' + data.error);
        } else {
            appendChatMessage('ai', data.reply);
        }
    })
    .catch(err => {
        loadingDiv.remove();
        appendChatMessage('ai', 'Failed to connect to AI service.');
        console.error(err);
    });
}

function proposeAiFix(newCode) {
    if (!editor || !editorReady) return;
    aiOriginalCode = editor.getValue();
    aiSuggestedCode = newCode;
    
    // Show banner
    document.getElementById('aiDiffBanner').style.display = 'flex';
    // Load new code so user can preview
    editor.setValue(newCode);
}

function acceptAiFix() {
    document.getElementById('aiDiffBanner').style.display = 'none';
    syncCodeBeforePostback(); // save content
    appendChatMessage('user', '<i class="fas fa-check-circle text-success"></i> Accepted the code fix.' , true );
}

function rejectAiFix() {
    document.getElementById('aiDiffBanner').style.display = 'none';
    if (editor && aiOriginalCode !== null) {
        editor.setValue(aiOriginalCode);
    }
    appendChatMessage('user', '<i class="fas fa-times-circle text-danger"></i> Rejected the code fix.' , true );
}

function triggerAutoFix() {
    if (document.getElementById('aiSidebar').style.display === 'none') {
        toggleAiSidebar();
    }
    
    const rawErrors = document.getElementById('hfLastErrors').value;
    if (!rawErrors || rawErrors === '[]') {
        appendChatMessage('ai', 'No recent errors found to auto-fix. Please run the code first to capture errors.');
        return;
    }
    
    appendChatMessage('user', '<i class="fas fa-tools text-warning"></i> Requested Auto-Fix based on recent execution errors.' , true );
    
    let errorsJson;
    try {
        errorsJson = JSON.parse(rawErrors);
    } catch(e) {
        errorsJson = rawErrors;
    }
    
    const prompt = `I ran the code and got these errors:\n${JSON.stringify(errorsJson, null, 2)}\n\nPlease fix the active file and output the COMPLETE corrected file in a markdown code block.`;
    sendAiMessage(true, prompt);
}

function aiActionSelected(action) {
    if (!editor || !editorReady) return;
    const selection = editor.getSelection();
    const selectedText = editor.getModel().getValueInRange(selection);
    
    if (!selectedText || selectedText.trim() === '') {
        appendChatMessage('ai', 'Please select some code in the editor first before clicking this button.');
        return;
    }
    
    if (document.getElementById('aiSidebar').style.display === 'none') {
        toggleAiSidebar();
    }
    
    let prompt = "";
    if (action === 'explain') {
        prompt = `Please explain the following selected code:\n\`\`\`\n${selectedText}\n\`\`\``;
        appendChatMessage('user', '<i class="fas fa-question-circle text-info"></i> Explain Selected Code' , true );
    } else if (action === 'optimize') {
        prompt = `Please optimize or refactor the following selected code for better performance, readability, and security. Provide the updated code in a markdown block so I can accept it:\n\`\`\`\n${selectedText}\n\`\`\``;
        appendChatMessage('user', '<i class="fas fa-magic text-success"></i> Optimize Selected Code'  , true );
    }
    
    sendAiMessage(true, prompt); // Use isAutoFix=true mode so it sends our raw prompt without appending to chat again
}

    </script>
</asp:Content>