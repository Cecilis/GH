using GH.Models;
using GH.Utilities;
using GH.ViewModels;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;

namespace GH.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public IMembershipService MembershipService { get; set; }

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var userName = context.UserName;
                    var password = context.Password;

                    if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

                    if (MembershipService.ValidateUser(userName, password))
                    {
                        var msUserInfo = Membership.GetUser(context.UserName);
                        var claims = new List<Claim>()
                                                    {
                                                        new Claim(ClaimTypes.Name, msUserInfo.UserName),
                                                        new Claim(ClaimTypes.Email, msUserInfo.Email)
                                                    };

                        foreach (var role in Roles.GetRolesForUser(msUserInfo.UserName))
                            claims.Add(new Claim(ClaimTypes.Role, role));

                        var data = new Dictionary<string, string>
                                                    {
                                                        { "userName", msUserInfo.UserName },
                                                        { "roles", string.Join(",", Roles.GetRolesForUser(context.UserName).ToJsonString())}
                                                    };

                        var properties = new AuthenticationProperties(data);

                        ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                            Startup.OAuthServerOptions.AuthenticationType);

                        var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                        context.Validated(ticket);
                    }
                    else
                    {
                        //context.SetError("invalid_grant", "The user name or password is incorrect");
                        context.Response.StatusCode = 400;
                        context.SetCustomError("Usuario o contraseña incorrecta");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 400;
                    context.SetCustomError(ex.InnerException.Message);
                }
            });
        }


        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string,
            string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication
        (OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri
        (OAuthValidateClientRedirectUriContext context)
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

        //public static AuthenticationProperties CreateProperties(string userName, string roles)
        //{
        //    IDictionary<string, string>
        //    data = new Dictionary<string, string>
        //    {
        //        { "userName", userName },
        //         {"roles", roles}
        //    };
        //    return new AuthenticationProperties(data);
        //}
    }
}