/*****************************************************************************

	Author:		Alan How for BeagleStreet Insurance Technical Test
	Date:		20/02/2023
	Purpose:	T-SQL queries for creating the relevant Database and
				its subsequent Tables
 
******************************************************************************
Script Operations:
	1) RE/CREATE 'WeatherAPI' DATABASE
	2) CREATE TABLES: 
		2.1 Lookup
		2.2 Location
		2.3 Weather

******************************************************************************/
/*****************************************************************************/
-- 1) RE/CREATE 'WeatherAPI' DATABASE
/*****************************************************************************/

WHILE EXISTS(SELECT NULL from sys.databases WHERE name = 'WeatherAPI')
BEGIN
	USE master --To stop the session from being used, so that it can be dropped.
    DECLARE @SQL VARCHAR(MAX)
    SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + CONVERT(VARCHAR, SPId) + ';'
    FROM MASTER..SysProcesses
    WHERE DBId = DB_ID(N'WeatherAPI') AND SPId <> @@SPId
    EXEC(@SQL)
    DROP DATABASE [WeatherAPI]
	IF (db_id(N'WeatherAPI') IS NULL)
		BEGIN
			PRINT 'The [WeatherAPI] database was removed succesfully.'
		END
END
GO

BEGIN
	CREATE DATABASE [WeatherAPI]
	PRINT 'The [WeatherAPI] database was created succesfully.'
END;
GO
/*****************************************************************************/
-- 2) CREATE TABLES
/*****************************************************************************/
USE [WeatherAPI]
-------------------------------------------------------------------------------
-- 2.1 Create 'Lookup' table
-------------------------------------------------------------------------------
CREATE TABLE [dbo].[Lookup]
(
	[id]			INT IDENTITY( 1, 1) NOT NULL,
	[order]			TINYINT NOT NULL,
	[type]			VARCHAR(50) NOT NULL,
	[value]			VARCHAR(MAX) NOT NULL,
	[is_active]		BIT DEFAULT 1,
	PRIMARY KEY CLUSTERED ([id] ASC)
)
GO
PRINT '[User] table was created'
-------------------------------------------------------------------------------
-- 2.2 Create 'Location' table
-------------------------------------------------------------------------------
CREATE TABLE [dbo].[Location]
(
	[id]				INT IDENTITY( 1, 1) NOT NULL,
	[id_unit]			INT NOT NULL,
	[id_country_code]	INT NOT NULL,
	[name_city]			VARCHAR(56) NOT NULL,
	[latitude]			DECIMAL(8,6) NOT NULL,
	[longitude]			DECIMAL(9,6) NOT NULL,
	PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_Location_id_country_code.Lookup_id] FOREIGN KEY ([id_country_code]) REFERENCES [Lookup] ([id]),
	CONSTRAINT [FK_Location_id_unit.Lookup_id] FOREIGN KEY ([id_unit]) REFERENCES [Lookup] ([id])
)
GO
PRINT '[Location] table was created'