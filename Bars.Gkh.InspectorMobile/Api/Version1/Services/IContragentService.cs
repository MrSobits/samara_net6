using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с контрагентами
    /// </summary>
    public interface IContragentService
    {
        /// <summary>
        /// Получить контрагента по идентификатору
        /// </summary>
        /// <param name="contragentId">Идентификатор контрагента</param>
        /// <returns>Контрагент</returns>
        Task<object> GetAsync(long contragentId);

        /// <summary>
        /// Получить список контрагентов
        /// </summary>
        /// <param name="fullList">Признак получения полного списка контрагентов</param>
        /// <param name="contragentIds">Идентификаторы контрагентов</param>
        /// <returns>Список контрагентов</returns>
        Task<IEnumerable<object>> GetListAsync(bool fullList, long[] contragentIds);
    }
}