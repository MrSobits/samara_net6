namespace Bars.Gkh.Gis.DomainService.Helper.HqlHelper.Impl
{
    using System.Linq;
    using System.Text;
    using B4;

    public class HqlHelper : IHqlHelper
    {
        /// <summary>
        /// Получить HQL запрос сортировки данных по входным параметрам
        /// </summary>
        public string GetOrderHqlQuery(LoadParam loadParam)
        {
            return loadParam.Order.Any()
                ? string.Format(" ORDER BY {0} ",
                    string.Join(", ", loadParam.Order.Select(x => x.Name + (x.Asc ? " ASC " : " DESC "))))
                : "";
        }

        /// <summary>
        /// Получить HQL запрос фильтрации данных по ComplexFilter'у
        /// </summary>
        public string GetComplexFilterHqlQuery(LoadParam loadParam)
        {
            return loadParam.ComplexFilter == null
                ? ""
                : ReadComplexFilter(loadParam.ComplexFilter, new StringBuilder()).ToString();
        }

        /// <summary>
        /// Обход ComplexFilter'a
        /// </summary>
        private StringBuilder ReadComplexFilter(ComplexFilter complexFilter, StringBuilder result)
        {
            switch (complexFilter.Operator)
            {
                case ComplexFilterOperator.eq:
                    {
                        result.AppendFormat(" AND {0} = '{1}' ", complexFilter.Field, complexFilter.Value);
                        break;
                    }
                case ComplexFilterOperator.icontains:
                    {
                        result.AppendFormat(" AND UPPER({0}) LIKE UPPER('%{1}%') ", complexFilter.Field, complexFilter.Value);
                        break;
                    }
            }

            if (complexFilter.Left != null)
            {
                result.Append(ReadComplexFilter(complexFilter.Left, result));
            }
            if (complexFilter.Right != null)
            {
                result.Append(ReadComplexFilter(complexFilter.Right, result));
            }
            return result;
        }
    }
}