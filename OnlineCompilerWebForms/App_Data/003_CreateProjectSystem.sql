IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Projects' AND xtype='U')
BEGIN
    CREATE TABLE Projects (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Language NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),
        UpdatedAt DATETIME DEFAULT GETDATE(),
        IsDeleted BIT DEFAULT 0,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Files' AND xtype='U')
BEGIN
    CREATE TABLE Files (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        ProjectId UNIQUEIDENTIFIER NOT NULL,
        FileName NVARCHAR(255) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        IsEntryPoint BIT DEFAULT 0,
        CreatedAt DATETIME DEFAULT GETDATE(),
        UpdatedAt DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
    );
END
