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

    public class GetDocumentsController : ApiController
    {
        [HttpPost]
        [Route("api/GetDocuments")]
        public IHttpActionResult GetDocuments([FromBody]NSALK.Models.GetDocuments.SystemDataMessage incoming)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
            NSALK.Models.GetDocuments.SystemDataResponse outgoing = new Models.GetDocuments.SystemDataResponse();
            if (incoming != null)
            {
                NSALK.Models.GetDocuments.SystemDataResponseDocuments docs = new Models.GetDocuments.SystemDataResponseDocuments();

                int memberId = Convert.ToInt32(User.Identity.GetUserId());
                string role = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                using (NSAKLEntities context = new NSAKLEntities())
                {
                    #region Get Admin Documents
                    if (role == "Admin")
                    {
                        var dcs = context.documents.Where(x => x.status == true).ToList();
                        foreach (var dc in dcs)
                        {
                            string dtype = context.doc_types.Where(x => x.recid == dc.doc_type_id).Select(x => x.doc_type).FirstOrDefault();
                            NSALK.Models.GetDocuments.SystemDataResponseDocument dct = new Models.GetDocuments.SystemDataResponseDocument()
                            {
                                recid = dc.recid,
                                doc_title = dc.doc_title,
                                doc_type = dtype,
                                doc_author = dc.doc_author,
                                doc_publisher = dc.doc_publisher,
                                doc_date = Convert.ToDateTime(dc.doc_date),
                                doc_number = dc.doc_number,
                                doc_remarks = dc.doc_remarks,
                                doc_path = dc.doc_path,
                                status = Convert.ToBoolean(dc.status),
                                uploaded_by = dc.uploaded_by,
                                deleted_by = dc.deleted_by,
                                date_deleted = Convert.ToDateTime(dc.date_deleted)
                            };
                            docs.Document.Add(dct);
                        }
                        outgoing.Documents.Add(docs);
                    }
                    #endregion
                    #region Get User Documents
                    if (role == "Member")
                    {
                        var dcs = context.documents.Where(x => x.status == true && x.doc_type_id == 7).ToList();
                        foreach (var dc in dcs)
                        {
                            string dtype = context.doc_types.Where(x => x.recid == dc.doc_type_id).Select(x => x.doc_type).FirstOrDefault();
                            NSALK.Models.GetDocuments.SystemDataResponseDocument dct = new Models.GetDocuments.SystemDataResponseDocument()
                            {
                                recid = dc.recid,
                                doc_title = dc.doc_title,
                                doc_type = dtype,
                                doc_author = dc.doc_author,
                                doc_publisher = dc.doc_publisher,
                                doc_date = Convert.ToDateTime(dc.doc_date),
                                doc_number = dc.doc_number,
                                doc_remarks = dc.doc_remarks,
                                doc_path = dc.doc_path,
                                status = Convert.ToBoolean(dc.status),
                                uploaded_by = dc.uploaded_by,
                                deleted_by = dc.deleted_by,
                                date_deleted = Convert.ToDateTime(dc.date_deleted)
                            };
                            docs.Document.Add(dct);
                        }
                        outgoing.Documents.Add(docs);
                    }
                    #endregion
                }
            }
            return Ok(outgoing);
        }
    }
}
