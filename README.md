# 🚀 MouSou Online Compiler Platform

## 🧠 Overview

A full-stack, enterprise-grade **AI-powered online compiler platform** designed to deliver secure, scalable, and intelligent code execution in the browser.

This system combines:

* 🔐 Advanced authentication & authorization
* 🐳 Sandboxed code execution using Docker
* 🤖 AI-assisted development tools
* 🧩 Multi-file project management
* ⚡ Real-time code editing (Monaco Editor)

---

## ✨ Key Features

### 🔐 Authentication & User Management

* Secure Login & Registration system
* Email verification & validation
* Password reset & recovery flow
* Account locking/unlocking
* Role-based access control (Admin/User)
* User profile & account settings

---

### 🐳 Secure Code Execution Engine

* Docker-based isolated execution
* Protection against malicious code
* Resource and time limits enforcement
* Multi-language support:

  * Python 🐍
  * C# ⚙️
* Support for:

  * Multi-file projects
  * Runtime arguments (argc / argv)

---

### 🧩 Project & File Management

* Create, update, delete projects
* Multi-file editing support
* Structured file system
* Persistent storage

---

### 🖥️ Code Editor

* Monaco Editor (VS Code-like experience)
* Syntax highlighting
* Multi-file navigation

---

### 🤖 AI-Powered Features

#### 💬 AI Chat Assistant

* Context-aware chat
* Reads selected files
* Explains and improves code

#### 🛠️ AI Auto-Fix

* Detect runtime errors
* Suggest fixes instantly
* Accept / Reject workflow

#### ⚡ AI Code Optimization

* Optimize selected code blocks
* Improve performance and readability

#### 📄 AI Documentation

* Generate documentation from code

#### 🔍 AI Code Review

* Full project analysis
* Security & performance insights

#### 🏗️ AI Project Generator

* Generate full runnable projects from prompts

---

## 🏗️ Architecture

```plaintext
Frontend (WebForms + Monaco)
        ↓
Backend (.NET)
        ↓
AI Service Layer
        ↓
Compiler Microservice (Docker Sandbox)
```
all design img and work flow LLD , HLD , SRS , can be find inside docs folder 
---

## ⚙️ Tech Stack

* **Backend:** .NET (C#)
* **Frontend:** WebForms + Monaco Editor
* **AI Integration:** OpenAI / LLM APIs
* **Execution Engine:** Docker
* **Database:** SQL Server
* **Architecture:** Microservices-based

---

## 🔐 Security

* Sandboxed execution environment
* No direct host access
* Input validation & sanitization
* Role-based permissions
* AI output validation layer

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/online-compiler.git
cd online-compiler
```

---

### 2. Configure environment

Create:

```bash
webapp.config
```

Based on:

```bash
webapp.config.example
```

---

### 3. Run with Docker

```bash
docker-compose up --build
```

---

### 4. Start the application

```bash
dotnet run
```

---

## 📌 Future Enhancements

* Real-time collaboration (Google Docs style)
* Local AI model integration
* Additional language support (Java, C++)
* Plugin system
* CI/CD pipeline

---

## 👨‍💻 Author

Developed By Mouhamad Hammoud as an advanced full-stack + AI system project.

---

## 📄 License

MIT License
