using RapidBlazor.Application.Common.Interfaces;

namespace RapidBlazor.DbMigration.Fakes
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => "SYSTEM";

        public string GetUserName()
        {
            return "SYSTEM";
        }

        public bool IsInPolicy(string policyName)
        {
            return true;
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
