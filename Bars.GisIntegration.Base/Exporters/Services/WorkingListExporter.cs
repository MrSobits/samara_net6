namespace Bars.GisIntegration.Base.Exporters.Services
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Services;
    using Bars.GisIntegration.Base.Tasks.SendData.Services;

    /// <summary>
    /// Экспортер перечней работ и услуг на период
    /// </summary>
    public class WorkingListExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт перечня работ/услуг на период";

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description =>
            "Операция позволяет экспортировать в ГИС ЖКХ сведения о ценах планируемых на период работ/услуг";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 170;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
          //  var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var votingProtocolExporter = this.Container.Resolve<IDataExporter>("VotingProtocolExporter");
            var additionalServicesExporter = this.Container.Resolve<IDataExporter>("AdditionalServicesExporter");
            var contractDataExporter = this.Container.Resolve<IDataExporter>("ContractDataExporter");
            var organizationWorksExporter = this.Container.Resolve<IDataExporter>("OrganizationWorksExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
          //          dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    votingProtocolExporter.Name,
                    additionalServicesExporter.Name,
                    contractDataExporter.Name,
                    organizationWorksExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
          //      this.Container.Release(dataProviderExporter);
                this.Container.Release(votingProtocolExporter);
                this.Container.Release(additionalServicesExporter);
                this.Container.Release(contractDataExporter);
                this.Container.Release(organizationWorksExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(WorkingListExportTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(WorkingListPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.Nsi;
    }
}
