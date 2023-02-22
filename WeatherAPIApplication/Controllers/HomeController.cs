using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using WeatherAPIApplication.Models.Core;
using WeatherAPIApplication.Models.ViewModels;
using System.Globalization;
using System.Web.Configuration;

namespace WeatherAPIApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherAPIEntities db = new WeatherAPIEntities();
        public ActionResult Index()
        {
            var allLocations = db.Locations.ToList().Select(l => PopulateLocation(l));
            return View(allLocations);
        }

        #region Create
        [HttpGet]
        public ActionResult AddLocation()
        {
            ViewBag.Units = GetUnits();
            ViewBag.CountryCodes = GetCountryCodes(); 
            return PartialView("_AddLocation", new LocationViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> AddLocation(LocationViewModel location)
        {
            if (location != null)
            {
                try
                {
                    var APIKey = WebConfigurationManager.AppSettings["APIKey"];
                    var unit = db.Lookups.Find(location.UnitID);
                    var cityName = location.CityName;
                    var countryCodeID = location.CountryCodeID;
                    var countryCode = db.Lookups.Where(l => l.id == countryCodeID).FirstOrDefault().value;
                    if (countryCode != null)
                        location.CountryCode = countryCode;
                    var latitude = decimal.Zero;
                    var longitude = decimal.Zero;
                    var state = string.Empty;
                    //Check that the supplied values are correct
                    //By making the API call and checking the response
                    var locationResults = GetLocationCoordinates(cityName, countryCode, APIKey);
                    if (locationResults != null)
                    {
                        latitude = decimal.Parse(locationResults.Latitude.ToString(), CultureInfo.InvariantCulture);
                        longitude = decimal.Parse(locationResults.Longitude.ToString(), CultureInfo.InvariantCulture);
                        cityName = locationResults.CityName;
                        countryCode = locationResults.Country;
                        state = locationResults.State;
                    }
                    else
                        return Json(new { success = false, responseText = "The City Name Location provided is not valid." }, JsonRequestBehavior.AllowGet);

                    //Prepare to add new Location to database, if it does not already exist
                    int locationID;
                    //Matching on Long/Lat required rounding, but always adjusted it to .00001 out. Substituted to match on cityName and countryCodeID instead.
                    var locationInstance = db.Locations.Where(l => l.name_city == cityName && l.id_country_code == countryCodeID).FirstOrDefault();
                    var preExistingLocation = locationInstance != null;
                    if (!preExistingLocation)
                    {
                        Location newLocation = new Location()
                        {
                            name_city = cityName,
                            id_unit = location.UnitID,
                            id_country_code = countryCodeID,
                            latitude = latitude,
                            longitude = longitude
                        };
                        try
                        {
                            locationID = db.Locations.Add(newLocation).id;
                        }
                        catch (Exception e)
                        {
                            _ = e.Message;
                            return PartialView("_AddLocation", location);
                        }
                    }
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _ = e.Message;
                    throw e;
                }
            }
            return Json(new { success = true });
        }
        #endregion
        #region Update
        public ActionResult UpdateLocation(int locationID)
        {
            ViewBag.Units = GetUnits();
            ViewBag.CountryCodes = GetCountryCodes();
            var location = db.Locations.Find(locationID);
            LocationViewModel existingLocation = new LocationViewModel(location);
            return PartialView("_UpdateLocation", existingLocation);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateLocation(LocationViewModel location)
        {
            var locationToUpdate = db.Locations.FirstOrDefault(l => l.id == location.ID);
            if(new LocationViewModel(locationToUpdate) != location)
            {
                var countryCode = db.Lookups.FirstOrDefault(l => l.id == location.CountryCodeID).value;
                var updatedCoordinates = GetLocationCoordinates(location.CityName, countryCode);
                if (locationToUpdate != null && updatedCoordinates != null)
                {
                    locationToUpdate.name_city = location.CityName;
                    locationToUpdate.id_country_code = location.CountryCodeID;
                    locationToUpdate.id_unit = location.UnitID;
                    locationToUpdate.latitude = updatedCoordinates.Latitude;
                    locationToUpdate.longitude = updatedCoordinates.Longitude;
                    try
                    {
                        await db.SaveChangesAsync();
                        return Json(new { success = true } );
                    }
                    catch (Exception ex)
                    {
                        _ = ex.Message;
                    }
                }
                return Json(new { success = false, responseText = "The City Name Location provided is not valid." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Please provide NEW location information" });
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<ActionResult> RemoveLocation(int locationID)
        {
            var location = db.Locations.Find(locationID);
            if(location != null)
            {
                db.Locations.Remove(location);
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _ = e.Message;
                return Json(new { error = true });
            }
            return Json(new { success = true });
        }
        #endregion

        #region Methods
        private dynamic GetUnits()
        {
            return db.Lookups.Where(l => l.type == "Unit").OrderBy(l => l.order).Select(l => new SelectListItem()
            {
                Text = l.value,
                Value = l.id.ToString()
            }).ToList();
        }
        private dynamic GetCountryCodes()
        {
            var countryCodes = db.Lookups.Where(l => l.type == "CountryCode" && l.is_active == true).OrderBy(l => l.order);
            foreach (var countryCode in countryCodes)
            {
                var countryName = db.Lookups.Where(l => l.type == countryCode.value).Select(l => l.value).FirstOrDefault();
                if (countryName != null)
                    countryCode.value = countryCode.value + ", " + countryName;
            }
            return countryCodes.ToList().OrderBy(l => l.order).Select(l => new SelectListItem()
            {
                Text = l.value,
                Value = l.id.ToString()
            }).ToList();
        }
        private DateTime ConvertToDateTime(long timestamp)
        {
            var datetime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
            return datetime;
        }
        private LocationViewModel PopulateLocation(Location location)
        {
            var unit = db.Lookups.Find(location.id_unit).value;
            var countryCode = db.Lookups.Find(location.id_country_code).value;
            return new LocationViewModel(location)
            {
                CountryCode = countryCode,
                Unit = unit
            };
        }
        private LocationDTO GetLocationCoordinates(string cityName, string countryCode, string APIKey = "35633ec4120856408e06ba13784583bc")
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync("https://api.openweathermap.org/geo/1.0/direct?q=" + cityName + "," + countryCode + "&limit=1&appid=" + APIKey);
            response.Wait();
            if (response.IsCompleted && !string.IsNullOrEmpty(response.Result.Trim(']','[')))
            {
                //Attempted to do parse to type LocationDTO, but the lat, long and name properties were not being picked up.
                //var results = JsonConvert.DeserializeObject<List<LocationDTO>>(response.Result).First();
                var results = JsonConvert.DeserializeObject<List<dynamic>>(response.Result).First();
                if (results != null)
                {
                    return new LocationDTO()
                    {
                        CityName = results["name"],
                        Country = results["country"],
                        State = results["state"],
                        Latitude = results["lat"],
                        Longitude = results["lon"]
                    };
                }
            }
            return null;
        }
        #endregion
    }
}