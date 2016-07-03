using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestWebApp.Models.Entities;

namespace TestWebApp.Models.Repositories
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        T GetById(long id);
        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate); 

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        int Complete();
    }
}
