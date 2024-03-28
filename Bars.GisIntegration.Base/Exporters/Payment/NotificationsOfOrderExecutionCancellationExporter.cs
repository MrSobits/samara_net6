namespace Bars.GisIntegration.Base.Exporters.Payment
{
    using System;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Payment;
    using Bars.GisIntegration.Base.Tasks.SendData.Payment;

    /// <summary>
    /// Экспортёр для задачи экспорта "Экспорт документов «Аннулирование извещения о принятии к исполнению распоряжения»"
    /// </summary>
    public class NotificationsOfOrderExecutionCancellationExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт документов «Аннулирование извещения о принятии к исполнению распоряжения»";

        /// <summary>
        /// Порядок экспортера
        /// </summary>
        public override int Order => 260;

        /// <summary>
        /// Описание экспортера
        /// </summary>
        public override string Description => "Метод позволяет позволяет экспортировать в ГИС ЖКХ сведения об аннулировании квитанций об оплате выставленных счетов (платежных документов). Аннулируемый документ идентифицируется по бизнес-ключу OrderID.";

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportNotificationsOfOrderExecutionCancellationTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(NotificationsOfOrderExecutionCancellationPrepareDataTask);
    }
}