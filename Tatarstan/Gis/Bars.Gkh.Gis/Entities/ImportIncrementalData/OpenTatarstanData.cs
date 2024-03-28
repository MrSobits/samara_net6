namespace Bars.Gkh.Gis.Entities.ImportIncrementalData
{
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using Enum;

    /// <summary>
    /// Сущность инкременетальной загрузки от биллинга
    /// </summary>
    public class OpenTatarstanData : BaseEntity
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User B4User { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public virtual FileInfo Log { get; set; }

        /// <summary>
        /// Результат импорта
        /// </summary>
        public virtual ImportResult ImportResult { get; set; }

        /// <summary>
        /// Название импорта
        /// </summary>
        public virtual string ImportName { get; set; }

        /// <summary>
        /// Тескт ответа
        /// </summary>
        public virtual string ResponseInfo { get; set; }

        /// <summary>
        /// Код ответа
        /// </summary>
        public virtual string ResponseCode { get; set; }
    }
}