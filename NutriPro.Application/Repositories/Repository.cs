using NutriPro.Application.Configurations;
using NutriPro.Application.Models;
using NutriPro.Data;
using NutriPro.Data.Models.Management;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NutriPro.Application.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly NutriProContext _context;

        public Repository(NutriProContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetByListIdsAsync<TId>(IEnumerable<TId> ids)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            query = IncludeAllNavigationProperties(query);
            query = query.Where(e => ids.Contains(EF.Property<TId>(e, "Id")));

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            query = IncludeAllNavigationProperties(query);

            return await query.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id").Equals(id));
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            query = IncludeAllNavigationProperties(query);

            return await query.ToListAsync();
        }

        public async Task<PaginationResult<TEntity>> GetAllPaginatedAsync(UserDataTableParams dataTableParams, long? tenantId, List<long> listOrganizationId, bool isHost)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (isHost || tenantId.HasValue)
            {
                query = ApplyTenantOrganizationFilters(query, tenantId, listOrganizationId);

            }

            query = IncludeAllNavigationProperties(query);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(dataTableParams.Search?.Value))
            {
                string searchTerm = dataTableParams.Search.Value.ToLower();
                var entities = await query.ToListAsync();
                entities = entities.Where(entity => IsPropertyContainsValue(entity, searchTerm)).ToList();
                query = entities.AsQueryable();
            }

            int totalRecords = query.Count();

            if (dataTableParams.Order != null && dataTableParams.Order.Count > 0)
            {
                foreach (var order in dataTableParams.Order)
                {
                    string propertyName = query.ElementType.GetProperties()[order.Column].Name;
                    var property = typeof(TEntity).GetProperty(propertyName);

                    if (property != null)
                    {
                        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
                        Expression propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);

                        string methodName = order.Dir == "asc" ? "OrderBy" : "OrderByDescending";
                        MethodCallExpression orderByCallExpression = Expression.Call(typeof(Queryable), methodName,
                            new Type[] { typeof(TEntity), property.PropertyType },
                            query.Expression, Expression.Quote(orderByExpression));

                        query = query.Provider.CreateQuery<TEntity>(orderByCallExpression);
                    }
                }
            }


            // Paginação
            int pageNumber = dataTableParams.Start / dataTableParams.Length + 1;
            int pageSize = dataTableParams.Length;

            List<TEntity> result = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginationResult = new PaginationResult<TEntity>
            {
                Entities = result,
                TotalRecords = totalRecords
            };

            return paginationResult;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<TId> InsertAndGetIdAsync<TId>(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            var idProperty = typeof(TEntity).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("A entidade não possui uma propriedade 'Id' válida para retornar o novo ID.");
            }

            return (TId)Convert.ChangeType(idProperty.GetValue(entity), typeof(TId));
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<TId>(TId id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        #region  PRIVATE METHODS  
        private bool IsPropertyContainsValue(TEntity entity, string searchTerm)
        {
            var properties = entity.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var propertyValue = propertyInfo.GetValue(entity);
                if (propertyValue != null && propertyValue.ToString().ToLower().Contains(searchTerm))
                {
                    return true;
                }
            }
            return false;
        }

        private IQueryable<TEntity> IncludeAllNavigationProperties(IQueryable<TEntity> query)
        {
            var entityType = typeof(TEntity);
            var navigationProperties = entityType.GetProperties()
                .Where(p => p.PropertyType.IsClass && !p.PropertyType.FullName.StartsWith("System.")
                            || p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                .ToList();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return query;
        }

        private IQueryable<TEntity> ApplyTenantOrganizationFilters(IQueryable<TEntity> query, long? tenantId, List<long> listUnitsId)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity), "entity");

            var tenantIdProperty = typeof(TEntity).GetProperty("TenantId");
            if (tenantId.HasValue && tenantIdProperty != null)
            {
                MemberExpression tenantProperty = Expression.Property(param, "TenantId");
                ConstantExpression tenantValue = Expression.Constant(tenantId);
                BinaryExpression tenantFilter = Expression.Equal(tenantProperty, tenantValue);
                Expression<Func<TEntity, bool>> tenantLambda = Expression.Lambda<Func<TEntity, bool>>(tenantFilter, param);
                query = query.Where(tenantLambda);
            }

            var organizationIdProperty = typeof(TEntity).GetProperty("OrganizationId");
            if (listUnitsId != null && listUnitsId.Count() > 0 && organizationIdProperty != null)
            {
                MemberExpression organizationProperty = Expression.Property(param, "OrganizationId");
                ConstantExpression organizationValue = Expression.Constant(listUnitsId);
                BinaryExpression organizationFilter = Expression.Equal(organizationProperty, organizationValue);
                Expression<Func<TEntity, bool>> organizationLambda = Expression.Lambda<Func<TEntity, bool>>(organizationFilter, param);
                query = query.Where(organizationLambda);
            }

            return query;
        }

        #endregion
    }

    public static class ExpressionHelper
    {
        public static Expression<Func<TEntity, object>> GetPropertyExpression<TEntity>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            MemberExpression property = Expression.Property(parameter, propertyName);
            UnaryExpression conversion = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
        }
    }
}
