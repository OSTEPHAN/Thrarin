
namespace Thrarin.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public static class EntityQueryExtensions
    {
        public static IQueryable<T> Query<T>(this IEntityQuery entityQuery, params Expression<Func<T, object>>[] includes) where T : class, IEntity
        {
            var convertedIncludes = includes.Select(i => i.Body).MemberPaths().Where(i => !string.IsNullOrEmpty(i)).ToArray();
            return entityQuery.Query<T>(convertedIncludes);
        }

        public static IEnumerable<T> Query<T>(this IEntityQuery entityQuery, Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes) where T : class, IEntity
        {
            return entityQuery.Query<T>(includes).Where(whereCondition).ToList().AsEnumerable();
        }
    }
}
