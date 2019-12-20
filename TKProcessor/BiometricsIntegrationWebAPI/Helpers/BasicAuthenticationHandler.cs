﻿using BiometricsIntegrationWebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TKProcessor.Models.TK;

namespace BiometricsIntegrationWebAPI.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly WorkSiteService workSiteService;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            WorkSiteService workSiteService
            ) 
            : base(options, logger, encoder, clock)
        {
            this.workSiteService = workSiteService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            WorkSite site = null;

            try
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    eventLog.WriteEntry("Test1");
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    eventLog.WriteEntry("Test2");
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    eventLog.WriteEntry("Test3");

                    var username = credentials[0];
                    var password = credentials[1];
                    try
                    {
                        site = await workSiteService.Authenticate(username, password);
                    }
                    catch (Exception ex)
                    {
                        eventLog.WriteEntry(ex.Message);
                        throw ex;
                    }
                    eventLog.WriteEntry("Test4");

                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (site == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, site.Id.ToString()),
                new Claim(ClaimTypes.Name, site.IntegrationAuthUsername),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
