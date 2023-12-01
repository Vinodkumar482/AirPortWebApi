using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.Entities
{
    public class AddressToClient
    {
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinNo { get; set; }
        public Nullable<int> Id { get; set; }

        public string OwnerName { get; set; }
        public string AddressLine { get; set; }
    }
}