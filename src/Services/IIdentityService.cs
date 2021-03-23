using System.Threading.Tasks;

using DNSeed.Domain;
using DNSeed.Models.Command;

namespace DNSeed.Services
{
    public interface IIdentityService
    {
        Task<LoginResponseModel> SigninAsync(LoginRequest request);
        Task<LoginResponseModel> LoadOffTokenAsync(string request);
        //Task<CustomerDto> FetchByIdAsync(int id);
    }
}
