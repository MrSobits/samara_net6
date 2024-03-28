namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Экспортёр уставов
    /// </summary>
    public class CharterDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт уставов";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 80;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
           // var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var votingProtocolExporter = this.Container.Resolve<IDataExporter>("VotingProtocolExporter");
            var additionalServicesExporter = this.Container.Resolve<IDataExporter>("AdditionalServicesExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
                   // dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    votingProtocolExporter.Name,
                    votingProtocolExporter.Name,
                    additionalServicesExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
               // this.Container.Release(dataProviderExporter);
                this.Container.Release(votingProtocolExporter);
                this.Container.Release(additionalServicesExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportCharterDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(CharterPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.HomeManagement;
    }
}