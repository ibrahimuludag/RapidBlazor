using System.Threading.Tasks;

namespace RapidBlazor.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string GetUserName();

        bool IsInRole(string role);

        Task<bool> IsInPolicy(string policyName);

    }
}
