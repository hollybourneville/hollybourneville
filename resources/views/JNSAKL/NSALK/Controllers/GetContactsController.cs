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

namespace NSALK
{
    [Authorize]
    public class GetContactsController : ApiController
    {
        [HttpPost]
        [Route("api/GetContacts")]
        public IHttpActionResult Post([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();


                List<group_members> groups = new List<group_members>();
                List<member> adminContacts = new List<member>();

                using (NSAKLEntities context = new NSAKLEntities())
                {
                    if (role == "Admin")
                    {
                        adminContacts = context.members.ToList();
                        if (adminContacts != null)
                            #region Get Admin Contacts
                            foreach (var aContact in adminContacts)
                            {

                                NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                {
                                    Recid = aContact.recid,
                                    Firstname = aContact.firstname,
                                    Middlename = aContact.middlename,
                                    Lastname = aContact.lastname,
                                    Gender = aContact.gender,
                                    Physical_address_unit_number = aContact.physical_address_unit_number,
                                    Physical_address_number = aContact.physical_address_number,
                                    Physical_address_name = aContact.physical_address_name,
                                    Physical_address_suburb = aContact.physical_address_suburb,
                                    Physical_address_postcode = aContact.physical_address_postcode,
                                    Postal_address_number = aContact.postal_address_number,
                                    Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                    Postal_address_suburb = aContact.postal_address_suburb,
                                    Postal_address_postcode = aContact.postal_address_postcode,
                                    Phone_home = aContact.phone_home,
                                    Phone_work = aContact.phone_work,
                                    Phone_cell = aContact.phone_cell,
                                    Email_address_default = aContact.email_address_default,
                                    Email_address_alternate = aContact.email_address_alternate,
                                    Username = aContact.username,
                                    Ip_address = aContact.ip_address,
                                    Remarks = aContact.remarks,
                                    Occupation = aContact.occupation,
                                    Other_remarks = aContact.other_remarks,
                                    Last_updated_by = aContact.last_updated_by,
                                    Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                    Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                    Mailing_list_status = aContact.mailing_list_status,
                                    Status = aContact.status,
                                    Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                    Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                };
                                contacts.Contact.Add(contact);
                            }
                        outgoing.Items.Add(contacts);
                        #endregion

                    }
                    if (role == "Coordinator")
                    {
                        groups = context.group_members.Where(x => x.member_id == memberId).ToList();
                        if (groups != null)
                            #region Get Coordinator Contacts
                            foreach (var grp in groups)
                            {
                                var grpmembers = context.group_members.Where(x => x.member_id == grp.member_id).ToList();
                                if (grpmembers != null)
                                    foreach (var gpmem in grpmembers)
                                    {
                                        var aContact = context.members.Where(x => x.recid == gpmem.member_id).FirstOrDefault();
                                        if (aContact != null)
                                        {
                                            NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                            {
                                                Recid = aContact.recid,
                                                Firstname = aContact.firstname,
                                                Middlename = aContact.middlename,
                                                Lastname = aContact.lastname,
                                                Gender = aContact.gender,
                                                Physical_address_unit_number = aContact.physical_address_unit_number,
                                                Physical_address_number = aContact.physical_address_number,
                                                Physical_address_name = aContact.physical_address_name,
                                                Physical_address_suburb = aContact.physical_address_suburb,
                                                Physical_address_postcode = aContact.physical_address_postcode,
                                                Postal_address_number = aContact.postal_address_number,
                                                Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                                Postal_address_suburb = aContact.postal_address_suburb,
                                                Postal_address_postcode = aContact.postal_address_postcode,
                                                Phone_home = aContact.phone_home,
                                                Phone_work = aContact.phone_work,
                                                Phone_cell = aContact.phone_cell,
                                                Email_address_default = aContact.email_address_default,
                                                Email_address_alternate = aContact.email_address_alternate,
                                                Username = aContact.username,
                                                Ip_address = aContact.ip_address,
                                                Remarks = aContact.remarks,
                                                Occupation = aContact.occupation,
                                                Other_remarks = aContact.other_remarks,
                                                Last_updated_by = aContact.last_updated_by,
                                                Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                                Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                                Mailing_list_status = aContact.mailing_list_status,
                                                Status = aContact.status,
                                                Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                                Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                            };
                                            contacts.Contact.Add(contact);
                                        }
                                    }
                            }
                        outgoing.Items.Add(contacts);
                        #endregion
                    }
                    if (role == "Member")
                    {
                        groups = context.group_members.Where(x => x.member_id == memberId).ToList();
                        if (groups != null)
                            #region Get Member Contacts
                            foreach (var grp in groups)
                            {
                                var grpmembers = context.group_members.Where(x => x.group_id == grp.group_id).ToList(); 
                                if (grpmembers != null)
                                    foreach (var gpmem in grpmembers)
                                    {
                                        var aContact = context.members.Where(x => x.recid == gpmem.member_id).FirstOrDefault();
                                        if (aContact != null)
                                        {
                                            NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                            {
                                                Recid = aContact.recid,
                                                Firstname = aContact.firstname,
                                                Middlename = aContact.middlename,
                                                Lastname = aContact.lastname,
                                                Gender = aContact.gender,
                                                Physical_address_unit_number = aContact.physical_address_unit_number,
                                                Physical_address_number = aContact.physical_address_number,
                                                Physical_address_name = aContact.physical_address_name,
                                                Physical_address_suburb = aContact.physical_address_suburb,
                                                Physical_address_postcode = aContact.physical_address_postcode,
                                                Postal_address_number = aContact.postal_address_number,
                                                Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                                Postal_address_suburb = aContact.postal_address_suburb,
                                                Postal_address_postcode = aContact.postal_address_postcode,
                                                Phone_home = aContact.phone_home,
                                                Phone_work = aContact.phone_work,
                                                Phone_cell = aContact.phone_cell,
                                                Email_address_default = aContact.email_address_default,
                                                Email_address_alternate = aContact.email_address_alternate,
                                                Username = aContact.username,
                                                Ip_address = aContact.ip_address,
                                                Remarks = aContact.remarks,
                                                Occupation = aContact.occupation,
                                                Other_remarks = aContact.other_remarks,
                                                Last_updated_by = aContact.last_updated_by,
                                                Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                                Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                                Mailing_list_status = aContact.mailing_list_status,
                                                Status = aContact.status,
                                                Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                                Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                            };
                                            contacts.Contact.Add(contact);
                                        }
                                    }
                            }
                        outgoing.Items.Add(contacts);
                        #endregion
                    }
                }
            }
            return Ok(outgoing);
        }
        [HttpPost]
        [Route("api/GetContactsAdmin")]
        public IHttpActionResult GetContactsAdmin([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                int selectedGroupId = 0;
                foreach (var items in incoming.Items)
                {
                    foreach (var ct in items.Contact)
                    {
                        selectedGroupId = ct.SelectedGroup;
                    }
                }
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();


                List<group_members> groups = new List<group_members>();
                List<member> adminContacts = new List<member>();

                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region GetContactsAdmin
                    var grpmembers = context.group_members.Where(x => x.group_id == selectedGroupId).ToList();
                    foreach (var gpmem in grpmembers)
                    {
                        var aContact = context.members.Where(x => x.recid == gpmem.member_id).FirstOrDefault();
                        if (aContact != null)
                        {
                            NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                            {
                                Recid = aContact.recid,
                                Firstname = aContact.firstname,
                                Middlename = aContact.middlename,
                                Lastname = aContact.lastname,
                                Gender = aContact.gender,
                                Physical_address_unit_number = aContact.physical_address_unit_number,
                                Physical_address_number = aContact.physical_address_number,
                                Physical_address_name = aContact.physical_address_name,
                                Physical_address_suburb = aContact.physical_address_suburb,
                                Physical_address_postcode = aContact.physical_address_postcode,
                                Postal_address_number = aContact.postal_address_number,
                                Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                Postal_address_suburb = aContact.postal_address_suburb,
                                Postal_address_postcode = aContact.postal_address_postcode,
                                Phone_home = aContact.phone_home,
                                Phone_work = aContact.phone_work,
                                Phone_cell = aContact.phone_cell,
                                Email_address_default = aContact.email_address_default,
                                Email_address_alternate = aContact.email_address_alternate,
                                Username = aContact.username,
                                Ip_address = aContact.ip_address,
                                Remarks = aContact.remarks,
                                Occupation = aContact.occupation,
                                Other_remarks = aContact.other_remarks,
                                Last_updated_by = aContact.last_updated_by,
                                Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                Mailing_list_status = aContact.mailing_list_status,
                                Status = aContact.status,
                                Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                            };
                            contacts.Contact.Add(contact);
                        }
                    }
                    outgoing.Items.Add(contacts);
                    #endregion                    
                }
            }
            return Ok(outgoing);
        }
    }

    [Authorize]
    public class GetOtherContactsController : ApiController
    {
        [HttpPost]
        [Route("api/GetOtherContacts")]
        public IHttpActionResult GetOtherContacts([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region Get Other Contacts
                    try
                    {
                        //var ms = context.group_members.Where(x => x.member_id != memberId).ToList();
                        var a1 = context.group_members.Where(x => x.member_id == memberId).Select(y => y.group_id).Distinct();
                        var a2 = context.group_members.Where(item => a1.Contains(item.group_id)).Select(y => y.member_id).Distinct();

                        List<int?> b = new List<int?>();
                        b.AddRange(a2.Distinct());

                        var aContactMaster = context.members.Where(f => !b.Contains(f.recid)).Distinct();


                        //var memids = context.group_members.Where(x => x.member_id != memberId).Select(y => y.member_id).Distinct();





                        //var group_membersMaster = context.group_members.Select(x=>x.member_id).ToList();
                        // var aContactMaster1 = context.members.Where(item => memids.Contains(item.recid)).ToList();

                        //foreach (var grp in groups)
                        //{
                        //var group_members = group_membersMaster.Where(x => x.group_id == grp).Select(y => y.member_id).ToList();
                        foreach (var aContact in aContactMaster)
                        {
                            //var aContact = aContactMaster.Where(x => x.recid == mid).FirstOrDefault();
                            if (aContact != null)
                            {
                                NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                {
                                    Recid = (aContact.recid > 0 ? aContact.recid : 0),

                                    Firstname = (!string.IsNullOrEmpty(aContact.firstname) ? aContact.firstname : ""),
                                    Middlename = (!string.IsNullOrEmpty(aContact.middlename) ? aContact.middlename : ""),
                                    Lastname = (!string.IsNullOrEmpty(aContact.lastname) ? aContact.lastname : ""),
                                    Gender = (!string.IsNullOrEmpty(aContact.gender) ? aContact.gender : ""),

                                    Physical_address_unit_number = (!string.IsNullOrEmpty(aContact.physical_address_unit_number) ? aContact.physical_address_unit_number : ""),
                                    Physical_address_number = (!string.IsNullOrEmpty(aContact.physical_address_number) ? aContact.physical_address_number : ""),
                                    Physical_address_name = (!string.IsNullOrEmpty(aContact.physical_address_name) ? aContact.physical_address_name : ""),
                                    Physical_address_suburb = (!string.IsNullOrEmpty(aContact.physical_address_suburb) ? aContact.physical_address_suburb : ""),
                                    Physical_address_postcode = (!string.IsNullOrEmpty(aContact.physical_address_postcode) ? aContact.physical_address_postcode : ""),
                                    Postal_address_number = (!string.IsNullOrEmpty(aContact.postal_address_number) ? aContact.postal_address_number : ""),
                                    Postal_address_box_lobby_location = (!string.IsNullOrEmpty(aContact.postal_address_box_lobby_location) ? aContact.postal_address_box_lobby_location : ""),
                                    Postal_address_suburb = (!string.IsNullOrEmpty(aContact.postal_address_suburb) ? aContact.postal_address_suburb : ""),
                                    Postal_address_postcode = (!string.IsNullOrEmpty(aContact.postal_address_postcode) ? aContact.postal_address_postcode : ""),
                                    Phone_home = (!string.IsNullOrEmpty(aContact.phone_home) ? aContact.phone_home : ""),
                                    Phone_work = (!string.IsNullOrEmpty(aContact.phone_work) ? aContact.phone_work : ""),
                                    Phone_cell = (!string.IsNullOrEmpty(aContact.phone_cell) ? aContact.phone_cell : ""),
                                    Email_address_default = (!string.IsNullOrEmpty(aContact.email_address_default) ? aContact.email_address_default : ""),
                                    Email_address_alternate = (!string.IsNullOrEmpty(aContact.email_address_alternate) ? aContact.email_address_alternate : ""),
                                    Username = (!string.IsNullOrEmpty(aContact.username) ? aContact.username : ""),
                                    Ip_address = (!string.IsNullOrEmpty(aContact.ip_address) ? aContact.ip_address : ""),
                                    Remarks = (!string.IsNullOrEmpty(aContact.remarks) ? aContact.remarks : ""),
                                    Occupation = (!string.IsNullOrEmpty(aContact.occupation) ? aContact.occupation : ""),
                                    Other_remarks = (!string.IsNullOrEmpty(aContact.other_remarks) ? aContact.other_remarks : ""),
                                    Last_updated_by = (!string.IsNullOrEmpty(aContact.last_updated_by) ? aContact.last_updated_by : ""),

                                    Signup_date = (!(aContact.signup_date.HasValue)) ? Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm") : (new DateTime(1900 - 01 - 01).ToString("dd/MM/yyyy HH:mm")),
                                    Emergency_volunteering_status = ((!aContact.emergency_volunteering_status.HasValue) ? aContact.emergency_volunteering_status : false),
                                    Mailing_list_status = ((!aContact.mailing_list_status.HasValue) ? aContact.mailing_list_status : false),
                                    Status = ((!aContact.status.HasValue) ? aContact.status : false),
                                    Birth_date = (!(aContact.birth_date.HasValue)) ? Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm") : (new DateTime(1900 - 01 - 01).ToString("dd/MM/yyyy HH:mm")),
                                    Last_updated_date = (!(aContact.last_updated_date.HasValue)) ? Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm") : (new DateTime(1900 - 01 - 01).ToString("dd/MM/yyyy HH:mm")),
                                };
                                contacts.Contact.Add(contact);
                            }
                        }
                        //}
                        outgoing.Items.Add(contacts);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            return Ok(outgoing);
        }

    }

    [Authorize]
    public class GetContactsLocationController : ApiController
    {
        [HttpPost]
        [Route("api/GetContactsLocation")]
        public IHttpActionResult GetContactsLocation([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region Get Connected Contacts
                    var groups = context.group_members.Where(x => x.member_id == memberId).Select(y => y.group_id).ToList();
                    if (groups != null)
                        foreach (var grp in groups)
                        {
                            var group_members = context.group_members.Where(x => x.group_id == grp).Select(y => y.member_id).ToList();
                            if (group_members != null) foreach (var t in group_members)
                                {
                                    var aContact = context.members.Where(x => x.recid == t).FirstOrDefault();
                                    if (aContact != null)
                                    {

                                        NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                        {
                                            Recid = aContact.recid,
                                            Firstname = aContact.firstname,
                                            Middlename = aContact.middlename,
                                            Lastname = aContact.lastname,
                                            Gender = aContact.gender,
                                            Physical_address_unit_number = aContact.physical_address_unit_number,
                                            Physical_address_number = aContact.physical_address_number,
                                            Physical_address_name = aContact.physical_address_name,
                                            Physical_address_suburb = aContact.physical_address_suburb,
                                            Physical_address_postcode = aContact.physical_address_postcode,
                                            Postal_address_number = aContact.postal_address_number,
                                            Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                            Postal_address_suburb = aContact.postal_address_suburb,
                                            Postal_address_postcode = aContact.postal_address_postcode,
                                            Phone_home = aContact.phone_home,
                                            Phone_work = aContact.phone_work,
                                            Phone_cell = aContact.phone_cell,
                                            Email_address_default = aContact.email_address_default,
                                            Email_address_alternate = aContact.email_address_alternate,
                                            Username = aContact.username,
                                            Ip_address = aContact.ip_address,
                                            Remarks = aContact.remarks,
                                            Occupation = aContact.occupation,
                                            Other_remarks = aContact.other_remarks,
                                            Last_updated_by = aContact.last_updated_by,
                                            Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                            Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                            Mailing_list_status = aContact.mailing_list_status,
                                            Status = aContact.status,
                                            Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                            Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                        }; contacts.Contact.Add(contact);
                                    }

                                }
                        }
                    outgoing.Items.Add(contacts);
                    #endregion
                }
            }
            return Ok(outgoing);
        }
        [HttpPost]
        [Route("api/GetContactsLocationAdmin")]
        public IHttpActionResult GetContactsLocationAdmin([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    int selectedId = 0;
                    foreach (var items in incoming.Items)
                    {
                        foreach (var gr in items.Contact)
                        {
                            selectedId = gr.SelectedGroup;
                        }
                    }
                    #region Get Connected Contacts Admin
                    var group_members = context.group_members.Where(x => x.group_id == selectedId).Select(y => y.member_id).ToList();
                    if (group_members != null)
                    foreach (var t in group_members)
                    {
                        var aContact = context.members.Where(x => x.recid == t).FirstOrDefault();
                        if (aContact != null)
                        {

                            NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                            {
                                Recid = aContact.recid,
                                Firstname = aContact.firstname,
                                Middlename = aContact.middlename,
                                Lastname = aContact.lastname,
                                Gender = aContact.gender,
                                Physical_address_unit_number = aContact.physical_address_unit_number,
                                Physical_address_number = aContact.physical_address_number,
                                Physical_address_name = aContact.physical_address_name,
                                Physical_address_suburb = aContact.physical_address_suburb,
                                Physical_address_postcode = aContact.physical_address_postcode,
                                Postal_address_number = aContact.postal_address_number,
                                Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                Postal_address_suburb = aContact.postal_address_suburb,
                                Postal_address_postcode = aContact.postal_address_postcode,
                                Phone_home = aContact.phone_home,
                                Phone_work = aContact.phone_work,
                                Phone_cell = aContact.phone_cell,
                                Email_address_default = aContact.email_address_default,
                                Email_address_alternate = aContact.email_address_alternate,
                                Username = aContact.username,
                                Ip_address = aContact.ip_address,
                                Remarks = aContact.remarks,
                                Occupation = aContact.occupation,
                                Other_remarks = aContact.other_remarks,
                                Last_updated_by = aContact.last_updated_by,
                                Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                Mailing_list_status = aContact.mailing_list_status,
                                Status = aContact.status,
                                Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                            }; contacts.Contact.Add(contact);
                        }

                    }
                    outgoing.Items.Add(contacts);
                    #endregion
                }
            }
            return Ok(outgoing);
        }
    }
    [Authorize]
    public class GetCoordinatorsController : ApiController
    {
        [HttpPost]
        [Route("api/GetCoordinators")]
        public IHttpActionResult GetCoordinators([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                using (NSAKLEntities context = new NSAKLEntities())
                {
                    try
                    {
                        #region Admin
                        if (role == "Admin" || role == "Coordinator")
                        {
                            var areacoordinators1 = context.group_members.Join(context.groups,
                                    grpmem => grpmem.group_id,
                                    grp => grp.recid,
                                    (grpmem, grp) => grp).DistinctBy(p => p.area_coordinator_id).ToList();

                            var gs = context.group_members.Select(y => y.group_id).Distinct().ToList();

                            var areacoordinators = context.coordinators.ToList();

                            #region Admin/Coordinator Contacts
                            foreach (var ac in areacoordinators)
                            {
                                //var acid = context.groups.Where(x => x.recid == grp).Select(y => y.area_coordinator_id).FirstOrDefault();
                                if (ac != null)
                                {
                                    var aContact = context.members.Where(x => x.recid == ac.member_id).FirstOrDefault();
                                    if (aContact != null)
                                    {
                                        NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                        {
                                            Recid = aContact.recid,
                                            AreaName = ac.area_name,
                                            Suburb = ac.suburb,
                                            Firstname = aContact.firstname,
                                            Middlename = aContact.middlename,
                                            Lastname = aContact.lastname,
                                            Gender = aContact.gender,
                                            Physical_address_unit_number = aContact.physical_address_unit_number,
                                            Physical_address_number = aContact.physical_address_number,
                                            Physical_address_name = aContact.physical_address_name,
                                            Physical_address_suburb = aContact.physical_address_suburb,
                                            Physical_address_postcode = aContact.physical_address_postcode,
                                            Postal_address_number = aContact.postal_address_number,
                                            Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                            Postal_address_suburb = aContact.postal_address_suburb,
                                            Postal_address_postcode = aContact.postal_address_postcode,
                                            Phone_home = aContact.phone_home,
                                            Phone_work = aContact.phone_work,
                                            Phone_cell = aContact.phone_cell,
                                            Email_address_default = aContact.email_address_default,
                                            Email_address_alternate = aContact.email_address_alternate,
                                            Username = aContact.username,
                                            Ip_address = aContact.ip_address,
                                            Remarks = aContact.remarks,
                                            Occupation = aContact.occupation,
                                            Other_remarks = aContact.other_remarks,
                                            Last_updated_by = aContact.last_updated_by,
                                            Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                            Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                            Mailing_list_status = aContact.mailing_list_status,
                                            Status = aContact.status,
                                            Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                            Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                        };
                                        contacts.Contact.Add(contact);
                                    }
                                }
                            }
                            #endregion
                        }

                        #endregion
                        #region Member
                        if (role == "Member")
                        {
                            var gs = context.group_members.Where(x => x.member_id == memberId).Select(y => y.group_id).Distinct().ToList();
                            var areacoordinators = context.coordinators.ToList();
                            #region Coordinator Contacts
                            foreach (var grp in gs)
                            {
                                var acid = context.groups.Where(x => x.recid == grp).Select(y => y.area_coordinator_id).FirstOrDefault();
                                if (acid != null)
                                {
                                    var aContact = context.members.Where(x => x.recid == acid).FirstOrDefault();
                                    if (aContact != null)
                                    {
                                        string area_name, suburb = "";
                                        area_name = areacoordinators.Where(x => x.member_id == acid).Select(y => y.area_name).FirstOrDefault();
                                        suburb = areacoordinators.Where(x => x.member_id == acid).Select(y => y.suburb).FirstOrDefault();
                                        NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                        {
                                            Recid = aContact.recid,
                                            AreaName = area_name,
                                            Suburb = suburb,
                                            Firstname = aContact.firstname,
                                            Middlename = aContact.middlename,
                                            Lastname = aContact.lastname,
                                            Gender = aContact.gender,
                                            Physical_address_unit_number = aContact.physical_address_unit_number,
                                            Physical_address_number = aContact.physical_address_number,
                                            Physical_address_name = aContact.physical_address_name,
                                            Physical_address_suburb = aContact.physical_address_suburb,
                                            Physical_address_postcode = aContact.physical_address_postcode,
                                            Postal_address_number = aContact.postal_address_number,
                                            Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                            Postal_address_suburb = aContact.postal_address_suburb,
                                            Postal_address_postcode = aContact.postal_address_postcode,
                                            Phone_home = aContact.phone_home,
                                            Phone_work = aContact.phone_work,
                                            Phone_cell = aContact.phone_cell,
                                            Email_address_default = aContact.email_address_default,
                                            Email_address_alternate = aContact.email_address_alternate,
                                            Username = aContact.username,
                                            Ip_address = aContact.ip_address,
                                            Remarks = aContact.remarks,
                                            Occupation = aContact.occupation,
                                            Other_remarks = aContact.other_remarks,
                                            Last_updated_by = aContact.last_updated_by,
                                            Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                            Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                            Mailing_list_status = aContact.mailing_list_status,
                                            Status = aContact.status,
                                            Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                            Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                        };
                                        contacts.Contact.Add(contact);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                        outgoing.Items.Add(contacts);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return Ok(outgoing);
        }
        [HttpPost]
        [Route("api/GetCoordinatorsAdmin")]
        public IHttpActionResult GetCoordinatorsAdmin([FromBody]NSALK.Models.GetContacts.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetContacts.SystemDataResponse outgoing = new Models.GetContacts.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetContacts.SystemDataResponseContacts contacts = new Models.GetContacts.SystemDataResponseContacts();
                int selectedCoordId = 0;
                foreach (var items in incoming.Items)
                {
                    foreach (var ct in items.Contact)
                    {
                        selectedCoordId = ct.SelectedCoordinator;
                    }
                }
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                using (NSAKLEntities context = new NSAKLEntities())
                {
                    try
                    {
                        var gs = context.group_members.Where(x => x.member_id == selectedCoordId).Select(y => y.group_id).Distinct().ToList();
                        var areacoordinators = context.coordinators.ToList();
                        #region Coordinator Selected
                        foreach (var grp in gs)
                        {
                            var acid = context.groups.Where(x => x.recid == grp).Select(y => y.area_coordinator_id).FirstOrDefault();
                            if (acid != null)
                            {
                                var aContact = context.members.Where(x => x.recid == acid).FirstOrDefault();
                                if (aContact != null)
                                {
                                    string area_name, suburb = "";
                                    area_name = areacoordinators.Where(x => x.member_id == acid).Select(y => y.area_name).FirstOrDefault();
                                    suburb = areacoordinators.Where(x => x.member_id == acid).Select(y => y.suburb).FirstOrDefault();
                                    NSALK.Models.GetContacts.SystemDataResponseContact contact = new Models.GetContacts.SystemDataResponseContact()
                                    {
                                        Recid = aContact.recid,
                                        AreaName = area_name,
                                        Suburb = suburb,
                                        Firstname = aContact.firstname,
                                        Middlename = aContact.middlename,
                                        Lastname = aContact.lastname,
                                        Gender = aContact.gender,
                                        Physical_address_unit_number = aContact.physical_address_unit_number,
                                        Physical_address_number = aContact.physical_address_number,
                                        Physical_address_name = aContact.physical_address_name,
                                        Physical_address_suburb = aContact.physical_address_suburb,
                                        Physical_address_postcode = aContact.physical_address_postcode,
                                        Postal_address_number = aContact.postal_address_number,
                                        Postal_address_box_lobby_location = aContact.postal_address_box_lobby_location,
                                        Postal_address_suburb = aContact.postal_address_suburb,
                                        Postal_address_postcode = aContact.postal_address_postcode,
                                        Phone_home = aContact.phone_home,
                                        Phone_work = aContact.phone_work,
                                        Phone_cell = aContact.phone_cell,
                                        Email_address_default = aContact.email_address_default,
                                        Email_address_alternate = aContact.email_address_alternate,
                                        Username = aContact.username,
                                        Ip_address = aContact.ip_address,
                                        Remarks = aContact.remarks,
                                        Occupation = aContact.occupation,
                                        Other_remarks = aContact.other_remarks,
                                        Last_updated_by = aContact.last_updated_by,
                                        Signup_date = Convert.ToDateTime(aContact.signup_date).ToString("dd/MM/yyyy HH:mm"),
                                        Emergency_volunteering_status = aContact.emergency_volunteering_status,
                                        Mailing_list_status = aContact.mailing_list_status,
                                        Status = aContact.status,
                                        Birth_date = Convert.ToDateTime(aContact.birth_date).ToString("dd/MM/yyyy HH:mm"),
                                        Last_updated_date = Convert.ToDateTime(aContact.last_updated_date).ToString("dd/MM/yyyy HH:mm")
                                    };
                                    contacts.Contact.Add(contact);
                                }
                            }
                        }
                        #endregion
                        outgoing.Items.Add(contacts);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return Ok(outgoing);
        }

    }
    public class FriendlyException
    {
        public static void RaiseExeption(string content, string reason, HttpStatusCode code)
        {
            var resp = new HttpResponseMessage(code)
            {
                Content = new StringContent(string.Format(content)),
                ReasonPhrase = reason
            };
            throw new HttpResponseException(resp);
        }
    }
}
