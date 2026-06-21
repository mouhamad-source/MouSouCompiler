-- 002_CreatePasswordReset.sql
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PasswordResets]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PasswordResets] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [UserId] UNIQUEIDENTIFIER NOT NULL,
        [Token] NVARCHAR(512) NOT NULL,
        [ExpiryDate] DATETIME NOT NULL,
        [IsUsed] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [FK_PasswordResets_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE
    );
END
