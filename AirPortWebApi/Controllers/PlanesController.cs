using AirPortWebApi.Models.DataLayer;
using AirPortWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AirPortWebApi.Controllers
{
    [EnableCors("*","*","*")]
    public class PlanesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [Route("api/Planes/GetAddress")]
        public IHttpActionResult Get(string email)
        {
            AddressToClient a = PlanesDbOperations.GetAddress(email);
            if (a != null)
            {
                return Ok(a);
            }
            else
            {
                return Content<AddressToClient>(HttpStatusCode.BadRequest, null);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Route("api/Planes/AddPlane")]
        public IHttpActionResult AddPlane([FromBody] PlaneDetails p)
        {
            PlanesDbOperations dB = new PlanesDbOperations();
            string s = dB.AddPlane(p);
            List<string> list = s.Split(',').ToList();
            if (list[0] == "0")
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
    }
}