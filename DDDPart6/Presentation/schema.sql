IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[failedClasses_to_Student]') AND OBJECTPROPERTY(id, N'ISFOREIGNKEY') = 1) ALTER TABLE [dbo].[failedClasses] DROP CONSTRAINT failedClasses_to_Student
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[passedClasses_to_Student]') AND OBJECTPROPERTY(id, N'ISFOREIGNKEY') = 1) ALTER TABLE [dbo].[passedClasses] DROP CONSTRAINT passedClasses_to_Student
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Registration_to_Student]') AND OBJECTPROPERTY(id, N'ISFOREIGNKEY') = 1) ALTER TABLE [dbo].[Registration] DROP CONSTRAINT Registration_to_Student
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Events]
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Aggregates]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Aggregates]
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Class]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Class]
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[failedClasses]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[failedClasses]
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[passedClasses]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[passedClasses]
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Student]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Student]
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Registration]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Registration]

CREATE TABLE [dbo].[Events] ( [Id] uniqueidentifier NOT NULL, aggregate_id uniqueidentifier NOT NULL, [version] int NOT NULL, [data] varbinary(4000) NOT NULL, PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]) ON [PRIMARY]
CREATE TABLE [dbo].[Aggregates] ( [aggregate_id] uniqueidentifier NOT NULL, [version] int NOT NULL, PRIMARY KEY CLUSTERED ([aggregate_id]) ON [PRIMARY]) ON [PRIMARY]
CREATE TABLE [dbo].[Class]([Id] [uniqueidentifier] NOT NULL,[version] [datetime] NOT NULL,[name] [nvarchar](255) NULL,[credits] [int] NULL, PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]) ON [PRIMARY]
CREATE TABLE [dbo].[failedClasses]([StudentFailed_Id] [uniqueidentifier] NOT NULL,[elt] [uniqueidentifier] NULL) ON [PRIMARY]
CREATE TABLE [dbo].[Student]([Id] [uniqueidentifier] NOT NULL,[version] [datetime] NOT NULL,[firstName] [nvarchar](255) NULL,[lastName] [nvarchar](255) NULL,[hasGraduated] [bit] NULL,[credits] [int] NULL,[registrationSequence] [int] NULL, PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]) ON [PRIMARY]
CREATE TABLE [dbo].[passedClasses]([StudentPassed_Id] [uniqueidentifier] NOT NULL, [elt] [uniqueidentifier] NULL) ON [PRIMARY]
CREATE TABLE [dbo].[Registration]([aggregateRoot] [uniqueidentifier] NOT NULL,[Id] [int] NOT NULL,[version] [datetime] NOT NULL,[classId] [uniqueidentifier] NULL,[classCredits] [int] NULL, PRIMARY KEY CLUSTERED ([aggregateRoot] ASC, [Id] ASC) ON [PRIMARY]) ON [PRIMARY]
ALTER TABLE [dbo].[failedClasses] ADD CONSTRAINT [failedClasses_to_Student] FOREIGN KEY([StudentFailed_Id]) REFERENCES [dbo].[Student] ([Id])
ALTER TABLE [dbo].[passedClasses] ADD CONSTRAINT [passedClasses_to_Student] FOREIGN KEY([StudentPassed_Id]) REFERENCES [dbo].[Student] ([Id])
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD  CONSTRAINT [Registration_to_Student] FOREIGN KEY([aggregateRoot]) REFERENCES [dbo].[Student] ([Id])