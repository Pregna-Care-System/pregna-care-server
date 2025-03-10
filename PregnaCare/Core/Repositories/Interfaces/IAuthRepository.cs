﻿using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task RegisterAsync(User userAccount);
        Task LoginAsync();
    }
}
