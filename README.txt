
////////////////////
----REQUIREMENTS----
////////////////////
-Visual Studio
-SQL Server Management Studio
-.NET 4.5 (or above)

//////////////
----SET-UP----
//////////////
1. Run the 'Schema.sql' from the .Data project.
	a. This script can be ran again to drop the database (and subsequent tables) then re-creating them again.
2. Run the 'Seed.sql' from the .Data project.
	a. This script will populate the Lookup table with 'CountryCode', 'Country' and 'Unit' fields.
	b. This ensures that only correct 
3. Add a Login to LocalDB 'WeatherAdmin' with password 'Let1Me2In3'.
4. Grant the 'WeatherAdmin' login 'db_datareader' and 'db_datawriter' access to 'WeatherAPI' database.
	a. If the database is dropped, this will need to be applied again.

//////////////
----HOW-TO----
//////////////
The application works by holding the chosen locations in a local database table.
The CountryCode, CityName, Latitude and Longitude are all saved to the database.
These locations are then displayed in a table on the Home page.
There is not limit to the amount of locations you can add, although this could be implemented.
Clicking 'Load' in the Actions column will perform an ajax api call, using the latitude and longitude as parameters.
Clicking 'Update' in the Actions column will open a bootstrap modal allowing the user to update the location details.
Clicking 'Remove' in the Actions column will permanently remove the relevant row of data from the database.

///////////////
-----TO-DO-----
///////////////
--Highlights of what I would like to achieve with this application. 
--I either did not give myself the time to, I lacked the knowledge or I realised them in hindsight.

Obfuscation and generalisation of credentials; The API key & SQL Login details.
The API key is a unique key being used by the ajax calls. 
This should be stored in a more relevant location, such as a globally accessible Static class or a configuration file.
This key should not be passed to the source control.

Use StoredProcedures to insert and remove data. This can restrict the access required for each query.

Use Schemas for the tables to restrict the access of users and restrict the available actions.

Work on Serializing the JSON to DTO.

Add metric and imperial display. (Celcius or Farrenheit symbols...)

Create a Business Logic Layer project for the CRUD functionality.
Relate dependencies to one another; UI/Web references BLL/Logic, BLL references DAL/Data

Introduce a struct for Weather, to be used by Location.

Incorporate best practice SOLID principles;
- Use abstract classes where possible to encapsulate the common functionality.
--
- Design lightweight classes with only single responsibility. For any new responsibilities, create a new class.
-- 
- Interfaces; with common members.
-- Extract the ajax logic into separate interfaces. GetWeatherInfo(request), GetLocationCoords(request)...

Introduce UnitTesting to provide a check of the input fields.

Visually very lacking - structure, icons and clarity required for the visuals.
Logic first, design after. If possible, both at the same time.

///////////////
-----NOTES-----
///////////////

ISO3166 CountryCodes from: https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes
Sunrise/Sunset Timestamp conversion checks done using: https://www.epochconverter.com/

SRP: Single Responsibility Principle
Each module in an application should change only for a single reason.
Controllers soleyly responsibile for manipulating data in response to GUI-driven events.
If a new button is introduced, the controller should only change if that button is introducing a new feature.

OCP: Open/Closed Principle
Closed for modification, open for extension.
If a method returns a sum, the values of this sum can be modified without effecting the existing code. 
This can be achieved via abstraction, overriding methods to provide a different result.

LSP: Liskov Substitution Principle
Dervied types must be substitutable for their base types.
Replacing a base class object with the derived class objects.
Anywhere 'Rubber : Duck' is used, 'Duck' should be usable in it's place.

ISP: Interface Segregation Principle
Clients should not be forced to depend upon interfaces that they do not use.
The interface exposed to the client should contain only the methods required by the client, not all.
IOrganisation() { Finance_Method(); HR_Method(); Services_Method(); }
IFinance() { Finance_Method(); } -> FinanceDepartment ...
Every Employee has a 'Salary()' and 'Department()' but only Managers have 'Manage()'
interface IManager { public void Manage(); }
interface IEmployee { public void Salary(); public void Department(); }
class Manager : IEmployee, IManager { ...Manager, Salary, Department... }
class Executive: IEmployee { ...Salary, Department... }

DIP: Dependency Inversion Principle
High level modules should not depend upon low level modules. Both should depend upon abstractions.
Do not incorporate reliance on Concrete classes.
This allows the freedom of modifying classes without affecting anything else.
