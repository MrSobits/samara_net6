namespace Bars.GisIntegration.Base.Exporters.Services
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Services;
    using Bars.GisIntegration.Base.Tasks.SendData.Services;

    /// <summary>
    /// Экспортер "Экспорт актуальных планов по перечню работ/услуг"
    /// </summary>
    public class WorkingPlanExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера 
        /// </summary>
        public override string Name => "Экспорт актуальных планов по перечню работ/услуг";

        /// <summary>
        /// Описание метода
        /// </summary>
        public override string Description => @"Операция позволяет импортировать в ГИС ЖКХ сведения о датах планируемых в соответствии с перечнем (см. importWorkingList) работ/услуг. Сведения о планируемых работах заменяют имеющиеся ранее.
Планы могут быть импортированы только к перечню в статусе «Утвержден»";

        /// <summary>
        /// Очередность
        /// </summary>
        public override int Order => 180;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
       //     var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var votingProtocolExporter = this.Container.Resolve<IDataExporter>("VotingProtocolExporter");
            var additionalServicesExporter = this.Container.Resolve<IDataExporter>("AdditionalServicesExporter");
            var contractDataExporter = this.Container.Resolve<IDataExporter>("ContractDataExporter");
            var organizationWorksExporter = this.Container.Resolve<IDataExporter>("OrganizationWorksExporter");
            var workingListExporter = this.Container.Resolve<IDataExporter>("WorkingListExporter");

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
                    organizationWorksExporter.Name,
                    workingListExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
        //        this.Container.Release(dataProviderExporter);
                this.Container.Release(votingProtocolExporter);
                this.Container.Release(additionalServicesExporter);
                this.Container.Release(contractDataExporter);
                this.Container.Release(organizationWorksExporter);
                this.Container.Release(workingListExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(WorkingPlanExportTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(WorkingPlanPrepareDataTask);
    }
}
