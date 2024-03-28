namespace Bars.Gkh.Overhaul.Hmao.Entities.Version
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Решение собственников об изменение года работы
    /// </summary>
    public class ChangeYearOwnerDecision : BaseEntity
    {
        /// <summary>
        /// Документ основание
        /// </summary>
        public virtual string DocumentBase { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// Ссылка на запись первого этапа, у которой, собственно, изменяли год
        /// </summary>
        public virtual VersionRecordStage1 VersionRecordStage1 { get; set; }

        /// <summary>
        /// Старый год
        /// </summary>
        public virtual int OldYear { get; set; }

        /// <summary>
        /// Новый год
        /// </summary>
        public virtual int NewYear { get; set; }
    }
}