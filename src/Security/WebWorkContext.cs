using DNSeed.Models.Command;

namespace DNSeed.Security
{
    public interface IWebWorkContext
    {
        int UserId { get; }
        int AffiliateId { get; }
        bool IsSystemAccount { get; }

        void Handle(LoginResponseModel response);
    }

    internal sealed class WebWorkContext : IWebWorkContext
    {
        public int UserId { get; private set; }
        public bool IsSystemAccount { get; private set; }
        public int AffiliateId { get; private set; }

        public void Handle(LoginResponseModel response)
        {
            UserId = response.Id;
        }
    }
}
