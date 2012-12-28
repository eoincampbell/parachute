IF NOT EXISTS (
	SELECT	* 
	FROM	INFORMATION_SCHEMA.TABLES 
	WHERE	TABLE_NAME = '__ParachuteSchemaChangeLogs'
	AND		TABLE_SCHEMA = 'dbo')
BEGIN
	PRINT 'Parachute Setup - Creating Table [dbo].[__ParachuteSchemaChangeLogs]'
	CREATE TABLE [dbo].[__ParachuteSchemaChangeLogs]
	(
		[ParachuteSchemaChangeLogId] [int] IDENTITY(1,1) NOT NULL,
		[MajorReleaseNumber] [varchar](2) NOT NULL,
		[MinorReleaseNumber] [varchar](2) NOT NULL,
		[PointReleaseNumber] [varchar](4) NOT NULL,
		[ScriptName] [nvarchar](200) NOT NULL,
		[DateApplied] [datetime2] DEFAULT (GetDate()) NOT NULL,
		[AppliedBy] [nvarchar](50) DEFAULT System_User NOT NULL

		CONSTRAINT [PK___ParachuteSchemaChangeLogs] PRIMARY KEY CLUSTERED ([ParachuteSchemaChangeLogId] ASC)
	)
END
ELSE
BEGIN
	PRINT 'Parachute Setup - [dbo].[__ParachuteSchemaChangeLog] Table Already Exists...'
END

IF NOT EXISTS (
	SELECT	* 
	FROM	INFORMATION_SCHEMA.TABLES 
	WHERE	TABLE_NAME = '__ParachuteAppliedScriptsLogs'
	AND		TABLE_SCHEMA = 'dbo')
BEGIN
	PRINT 'Parachute Setup - Creating Table [dbo].[__ParachuteAppliedScriptsLogs]'
	CREATE TABLE [dbo].[__ParachuteAppliedScriptsLogs]
	(
		[ParachuteAppliedScriptsLogId] [int] IDENTITY(1,1) NOT NULL,
		[ScriptName] [nvarchar](200) NOT NULL,
		[Hash] [nvarchar](32) NOT NULL,
		[SchemaVersion] [nvarchar](10) NOT NULL,
		[DateApplied] [datetime2] DEFAULT (GetDate()) NOT NULL,
		[AppliedBy] [nvarchar](50) DEFAULT System_User NOT NULL

		CONSTRAINT [PK___ParachuteAppliedScriptsLog] PRIMARY KEY CLUSTERED ([ParachuteAppliedScriptsLogId] ASC)
	)
END
ELSE
BEGIN
	PRINT 'Parachute Setup - [dbo].[__ParachuteAppliedScriptsLogs] Table Already Exists...'
END