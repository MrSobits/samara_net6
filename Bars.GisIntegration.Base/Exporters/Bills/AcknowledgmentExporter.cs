namespace Bars.GisIntegration.Base.Exporters.Bills
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.IoC;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.SendData.Bills;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Bills;

    /// <summary>
    /// Экспортер сведений о квитировании
    /// </summary>
    public class AcknowledgmentExporter: BaseDataExporter
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        public override string Name => "Экспорт сведений о квитировании";

        /// <summary>
        /// Порядок импорта в списке
        /// </summary>
        public override int Order => 270;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
            //var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");
            //todo расскомментировать после добавления экспортеров
            //var paymentDocumentDataExporter = this.Container.Resolve<IDataExporter>("PaymentDocumentDataExporter");
            //var notificationsOfOrderExecutionExporter = this.Container.Resolve<IDataExporter>("NotificationsOfOrderExecutionExporter");

            using (this.Container.Using(
                orgRegistryExporter//, 
                //dataProviderExporter, 
                //paymentDocumentDataExporter,
                //notificationsOfOrderExecutionExporter
                ))
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
                    //dataProviderExporter.Name,
                    //paymentDocumentDataExporter.Name,
                    //notificationsOfOrderExecutionExporter.Name
                };
            }
        }

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Операция позволяет экспортировать в ГИС ЖКХ информацию о квитировании. Сведения о квитировании включают в себя: идентификатор платежного документа (счета на оплату), идентификатор извещения, сведения об услугах и сумме квитирования.";

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportAcknowledgmentTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(AcknowledgmentPrepareDataTask);
    }
}
