using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Specifications.Account;

namespace BusinessServiceLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<SystemAccount> _accountService;
        private readonly IMapper _mapper;

        public AccountService(IGenericRepository<SystemAccount> accountService, IMapper mapper) 
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<SystemAccountDTO?> LoginAsync(LoginPayload loginPayload)
        {
            var spec = new AccountSpecification(loginPayload);
            var user = await _accountService.GetEntityWithSpec(spec);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<SystemAccount, SystemAccountDTO>(user);
        }

        public async Task<bool> CheckEmailExistedAsync(string email)
        {
            var spec = new AccountSpecification(email);
            var user = await _accountService.GetEntityWithSpec(spec);
            if (user != null) return true;
            return false;
        }

        public async Task<SystemAccountDTO?> GetUserByEmailAsync(string email)
        {
            var spec = new AccountSpecification(email);
            var user = await _accountService.GetEntityWithSpec(spec);

            if (user == null) 
            {
                return null;
            }

            return _mapper.Map<SystemAccount, SystemAccountDTO>(user);
        }
    }
}
