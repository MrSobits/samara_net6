namespace Bars.GisIntegration.UI.ViewModel.Result
{
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Представление результата обработки объекта
    /// </summary>
    public class ObjectProcessingResultView
    {
        /// <summary>
        /// Идентификатор сущности Ris
        /// </summary>
        public long RisId { get; set; }

        /// <summary>
        /// Идентификатор сущности сторонней системы
        /// </summary>
        public long ExternalId { get; set; }

        /// <summary>
        /// Идентификатор сущности Gis
        /// </summary>
        public string GisId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public ObjectProcessingState State { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Наименование пакета
        /// </summary>
        public virtual string PackageName { get; set; }
    }
}
