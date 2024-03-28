namespace Bars.GisIntegration.Base.Exporters.Bills
{
    using System;

    using Bars.GisIntegration.Base.Tasks.PrepareData.Bills;
    using Bars.GisIntegration.Base.Tasks.SendData.Bills;

    /// <summary>
    /// Метод позволяет импортировать информацию о страховых продуктах
    /// </summary>
    public class InsuranceProductExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт данных о страховых продуктах";

        /// <summary>
        /// Описание метода
        /// </summary>
        public override string Description => "Метод позволяет импортировать информацию о страховых продуктах";

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
        public override Type SendDataTaskType => typeof(ExportInsuranceProductTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(InsuranceProductPrepareDataTask);
    }
}