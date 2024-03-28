namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Класс экспортер данных о ПУ
    /// </summary>
    public class MeteringDeviceDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт данных приборов учета";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 210;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var result = new List<string>();

            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
            //var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var houseOMSDataExporter = this.Container.Resolve<IDataExporter>("HouseOMSDataExporter");
            var houseRSODataExporter = this.Container.Resolve<IDataExporter>("HouseRSODataExporter");
            var houseUODataExporter = this.Container.Resolve<IDataExporter>("HouseUODataExporter");
            var accountDataExporter = this.Container.Resolve<IDataExporter>("AccountDataExporter");


            try
            {
                result.Add(orgRegistryExporter.Name);
               // result.Add(dataProviderExporter.Name);
                result.Add("Экспортировать список справочников");
                result.Add("Экспортировать данные справочника");
                result.Add(houseOMSDataExporter.Name);
                result.Add(houseRSODataExporter.Name);
                result.Add(houseUODataExporter.Name);
                result.Add(accountDataExporter.Name);

            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
             //   this.Container.Release(dataProviderExporter);
                this.Container.Release(houseOMSDataExporter);
                this.Container.Release(houseRSODataExporter);
                this.Container.Release(houseUODataExporter);
                this.Container.Release(accountDataExporter);
            }

            return result;
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportMeteringDeviceDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(MeteringDevicePrepareDataTask);
    }
}
