using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedical_DataAccess.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		IQueryable<T> Entities { get; }
		IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> GetAll();
		T? GetById(object id);
		void Insert(T obj);
		Task InsertAsync(T obj);
		void InsertRange(List<T> obj);
		Task InsertRangeAsync(List<T> obj);
		void Update(T obj);
		void Delete(object entity);
		void Save();
		Task<T?> GetByIdAsync(object id);
		Task<IEnumerable<T>> GetAllAsync();
		Task UpdateAsync(T obj);
		Task DeleteAsync(object entity);
		Task SaveAsync();
		T? Find(Expression<Func<T, bool>> predicate);
		Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
	}
}
