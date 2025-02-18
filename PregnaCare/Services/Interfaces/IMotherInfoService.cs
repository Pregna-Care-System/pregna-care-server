using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IMotherInfoService
    {
        public IEnumerable<MotherInfo> GetAllMotherInfosByUserId(Guid userId);
    }
}
