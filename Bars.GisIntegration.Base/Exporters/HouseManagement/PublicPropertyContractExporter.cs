namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Класс - экспортер договоров на пользование общим имуществом
    /// </summary>
    public class PublicPropertyContractExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт договора на пользование общим имуществом";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 90;

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет создавать, изменять и удалять информацию о договорах на пользование общим имуществом";

        /// <summary>
        /// Порядок действий
        /// </summary>
        /// <returns>Список действий</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
          //  var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var votingProtocolExporter = this.Container.Resolve<IDataExporter>("VotingProtocolExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
             //       dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    votingProtocolExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
          //      this.Container.Release(dataProviderExporter);
                this.Container.Release(votingProtocolExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportPublicPropertyContractDataTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(PublicPropertyContractPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.HomeManagement;
    }
}
