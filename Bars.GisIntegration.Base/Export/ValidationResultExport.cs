namespace Bars.GisIntegration.Base.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    /// <summary>
    /// Сервис выгрузки результатов валидации в excel
    /// </summary>
    public class ValidationResultExport : BaseDataExportService
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
                return taskManager.GetValidationResultList(triggerId);
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }
    }
}