namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict;

    /// <summary>
    /// API. Интерфейс сервиса для взаимодействием со справочниками
    /// </summary>
    public interface IDictService
    {
        /// <summary>
        /// Получить перечисление <see cref="Dict"/>
        /// </summary>
        /// <param name="date">Дата последней актуализации справочников</param>
        IEnumerable<Dict> GetList(DateTime? date);
        
        /// <summary>
        /// Получить перечисление <see cref="GroupViolations"/>
        /// </summary>
        /// <param name="date">Дата последней актуализации справочников</param>
        IEnumerable<GroupViolations> GroupViolationsList(DateTime? date);

        /// <summary>
        /// Получить список ФИАС адресов
        /// </summary>
        /// <param name="lastDateUpdate">Дата последнего обновления</param>
        /// <param name="municipalityId">Уникальный идентификатор муниципального образования</param>
        /// <returns>Модель объектов и домов ФИАС</returns>
        Task<FiasResponse> Fias(long municipalityId, DateTime? lastDateUpdate);

        /// <summary>
        /// Получить список статусов объектов
        /// </summary>
        Task<IEnumerable<TransferDocStatus>> DocStatusListAsync();
    }
}