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
Clicking 'Load' in the Actions column of the relevant row will perform an ajax api call, using the latitude and longitude as parameters.
//////////////////////////
-----PROCESS-AND-TIME-----
//////////////////////////
Analysing, planning and testing: 2 hours (1 hour was spent attempting to use the metoffice API)
Development: 10 hours. (1-2 hours spent on functionality that I have since removed && <= 1 hour was spent updating syntax for the modal (I'm used to an older Bootstrap syntax))
Clear-up, documentation, commenting and uploading: 1-2 hours

I wanted to feel confident, so I used what I am familiar with; MVC, SQL and Bootstrap. The latest versions (that still have LTS) were used.

MVC is an appropriate design pattern which helps achieve a clean separation of concerns. 
Separating the user interface, data and application logic.
The use of MVC makes implementing the SOLID principles affective and grants ease of management; 

EntityFramework (EF) is the method for database communication, I'm familiar with usin LINQ queries and haven't explored Dapper yet. So I was confident with this choice.
EF accommodates for SQL injection, as well as generation of entities, which helps to speed the process along. 
Also, the provision of a relationship diagram allows me to know if the relationship is set-up as expected.

Originally there was a 'Weather' table, which I felt would be used for storing and accessing the weather details.
In hindsight this would be a slow process, provide useless static data and not provide the required functionality.
The viewmodel for this was used for the headers of the Weather table, but it has now been left as hardcoded values.
Given more time I would probably reintroduce a viewmodel and/or a struct, so that the display values can be displayed. (With MetaData/Partial Classes)
Each LocationViewModel would have an instance of 'Weather'. This could extract the logic of converting timestamp to datetime to belong to Weather only.

The Bootstrap modal used for adding a location allows the user to select from the dropdowns provided, allowing these methods to only have a single responsibility.
The syntax for this was slightly different and this stumped me for a few minutes.

Caching was new to me, but when taking a step-back look at it, it's very clear that the object being returned can just be stored and accessed.
The caching time is set when it is accessed, allowing the lifespan to be adapted or grow depending on it's use.
This took me a couple of hours to get right, I managed to implement it into the existing functionality and this further allowed for a level of abstraction.
The api call requires an object to be passed to it, this can be either the cached response or not.
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