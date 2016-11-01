using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    }
}
