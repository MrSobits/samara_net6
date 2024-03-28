using Bars.B4;
using Bars.B4.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bars.Gkh.Overhaul.Hmao.Helpers
{
    /// <summary>
    /// Реализации функций для табличек на фронте для списка
    /// </summary>
    static public class EnumerableGridExtensions
    {
        /// <summary>
        /// Разбивание на страницы
        /// </summary>
        static public List<T> Paging<T>(this List<T> list, BaseParams baseParams)
        {
            var startstr = baseParams.Params.GetAs<string>("start");
            var limitstr = baseParams.Params.GetAs<string>("limit");

            int start;
            int limit;

            if (!int.TryParse(startstr, out start) || !int.TryParse(limitstr, out limit))
                return list;

            if (list.Count < start)
                return new List<T>();
            else if (list.Count < start + limit)
                return list.GetRange(start, list.Count - start);
            else
                return list.GetRange(start, limit);
        }

        /// <summary>
        /// Сортировка
        /// </summary>
        static public List<T> Order<T>(this List<T> list, BaseParams baseParams)
        {
            var sorts = baseParams.Params.GetAs<IEnumerable>("sort");
            if (sorts == null)
                return list;

            var type = list.GetType().GetGenericArguments()[0];

            foreach (var sort1 in sorts)
            {
                var sort = sort1 as DynamicDictionary;

                var property = type.GetProperty(sort.GetAs<string>("property"));
                var direction = sort.GetAs<string>("direction").toEnum();

                list.Sort(new ListComparer<T>(property, direction));
            }
            
            return list;
        }

        /// <summary>
        /// Фильтрация
        /// </summary>
        static public IEnumerable<T> Filter<T>(this IEnumerable<T> list, BaseParams baseParams)
        {
            if (!list.Any())
                return list;

            var loadParam = baseParams.GetLoadParam();
            if(loadParam.ComplexFilter == null)
                return list;

            var type = list.GetType().GetGenericArguments()[0];
            if (type == null)
                return list;

            list = ProcessFilter(type, list, loadParam.ComplexFilter).ToList();

            return list;
        }

        private static IEnumerable<T> ProcessFilter<T>(Type type, IEnumerable<T> list, ComplexFilter complexFilter)
        {         

            switch (complexFilter.Operator)
            {
                case ComplexFilterOperator.icontains:
                    var property = type.GetProperty(complexFilter.Field);
                    if (property == null)
                        return list;
                    return Contains(list, property, complexFilter.Value);
                case ComplexFilterOperator.and:
                    var filtered = list;

                    if (complexFilter.Left != null)
                        filtered = ProcessFilter(type, list, complexFilter.Left);

                    if (complexFilter.Right != null)
                        filtered = ProcessFilter(type, list, complexFilter.Right);

                    return filtered;

                default:
                    //throw new Exception(complexFilter.Operator.ToString());
                    return list;
            }
        }

        private static IEnumerable<T> Contains<T>(IEnumerable<T> list, PropertyInfo property, object value)
        {
            if (property.PropertyType == typeof(string))
            {
                return list.WhereIf(true, x =>
                {
                    string currValue = (string)property.GetValue(x);
                    return currValue.Trim().ToLower().Contains(((string)value).Trim().ToLower());
                });
            }
            else if (property.PropertyType == typeof(int))
            {
                return list.WhereIf(true, x =>
                {
                    string currValue = ((int)property.GetValue(x)).ToString();
                    return currValue.Trim().ToLower().Contains(((string)value).Trim().ToLower());
                });
            }
            else if (property.PropertyType == typeof(decimal))
            {
                return list.WhereIf(true, x =>
                {
                    string currValue = ((decimal)property.GetValue(x)).ToString();
                    return currValue.Trim().ToLower().Contains(((string)value).Trim().ToLower());
                });
            }
            else return list;
        }

        private class ListComparer<T> : IComparer<T>
        {
            private PropertyInfo property;
            private Direction direction;

            public ListComparer(PropertyInfo property, Direction direction)
            {
                this.property = property;
                this.direction = direction;
            }

            public int Compare(T x, T y)
            {
                if (property.PropertyType == typeof(string))
                {
                    var xvalue = (string)property.GetValue(x);
                    var yvalue = (string)property.GetValue(y);

                    if (direction == Direction.Ascending)
                        return String.Compare(xvalue, yvalue);
                    else
                        return String.Compare(yvalue, xvalue);
                }
                else if (property.PropertyType == typeof(int))
                {
                    var xvalue = (int)property.GetValue(x);
                    var yvalue = (int)property.GetValue(y);

                    if (direction == Direction.Ascending)
                        return xvalue - yvalue;
                    else
                        return yvalue - xvalue;
                }
                else if (property.PropertyType == typeof(decimal))
                {
                    var xvalue = (decimal)property.GetValue(x);
                    var yvalue = (decimal)property.GetValue(y);

                    if (direction == Direction.Ascending)
                        return decimal.Compare(xvalue, yvalue);
                    else
                        return decimal.Compare(yvalue, xvalue);
                }
                else return 0;
            }
        }

        static Direction toEnum(this string s)
        {
            switch(s)
            {
                case "ASC":
                    return Direction.Ascending;
                case "DESC":
                    return Direction.Descending;
                default:
                    throw new ApplicationException($"Неопознанный порядок сортировки: {s}");
            }
        }

        enum Direction
        {
            Ascending,
            Descending
        }
    }
}
