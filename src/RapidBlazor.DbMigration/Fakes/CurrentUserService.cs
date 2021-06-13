using RapidBlazor.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace RapidBlazor.DbMigration.Fakes
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => "SYSTEM";

        public string GetUserName()
        {
            return "SYSTEM";
        }

        public Task<bool> IsInPolicy(string policyName)
        {
            return Task.FromResult(true);
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
