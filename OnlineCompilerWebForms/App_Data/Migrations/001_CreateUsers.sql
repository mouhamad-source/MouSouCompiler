-- 001_CreateUsers.sql
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Username] NVARCHAR(256) NOT NULL UNIQUE,
        [Email] NVARCHAR(256) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(MAX) NOT NULL,
        [Salt] NVARCHAR(256) NULL, -- if needed separately, though often combined in hash

        [IsEmailConfirmed] BIT NOT NULL DEFAULT 0,
        [EmailConfirmationToken] NVARCHAR(512) NULL,
        [EmailTokenExpiry] DATETIME NULL,

        [FailedLoginAttempts] INT NOT NULL DEFAULT 0,
        [IsLocked] BIT NOT NULL DEFAULT 0,
        [LockoutEnd] DATETIME NULL,

        [Role] NVARCHAR(50) NOT NULL DEFAULT 'User',

        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
