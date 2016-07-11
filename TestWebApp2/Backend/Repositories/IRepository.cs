using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestWebApp2.Backend.Repositories
{
	public interface IRepository<TEntity, TKey> where TEntity : class
	{
		TEntity GetById(TKey id);
		IEnumerable<TEntity> GetAll();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
	}
}
