namespace Bars.GkhGji.Entities
{
    // Коды статусов сущности
    public partial class AppealCits
    {
        /// <summary>
        /// Идентификатор статуса
        /// </summary>
        public const string TypeId = "gji_appeal_citizens";

        /// <summary>
        /// В работе
        /// </summary>
        public const string Work = "1";

        /// <summary>
        /// Закрыто
        /// </summary>
        public const string Closed = "2";

        /// <summary>
        /// В ожидании
        /// </summary>
        public const string Pending = "5";

        /// <summary>
        /// Не принято в работу
        /// </summary>
        public const string NotAccepted = "6";

        /// <summary>
        /// Завершена успешно
        /// </summary>
        public const string Success = "7";

        /// <summary>
        /// Завершена неуспешно
        /// </summary>
        public const string Failure = "8";

        /// <summary>
        /// Требует отмены
        /// </summary>
        public const string CancellationRequire = "9";

        /// <summary>
        /// Отменено
        /// </summary>
        public const string Cancelled = "10";
    }
}