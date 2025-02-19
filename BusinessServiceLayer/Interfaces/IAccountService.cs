using BusinessServiceLayer.DTOs;
using RepositoryLayer.Specifications.Account;
using RepositoryLayer.Specifications.NewsArticles;

namespace BusinessServiceLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SystemAccountDTO?> LoginAsync(LoginPayload loginDTO);
        Task<bool> CheckEmailExistedAsync(string email);
        Task<SystemAccountDTO?> GetUserByEmailAsync(string email);
        Task<IReadOnlyList<SystemAccountDTO>> GetAccountsAsync(AccountSpecParams specParams);
        Task<int> CountAccountsAsync(AccountSpecParams specParams);
        Task<SystemAccountDTO> GetAccountByIdAsync(int id);
        Task<bool> CreateAccountAsync(SystemAccountToAddOrUpdateDTO account);
        Task<bool> UpdateAccountAsync(int id, SystemAccountToAddOrUpdateDTO account);
        Task<bool> DeleteAccountAsync(int id);
    }
}
