namespace Bars.GisIntegration.Base.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    /// <summary>
    /// Сервис выгрузки результатов загрузки вложений в excel
    /// </summary>
    public class UploadAttachmentResultExport : BaseDataExportService
    {
        /// <summary>
        /// Получить данные для экспорта
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список объектов для экспорта</returns>
        public override IList GetExportData(BaseParams baseParams)
        {
            var triggerId = baseParams.Params.GetAs<long>("triggerId");

            if (triggerId == 0)
            {
                throw new Exception("Пустой идентификатор триггера");
            }

            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                return taskManager.GetUploadAttachmentResultList(triggerId);
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }
    }
}
