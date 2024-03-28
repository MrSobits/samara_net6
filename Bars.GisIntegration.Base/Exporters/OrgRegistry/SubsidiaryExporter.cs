namespace Bars.GisIntegration.Base.Exporters.OrgRegistry
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistry;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistry;

    /// <summary>
    /// Экспортер сведений об обособленных подразделениях
    /// </summary>
    public class SubsidiaryExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Экспорт сведений об обособленных подразделениях";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Экспорт сведений об обособленных подразделениях";

        /// <summary>
        /// Порядок выполнения
        /// </summary>
        public override int Order => 60;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var result = new List<string>();

            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
         //   var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");          

            try
            {
                result.Add(orgRegistryExporter.Name);
         //       result.Add(dataProviderExporter.Name);               
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
           //     this.Container.Release(dataProviderExporter);
            }

            return result;
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(SubsidiaryExportTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(SubsidiaryPrepareDataTask);
    }
}
