using System.Threading.Tasks;
using DNSeed.Domain;

namespace DNSeed.Repositories
{
    public interface IUserRepository
    {
        Task<LoginResponse> SigininAsync(LoginRequest request);
        Task<LoginResponse> GetByIdAsync(int id);
    }
}
