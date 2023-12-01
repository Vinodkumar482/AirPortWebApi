using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.DataLayer
{
    public class LoginDbOperations
    {
        public Login GetUser(string email)
        {
            AirportManagementEntities2 entities = new AirportManagementEntities2();
            return entities.Logins.FirstOrDefault(x => x.Email == email);
        }
    }
}