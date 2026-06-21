# MouSou Online Compiler Platform

Welcome to the **MouSou Online Compiler**, a premium, highly scalable, and AI-powered web-based Integrated Development Environment (IDE). Designed for modern developers, it combines the robust reliability of ASP.NET Web Forms with bleeding-edge AI capabilities.

## 👑 Creators
- **Mouhamad Hammoud** - Lead Architect & Founder
- **Mamdouh Ossman** - Co-Founder & UI/UX Director

## 🚀 Key Features
- **Project-Based Workspace**: Manage entire projects, not just single files. Add, delete, and rename files seamlessly.
- **Microservice Compilation**: Secure code execution via a dedicated backend microservice.
- **AI Coding Assistant**: Chat with an AI that inherently understands your entire project context.
- **AI Project Generator**: Scaffold entire multi-file projects using a single natural language prompt.
- **AI Code Review Dashboard**: Get a comprehensive "Health Report" scoring your project's Security, Readability, and Performance.
- **Context-Aware Auto-Fix**: Automatically capture compiler errors and instantly receive inline code patches.
- **Community Feed**: Browse and clone public projects shared by the community.

## 🛠️ Technology Stack
- **Frontend**: HTML5, CSS3, Bootstrap 5, JavaScript (ES6+), Monaco Editor.
- **Backend Framework**: ASP.NET Web Forms (.NET Framework 4.7.2).
- **AI Integration**: OpenRouter API (Qwen Coder / DeepSeek).
- **Execution Engine**: ASP.NET Web API Microservice.
- **Database**: SQL Server LocalDB with raw ADO.NET (Repository Pattern).

## ⚙️ Setup & Configuration
1. Open the solution in Visual Studio 2022.
2. Update the `Web.config` to include your OpenRouter/OpenAI API key.
3. Ensure the LocalDB connection string points to the correct `.mdf` file.
4. Run the project!

## 📜 License
Proprietary software built by Mouhamad Hammoud and Mamdouh Ossman.
