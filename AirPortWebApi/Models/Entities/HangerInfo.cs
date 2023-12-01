using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.Entities
{
    public class HangerInfo
    {
        public string HangerId { get; set; }
        public string HangerLocation { get; set; }

        public int HangerCapacity { get; set; }
    }
}