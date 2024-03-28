namespace Bars.Gkh.Ris.Integration.Payment.Exporters
{
    using System;
    using System.Collections.Generic;
    using Tasks.Payment.SupplierNotificationsOfOrderExecution;

    /// <summary>
    /// Класс экспортер документов «Извещение о принятии к исполнению распоряжения, размещаемых исполнителем»
    /// </summary>
    public class SupplierNotificationsOfOrderExecutionExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт документов «Извещение о принятии к исполнению распоряжения, размещаемых исполнителем»";

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
        /// Получить тип задачи получения результатов экспорта
        /// </summary>
        /// <returns>Тип задачи</returns>
        public override Type GetSendDataTaskType()
        {
            return typeof(ExportSupplierNotificationsOfOrderExecutionTask);
        }

        /// <summary>
        /// Получить тип задачи подготовки данных
        /// </summary>
        /// <returns>Тип задачи подготовки данных</returns>
        public override Type GetPrepareDataTaskType()
        {
            return typeof(SupplierNotificationsOfOrderExecutionPrepareDataTask);
        }
    }
}
