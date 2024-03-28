namespace Bars.GisIntegration.Base.Exporters.OrgRegistryCommon
{
    using System;

    using Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistryCommon;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;

    /// <summary>
    /// Экспорт сведений о поставщиках информации
    /// </summary>
    [Obsolete("Метод exportDataProvider упразднен")]
    public class DataProviderExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Импорт сведений о поставщиках информации";

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет получить коды организаций, связанных с информационной системой (в т.ч. неактивные) и ключ поставщика данных";

        /// <summary>
        /// Порядок импорта в списке.
        /// </summary>
        public override int Order => 20;

        /// <summary>
        /// Метод может выполняться только от имени поставщика данных
        /// </summary>
        public override bool DataSupplierIsRequired => false;

        /// <summary>
        /// Тип задачи для получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportDataProviderTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(DataProviderPrepareDataTask);
    }
}