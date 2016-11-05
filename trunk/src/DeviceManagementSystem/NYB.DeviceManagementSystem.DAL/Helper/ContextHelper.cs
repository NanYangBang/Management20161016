using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Entity
{
    public static class ContextHelper
    {
        public static IQueryable<T> Page<T, K>(this IQueryable<T> source, out int totalRecord, int pageIndex = 1, int pageSize = -1, Expression<Func<T, K>> orderby = null,
            bool ascending = true, bool addOrderByEmpty = true) where T : class
        {
            totalRecord = source.Count();

            if (orderby != null)
            {
                source = OrderBy(source, orderby, ascending);
            }
            else if (addOrderByEmpty)
            {
                source = source.OrderBy(p => "");
            }

            if (pageSize > 0)
            {
                //页码小于1，直接返回Count为零的List
                if (pageIndex < 1)
                {
                    return new List<T>().AsQueryable();
                }
                //页码大于最大页数，直接返回Count为零的List
                if ((pageIndex - 1) * pageSize >= totalRecord)
                {
                    return new List<T>().AsQueryable();
                }

                return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return source;
            }
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, out int totalRecord, string orderbyFieldName = null,
    bool ascending = true, int pageIndex = 1, int pageSize = -1, bool addOrderByEmpty = true) where T : class
        {
            totalRecord = source.Count();

            if (!string.IsNullOrWhiteSpace(orderbyFieldName))
            {
                source = source.OrderBy(orderbyFieldName, ascending);
            }
            else if (addOrderByEmpty)
            {
                source = source.OrderBy(p => "");
            }

            if (pageSize > 0)
            {
                //页码小于1，直接返回Count为零的List
                if (pageIndex < 1)
                {
                    return new List<T>().AsQueryable();
                }
                //页码大于最大页数，直接返回Count为零的List
                if ((pageIndex - 1) * pageSize >= totalRecord)
                {
                    return new List<T>().AsQueryable();
                }

                return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return source;
            }
        }

        public static IQueryable<T> OrderBy<T, K>(this IQueryable<T> source, Expression<Func<T, K>> orderby, bool ascending) where T : class
        {
            if (ascending)
            {
                return source.OrderBy(orderby);
            }
            else
            {
                return source.OrderByDescending(orderby);
            }
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending) where T : class
        {
            string[] propertyPath = propertyName.Split(new char[] { '.' });
            if (propertyPath.Length <= 1)
            {
                Type type = typeof(T);

                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentException("propertyName", "Not Exist");

                ParameterExpression param = Expression.Parameter(type, "p");
                Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
                LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

                string methodName = ascending ? "OrderBy" : "OrderByDescending";

                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

                return source.Provider.CreateQuery<T>(resultExp);
            }
            else
            {
                Type type = typeof(T);
                PropertyInfo include = type.GetProperty(propertyPath[0]);

                if (include == null)
                    throw new ArgumentException("propertyIncludeName", "Not Exist");

                Type typeInclude = include.PropertyType;
                PropertyInfo propertyInclude = typeInclude.GetProperty(propertyPath[1]);

                if (typeInclude == null)
                    throw new ArgumentException("propertyIncludesName", "Not Exist");

                ParameterExpression param = Expression.Parameter(type);

                Expression propertyAccessExpression = Expression.MakeMemberAccess(param, include);
                Expression propertyAccessExpressionInclude = Expression.MakeMemberAccess(propertyAccessExpression, propertyInclude);

                LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpressionInclude, param);

                string methodName = ascending ? "OrderBy" : "OrderByDescending";

                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, propertyInclude.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

                return source.Provider.CreateQuery<T>(resultExp);
            }

        }
    }
}
