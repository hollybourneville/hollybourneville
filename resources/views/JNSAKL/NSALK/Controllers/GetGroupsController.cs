using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;


namespace NSALK.Controllers
{
    [Authorize]
    public class GetGroupsController : ApiController
    {
        [HttpPost]
        [Route("api/GetGroups")]
        public IHttpActionResult Post([FromBody]NSALK.Models.GetGroups.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetGroups.SystemDataResponse outgoing = new Models.GetGroups.SystemDataResponse();
            if (incoming != null)
            {
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                NSALK.Models.GetGroups.SystemDataResponseGroups groups = new Models.GetGroups.SystemDataResponseGroups();
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region Groups                    
                    if (role == "Admin")
                    {
                        #region Get Admin Groups
                        var lgroup1 = context.groups.ToList();
                        foreach (var lgroup in lgroup1)
                        {
                            string area_coordinator_name = "";
                            var area_coordinator_name1 = context.members.Where(x => x.recid == lgroup.area_coordinator_id).FirstOrDefault();
                            if (area_coordinator_name1 != null)
                                area_coordinator_name = area_coordinator_name1.firstname + " " + area_coordinator_name1.middlename + " " + area_coordinator_name1.lastname;

                            NSALK.Models.GetGroups.SystemDataResponseGroup group = new Models.GetGroups.SystemDataResponseGroup()
                            {
                                recid = lgroup.recid,
                                group_name = lgroup.group_name,
                                street_name = lgroup.street_name,
                                suburb = lgroup.suburb,
                                area_coordinator_id = lgroup.area_coordinator_id,
                                area_coordinator_name = area_coordinator_name.ToString(),
                                remarks = lgroup.remarks,
                                setup_by = lgroup.setup_by,
                                last_updated = Convert.ToDateTime(lgroup.last_updated).ToString("dd/MM/yyyy HH:mm")
                            };
                            groups.Group.Add(group);                                
                        }
                        #endregion
                    }
                    if (role == "Coordinator")
                    {
                        #region Get Coordinator Groups
                        var lgroup1 = context.groups.ToList();
                        foreach (var lgroup in lgroup1)
                        {
                            string area_coordinator_name = "";
                            var area_coordinator_name1 = context.members.Where(x => x.recid == lgroup.area_coordinator_id).FirstOrDefault();
                            if (area_coordinator_name1 != null)
                                area_coordinator_name = area_coordinator_name1.firstname + " " + area_coordinator_name1.middlename + " " + area_coordinator_name1.lastname;

                            NSALK.Models.GetGroups.SystemDataResponseGroup group = new Models.GetGroups.SystemDataResponseGroup()
                            {
                                recid = lgroup.recid,
                                group_name = lgroup.group_name,
                                street_name = lgroup.street_name,
                                suburb = lgroup.suburb,
                                area_coordinator_id = lgroup.area_coordinator_id,
                                area_coordinator_name = area_coordinator_name.ToString(),
                                remarks = lgroup.remarks,
                                setup_by = lgroup.setup_by,
                                last_updated = Convert.ToDateTime(lgroup.last_updated).ToString("dd/MM/yyyy HH:mm")
                            };
                            groups.Group.Add(group);
                        }
                        #endregion
                    }
                    if (role == "Member")
                    {
                        var gps = context.group_members.Where(x => x.member_id == memberId).ToList();
                        if (gps != null)
                            #region Get Member Groups
                            foreach (var grp in gps)
                            {
                                var lgroup = context.groups.Where(x => x.recid == grp.group_id).FirstOrDefault();
                                if (lgroup != null)
                                {
                                    string area_coordinator_name = "";
                                    var area_coordinator_name1 = context.members.Where(x => x.recid == lgroup.area_coordinator_id).FirstOrDefault();
                                    if (area_coordinator_name1 != null)
                                        area_coordinator_name = area_coordinator_name1.firstname + " " + area_coordinator_name1.middlename + " " + area_coordinator_name1.lastname;
                                    NSALK.Models.GetGroups.SystemDataResponseGroup group = new Models.GetGroups.SystemDataResponseGroup()
                                    {
                                        recid = lgroup.recid,
                                        group_name = lgroup.group_name,
                                        street_name = lgroup.street_name,
                                        suburb = lgroup.suburb,
                                        area_coordinator_id = lgroup.area_coordinator_id,
                                        area_coordinator_name = area_coordinator_name.ToString(),
                                        remarks = lgroup.remarks,
                                        setup_by = lgroup.setup_by,
                                        last_updated = Convert.ToDateTime(lgroup.last_updated).ToString("dd/MM/yyyy HH:mm")
                                    };
                                    groups.Group.Add(group);
                                }
                            }
                        #endregion
                    }
                    outgoing.Items.Add(groups);
                    #endregion
                }
            }
            return Ok(outgoing);
        }
        [HttpPost]
        [Route("api/GetGroupsAdmin")]
        public IHttpActionResult GetGroupsAdmin([FromBody]NSALK.Models.GetGroups.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetGroups.SystemDataResponse outgoing = new Models.GetGroups.SystemDataResponse();
            if (incoming != null)
            {
                int selectedId = 0;
                foreach (var items in incoming.Items)
                {
                    foreach (var gr in items.Group)
                    {
                        selectedId = gr.selectedid;
                    }
                }
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                NSALK.Models.GetGroups.SystemDataResponseGroups groups = new Models.GetGroups.SystemDataResponseGroups();
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region Groups                    
                    var gps = context.group_members.Where(x => x.member_id == selectedId).ToList();
                    
                        #region Get Admin Groups Selected and non selected
                        //foreach (var grp in gps)
                        //{

                        var lgroup1 = context.groups.ToList();
                        foreach (var lgroup in lgroup1)
                        {
                            string area_coordinator_name = "";
                            var area_coordinator_name1 = context.members.Where(x => x.recid == lgroup.area_coordinator_id).FirstOrDefault();
                            if (area_coordinator_name1 != null)
                                area_coordinator_name = area_coordinator_name1.firstname + " " + area_coordinator_name1.middlename + " " + area_coordinator_name1.lastname;
                            var result = "false";
                            if (gps != null)
                            {
                                var item = gps.FirstOrDefault(i => i.group_id == lgroup.recid);
                                if(item != null)
                                result = "true";
                            }

                            NSALK.Models.GetGroups.SystemDataResponseGroup group = new Models.GetGroups.SystemDataResponseGroup()
                            {
                                recid = lgroup.recid,
                                group_name = lgroup.group_name,
                                street_name = lgroup.street_name,
                                suburb = lgroup.suburb,
                                area_coordinator_id = lgroup.area_coordinator_id,
                                area_coordinator_name = area_coordinator_name.ToString(),
                                remarks = lgroup.remarks,
                                setup_by = lgroup.setup_by,
                                last_updated = Convert.ToDateTime(lgroup.last_updated).ToString("dd/MM/yyyy HH:mm"),
                                selectedgroup = result
                            };
                            groups.Group.Add(group);
                        
                        }
                    #endregion
                    groups.Group = groups.Group.OrderByDescending(x => x.selectedgroup.Substring(0)).ToList();

                    outgoing.Items.Add(groups);
                    #endregion
                }
            }
            return Ok(outgoing);
        }

        [HttpPost]
        [Route("api/UpdateGroupsAdmin")]
        public IHttpActionResult UpdateGroupsAdmin([FromBody]NSALK.Models.GetGroups.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetGroups.SystemDataResponse outgoing = new Models.GetGroups.SystemDataResponse();
            if (incoming != null)
            {
                int selectedId = 0, selectedgroupId = 0;
                string userName = (User.Identity.GetUserName());
                foreach (var items in incoming.Items)
                {
                    foreach (var gr in items.Group)
                    {
                        selectedId = gr.selectedid;
                        selectedgroupId = gr.selectedgroupid;
                    }
                }
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                NSALK.Models.GetGroups.SystemDataResponseGroups groups = new Models.GetGroups.SystemDataResponseGroups();
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region Update Groups for Selected ID               
                    var gps = context.group_members.Where(x => x.member_id == selectedId && x.group_id == selectedgroupId).FirstOrDefault();
                    if (gps != null)
                    {
                        context.Entry(gps).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    else
                    {
                        var gp = context.group_members.Add(new group_members
                        {
                            group_id = selectedgroupId,
                            member_id=selectedId,
                            last_updated=DateTime.UtcNow,
                            last_updated_by = userName,
                            membership_status=true,
                            street_contact_status=true
                        });
                        context.SaveChanges();
                    }
                    outgoing.Items.Add(groups);
                    #endregion
                }
            }
            return Ok(outgoing);
        }
    }
}
