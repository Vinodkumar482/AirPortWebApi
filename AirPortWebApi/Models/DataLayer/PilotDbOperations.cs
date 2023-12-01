using AirPortWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AirPortWebApi.Models.DataLayer
{
    public class PilotDbOperations
    {
        public static int GetLastPilotId()
        {
            AirportManagementEntities2 AE = new AirportManagementEntities2();
            var id = AE.Pilots.OrderByDescending(item => item.Id).Take(1).FirstOrDefault();
            if (id == null)
            {
                return 31;
            }
            else
            {
                int? no = id.Id;
                no = no + 1;
                return (int)no;
            }
        }
        public string AddingPilot(PilotCls p)
        {
            try
            {
                using (var pilot = new AirportManagementEntities2())
                {
                    var SsExists = pilot.Managers.FirstOrDefault(x => x.SSNo == p.SocialSecurityNo);
                    if (SsExists != null)
                    {
                        return "1,Social Security Exists";
                    }
                    else
                    {
                        var AddressId = pilot.Addresses.FirstOrDefault(x => x.HouseNo == p.HouseNo  && x.City.Equals(p.City, StringComparison.OrdinalIgnoreCase));
                        if (AddressId == null)
                        {
                            int Address_id = AddressDbOPerations.GetLastAddressId();
                            Address address = new Address();
                            address.HouseNo = p.HouseNo;
                            address.City = p.City;
                            address.AddressLine = p.AddressLine;
                            address.Country = p.Country;
                            address.State = p.State;
                            address.PinNo = p.PinNo;
                            string AId = p.City.Substring(0, Math.Min(p.City.Length, 3)).ToUpper() + Address_id;
                            address.AddressId = AId;
                            address.Id = Address_id;
                            pilot.Addresses.Add(address);
                            //pilot.SaveChanges();

                            Pilot Pobj = new Pilot();
                            Pobj.AddressId = AId;
                            Pobj.PilotName = p.PilotName;
                            Pobj.LicenceNo = p.LicenseNo;
                            Pobj.SSNo = p.SocialSecurityNo;
                            Pobj.Dob = p.DateOfBirth;
                            Pobj.Gender = p.Gender;
                            Pobj.Email = p.EmailAddress;
                            Pobj.MobileNo = p.MobileNo;
                            int PilotAddress_id = GetLastPilotId();
                            Pobj.PilotId = p.SocialSecurityNo.Substring(p.SocialSecurityNo.Length - 4).ToUpper() + PilotAddress_id;
                            Pobj.Id = PilotAddress_id;
                            pilot.Pilots.Add(Pobj);
                            pilot.SaveChanges();
                            return $"0,pilot generated with Id: {Pobj.PilotId}";
                        }
                        else
                        {

                            Pilot Pobj = new Pilot();
                            Pobj.AddressId = AddressId.AddressId;
                            Pobj.PilotName = p.PilotName;
                            Pobj.LicenceNo = p.LicenseNo;
                            Pobj.SSNo = p.SocialSecurityNo;//still need to check ssno is not there in manager too
                            Pobj.Dob = p.DateOfBirth;
                            Pobj.Gender = p.Gender;
                            Pobj.Email = p.EmailAddress;
                            Pobj.MobileNo = p.MobileNo;
                            int PilotAddress_id = GetLastPilotId();
                            Pobj.PilotId = p.SocialSecurityNo.Substring(p.SocialSecurityNo.Length - 4).ToUpper() + PilotAddress_id;
                            Pobj.Id = PilotAddress_id;
                            pilot.Pilots.Add(Pobj);
                            pilot.SaveChanges();
                            return $"0,pilot generated with Id: {Pobj.PilotId}";
                        }
                    }

                }
            }
            catch (DbUpdateException d)
            {
                SqlException s = d.GetBaseException() as SqlException;
                if (s.Message.Contains("PK_Pilot"))
                {
                    return "1,invalid pilotId";
                }
                else if (s.Message.Contains("UQ_Email"))
                {
                    return "1,Email address already exists";
                }
                else if (s.Message.Contains("UQ_MobileNo"))
                {
                    return "1,Mobile Number already exists";
                }
                else if (s.Message.Contains("UQ_LicenceNo"))
                {
                    return "1,License Number should be unique";
                }
                else if (s.Message.Contains("UQ_SSNo"))
                {
                    return "1,Security Social number should be unique";
                }
                else
                {
                    return "1,error occured";
                }
            }
            catch(DbEntityValidationException d)
            {
                string s = "";
                foreach (var eve in d.EntityValidationErrors)
                {
                    
                    foreach (var ve in eve.ValidationErrors)
                    {
                       s=("1,Error on- Property: \"{0}\"",
                            ve.PropertyName)+" "+s;
                    }
                }
                return s;
            }
            catch (AggregateException a)
            {
                return "1,try again later";
            }
            catch (Exception E)
            {
                return "1,Unknown error occured please try again later";
            }
        }
    }
}