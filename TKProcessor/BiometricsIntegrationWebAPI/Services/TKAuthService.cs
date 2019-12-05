using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;
using TKProcessor.Services.Maintenance;

namespace BiometricsIntegrationWebAPI.Services
{
    public class TKAuthService
    {
        private IConfiguration configuration;
        private TKContext context;
        User LoggedInUser;

        public TKAuthService(TKContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public User GetUser()
        {
            if(LoggedInUser == null)
            {
                UserService userService = new UserService(context);

                if(!userService.TryLogin(configuration.GetSection("TKAuth")["username"], configuration.GetSection("TKAuth")["password"], out LoggedInUser))
                {
                    throw new System.Exception("Invalid TK credentials");
                }
            }
            return LoggedInUser;
        }
    

    }
}
