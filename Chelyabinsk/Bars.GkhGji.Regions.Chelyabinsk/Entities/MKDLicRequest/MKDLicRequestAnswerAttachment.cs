namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Вложения ответа
    /// </summary>
    public class MKDLicRequestAnswerAttachment : BaseEntity
    {
        /// <summary>
        /// Ответ на обращение
        /// </summary>
        public virtual MKDLicRequestAnswer MKDLicRequestAnswer { get; set; }

        /// <summary>
        /// Описание файла
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}