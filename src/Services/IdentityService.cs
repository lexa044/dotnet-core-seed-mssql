using System.Threading.Tasks;

using AutoMapper;
using DNSeed.Domain;
using DNSeed.Models.Command;
using DNSeed.Repositories;

namespace DNSeed.Services
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly IDalSession _session;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public IdentityService(
            IDalSession session, 
            IUserRepository repository, 
            IMapper mapper)
        {
            _session = session;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LoginResponseModel> SigninAsync(LoginRequest request)
        {
            LoginResponseModel response = new LoginResponseModel();
            LoginResponse existingUser = await _repository.SigininAsync(request);
            if (null != existingUser)
            {
                response.Id = existingUser.Id;
                response.Token = string.Concat(response.Id, ".", existingUser.Token);
            }
            // Commit the database changes from both repositories.
            _session.GetUnitOfWork().CommitChanges();

            return response;
        }

        public async Task<LoginResponseModel> LoadOffTokenAsync(string request)
        {
            LoginResponseModel response = new LoginResponseModel();
            if(!string.IsNullOrEmpty(request))
            {
                string[] tokens = request.Split('.');
                if (tokens.Length == 2)
                {
                    if (int.TryParse(tokens[0], out int customerId) && customerId > 0)
                    {
                        LoginResponse existingUser = await _repository.GetByIdAsync(customerId);
                        if (null != existingUser && existingUser.Token == tokens[1])
                        {
                            response.Id = existingUser.Id;
                            response.Token = string.Concat(response.Id, ".", existingUser.Token);
                        }
                    }
                }
            }

            return response;
        }
    }
}
