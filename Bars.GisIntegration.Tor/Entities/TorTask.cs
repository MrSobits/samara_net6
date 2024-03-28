namespace Bars.GisIntegration.Tor.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.GisIntegration.Tor.Enums;
    using Bars.GkhGji.Entities;

    public class TorTask : BaseEntity
    {
        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual TypeRequest TypeRequest { get; set; }

        /// <summary>
        /// Объект отправки
        /// </summary>
        public virtual TypeObject SendObject { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public virtual TorTaskState TorTaskState { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Пакет запроса
        /// </summary>
        public virtual FileInfo RequestFile { get; set; }

        /// <summary>
        /// Пакет ответа
        /// </summary>
        public virtual FileInfo ResponseFile { get; set; }

        /// <summary>
        /// Файл лога
        /// </summary>
        public virtual FileInfo LogFile { get; set; }
    }
}