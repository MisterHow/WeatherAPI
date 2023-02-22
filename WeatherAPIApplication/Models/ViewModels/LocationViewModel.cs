using System.ComponentModel.DataAnnotations;
using WeatherAPIApplication.Models.Core;

namespace WeatherAPIApplication.Models.ViewModels
{
    public class LocationViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "{0} is required"), Display(Name = "Unit")]
        public int UnitID { get; set; }
        public string Unit { get; set; }
        [Required(ErrorMessage = "{0} is required"), Display(Name = "City Name"), 
            RegularExpression("^[a-zA-Z][a-zA-Z ]+[a-zA-Z]$", ErrorMessage = "Please Enter Alphabetic Values only")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "{0} is required"), Display(Name = "Country Code")]
        public int CountryCodeID { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public LocationViewModel() { }
        public LocationViewModel(Location location)
        {
            ID = location.id;
            UnitID = location.id_unit;
            CityName = location.name_city;
            CountryCodeID = location.id_country_code;
            Latitude = location.latitude;
            Longitude = location.longitude;
        }
    }
}