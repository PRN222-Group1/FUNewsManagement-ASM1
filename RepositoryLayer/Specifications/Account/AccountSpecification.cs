using RepositoryLayer.Entities;

namespace RepositoryLayer.Specifications.Account
{
    public class AccountSpecification : BaseSpecification<SystemAccount>
    {
        public AccountSpecification(string email) : base(x => x.AccountEmail == email)
        {

        }

        public AccountSpecification(LoginPayload loginPayload) :
            base(x => x.AccountEmail == loginPayload.AccountEmail 
                && x.AccountPassword == loginPayload.AccountPassword)
        {
        }
    }
}
