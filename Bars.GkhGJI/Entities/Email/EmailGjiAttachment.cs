namespace Bars.GkhGji.Entities.Email
{
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Вложение письма ГЖИ
    /// </summary>
    public class EmailGjiAttachment : BaseEntity
    {
        /// <summary>
        /// Родительское письмо
        /// </summary>
        public virtual EmailGji Message { get; set; }

        /// <summary>
        /// Файл вложения
        /// </summary>
        public virtual FileInfo AttachmentFile { get; set; }
    }
}