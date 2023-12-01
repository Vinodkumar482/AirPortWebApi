using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.Entities
{
    public class PilotCls
    {
        public string PilotName { get; set; }
        public string LicenseNo { get; set; }
        public string SocialSecurityNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinNo { get; set; }
    }
}