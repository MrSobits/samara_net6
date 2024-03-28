namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;

    using Entities;

    /// <summary>
    /// Кеш расчитанных параметров
    /// </summary>
    public interface ICalculatedParameterCache : IDisposable
    {
        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="periods">Периоды</param>
        void Init(IQueryable<BasePersonalAccount> query, IPeriod[] periods);

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="accountIds">Список идентификаторов</param>
        /// <param name="periods">Периоды</param>
        void Init(long[] accountIds, IPeriod[] periods);

        /// <summary>
        /// Получить параметры
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="period">Период</param>
        Dictionary<string, object> GetParameters(BasePersonalAccount account, IPeriod period);
    }
}