namespace Bars.Gkh.RegOperator.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Domain.ParameterVersioning.Proxy;
    using Bars.Gkh.Entities;

    using Entities;
    using ParametersVersioning;

    /// <summary>
    /// Интерфейс для отслеживания изменений версионируемых параметров
    /// </summary>
    public interface IParameterTracker
    {
        /// <summary>
        /// Получить изменения
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="period">Период начисления</param>
        /// <returns>Список логов</returns>
        IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, IPeriod period);

        /// <summary>
        /// Получить изменения
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="date">Дата</param>
        /// <returns>Список логов</returns>
        IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, DateTime date);

        /// <summary>
        /// Получить фасад для работы с параметром
        /// </summary>
        /// <param name="parameterName">Имя параметра версионирования</param>
        /// <param name="account">Лицевой счет</param>
        /// <param name="period">Период начисления</param>
        IVersionedParameter GetParameter(string parameterName, BasePersonalAccount account, IPeriod period);
    }
}