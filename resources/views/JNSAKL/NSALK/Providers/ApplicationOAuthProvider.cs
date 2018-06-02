using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using NSALK.Models;
//using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Data.Entity.Infrastructure;


namespace NSALK
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password))
            {
                context.SetError("invalid_grant", "The user name or password not supplied.");
                return;
            }
            var extraParams = await context.Request.ReadFormAsync();
            string username = extraParams["username"];
            string password = extraParams["password"];
            
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                context.SetError("invalid_grant", "username or password not provided.");
                return;
            }
            int memberId = 0;
            string role = "Member";
            string AuthenticationType = "Authenticated";
            bool isAdmin = false;
            string apiUrl = "";
            using (NSAKLEntities asContext = new NSAKLEntities())
            {
                string hashPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(context.Password, "sha1");
                try
                {
                    var queryResult = "";
                    //var queryResult = asContext.Database.SqlQuery<List<string>>("exec spInternetUserWithTermsAccept_Validate '" + context.UserName + "','" + hashPassword + "','',''").ToList();
                }
                catch (SqlException ex)
                {
                    context.SetError("invalid_user", ex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    context.SetError("invalid_user", ex.Message);
                    return;
                }
                var internetUser = asContext.users.Where(x => x.username == context.UserName && x.password == hashPassword).FirstOrDefault();
                if (internetUser == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                apiUrl = System.Configuration.ConfigurationManager.AppSettings["APIUrl"];
                var memberinfo = asContext.members.Where(x => x.username == internetUser.username).FirstOrDefault();
                
                if (internetUser == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                if (internetUser != null)
                {
                    if (memberinfo != null)
                    {
                        memberId = memberinfo.recid;
                        var coordinatorinfo = asContext.coordinators.Where(x => x.member_id == memberId).FirstOrDefault();
                        var admininfo = asContext.committees.Where(x => x.user_id == memberId).FirstOrDefault();
                        if (coordinatorinfo != null)
                            role = "Coordinator";
                        if (admininfo != null)
                            role = "Admin";
                    }
                    if (string.IsNullOrEmpty(internetUser.username))//if interface doesnt' have IMEI and MAC, let the user log in
                    {
                        AuthenticationType = "UserNameBlank";
                    }
                    else
                    {
                        AuthenticationType = "AuthenticatedValueMatch";
                    }
                }
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, memberId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));


            AuthenticationProperties properties = CreateProperties(context.UserName, AuthenticationType);
            
            
            AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);
            
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName,string AuthenticationType)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"userName", userName },
                {"authenticationType", AuthenticationType}
            };
            return new AuthenticationProperties(data);
        }
    }
}