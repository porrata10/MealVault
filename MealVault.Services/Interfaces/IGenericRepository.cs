using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MealVault.Services.Interfaces
{
    public interface IGenericRepository<T> : IDisposable
    {
        Task AddAndSaveAsync(T entity, CancellationToken token = default);
        Task AddRangeAndSaveAsync(IEnumerable<T> entity, CancellationToken token = default);

        Task UpdateAndSaveAsync(T entity, CancellationToken token = default);
        Task UpdateAndSaveDiffAsync(T entity, T originalEntity, CancellationToken token = default);
        void RemoveAndSave(T entity);
        Task RemoveAndSaveAsync(T entity, CancellationToken token = default);
        void RemoveRangeAndSave(IEnumerable<T> entity);
        Task RemoveRangeAndSaveAsync(IEnumerable<T> entity, CancellationToken token = default);

        Task<T> FindByIdAsync(int id, CancellationToken token = default);
        Task<T> FindByGuidAsync(Guid guid, CancellationToken token = default);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, CancellationToken token = default);
        Task<IEnumerable<T>> FindAllAsync(CancellationToken token = default);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);

        Task<T> GetFirst(CancellationToken token = default);

        T FirstByCondition(Expression<Func<T, bool>> expression);
        Task<T> FirstByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
        Task<T> FirstByConditionNoTrackingAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);


        IEnumerable<T> GetUsingIncludes(IEnumerable<Expression<Func<T, object>>> includes, Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetUsingIncludes(IEnumerable<string> includes, Expression<Func<T, bool>> predicate);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        IQueryable<T> Select(Expression<Func<T, T>> selector);

        //ICollection<T> ExcuteSqlQuery(string sqlQuery, CommandType commandType, SqlParameter[] parameters = null);

        // 2. SqlCommand approach

        Task<ICollection<T>> ExecuteQueryAsync(string commandText, CommandType commandType, SqlParameter[] parameters = null, CancellationToken token = default);



    }
}
