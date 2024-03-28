namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Настройки контекстов для передачи файлов
    /// </summary>
    public class ContextSettings : BaseEntity
    {
        /// <summary>
        /// Хранилище данных ГИС
        /// </summary>
        public virtual FileStorageName FileStorageName { get; set; }

        /// <summary>
        /// Контекст
        /// </summary>
        public virtual string Context { get; set; }
    }
}
