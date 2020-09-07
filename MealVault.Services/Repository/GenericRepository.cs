using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MealVault.Services.Repository
{
    public class GenericRepository<T> : Interfaces.IGenericRepository<T> where T : class
    {
        internal DbContext context;
        internal DbSet<T> dbSet;

        //beneficios del generic repository todo tiene no tracking..
        //otra copa
        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }


        public async Task AddAndSaveAsync(T entity, CancellationToken token = default)
        {
            dbSet.Add(entity);
            await context.SaveChangesAsync(token);
        }

        public async Task AddRangeAndSaveAsync(IEnumerable<T> entity, CancellationToken token = default)
        {
            dbSet.AddRange(entity);
            await context.SaveChangesAsync(token);
        }

        public void RemoveAndSave(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public async Task RemoveAndSaveAsync(T entity, CancellationToken token = default)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(token);
        }

        public void RemoveRangeAndSave(IEnumerable<T> entity)
        {
            context.Set<T>().RemoveRange(entity);
            context.SaveChanges();
        }

        public async Task RemoveRangeAndSaveAsync(IEnumerable<T> entity, CancellationToken token = default)
        {
            context.Set<T>().RemoveRange(entity);
            await context.SaveChangesAsync(token);
        }

        public async Task UpdateAndSaveAsync(T entity, CancellationToken token = default)
        {

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(token);
        }

        public async Task UpdateAndSaveDiffAsync(T entity, T originalEntity, CancellationToken token = default)
        {
            var entry = context.Entry(entity);
            var ogEntry = context.Entry(originalEntity);

            // Get the old field value from the database.
            var entityDB = entry.GetDatabaseValues();



            bool filterPrimaryKeys(IProperty property)
            {

                //Debug.WriteLine(property.Name + " is primary key? " + property.IsPrimaryKey());
                return property.IsPrimaryKey() == false;

            }



            List<IProperty> filteredProps = entry.OriginalValues.Properties.Where(filterPrimaryKeys).ToList();



            foreach (var property in filteredProps)
            {

                var dbValue = entityDB[property.Name];
                // Get the current value from posted edit page.
                var modifiedValue = entry.CurrentValues[property.Name];
                var unmodifieldValue = ogEntry.CurrentValues[property.Name];


                if (!Equals(dbValue, modifiedValue) && Equals(dbValue, unmodifieldValue))
                {
                    entry.Property(property.Name).IsModified = true;
                }
            }

            await context.SaveChangesAsync(token);


        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return dbSet.Where(predicate).AsNoTracking().ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(CancellationToken token = default)
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> FindByGuidAsync(Guid guid, CancellationToken token = default)
        {
            return await dbSet.FindAsync(guid, token);
        }

        public async Task<T> FindByIdAsync(int id, CancellationToken token = default)
        {
            return await dbSet.FindAsync(id, token);
        }

        public T FirstByCondition(Expression<Func<T, bool>> expression)
        {
            return dbSet.AsNoTracking().FirstOrDefault(expression);
        }

        public async Task<T> FirstByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken token = default)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(expression, token);


        }


        public async Task<T> FirstByConditionNoTrackingAsync(Expression<Func<T, bool>> expression, CancellationToken token = default)
        {
            return await dbSet.FirstOrDefaultAsync(expression, token);

        }

        public async Task<T> GetFirst(CancellationToken token = default)
        {
            return await dbSet.FirstOrDefaultAsync(token);

        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return await dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }




        public IEnumerable<T> GetUsingIncludes(IEnumerable<Expression<Func<T, object>>> includes, Expression<Func<T, bool>> predicate)
        {
            return includes.Aggregate(
                    // The initial accumulator value.
                    seed: dbSet.AsQueryable(),
                    // An accumulator funcion to be invoked on each element.
                    func: (query, navigationProperty) => query.Include(navigationProperty)
                ).Where(predicate)
                .ToList();
        }

        public IEnumerable<T> GetUsingIncludes(IEnumerable<string> includes, Expression<Func<T, bool>> predicate)
        {
            // convert the DbSet object into a Queryable to allow appending to the generated Select statement
            var query = dbSet.AsQueryable();

            // loop through all the navigation properties in the T entity and append the JOIN clause 
            // property name must be passed in correctly 
            foreach (string navigationProperty in includes)
            {
                query = query.Include(navigationProperty);
            }

            // return the final query
            return query.Where(predicate);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            var query = dbSet.AsQueryable();

            return query.Where(predicate);
        }

        public IQueryable<T> Select(Expression<Func<T, T>> selector)
        {
            var query = dbSet.AsQueryable();

            return query.Select(selector);
        }

        //sp stuff based on ref: http://csharpdocs.com/call-stored-procedure-using-repository-pattern/    
        //dbs change for efcore      

        public async Task<ICollection<T>> ExecuteQueryAsync(string commandText, CommandType commandType, SqlParameter[] parameters = null, CancellationToken token = default)
        {
            var connection = context.Database.GetDbConnection();

            connection.Close();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = commandType;

                try
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        var mapper = new DataReaderMapper<T>();
                        return await mapper.MapToList(reader, token);
                    }
                }
                catch (Exception e)
                {
                    
                }

                using (var reader = command.ExecuteReader())
                {
                    var mapper = new DataReaderMapper<T>();
                    return await mapper.MapToList(reader, token);
                }
            }
        }




        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
