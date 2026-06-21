<%@ Page Title="Community Feed" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Community.aspx.cs" Inherits="OnlineCompilerWebForms.pages.Community" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .community-container {
            max-width: 1200px;
            margin: 40px auto;
            color: #fff;
        }
        .section-title {
            text-align: center;
            margin-bottom: 40px;
            color: #00d2ff;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 2px;
        }

        /* 3D Owners Section */
        .owners-section {
            margin-bottom: 60px;
            perspective: 1000px;
            display: flex;
            justify-content: center;
            gap: 40px;
            flex-wrap: wrap;
        }
        .owner-card {
            width: 300px;
            height: 400px;
            position: relative;
            transform-style: preserve-3d;
            transition: transform 0.6s cubic-bezier(0.4, 0.2, 0.2, 1);
            cursor: pointer;
            animation: float 4s ease-in-out infinite;
        }
        @keyframes float {
            0% { transform: translateY(0px) rotateY(0deg); }
            50% { transform: translateY(-10px) rotateY(2deg); }
            100% { transform: translateY(0px) rotateY(0deg); }
        }
        .owner-card:hover {
            transform: rotateY(180deg) scale(1.05);
            animation: none;
        }
        .owner-card-face {
            position: absolute;
            width: 100%;
            height: 100%;
            backface-visibility: hidden;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 15px 35px rgba(0,0,0,0.5);
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            background: linear-gradient(145deg, #1e1e1e, #2a2a2a);
            border: 1px solid #3c3c3c;
        }
        .owner-card-front {
            background: linear-gradient(135deg, #2b5876 0%, #4e4376 100%);
        }
        .owner-card-back {
            transform: rotateY(180deg);
            background: #1e1e1e;
            padding: 20px;
            text-align: center;
        }
        .owner-avatar {
            width: 120px;
            height: 120px;
            border-radius: 50%;
            border: 4px solid #00d2ff;
            margin-bottom: 20px;
            background: #333;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 3em;
            color: #00d2ff;
        }
        .owner-name {
            font-size: 1.5em;
            font-weight: bold;
            color: #fff;
            margin: 0;
            text-shadow: 0 2px 4px rgba(0,0,0,0.5);
        }
        .owner-role {
            color: #00d2ff;
            font-size: 1.1em;
            margin-top: 5px;
        }
        .owner-bio {
            color: #bbb;
            font-size: 0.9em;
            line-height: 1.6;
        }

        /* Projects Grid */
        .projects-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
            gap: 25px;
        }
        .project-card {
            background: #1e1e1e;
            border-radius: 12px;
            overflow: hidden;
            border: 1px solid #333;
            transition: all 0.3s ease;
            display: flex;
            flex-direction: column;
            animation: fadeInUp 0.5s ease-out;
        }
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        .project-card:hover {
            transform: translateY(-8px) scale(1.02);
            border-color: #00d2ff;
            box-shadow: 0 15px 30px rgba(0, 210, 255, 0.2);
        }
        .project-image {
            width: 100%;
            height: 160px;
            object-fit: cover;
            background: linear-gradient(45deg, #2d2d2d, #1a1a1a);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 3em;
            color: #00d2ff;
        }
        .project-content {
            padding: 20px;
        }
        .project-title {
            font-size: 1.25em;
            color: #fff;
            margin-bottom: 10px;
            font-weight: bold;
        }
        .project-desc {
            color: #858585;
            flex-grow: 1;
            margin-bottom: 15px;
            font-size: 0.95em;
        }
        .project-meta {
            display: flex;
            justify-content: space-between;
            align-items: center;
            color: #aaa;
            font-size: 0.85em;
            margin-bottom: 15px;
        }
        .btn-view {
            background: transparent;
            border: 1px solid #00d2ff;
            color: #00d2ff;
            padding: 8px 15px;
            border-radius: 6px;
            text-align: center;
            text-decoration: none;
            transition: all 0.2s;
            display: inline-block;
        }
        .btn-view:hover {
            background: #00d2ff;
            color: #000;
            text-decoration: none;
        }
        .no-projects {
            text-align: center;
            color: #aaa;
            font-size: 1.2em;
            padding: 40px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="community-container">
        
        <!-- Owners Section -->
        <h2 class="section-title"><i class="fas fa-crown"></i> Platform Creators</h2>
        <div class="owners-section">
            
            <!-- Mouhamad Hammoud - Lead Teacher at MouSou -->
            <div class="owner-card">
                <div class="owner-card-face owner-card-front" style="background: linear-gradient(135deg, #00d2ff, #3a7bd5);">
                    <div class="owner-avatar"><i class="fas fa-chalkboard-teacher"></i></div>
                    <h3 class="owner-name">Mouhamad Hammoud</h3>
                    <p class="owner-role">Lead Teacher @ MouSou</p>
                </div>
                <div class="owner-card-face owner-card-back">
                    <h3 class="owner-name text-info mb-3">Mouhamad</h3>
                    <p class="owner-bio">Passionate educator and backend architect. Leads the MouSou academy, shaping the next generation of full-stack developers with real-world compiler technologies and AI integration.</p>
                    <div class="mt-4">
                        <i class="fab fa-github fa-2x mx-2 text-secondary"></i>
                        <i class="fab fa-linkedin fa-2x mx-2 text-secondary"></i>
                    </div>
                </div>
            </div>

           
            

        </div>

        <!-- Community Feed -->
        <h2 class="section-title mt-5"><i class="fas fa-globe"></i> Community Projects</h2>
        <div class="projects-grid">
            <asp:Repeater ID="rptProjects" runat="server">
                <ItemTemplate>
                    <div class="project-card">
                        <!-- Random image from LoremFlick or Font Awesome icon -->
                        <div class="project-image">
                            <i class="fas fa-code"></i>
                        </div>
                        <div class="project-content">
                            <div class="project-title"><i class="fas fa-code text-info me-2"></i><%# Eval("Name") %></div>
                            <div class="project-desc"><%# Eval("Description") %></div>
                            <div class="project-meta">
                                <span><i class="fas fa-layer-group"></i> <%# Eval("Language") %></span>
                                <span><i class="far fa-calendar-alt"></i> <%# Eval("CreatedAt", "{0:MMM dd, yyyy}") %></span>
                            </div>
                            <a href='/pages/Editor.aspx?id=<%# Eval("Id") %>' class="btn-view">View Code <i class="fas fa-arrow-right"></i></a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <asp:Literal ID="litNoProjects" runat="server" Visible="false">
            <div class="no-projects"><i class="fas fa-folder-open fa-3x mb-3 d-block"></i> No community projects available yet. <br /> Be the first to create one!</div>
        </asp:Literal>
    </div>

    <script>
        // Optional: Add dynamic random colors to project images
        document.querySelectorAll('.project-image').forEach(img => {
            const colors = ['#00d2ff', '#3a7bd5', '#11998e', '#38ef7d', '#ff6a00', '#ee0979'];
            const randomColor = colors[Math.floor(Math.random() * colors.length)];
            img.style.background = `linear-gradient(135deg, ${randomColor}, #1e1e1e)`;
        });
    </script>
</asp:Content>