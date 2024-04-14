using NutriPro.Application.Configurations;
using NutriPro.Application.Models;
using NutriPro.Application.Repositories;
using NutriPro.Data.Models.Management;

namespace NutriPro.Application
{
    public interface ITransientDependency
    {
    }

    public abstract class NutriProAppServiceBase<TEntity> : ITransientDependency where TEntity : class
    {
        protected readonly NutriProSession _session;
        protected readonly IRepository<TEntity> _repository;

        public NutriProAppServiceBase(NutriProSession session, IRepository<TEntity> repository)
        {
            _session = session;
            _repository = repository;
        }

        public async Task<List<TEntity>> GetByListIdsAsync<TId>(List<TId> ids)
        {
            return await _repository.GetByListIdsAsync(ids);
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PaginationResult<TEntity>> GetAllPaginatedAsync(UserDataTableParams dataTableParams, long? tenantId, List<long> listOrganizationId, bool isHost)
        {
            return await _repository.GetAllPaginatedAsync(dataTableParams, tenantId, listOrganizationId, isHost);
        }

        public Task CreateAsync(TEntity entity)
        {
            return _repository.AddAsync(entity);
        }


        public async Task<TId> InsertAndGetIdAsync<TId>(TEntity entity)
        {
            return await _repository.InsertAndGetIdAsync<TId>(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            return _repository.UpdateAsync(entity);
        }

        public Task DeleteAsync<TId>(TId id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}


