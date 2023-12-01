using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.Entities
{
    public class BookingInfo
    {
        public string HangerId { get; set; }
        public string HangerLocation { get; set; }
        public Nullable<int> HangerCapacity { get; set; }
        public string PlaneId { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
    }
}