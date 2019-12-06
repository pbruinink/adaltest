using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ADAL_Android.Services
{
    public interface IAuthenticator
    {
        Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri);
        void Logout(string authority);
        bool hasLoginData(string authority);
    }
}
