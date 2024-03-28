namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    using Entities;
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Domain.ParameterVersioning.Proxy;

    /// <summary>
    /// Фасадный интерфейс для работы с версионируемыми параметрами
    /// </summary>
    public interface IVersionedParameter
    {
        /// <summary>
        /// Имя параметра
        /// </summary>
        string ParameterName { get; set; }

        /// <summary>
        /// Получить изменения параметров за период начисления
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="period">Период начисления</param>
        /// <returns>Список изменений</returns>
        IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, IPeriod period);

        IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, DateTime date);

        /// <summary>
        /// Получить последнее изменение параметра на дату <see cref="date"/>
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения параметра</typeparam>
        /// <param name="account">Лицевой счет</param>
        /// <param name="period">Период начисления</param>
        /// <param name="date">Дата, на которую нужно получить значение параметра</param>
        /// <returns>Значение параметра</returns>
        KeyValuePair<object, T> GetLastChangedByDate<T>(BasePersonalAccount account, IPeriod period, DateTime date);

        /// <summary>
        /// Получить актуальное действующее значение параметра
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения параметра</typeparam>
        /// <param name="account">Лицевой счет</param>
        /// <param name="date">Дата, на которую нужно получить актуальный параметр</param>
        /// <param name="useCache">Использовать значения из кеша, либо вычислять всегда</param>
        /// <param name="limitDateApplied">Ограничивать дату применения или нет сверху параметром date</param>
        /// <returns>Значение параметра</returns>
        KeyValuePair<object, T> GetActualByDate<T>(BasePersonalAccount account, DateTime date, bool useCache, bool limitDateApplied = false);
    }
}