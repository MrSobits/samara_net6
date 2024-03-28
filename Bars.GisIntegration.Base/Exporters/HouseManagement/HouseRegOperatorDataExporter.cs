namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Класс экспортер данных домов для регионального оператора
    /// </summary>
    public class HouseRegOperatorDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        public override string Name => "Экспорт сведений о доме для регионального оператора";

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var result = new List<string>();

            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");

            try
            {
                result.Add(orgRegistryExporter.Name);
                result.Add("Экспортировать список справочников");
                result.Add("Экспортировать данные справочника");
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
            }

            return result;
        }

        /// <summary>
        /// Порядок экспортера в списке
        /// </summary>
        public override int Order => 141;

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(HouseRegOperatorExportDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(HouseRegOperatorPrepareDataTask);
    }
}
