namespace Bars.GisIntegration.Base.Exporters.DeviceMetering
{
    using System;
    using System.Collections.Generic;
    using Exporters;
    using Tasks.PrepareData.DeviceMetering;
    using Tasks.SendData.DeviceMetering;

    /// <summary>
    /// Экспортер "Экспорт сведений о показаниях приборов учета"
    /// </summary>
    public class MeteringDeviceValuesExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт сведений о показаниях приборов учета";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 220;

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Метод позволяет экспортировать в ГИС ЖКХ сведения о показаниях приборов учета в разрезе дома. Сведения включают в себя идентификатор прибора учета, ссылку на лицевой счет и показатели потребления.";

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
            //var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var accountDataExporter = this.Container.Resolve<IDataExporter>("AccountDataExporter");
            var meteringDeviceDataExporter = this.Container.Resolve<IDataExporter>("MeteringDeviceDataExporter");
            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
                    //dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    "Экспорт сведений о доме",
                    accountDataExporter.Name,
                    meteringDeviceDataExporter.Name,
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
                //this.Container.Release(dataProviderExporter);
                this.Container.Release(accountDataExporter);
                this.Container.Release(meteringDeviceDataExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(MeteringDeviceValuesExportTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(MeteringDeviceValuesPrepareDataTask);
    }
}
