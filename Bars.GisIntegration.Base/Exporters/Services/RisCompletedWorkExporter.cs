namespace Bars.GisIntegration.Base.Exporters.Services
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Services;
    using Bars.GisIntegration.Base.Tasks.SendData.Services;

    /// <summary>
    /// Экспортёр выполненных работ
    /// </summary>
    public class RisCompletedWorkExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт сведений о выполненных работах/услугах";

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет импортировать в ГИС ЖКХ сведения о фактически выполненных "
            + "работах/услугах, включая сведения об фактической стоимости и количестве выполненной работы/предоставленной услуге,"
            + " актах. Сведения могут относится как к плановой или внеплановой работе/услуге (в т.ч. аварийных работах и недопоставках).";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 190;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
        //    var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var votingProtocolExporter = this.Container.Resolve<IDataExporter>("VotingProtocolExporter");
            var additionalServicesExporter = this.Container.Resolve<IDataExporter>("AdditionalServicesExporter");
            var contractDataExporter = this.Container.Resolve<IDataExporter>("ContractDataExporter");
            var organizationWorksExporter = this.Container.Resolve<IDataExporter>("OrganizationWorksExporter");
            var workingListExporter = this.Container.Resolve<IDataExporter>("WorkingListExporter");
            var workingPlanExporter = this.Container.Resolve<IDataExporter>("WorkingPlanExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
           //         dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    votingProtocolExporter.Name,
                    additionalServicesExporter.Name,
                    contractDataExporter.Name,
                    organizationWorksExporter.Name,
                    workingListExporter.Name,
                    workingPlanExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
            //    this.Container.Release(dataProviderExporter);
                this.Container.Release(votingProtocolExporter);
                this.Container.Release(additionalServicesExporter);
                this.Container.Release(contractDataExporter);
                this.Container.Release(organizationWorksExporter);
                this.Container.Release(workingListExporter);
                this.Container.Release(workingPlanExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(CompletedWorkExportTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(CompletedWorkPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.Nsi;
    }
}
