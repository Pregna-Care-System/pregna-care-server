using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class MotherInfoService : IMotherInfoService
    {
        private readonly PregnaCareAppDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public MotherInfoService(PregnaCareAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MotherInfo> GetAllMotherInfosByUserId(Guid userId)
        {
            return _context.MotherInfos.AsNoTracking().Include(x => x.PregnancyRecord)
                                               .Include(x => x.PregnancyRecord.User)
                                               .Where(x => x.PregnancyRecord.UserId == userId);
        }
    }
}
