using GH.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(GH.Startup))]
namespace GH
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        public static string PublicClientId { get; private set; }

        //  Mas Info: http://go.microsoft.com/fwlink/?LinkId=301864      


        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            ConfigureAuth(app);
            WebApiConfig.Register(config);
            app.UseWebApi(config); 
        }


        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            // Configura la aplicación para el uso de OAuth 
            PublicClientId = "self";

            OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/Account/Login"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // habilita el uso de bearers token para autenticar los usuarios
            //app.UseOAuthBearerTokens(OAuthServerOptions);

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //app.UseOAuthAuthorizationServer(OAuthServerOptions);
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            //{
            //    AuthenticationType = "Bearer",
            //    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active
            //});

        }

    }
}


//web.config
//<appSettings>
//  <add key = "cors:Origins" value="*" />
//  <add key = "cors:Headers" value="*" />
//  <add key = "cors:Methods" value="GET, POST, OPTIONS, PUT, DELETE" />
//</appSettings>
//Startup.cs
//var appSettings = WebConfigurationManager.AppSettings;

//// If CORS settings are present in Web.config
//if (!string.IsNullOrWhiteSpace(appSettings["cors:Origins"]))
//{
//    // Load CORS settings from Web.config
//    var corsPolicy = new EnableCorsAttribute(
//        appSettings["cors:Origins"],
//        appSettings["cors:Headers"],
//        appSettings["cors:Methods"]);

//// Enable CORS for ASP.NET Identity
//app.UseCors(new CorsOptions
//    {
//        PolicyProvider = new CorsPolicyProvider
//        {
//            PolicyResolver = request =>
//                request.Path.Value == "/token" ?
//                corsPolicy.GetCorsPolicyAsync(null, CancellationToken.None) :
//                Task.FromResult<CorsPolicy>(null)
//        }
//    });

//    // Enable CORS for Web API
//    config.EnableCors(corsPolicy);
//}


//app.UseCors(new CorsOptions
//            {
//                PolicyProvider = new CorsPolicyProvider
//                {
//                    PolicyResolver = context => Task.FromResult(new CorsPolicy
//                    {
//                        AllowAnyHeader = true,
//                        AllowAnyMethod = true,
//                        AllowAnyOrigin = true,
//                        SupportsCredentials = false,
//                        PreflightMaxAge = Int32.MaxValue // << ---- THIS
//                    })
//                }
//            });




//-----------------------------------
//            var corsPolicy = new EnableCorsAttribute(
//                appSettings["cors:Origins"],
//                appSettings["cors:Headers"],
//                appSettings["cors:Methods"],
//                appSettings["cors:ExposedHeaders"]);

//var corsPolicy = new CorsPolicy
//{
//    AllowAnyMethod = true,
//    AllowAnyHeader = true
//};

//// Try and load allowed origins from web.config
//// If none are specified we'll allow all origins

//var origins = ConfigurationManager.AppSettings[Constants.CorsOriginsSettingKey];

//            if (origins != null)
//            {
//                foreach (var origin in origins.Split(';'))
//                {
//                    corsPolicy.Origins.Add(origin);
//                }
//            }
//            else
//            {
//                corsPolicy.AllowAnyOrigin = true;
//            }

//            var corsOptions = new CorsOptions
//            {
//                PolicyProvider = new CorsPolicyProvider
//                {
//                    PolicyResolver = context => Task.FromResult(corsPolicy)
//                }
//            };

//app.UseCors(corsOptions);