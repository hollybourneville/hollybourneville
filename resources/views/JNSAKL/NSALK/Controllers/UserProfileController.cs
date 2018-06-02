using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using System.IO;

namespace NSALK
{

    public class UserProfileController : ApiController
    {
        [HttpPost]
        [Route("api/GetUserProfile")]
        public IHttpActionResult GetUserProfile([FromBody]NSALK.Models.UserProfile.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.UserProfile.SystemDataResponse outgoing = new Models.UserProfile.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.UserProfile.SystemDataResponseContacts contacts = new Models.UserProfile.SystemDataResponseContacts();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());

                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region UserProfile Info
                    var aContact = context.members.Where(x => x.recid == memberId).FirstOrDefault();
                    if (aContact != null)
                    {
                        NSALK.Models.UserProfile.SystemDataResponseContact contact = new Models.UserProfile.SystemDataResponseContact()
                        {
                            Recid = aContact.recid,
                            Firstname = aContact.firstname,
                            Middlename = aContact.middlename,
                            Lastname = aContact.lastname,
                            Gender = aContact.gender,
                            Physicaladdressunitnumber = aContact.physical_address_unit_number,
                            Physicaladdressnumber = aContact.physical_address_number,
                            Physicaladdressname = aContact.physical_address_name,
                            Physicaladdresssuburb = aContact.physical_address_suburb,
                            Physicaladdresspostcode = aContact.physical_address_postcode,
                            Postaladdressnumber = aContact.postal_address_number,
                            Postaladdressboxlobbylocation = aContact.postal_address_box_lobby_location,
                            Postaladdresssuburb = aContact.postal_address_suburb,
                            Postaladdresspostcode = aContact.postal_address_postcode,
                            Phonehome = aContact.phone_home,
                            Phonework = aContact.phone_work,
                            Phonecell = aContact.phone_cell,
                            Emailaddressdefault = aContact.email_address_default,
                            Emailaddressalternate = aContact.email_address_alternate,
                            Username = aContact.username,
                            Ipaddress = aContact.ip_address,
                            Remarks = aContact.remarks,
                            Occupation = aContact.occupation,
                            Otherremarks = aContact.other_remarks,
                            Lastupdatedby = aContact.last_updated_by,
                            Signupdate = (aContact.signup_date).ToString(),
                            Emergencyvolunteeringstatus = Convert.ToBoolean(aContact.emergency_volunteering_status).ToString(),
                            Mailingliststatus = Convert.ToBoolean(aContact.mailing_list_status).ToString(),
                            Status = Convert.ToBoolean(aContact.status).ToString(),
                            Birthdate = (aContact.birth_date).ToString(),
                            Lastupdateddate = (aContact.last_updated_date).ToString()
                        };
                        contacts.Contact.Add(contact);
                    }
                    outgoing.Items.Add(contacts);
                    #endregion
                }
            }
            return Ok(outgoing);
        }

        [HttpPost]
        [Route("api/RegisterUserProfile")]
        public IHttpActionResult RegisterUserProfile([FromBody]NSALK.Models.UserProfile.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.UserProfile.SystemDataResponse outgoing = new Models.UserProfile.SystemDataResponse();
            if (incoming != null)
            {
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                foreach (var cts in incoming.Contacts)
                {
                    NSALK.Models.UserProfile.SystemDataResponseContacts contacts = new Models.UserProfile.SystemDataResponseContacts();

                    foreach (var ct in cts.Contact)
                    {
                        NSALK.Models.UserProfile.SystemDataResponseContact contact = new Models.UserProfile.SystemDataResponseContact();
                        using (NSAKLEntities context = new NSAKLEntities())
                        {
                            #region Register User
                            var usr = context.users.Where(x => x.username == ct.Username).FirstOrDefault();
                            if (usr != null)
                                FriendlyException.RaiseExeption("Login name already in use please choose another one", "Error", HttpStatusCode.PreconditionFailed);


                            string newPassword = "xu2b1z";
                            if (!string.IsNullOrEmpty(ct.Password))
                            {
                                newPassword = ct.Password;
                            }
                            else
                                newPassword = NSALK.MvcApplication.RandomPassword.Generate(6);
                            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");

                            context.members.Add(new member
                            {
                                //recid = ct.recid,
                                firstname = !string.IsNullOrEmpty(ct.Firstname) ? ct.Firstname:"",
                                middlename = !string.IsNullOrEmpty(ct.Middlename) ? ct.Middlename:"",
                                lastname = !string.IsNullOrEmpty(ct.Lastname) ? ct.Lastname:"",
                                gender = !string.IsNullOrEmpty(ct.Gender) ? ct.Gender:"",
                                physical_address_unit_number = !string.IsNullOrEmpty(ct.Physicaladdressunitnumber) ? ct.Physicaladdressunitnumber:"",
                                physical_address_number = !string.IsNullOrEmpty(ct.Physicaladdressnumber) ? ct.Physicaladdressnumber:"",
                                physical_address_name = !string.IsNullOrEmpty(ct.Physicaladdressname) ? ct.Physicaladdressname:"",
                                physical_address_suburb = !string.IsNullOrEmpty(ct.Physicaladdresssuburb) ? ct.Physicaladdresssuburb:"",
                                physical_address_postcode = !string.IsNullOrEmpty(ct.Physicaladdresspostcode) ? ct.Physicaladdresspostcode:"",
                                postal_address_number = !string.IsNullOrEmpty(ct.Postaladdressnumber) ? ct.Postaladdressnumber:"",
                                postal_address_box_lobby_location = !string.IsNullOrEmpty(ct.Postaladdressboxlobbylocation) ? ct.Postaladdressboxlobbylocation:"",
                                postal_address_suburb = !string.IsNullOrEmpty(ct.Postaladdresssuburb) ? ct.Postaladdresssuburb:"",
                                postal_address_postcode = !string.IsNullOrEmpty(ct.Postaladdresspostcode) ? ct.Postaladdresspostcode:"",
                                phone_home = !string.IsNullOrEmpty(ct.Phonehome) ? ct.Phonehome:"",
                                phone_work = !string.IsNullOrEmpty(ct.Phonework) ? ct.Phonework:"",
                                phone_cell = !string.IsNullOrEmpty(ct.Phonecell) ? ct.Phonecell:"",
                                email_address_default = !string.IsNullOrEmpty(ct.Emailaddressdefault) ? ct.Emailaddressdefault:"",
                                email_address_alternate = !string.IsNullOrEmpty(ct.Emailaddressalternate) ? ct.Emailaddressalternate:"",
                                username = !string.IsNullOrEmpty(ct.Username) ? ct.Username:"",
                                ip_address = !string.IsNullOrEmpty(ct.Ipaddress) ? ct.Ipaddress:"",
                                remarks = !string.IsNullOrEmpty(ct.Remarks) ? ct.Remarks:"",
                                occupation = !string.IsNullOrEmpty(ct.Occupation) ? ct.Occupation:"",
                                other_remarks = !string.IsNullOrEmpty(ct.Otherremarks) ? ct.Otherremarks:"",
                                last_updated_by = !string.IsNullOrEmpty(ct.Lastupdatedby) ? ct.Lastupdatedby:"",
                                signup_date = !string.IsNullOrEmpty(ct.Signupdate) ? Convert.ToDateTime(ct.Signupdate) : DateTime.Now,
                                emergency_volunteering_status = !string.IsNullOrEmpty(ct.Emergencyvolunteeringstatus) ? Convert.ToBoolean(ct.Emergencyvolunteeringstatus): false,
                                mailing_list_status = !string.IsNullOrEmpty(ct.Mailingliststatus) ? Convert.ToBoolean(ct.Mailingliststatus): false,
                                status = !string.IsNullOrEmpty(ct.Status) ? Convert.ToBoolean(ct.Status):false,
                                birth_date = !string.IsNullOrEmpty(ct.Birthdate) ? Convert.ToDateTime(ct.Birthdate) : new DateTime(1900,01,01),
                                last_updated_date = DateTime.Now

                            });

                            if (MvcApplication.isValidEmail(ct.Emailaddressdefault))
                            {
                                string errorMessage = "";
                                string email = ct.Emailaddressdefault;
                                if (MvcApplication.isValidEmail(ct.Emailaddressdefault))
                                    email = ct.Emailaddressdefault;
                                else
                                    if (MvcApplication.isValidEmail(ct.Emailaddressalternate))
                                    email = ct.Emailaddressalternate;


                                if (MvcApplication.SendEmail(email, "NSAKL Registration", "Rgistered Successfully. Your username is :" + ct.Username + Environment.NewLine + "Your new password is" + Environment.NewLine + newPassword, false, ref errorMessage))
                                {
                                    contact.Username = ct.Username;
                                    var user = context.users.Add( new user{
                                        username = ct.Username,
                                        password = hashedPassword
                                    });
                                    context.SaveChanges();

                                }
                                else
                                    FriendlyException.RaiseExeption("An error occurred while sending email", "Error", HttpStatusCode.InternalServerError);
                            }
                            else
                            {
                                FriendlyException.RaiseExeption("User don't have a valid email address. Please contact " + MvcApplication.HELP_DESK_EMAIL, "Error", HttpStatusCode.PreconditionFailed);
                            }
                            #endregion
                        }
                        contacts.Contact.Add(contact);
                    }
                    outgoing.Items.Add(contacts);
                }
            }
            return Ok(outgoing);
        }
        [HttpPost]
        [Route("api/ChangeUserPassword")]
        public IHttpActionResult ChangeUserPassword([FromBody]NSALK.Models.UserProfile.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.UserProfile.SystemDataResponse outgoing = new Models.UserProfile.SystemDataResponse();
            if (incoming != null)
            {
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                foreach (var cts in incoming.Contacts)
                {
                    NSALK.Models.UserProfile.SystemDataResponseContacts contacts = new Models.UserProfile.SystemDataResponseContacts();

                    foreach (var ct in cts.Contact)
                    {
                        NSALK.Models.UserProfile.SystemDataResponseContact contact = new Models.UserProfile.SystemDataResponseContact();
                        using (NSAKLEntities context = new NSAKLEntities())
                        {
                            #region Update Password for User
                            var user = context.members.Where(x => x.recid == memberId).FirstOrDefault();

                            if (user == null)
                                FriendlyException.RaiseExeption("Login name does not exist", "Error", HttpStatusCode.PreconditionFailed);

                            if (MvcApplication.isValidEmail(user.email_address_default) || MvcApplication.isValidEmail(user.email_address_alternate))
                            {
                                string newPassword = "xu2b1z";
                                newPassword = ct.Password;
                                string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");

                                var usr = context.users.Where(x => x.username == user.username).FirstOrDefault();
                                usr.password = hashedPassword;


                                string errorMessage = "";
                                string email = ct.Emailaddressdefault;
                                if (MvcApplication.isValidEmail(user.email_address_default))
                                    email = user.email_address_default;
                                else
                                    if (MvcApplication.isValidEmail(user.email_address_alternate))
                                    email = user.email_address_alternate;


                                if (MvcApplication.SendEmail(email, "Your password changed", "Your password changed successfully at " + DateTime.Now.ToString("yyyy-dd-MM hh:mm:ss") + Environment.NewLine , false, ref errorMessage))
                                {
                                    contact.Username = user.username;
                                    context.SaveChanges();

                                }
                                else
                                    FriendlyException.RaiseExeption("An error occurred while sending email", "Error", HttpStatusCode.InternalServerError);
                            }
                            else
                            {
                                FriendlyException.RaiseExeption("User don't have a valid email address. Please contact " + MvcApplication.HELP_DESK_EMAIL, "Error", HttpStatusCode.PreconditionFailed);
                            }
                            #endregion
                        }
                        contacts.Contact.Add(contact);
                    }
                    outgoing.Items.Add(contacts);
                }
            }
            return Ok(outgoing);
        }

        [HttpPost]
        [Route("api/StartAGroup")]
        public IHttpActionResult StartAGroup([FromBody]NSALK.Models.UserProfile.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.UserProfile.SystemDataResponse outgoing = new Models.UserProfile.SystemDataResponse();
            if (incoming != null)
            {
                //int memberId = Convert.ToInt32(User.Identity.GetUserId());
                foreach (var cts in incoming.Contacts)
                {
                    NSALK.Models.UserProfile.SystemDataResponseContacts contacts = new Models.UserProfile.SystemDataResponseContacts();

                    foreach (var ct in cts.Contact)
                    {
                        NSALK.Models.UserProfile.SystemDataResponseContact contact = new Models.UserProfile.SystemDataResponseContact();
                        using (NSAKLEntities context = new NSAKLEntities())
                        {
                            #region Start A Group
                            if (MvcApplication.isValidEmail(ct.Emailaddressdefault))
                            {
                                string body = string.Empty;
                                string errorMessage = "Start a Group Message Failure";
                                string name = ct.Firstname;
                                string email = ct.Emailaddressdefault;
                                string phonenumber = ct.Phonecell;
                                string streetaddress = ct.Physicaladdressname;
                                string suburb = ct.Physicaladdresssuburb;
                                string city = ct.Physicaladdressunitnumber;
                                string region = ct.Physicaladdresspostcode;

                                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/StartaGroupEmailTemplate.html")))
                                {
                                    body = reader.ReadToEnd();
                                }
                                body = body.Replace("{Name}", name);
                                body = body.Replace("{Email}", email);
                                body = body.Replace("{PhoneNumber}", phonenumber); 
                                body = body.Replace("{StreetAddress}", streetaddress); 
                                body = body.Replace("{Suburb}", suburb);
                                body = body.Replace("{City}", city);
                                body = body.Replace("{Region}", region);

                                MvcApplication.SendEmail(MvcApplication.HELP_DESK_EMAIL, "Start a Group request from "+ name, body, true, ref errorMessage);
                            }
                            else
                            {
                                FriendlyException.RaiseExeption("Don't have a valid email address. Please contact office@nsakl.org.nz ", "Error", HttpStatusCode.PreconditionFailed);
                            }
                            #endregion
                        }
                        contacts.Contact.Add(contact);
                    }
                    outgoing.Items.Add(contacts);
                }
            }
            return Ok(outgoing);
        }
    }
    public class ForgotUserPasswordController : ApiController
    {
        [HttpPost]
        [Route("api/ForgotUserPassword")]
        public IHttpActionResult ForgotUserPassword([FromBody]NSALK.Models.UserProfile.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.UserProfile.SystemDataResponse outgoing = new Models.UserProfile.SystemDataResponse();
            if (incoming != null)
            {
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                foreach (var cts in incoming.Contacts)
                {
                    NSALK.Models.UserProfile.SystemDataResponseContacts contacts = new Models.UserProfile.SystemDataResponseContacts();

                    foreach (var ct in cts.Contact)
                    {
                        NSALK.Models.UserProfile.SystemDataResponseContact contact = new Models.UserProfile.SystemDataResponseContact();
                        using (NSAKLEntities context = new NSAKLEntities())
                        {
                            #region Update Password for User
                            var user = context.members.Where(x => x.username == ct.Username).FirstOrDefault();

                            if (user == null)
                                FriendlyException.RaiseExeption("Login name does not exist", "Error", HttpStatusCode.PreconditionFailed);

                            if (MvcApplication.isValidEmail(user.email_address_default) || MvcApplication.isValidEmail(user.email_address_alternate))
                            {
                                string newPassword = "xu2b1z";
                                newPassword = NSALK.MvcApplication.RandomPassword.Generate(6);
                                string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");

                                var usr = context.users.Where(x => x.username == ct.Username).FirstOrDefault();
                                usr.password = hashedPassword;


                                string errorMessage = "";
                                string email = ct.Emailaddressdefault;
                                if (MvcApplication.isValidEmail(user.email_address_default))
                                    email = user.email_address_default;
                                else
                                    if (MvcApplication.isValidEmail(user.email_address_alternate))
                                    email = user.email_address_alternate;


                                if (MvcApplication.SendEmail(email, "Your NSAKL password", "Your new NSAKL password is" + Environment.NewLine + newPassword, false, ref errorMessage))
                                {
                                    contact.Username = ct.Username;
                                    context.SaveChanges();

                                }
                                else
                                    FriendlyException.RaiseExeption("An error occurred while sending email", "Error", HttpStatusCode.InternalServerError);
                            }
                            else
                            {
                                FriendlyException.RaiseExeption("User don't have a valid email address. Please contact " + MvcApplication.HELP_DESK_EMAIL, "Error", HttpStatusCode.PreconditionFailed);
                            }
                            #endregion
                        }
                        contacts.Contact.Add(contact);
                    }
                    outgoing.Items.Add(contacts);
                }
            }
            return Ok(outgoing);
        }
    }
}
