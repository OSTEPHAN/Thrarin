
namespace Thrarin.Identity
{
    public static class IdentityProviderExtensions
    {
        public static bool Validate(this IIdentityProvider provider, string identifier, string password, string role = "")
        {
            var principal = provider.Get(identifier);
            if (null == principal)
            {
                return false;
            }
            if (false == string.IsNullOrEmpty(role) && false == principal.IsInRole(role))
            {
                return false;
            }
            return provider.Validate(principal.Identity, password);
        }
    }
}
