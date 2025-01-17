﻿using PregnaCare.Api.Controllers.Auth;

namespace PregnaCare.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> LoginGoogleAsync(LoginRequest request);

    }
}
