namespace Bars.GisIntegration.Base.Exporters.OrgRegistryCommon
{
    using System;

    using Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistryCommon;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;

    /// <summary>
    /// Метод для получения orgRootEntityGuid и orgVersionGuid
    /// </summary>
    public class OrgRegistryExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Импорт данных организаций";

        /// <summary>
        /// Описание метода
        /// </summary>
        public override string Description => "Получение orgRootEntityGuid, orgVersionGuid, orgPPAGUID";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 10;

        /// <summary>
        /// Метод может выполняться только от имени поставщика данных
        /// </summary>
        public override bool DataSupplierIsRequired => false;

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportOrgRegistryTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(OrgRegistryPrepareDataTask);
    }
}