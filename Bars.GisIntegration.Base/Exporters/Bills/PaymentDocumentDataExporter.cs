namespace Bars.GisIntegration.Base.Exporters.Bills
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Bills;
    using Bars.GisIntegration.Base.Tasks.SendData.Bills;

    /// <summary>
    /// Экспортер сведений о платежных документах
    /// </summary>
    public class PaymentDocumentDataExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт сведений о платежных документах";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 240;

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет экспортировать в ГИС ЖКХ сведения о платежных документах";

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
            //var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            var accountDataExporter = this.Container.Resolve<IDataExporter>("AccountDataExporter");
            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
                    //dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника",
                    "Экспорт сведений о доме",
                    accountDataExporter.Name
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
                //this.Container.Release(dataProviderExporter);
                this.Container.Release(accountDataExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportPaymentDocumentTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(PaymentDocumentPrepareDataTask);
    }
}