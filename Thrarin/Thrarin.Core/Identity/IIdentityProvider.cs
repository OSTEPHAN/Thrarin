
namespace Thrarin.Identity
{
    using System.Security.Principal;

    public interface IIdentityProvider
    {
        bool Create(string identifier, string password, params string[] roles);
        IPrincipal Get(string identifier);
        bool Validate(IIdentity identity, string password);
    }
}