using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IReadRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Queryable();
        T Find(object entityId);
        IQueryable<T> Filter(Expression<Func<T, bool>> expression);
        bool Any(Expression<Func<T, bool>> expression);
        int Count(Expression<Func<T, bool>> expression);

        IQueryable<T> FromSqlRaw(string sql, params object[] parameters);
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T>? ById<T>(this IQueryable<T> query, string columnName, Guid id)
        {
            // This expression is lambad : e => e.Id == id
            var parameter = Expression.Parameter(typeof(T));
            var left = Expression.Property(parameter, columnName);
            var right = Expression.Constant(id);
            var equal = Expression.Equal(left, right);
            var byId = Expression.Lambda<Func<T, bool>>(equal, parameter);
            return query.Where(byId);
        }
    }
}
