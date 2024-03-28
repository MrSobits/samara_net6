namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Класс экспортер данных домов для управляющих организаций
    /// </summary>
    public class HouseUODataExporter: BaseDataExporter
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        public override string Name => "Экспорт сведений о доме для управляющих организаций";

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var result = new List<string>();

            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
            //var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");

            try
            {
                result.Add(orgRegistryExporter.Name);
             //   result.Add(dataProviderExporter.Name);
                result.Add("Экспортировать список справочников");
                result.Add("Экспортировать данные справочника");
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
            //    this.Container.Release(dataProviderExporter);
            }

            return result;
        }

        /// <summary>
        /// Порядок экспортера в списке
        /// </summary>
        public override int Order => 140;

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(HouseUOExportDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(HouseUOPrepareDataTask);
    }
}
