using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Infrastructure.UnitOfWork
{
    /// <summary>
    /// UnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Dictionary<Type, object> _repositories;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _repositories = new();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }

        /// <summary>
        /// IGenericRepository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<,>).MakeGenericType(type, typeof(TKey));
                var repositoryInstance = Activator.CreateInstance(repositoryType, _applicationDbContext);
                _repositories[type] = repositoryInstance;
            }

            return (IGenericRepository<T, TKey>)_repositories[type];
        }

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
