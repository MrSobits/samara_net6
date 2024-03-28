using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using GisGkhLibrary.NsiServiceAsync;
using Sobits.GisGkh.Entities;

namespace Sobits.GisGkh.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ЖКХ
    /// </summary>
    public interface IExportDataProviderNsiItemsService
    {
        /// <summary>
        /// Сохранить запрос
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="registryNumber">Номер справочника</param>
        void SaveRequest(GisGkhRequests req, exportDataProviderNsiItemRequestRegistryNumber registryNumber);

        /// <summary>
        /// Проверить ответы
        /// </summary>
        /// <param name="req">Запрос в ГИС ЖКХ</param>
        /// <param name="orgPPAGUID">GUID организации</param>
        void CheckAnswer(GisGkhRequests req, string orgPPAGUID);

        /// <summary>
        /// Обработать ответ
        /// </summary>
        /// <param name="req">Запрос в ГИС ЖКХ</param>
        void ProcessAnswer(GisGkhRequests req);
    }
}
