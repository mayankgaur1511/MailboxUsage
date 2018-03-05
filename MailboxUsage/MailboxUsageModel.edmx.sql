
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/05/2018 11:44:41
-- Generated from EDMX file: D:\Projects\Mailbox Usage\MailboxUsage\MailboxUsage\MailboxUsageModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MailboxUsage];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Mailboxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mailboxes];
GO
IF OBJECT_ID(N'[dbo].[SharedMailboxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SharedMailboxes];
GO
IF OBJECT_ID(N'[dbo].[Values]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Values];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UUID] nvarchar(max)  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Mailboxes'
CREATE TABLE [dbo].[Mailboxes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UMID] nvarchar(max)  NOT NULL,
    [ValueID] nvarchar(max)  NOT NULL,
    [LastUpdated] datetime  NULL,
    [UUID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SharedMailboxes'
CREATE TABLE [dbo].[SharedMailboxes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MailboxName] nvarchar(max)  NOT NULL,
    [MailboxEmail] nvarchar(max)  NOT NULL,
    [UMID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Values'
CREATE TABLE [dbo].[Values] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ValueID] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Mailboxes'
ALTER TABLE [dbo].[Mailboxes]
ADD CONSTRAINT [PK_Mailboxes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SharedMailboxes'
ALTER TABLE [dbo].[SharedMailboxes]
ADD CONSTRAINT [PK_SharedMailboxes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Values'
ALTER TABLE [dbo].[Values]
ADD CONSTRAINT [PK_Values]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------