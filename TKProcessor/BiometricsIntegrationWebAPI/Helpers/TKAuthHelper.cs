using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;
using TKProcessor.Services.Maintenance;
namespace BiometricsIntegrationWebAPI.Helpers
{
    public static class TKAuthHelper
    {
        static User LoggedInUser;
        public static bool LoginUser(string username, string password,TKContext context)
        {
            UserService userService = new UserService(context);

            return userService.TryLogin(username, password, out LoggedInUser);
        }

        public static User GetUser()
        {
            return LoggedInUser;
        }
    }
}
