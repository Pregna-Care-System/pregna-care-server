using PregnaCare.Core.Repositories.Interfaces;

namespace PregnaCare.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : class;
        Task SaveChangesAsync();
    }
}
