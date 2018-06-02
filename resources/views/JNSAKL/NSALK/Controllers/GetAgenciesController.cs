using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace NSALK.Controllers
{
    [Authorize]
    public class GetAgenciesController : ApiController
    {
        [HttpPost]
        [Route("api/GetAgencies")]
        public IHttpActionResult GetAgencies([FromBody]NSALK.Models.GetAgencies.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetAgencies.SystemDataResponse outgoing = new Models.GetAgencies.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetAgencies.SystemDataResponseServices services = new Models.GetAgencies.SystemDataResponseServices();
                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                using (NSAKLEntities context = new NSAKLEntities())
                {
                    try
                    {
                        #region Get GetAgencies Services
                        var svcs = context.useful_contacts.ToList();
                        foreach (var svc in svcs)
                        {
                            if (svc != null)
                            {
                                NSALK.Models.GetAgencies.SystemDataResponseService service = new Models.GetAgencies.SystemDataResponseService()
                                {
                                    Recid = 1,
                                    Service_name = svc.service_name,
                                    Service_phone_1 = svc.service_phone_1,
                                    service_phone_2 = svc.service_phone_2,
                                    service_email_1 = svc.service_email_1,
                                    service_email_2 = svc.service_email_2,
                                    service_url_1 = svc.service_url_1,
                                    service_url_2 = svc.service_url_2
                                };
                                services.Service.Add(service);
                            }
                        }
                        outgoing.Items.Add(services);
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
}
