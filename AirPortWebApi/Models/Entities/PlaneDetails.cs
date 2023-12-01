using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.Entities
{
    public class PlaneDetails
    {
        public string ManufacturerName { get; set; }
        public string RegistrationNo { get; set; }
        public string OwnerName { get; set; }
        public string ModelNo { get; set; }
        public string PlaneName { get; set; }
        public int Capacity { get; set; }
        public string Email { get; set; }
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string PinNo { get; set; }
    }
}