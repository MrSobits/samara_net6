using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems.Intf
{
    using Entities.ImportIncrementalData.LoadFromOtherSystems;

    public interface IRegisterFormatForOtherSystems
    {
        /// <summary>
        /// Добавление файла в file_upload
        /// </summary>
        /// <returns></returns>
        int AddFileToUpload();

        /// <summary>
        /// получение формата загрузки
        /// </summary>
        /// <returns></returns>
        Dictionary<string, List<Template>> GetFormat();

        /// <summary>
        /// получение заголовков секций формата
        /// </summary>
        /// <returns></returns>
        List<TemplateHeader> GetFormatHeaders();

        /// <summary>
        /// получить результат загрузки
        /// </summary>
        /// <returns></returns>
        bool GetLoadResult();

        /// <summary>
        /// Обновить статус загрузки
        /// </summary>
        /// <param name="statuses"></param>
        /// <param name="dataStatusId"></param>
        void UpdateFileStatus(UploadStatuses statuses, int dataStatusId);

        /// <summary>
        /// Обновление прогресса загрузки файла
        /// </summary>
        /// <param name="progress"></param>
        void UpdateFileProgress(decimal progress);

        /// <summary>
        /// Сортировка при загрузке
        /// </summary>
        void SetOrdering();

        void UpdateBankAndSupplier();
    }
}
