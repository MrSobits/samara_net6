namespace Bars.GisIntegration.Base.Exporters.Inspection
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Inspection;
    using Bars.GisIntegration.Base.Tasks.SendData.Inspection;

    /// <summary>
    /// Класс экспортер данных по планам проверок
    /// </summary>
    public class InspectionPlanExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт планов проверок";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 110;

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportInspectionPlanTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        /// <returns>Тип задачи подготовки данных</returns>
        public override Type PrepareDataTaskType => typeof(InspectionPlanPrepareDataTask);

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
                    "Импортировать список справочников",
                    "Импортировать данные справочника"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
            }
        }
    }
}
