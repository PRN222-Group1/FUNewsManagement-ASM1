﻿using BusinessServiceLayer.DTOs;
using RepositoryLayer.Specifications.Account;

namespace BusinessServiceLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SystemAccountDTO?> LoginAsync(LoginPayload loginDTO);
        Task<bool> CheckEmailExistedAsync(string email);
        Task<SystemAccountDTO?> GetUserByEmailAsync(string email);
    }
}
