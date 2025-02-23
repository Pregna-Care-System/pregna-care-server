using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly PregnaCareAppDbContext _pregnaCareAppDbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pregnaCareAppDbContext"></param>
        public AuthRepository(PregnaCareAppDbContext pregnaCareAppDbContext)
        {
            _pregnaCareAppDbContext = pregnaCareAppDbContext;
        }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public async Task RegisterAsync(User userAccount)
        {
            _ = await _pregnaCareAppDbContext.Users.AddAsync(userAccount);
            _ = await _pregnaCareAppDbContext.SaveChangesAsync();
        }
    }
}
