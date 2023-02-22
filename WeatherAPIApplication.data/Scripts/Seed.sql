/*****************************************************************************

	Author:		Alan How for BeagleStreet Insurance Technical Test
	Date:		20/02/2023
	Purpose:	T-SQL queries for populating the subsequent tables of the WeatherAPI Database
 
******************************************************************************
Script Operations:
	1) POPULATE TABLES: 
	1.1 Lookup,
	1.2 Location,
	1.3 Weather
******************************************************************************/

/*****************************************************************************/
-- 1) POPULATE TABLES
/*****************************************************************************/
USE [WeatherAPI]
-------------------------------------------------------------------------------
-- 1.1 Populate 'Lookup' table
-------------------------------------------------------------------------------
BEGIN TRY
	BEGIN TRANSACTION
		INSERT INTO [Lookup] ([order], [type], [value])
		VALUES
		(1, 'Unit', 'Metric'),
		(2, 'Unit', 'Imperial'),
		(1, 'CountryCode', 'GB'),
		(2, 'CountryCode', 'US'),
		(3, 'CountryCode', 'UY'),
		(4, 'CountryCode', 'AE'),
		(1, 'GB', 'Great Britain'),
		(1, 'US', 'United States of America'),
		(1, 'UY', 'Uruguay'),
		(1, 'AE', 'United Arab Emirates')
	COMMIT TRANSACTION
	PRINT 'The [Lookup] table was populated successfully'
END TRY
BEGIN CATCH
	PRINT  'There was an error while inserting sample Lookup data: ' + ERROR_MESSAGE()
	ROLLBACK TRANSACTION
END CATCH