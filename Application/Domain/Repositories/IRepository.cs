using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IRepository<T> : IReadRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> expression);
        void DeleteRange(IEnumerable<T> entities);

        void ExecuteSQL(string sql, params object[] paramertes);
    }
}
