using AirPortWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Xml;
 
namespace AirPortWebApi.Models.DataLayer
{
    public class HangerDetailsDbOperations
    {
        public static int GetLastHangerId()
        {
            AirportManagementEntities2 AE = new AirportManagementEntities2();
            var id = AE.HangerDetails.OrderByDescending(item => item.Id).Take(1).FirstOrDefault();
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
        public static int GetLastManagerId()
        {
            AirportManagementEntities2 AE = new AirportManagementEntities2();
            var id = AE.Managers.OrderByDescending(item => item.Id).Take(1).FirstOrDefault();
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
        public string AddHanger(Hanger H)
        {
            try
            {
                using (var Hd = new AirportManagementEntities2())
                {
                    var SsExists = Hd.Pilots.FirstOrDefault(x => x.SSNo == H.SocialSecuirtyNo);
                    if (SsExists != null)
                    {
                        return "1,Social Security Exists";
                    }
                    else
                    {
                        var ManagerDetails = Hd.Managers.FirstOrDefault(x => x.Email == H.Email && x.MobileNo == H.MobileNo && x.SSNo == H.SocialSecuirtyNo);
                        int HUniqueId = GetLastHangerId();
                        int MUniqueId = GetLastManagerId();
                        string Hid = H.HangerLocation.Substring(0, Math.Min(H.HangerLocation.Length, 3)).ToUpper() + GetLastHangerId();
                        HangerDetail h = new HangerDetail();
                        if (ManagerDetails == null)
                        {
                            var AddressId = Hd.Addresses.FirstOrDefault(x => x.HouseNo == H.HouseNo && x.City == H.City);
                            if (AddressId == null)
                            {
                                int Address_id = AddressDbOPerations.GetLastAddressId();
                                Address address = new Address();
                                address.HouseNo = H.HouseNo;
                                address.City = H.City;
                                address.Country = H.Country;
                                address.State = H.State;
                                address.AddressLine = H.AddressLine;
                                address.PinNo = H.PinNo;
                                address.Id = Address_id;
                                string AId = H.City.Substring(0, Math.Min(H.City.Length, 3)).ToUpper() + Address_id;
                                address.AddressId = AId;
                                Hd.Addresses.Add(address);
                                Hd.SaveChanges();
                                string Mid = H.SocialSecuirtyNo.Substring(H.SocialSecuirtyNo.Length - 4).ToUpper() + MUniqueId;
                                Manager m = new Manager();
                                m.ManagerId = Mid;
                                m.ManagerName = H.ManagerName;
                                m.SSNo = H.SocialSecuirtyNo;
                                m.Dob = H.Dob;
                                m.Gender = H.Gender;
                                m.Email = H.Email;
                                m.MobileNo = H.MobileNo;
                                m.AddressId = AId;
                                m.Id = MUniqueId;
                                Hd.Managers.Add(m);
                                Hd.SaveChanges();
                                h.ManagerId = Mid;
                                h.HangerId = Hid;
                                h.HangerLocation = H.HangerLocation;
                                h.HangerCapacity = H.HangerCapacity;
                                h.Id = HUniqueId;
                                Hd.HangerDetails.Add(h);
                                Hd.SaveChanges();
                                return $"0,hanger added {Hid}";

                            }
                            else
                            {
                                string Mid = H.SocialSecuirtyNo.Substring(H.SocialSecuirtyNo.Length - 4).ToUpper() + MUniqueId;
                                Manager m = new Manager();
                                m.ManagerId = Mid;
                                m.ManagerName = H.ManagerName;
                                m.SSNo = H.SocialSecuirtyNo;
                                m.Dob = H.Dob;
                                m.Gender = H.Gender;
                                m.Email = H.Email;
                                m.MobileNo = H.MobileNo;
                                m.AddressId = AddressId.AddressId;
                                m.Id = MUniqueId;
                                Hd.Managers.Add(m);
                                Hd.SaveChanges();
                                h.ManagerId = Mid;
                                h.HangerId = Hid;
                                h.HangerLocation = H.HangerLocation;
                                h.HangerCapacity = H.HangerCapacity;
                                h.Id = HUniqueId;
                                Hd.HangerDetails.Add(h);
                                Hd.SaveChanges();
                                return $"0,hanger added {Hid}";

                            }
                        }
                        else
                        {
                            h.ManagerId = ManagerDetails.ManagerId;
                            h.HangerId = Hid;
                            h.HangerLocation = H.HangerLocation;
                            h.HangerCapacity = H.HangerCapacity;
                            h.Id = HUniqueId;
                            Hd.HangerDetails.Add(h);
                            Hd.SaveChanges();
                            return $"0,hanger added {Hid}";
                        }
                    }
                }
            }
            catch (DbUpdateException d)
            {
                SqlException s = d.GetBaseException() as SqlException;
                if (s.Message.Contains("PK_HangerDetails"))
                {
                    return "1,invalid HangerId";

                }
                else if (s.Message.Contains("FK_HangerDetails_Manager"))
                {
                    return "1,check manager details";
                }
                else if (s.Message.Contains("PK_Manager"))
                {
                    return "1,Manager Id";
                }
                else if (s.Message.Contains("FK_Manager_Address"))
                {
                    return "1,Check address Details";
                }
                else if (s.Message.Contains("UQ_MEmail"))
                {
                    return "1,Email Already Exists";
                }
                else if (s.Message.Contains("UQ_MMobileNo"))
                {
                    return "1,Mobile Number Already Exists";
                }
                else if (s.Message.Contains("UQ_MSSNo"))
                {
                    return "1,Social Security number Already Exists";
                }
                else
                {
                    return "Unable to add Hanger";
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

        public List<GetAvailableHangarsDetails1_Result> GetHangers(DateTime? fromdate, DateTime? todate)
        {
            AirportManagementEntities2 Ae = new AirportManagementEntities2();
            return Ae.GetAvailableHangarsDetails1(fromdate, todate).ToList();
        }

        public List<GetAvailablePlanes_Result> GetAvailabePlanes(DateTime fromdate, DateTime todate)
        {
            AirportManagementEntities2 Ae = new AirportManagementEntities2();
            return Ae.GetAvailablePlanes(fromdate, todate).ToList();

        }
        public string AddBooking(Booking b)
        {
            try
            {
                AirportManagementEntities2 Ae = new AirportManagementEntities2();
                int bookingCount = Ae.Bookings
                    .Where(x =>
                        x.HangerId == b.HangerId &&
                        b.FromDate<=x.ToDate &&
                        b.ToDate>=x.FromDate)
                    .Count();
                int capacity =(int) Ae.HangerDetails.FirstOrDefault(x => x.HangerId == b.HangerId).HangerCapacity;
                if(capacity>bookingCount)
                {
                    Ae.Bookings.Add(b);
                    Ae.SaveChanges();
                    return $"0,Booking added from {b.FromDate} to {b.ToDate} for plane {b.PlaneId} in hanger {b.HangerId}";
                }
                else
                {
                    return $"1,Hanger full please select another hanger";
                }
            }
            catch (DbUpdateException d)
            {
                SqlException s = d.GetBaseException() as SqlException;
                if (s.Message.Contains("PK_Bookings"))
                {
                    return "1,Invalid BookingId";
                }
                else if (s.Message.Contains("FK_Bookings_HangerDetails"))
                {
                    return "1,Invalid Hanger";
                }
                else if (s.Message.Contains("FK_Bookings_Planes"))
                {
                    return "1,Invalid Plane";
                }
                else
                {
                    return "Unable to Book Hanger";
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
        public List<HangerInfo> GetAllHangers()
        {


            AirportManagementEntities2 Ae = new AirportManagementEntities2();
            List<HangerInfo> selectedHangers = Ae.HangerDetails.Select(h => new HangerInfo
            {
                HangerId = h.HangerId,
                HangerLocation = h.HangerLocation,
                HangerCapacity = (int)h.HangerCapacity
            }).ToList();

            return selectedHangers;
        }

        public List<BookingInfo> GetStatus(string HangerId, DateTime fromdate, DateTime todate)
        {
            AirportManagementEntities2 Ae = new AirportManagementEntities2();
            var bookingInfoList = Ae.Bookings
                .Where(booking => booking.FromDate <= todate && fromdate <= booking.ToDate && booking.HangerId == HangerId) // Example condition
                .Select(booking => new BookingInfo
                {
                    HangerId = booking.HangerId,
                    HangerLocation = booking.HangerDetail.HangerLocation,
                    HangerCapacity = booking.HangerDetail.HangerCapacity,
                    PlaneId = booking.PlaneId,
                    FromDate = booking.FromDate,
                    ToDate = booking.ToDate
                })
                .ToList();

            return bookingInfoList;
        }
    }
}