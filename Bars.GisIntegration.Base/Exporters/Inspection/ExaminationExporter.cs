namespace Bars.GisIntegration.Base.Exporters.Inspection
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Inspection;
    using Bars.GisIntegration.Base.Tasks.SendData.Inspection;

    /// <summary>
    /// Экспортер сведений о выполняющихся и проведенных проверках
    /// </summary>
    public class ExaminationExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт сведений о выполняющихся и проведенных проверках";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 120;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
            }
        }

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(ExaminationPrepareDataTask);

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExaminationExportTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.Inspection;
    }
}
