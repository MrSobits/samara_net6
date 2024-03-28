namespace Bars.GisIntegration.Base.Exporters.CapitalRepair
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair;
    using Bars.GisIntegration.Base.Tasks.SendData.CapitalRepair;

    /// <summary>
    /// Класс экспортер данных договоров на выполнение работ (оказание услуг) по капитальному ремонту
    /// </summary>
    public class CapitalRepairContractsDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        public override string Name => "Экспорт договоров на выполнение работ (оказание услуг) по капитальному ремонту";

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
      //      var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var crPlanImporter = this.Container.Resolve<IDataExporter>("PlanImporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
      //              dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    crPlanImporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
        //        this.Container.Release(dataProviderExporter);
                this.Container.Release(crPlanImporter);
            }
        }

        /// <summary>
        /// Порядок экспортера в списке
        /// </summary>
        public override int Order => 280;

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(CapitalRepairContractsPrepareDataTask);

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(CapitalRepairContractsExportDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.CapitalRepairPrograms;
    }
}
