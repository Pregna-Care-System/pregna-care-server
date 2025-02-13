using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class AccountRepository : GenericRepository<User, Guid>, IAccountRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;
        public AccountRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

    }
}
