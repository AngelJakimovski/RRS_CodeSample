
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/14/2015 14:07:19
-- Generated from EDMX file: D:\Projects\AARP\AARP.DAL\DbScopicAARP.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ScopicAARP];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Configurations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Configurations];
GO
IF OBJECT_ID(N'[dbo].[JobApplication]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobApplication];
GO
IF OBJECT_ID(N'[dbo].[JobPositionType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPositionType];
GO
IF OBJECT_ID(N'[dbo].[Reviewer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reviewer];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Configurations'
CREATE TABLE [dbo].[Configurations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(256)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Reviewers'
CREATE TABLE [dbo].[Reviewers] (
    [Id] int  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Email] nvarchar(256)  NOT NULL,
    [AssignedCount] int  NOT NULL,
    [RejectCount] int  NOT NULL,
    [AdvanceCount] int  NOT NULL
);
GO

-- Creating table 'JobApplications'
CREATE TABLE [dbo].[JobApplications] (
    [Id] int  NOT NULL,
    [CandidateId] nvarchar(256)  NOT NULL,
    [AppliedAt] datetime  NULL,
    [RejectAt] datetime  NULL,
    [LastActivity] datetime  NULL,
    [UrlCV] nvarchar(256)  NOT NULL,
    [ReviewerId] int  NULL,
    [RecorededAt] datetime  NOT NULL,
    [JobId] nvarchar(256)  NOT NULL,
    [ApplicationUrl] nvarchar(256)  NULL,
    [ReviewStatus] int  NOT NULL,
    [RejectionReason] nvarchar(max)  NULL,
    [RemindCount] int  NOT NULL,
    [CandidateName] nvarchar(256)  NULL,
    [JobName] nvarchar(256)  NULL
);
GO

-- Creating table 'JobPositionTypes'
CREATE TABLE [dbo].[JobPositionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Description] nvarchar(256)  NULL,
    [Interviewers] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [PK_Configurations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reviewers'
ALTER TABLE [dbo].[Reviewers]
ADD CONSTRAINT [PK_Reviewers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobApplications'
ALTER TABLE [dbo].[JobApplications]
ADD CONSTRAINT [PK_JobApplications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPositionTypes'
ALTER TABLE [dbo].[JobPositionTypes]
ADD CONSTRAINT [PK_JobPositionTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------