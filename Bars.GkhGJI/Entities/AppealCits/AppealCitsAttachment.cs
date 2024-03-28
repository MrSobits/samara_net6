using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Скан(ы) обращения
    /// </summary>
    public class AppealCitsAttachment : BaseEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

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

        /// <summary>
        /// Hash
        /// </summary>
        public virtual string Hash { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
    }
}
