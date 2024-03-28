namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса
    /// </summary>
    public enum TypeComplainsRequest
    {
        /// <summary>
        /// Запрос входящих жало
        /// </summary>
        [Display("Запрос входящих жалоб")]
        ComplaintsRequest = 0,

        /// <summary>
        /// Прием жалобы в работу
        /// </summary>
        [Display("Прием жалобы в работу")]
        Void = 1,

        /// <summary>
        /// Назначение исполнителя
        /// </summary>
        [Display("Назначение исполнителя")]
        Executor = 2,

        /// <summary>
        /// Отказ от исполнения жалобы
        /// </summary>
        [Display("Отказ от исполнения жалобы")]
        Decline = 3,

        /// <summary>
        /// Вынесние решения
        /// </summary>
        [Display("Вынесние решения")]
        Decision = 4,

        /// <summary>
        /// Запрос доп информации
        /// </summary>
        [Display("Запрос доп информации")]
        DataRequest = 5,

        /// <summary>
        /// Запрос доп информации
        /// </summary>
        [Display("Рассмотрение ходатайства")]
        Step = 6,
    }
}