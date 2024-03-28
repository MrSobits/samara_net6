namespace Bars.GisIntegration.Base.Exporters.Payment
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Payment;
    using Bars.GisIntegration.Base.Tasks.SendData.Payment;

    /// <summary>
    /// Класс экспортер документов «Извещение о принятии к исполнению распоряжения»
    /// </summary>
    public class NotificationsOfOrderExecutionExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт документов «Извещение о принятии к исполнению распоряжения»";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 250;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var paymentDocumentExporter = this.Container.Resolve<IDataExporter>("PaymentDocumentDataExporter");

            try
            {
                return new List<string> { paymentDocumentExporter?.Name };
            }
            finally
            {
                this.Container.Release(paymentDocumentExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportNotificationsOfOrderExecutionTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(NotificationsOfOrderExecutionPrepareDataTask);
    }
}
