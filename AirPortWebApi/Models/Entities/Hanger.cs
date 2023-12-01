using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.Entities
{
    public class Hanger
    {
        public Hanger() { }
        public string HangerLocation {get; set;}
        public int HangerCapacity { get; set;}
        public string ManagerName { get; set;}
        public string SocialSecuirtyNo { get; set;}
        public DateTime Dob { get; set;}
        public string Gender { get; set;}
        public string MobileNo { get; set;}
        public string Email { get; set;}
        public string HouseNo { get; set;}
        public string AddressLine { get; set;}
        public string City { get; set;}
        public string State { get; set;}
        public string Country { get; set;}
        public string PinNo { get; set;}


    }
}