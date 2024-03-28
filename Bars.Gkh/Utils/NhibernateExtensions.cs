namespace Bars.Gkh.Utils
{
    using System.Collections;
    using System.Collections.Generic;

    using NHibernate;

    /// <summary>
    /// Расширения для работы с классами Nhibhernate
    /// </summary>
    public static class NhibernateExtensions
    {
        /// <summary>
        /// Применить параметры к запросу
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="parameters">Параметры: набор пар (название параметра, значение)</param>
        /// <returns>Запрос с примененными параметрами</returns>
        public static IQuery SetParams(this IQuery query, IEnumerable<(string, object)> parameters)
        {
            foreach (var param in parameters)
            {
                if (param.Item2 is IEnumerable list)
                {
                    query.SetParameterList(param.Item1, list);
                }
                else
                {
                    query.SetParameter(param.Item1, param.Item2);
                }
            }

            return query;
        }
    }
}