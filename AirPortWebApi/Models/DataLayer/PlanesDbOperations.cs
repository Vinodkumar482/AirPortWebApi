using AirPortWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace AirPortWebApi.Models.DataLayer
{
    public class PlanesDbOperations
    {
        public static AddressToClient GetAddress(string email)
        {
            AirportManagementEntities2 entities = new AirportManagementEntities2();
            var address = entities.Owners.FirstOrDefault(x => x.Email == email);
            if (address == null)
            {
                return null;
            }
            else
            {
                var details = entities.Addresses.FirstOrDefault(x => x.AddressId == address.AddressId);
                AddressToClient a = new AddressToClient();
                a.HouseNo = details.HouseNo;
                a.City = details.City;
                a.State = details.State;
                a.Country = details.Country;
                a.AddressLine = details.AddressLine;
                a.Id = details.Id;
                a.PinNo = details.PinNo;
                a.OwnerName=address.OwnerName;
                return a;
            }
        }
        public static int GetLastPlaneId()
        {
            AirportManagementEntities2 AE = new AirportManagementEntities2();
            var id = AE.Planes.OrderByDescending(item => item.Id).Take(1).FirstOrDefault();
            if (id == null)
            {
                return 101;
            }
            else
            {
                int? no = id.Id;
                no = no + 1;
                return (int)no;
            }
        }
        public static int GetIDforOwner()
        {
            AirportManagementEntities2 AE = new AirportManagementEntities2();
            var id = AE.Owners.OrderByDescending(item => item.OwnerId).FirstOrDefault();
            if (id == null)
            {
                return 104;
            }
            else
            {
                int? no = id.OwnerId;
                no = no + 1;
                return (int)no;
            }
        }
        public string AddPlane(PlaneDetails p)
        {
            try
            {
                using (var context = new AirportManagementEntities2())
                {
                    var owner = context.Owners.FirstOrDefault(x => x.Email == p.Email);
                    int Pid = GetLastPlaneId();
                    string PlaneId = p.PlaneName.Substring(0, Math.Min(p.PlaneName.Length, 3)).ToUpper() + Pid;
                    if (owner == null)
                    {
                        var Isaddress = context.Addresses.FirstOrDefault(x => x.HouseNo == p.HouseNo && x.City == p.City);
                        int OwnerId = GetIDforOwner();
                        if (Isaddress == null)
                        {
                            int Aid = AddressDbOPerations.GetLastAddressId();
                            string Address_id = p.City.Substring(0, Math.Min(p.City.Length, 3)).ToUpper() + Aid;
                            Address address = new Address();
                            address.HouseNo = p.HouseNo;
                            address.City = p.City;
                            address.Country = p.Country;
                            address.AddressLine = p.AddressLine;
                            address.State = p.State;
                            address.PinNo = p.PinNo;
                            address.AddressId = Address_id;
                            address.Id = Aid;
                            context.Addresses.Add(address);



                            Owner o = new Owner();
                            o.AddressId = Address_id;
                            o.Email = p.Email;
                            o.OwnerName = p.OwnerName;
                            o.OwnerId = OwnerId;
                            context.Owners.Add(o);
                            Plane plane = new Plane();

                            plane.ManufacturerName = p.ManufacturerName;
                            plane.PlaneName = p.PlaneName;
                            plane.ModelNo = p.ModelNo;
                            plane.RegNo = p.RegistrationNo;
                            plane.Capacity = p.Capacity;
                            plane.OwnerId = OwnerId;
                            plane.PlaneId = PlaneId;
                            plane.Id = Pid;
                            context.Planes.Add(plane);
                            context.SaveChanges();
                            return $"0,Plane added successfully: {PlaneId}";
                        }
                        else
                        {
                            Owner o = new Owner();
                            o.AddressId = Isaddress.AddressId;
                            o.Email = p.Email;
                            o.OwnerName = p.OwnerName;
                            o.OwnerId = OwnerId;
                            context.Owners.Add(o);
                            Plane plane = new Plane();

                            plane.ManufacturerName = p.ManufacturerName;
                            plane.PlaneName = p.PlaneName;
                            plane.ModelNo = p.ModelNo;
                            plane.RegNo = p.RegistrationNo;
                            plane.Capacity = p.Capacity;
                            plane.OwnerId = OwnerId;
                            plane.PlaneId = PlaneId;
                            plane.Id = Pid;
                            context.Planes.Add(plane);
                            context.SaveChanges();
                            return $"0,Plane added successfully: {PlaneId}";
                        }
                    }
                    else
                    {
                        Plane plane = new Plane();

                        plane.ManufacturerName = p.ManufacturerName;
                        plane.PlaneName = p.PlaneName;
                        plane.ModelNo = p.ModelNo;
                        plane.RegNo = p.RegistrationNo;
                        plane.Capacity = p.Capacity;
                        plane.OwnerId = owner.OwnerId;
                        plane.PlaneId = PlaneId;
                        plane.Id = Pid;
                        context.Planes.Add(plane);
                        context.SaveChanges();
                        return $"0,Plane added successfully: {PlaneId}";
                    }
                }
            }
            catch (DbUpdateException d)
            {
                SqlException s = d.GetBaseException() as SqlException;
                if (s.Message.Contains("IX_Planes"))
                {
                    return "1,registration number already exists";
                }
                else if (s.Message.Contains("PK_Planes"))
                {
                    return "1,Invalid planeId Generated";
                }
                else if (s.Message.Contains("FK_Planes_Owner"))
                {
                    return "1,Owner details no found";
                }
                else if (s.Message.Contains("IX_Owner"))
                {
                    return "1,Owner Email Already Exists";
                }
                else if (s.Message.Contains("PK_Owner"))
                {
                    return "1,Owner Details Already Exists";
                }
                else
                {
                    return "1,Unable to add owner";
                }
            }
            catch (DbEntityValidationException d)
            {
                string s = "";
                foreach (var eve in d.EntityValidationErrors)
                {

                    foreach (var ve in eve.ValidationErrors)
                    {
                        s = ("1,Error on- Property: \"{0}\"",
                             ve.PropertyName) + " " + s;
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