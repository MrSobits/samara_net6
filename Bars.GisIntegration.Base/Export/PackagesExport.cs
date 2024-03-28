namespace Bars.GisIntegration.Base.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    /// <summary>
    /// Сервис экспорта списка пакетов
    /// </summary>
    public class PackagesExport : BaseDataExportService
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
                var prepareDataResult = taskManager.GetPreparingDataTriggerResult(triggerId);

                return prepareDataResult.Packages.Select(x => new { x.Id, x.Name, NotSignedData = "XML с данными" }).ToList();
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }
    }
}