namespace Bars.GisIntegration.Base.Exporters.CapitalRepair
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair;
    using Bars.GisIntegration.Base.Tasks.SendData.CapitalRepair;

    /// <summary>
    /// Импорт краткосрочных планов ремонта из ГИС
    /// </summary>
    public class PlanImporter : BaseDataExporter
    {
        public override string Name => "Импорт краткосрочных планов ремонта";

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет выгрузить ранее импортированные краткосрочные планы ремонта";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 100;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
       //     var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
         //           dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
           //     this.Container.Release(dataProviderExporter);
            }
        }

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        /// <returns>Тип задачи подготовки данных</returns>
        public override Type PrepareDataTaskType => typeof(PlanPrepareDataTask);

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(PlanImportDataTask);
    }
}
