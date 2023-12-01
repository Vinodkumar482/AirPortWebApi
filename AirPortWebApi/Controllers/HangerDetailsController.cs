using AirPortWebApi.Models;
using AirPortWebApi.Models.DataLayer;
using AirPortWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AirPortWebApi.Controllers
{
    [EnableCors("*","*","*")]
    public class HangerDetailsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [Route("api/HangerDetails/GetAvailableHangers")]
        public IHttpActionResult Get(string fromdate, string todate)
        {
            HangerDetailsDbOperations db = new HangerDetailsDbOperations();
            DateTime FD = DateTime.ParseExact(fromdate, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            DateTime TD = DateTime.ParseExact(todate, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            string formattedFromDate = FD.ToString("yyyy-MM-dd");
            DateTime parsedFromDate = DateTime.ParseExact(formattedFromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string formattedToDate = TD.ToString("yyyy-MM-dd");
            DateTime parsedToDate = DateTime.ParseExact(formattedToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //DateTime fromDateFormatted = DateTime.ParseExact(fromdate.ToString(),"dd-MM-yyyy", CultureInfo.InvariantCulture);
            //DateTime toDateFormatted = DateTime.ParseExact(todate.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            List<GetAvailableHangarsDetails1_Result> l = db.GetHangers(parsedFromDate, parsedToDate);
            if (l != null && l.Count > 0)
            {
                return Ok(l);
            }
            else

            {
                return Content(HttpStatusCode.BadRequest, l);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Route("api/HangerDetails/AddHanger")]
        public IHttpActionResult AddHanger([FromBody] Hanger h)
        {
            
            HangerDetailsDbOperations Hd = new HangerDetailsDbOperations();
            string s = Hd.AddHanger(h);
            List<string> list = s.Split(',').ToList();
            if (list[0].Equals("0"))
            {
                return Ok(list[1]);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, list[1]);
            }
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("api/HangerDetails/GetAvailablePlanes")]

        public IHttpActionResult GetPlanes(string fromdate, string todate)
        {
            HangerDetailsDbOperations hd = new HangerDetailsDbOperations();
            DateTime FD = DateTime.ParseExact(fromdate, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            DateTime TD = DateTime.ParseExact(todate, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            string formattedFromDate = FD.ToString("yyyy-MM-dd");
            DateTime parsedFromDate = DateTime.ParseExact(formattedFromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string formattedToDate = TD.ToString("yyyy-MM-dd");
            DateTime parsedToDate = DateTime.ParseExact(formattedToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            List<GetAvailablePlanes_Result> l = hd.GetAvailabePlanes(parsedFromDate, parsedToDate);
            if (l != null || l.Count > 0)
            { return Ok(l); }
            else
            {
                return Content(HttpStatusCode.BadRequest, l);
            }
        }
        [HttpPost]
        [Route("api/HangerDetails/AddBooking")]
        public IHttpActionResult AddBooking([FromBody] Booking b)
        {
            HangerDetailsDbOperations Hd = new HangerDetailsDbOperations();
            string s = Hd.AddBooking(b);
            List<string> list = s.Split(',').ToList();
            if (list[0].Equals("0"))
            {
                return Ok(list[1]);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, list[1]);
            }
        }
        [Route("api/HangerDetails/GetAllHangers")]
        public IHttpActionResult GetAllHangers()
        {
            HangerDetailsDbOperations h = new HangerDetailsDbOperations();
            List<HangerInfo> list = h.GetAllHangers();
            return Ok(list);
        }
        [HttpGet]
        [Route("api/HangerDetails/GetStatus")]
        public IHttpActionResult GetStatus(string HangerId, DateTime fromdate, DateTime todate)
        {
            HangerDetailsDbOperations h = new HangerDetailsDbOperations();
            List<BookingInfo> l = h.GetStatus(HangerId, fromdate, todate);
            return Ok(l);
        }
    }
}