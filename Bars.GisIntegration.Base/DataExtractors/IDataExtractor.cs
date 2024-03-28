namespace Bars.GisIntegration.Base.DataExtractors
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    ///  Интерфейс компонента для извленения данных из сторонней системы в таблицы Ris
    /// </summary>
    /// <typeparam name="TRisEntity">Тип сущности Ris</typeparam>
    public interface IDataExtractor<TRisEntity> where TRisEntity : BaseEntity
    {
        /// <summary>
        /// Поставщик данных
        /// </summary>
        RisContragent Contragent { get; set; }

        /// <summary>
        /// Извлечь данные Ris
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        /// <returns>Ris сущности</returns>
        List<TRisEntity> Extract(DynamicDictionary parameters = null);

        /// <summary>
        /// Лог экстрактора
        /// </summary>
        List<ILogRecord> Log { get; set; }
    }
}
