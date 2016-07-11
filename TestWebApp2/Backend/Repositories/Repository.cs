using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestWebApp2.Backend.Repositories
{
	public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
	{
		internal Repository(AppContext context)
		{
			Context = context;
		}

		#region Main Methods

		public TEntity GetById(TKey id) => Context.Set<TEntity>()
		                                          .Find(id);

		public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>()
		                                               .ToList();

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => Context.Set<TEntity>()
		                                                                                      .Where(predicate)
		                                                                                      .ToList();

		public void Add(TEntity entity)
		{
			Context.Set<TEntity>()
			       .Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			Context.Set<TEntity>()
			       .AddRange(entities);
		}

		public void Remove(TEntity entity)
		{
			Context.Set<TEntity>()
			       .Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			Context.Set<TEntity>()
			       .RemoveRange(entities);
		}

		#endregion

		protected AppContext Context { get; }
	}
}
