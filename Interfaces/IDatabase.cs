namespace Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq.Expressions;

    /// <summary>
    /// Interface to define how data should be fetch from a resource
    /// </summary>
    /// <typeparam name="T">The object to map</typeparam>
    public interface IDatabase<T>
    {
        Task<List<T>> GetAll();

        Task Create(T data);

        Task Update(T data);

        Task<T> Get(int id);

        Task<T> Get(string id);

        Task<List<T>> Query(Expression<Func<T, bool>> predicate);
    }
}
