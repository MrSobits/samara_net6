namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using System;

    /// <summary>
    /// Рассылки
    /// </summary>
    public class EmailLists : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string MailTo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime SendDate { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string AppealNumber { get; set; }

        /// <summary>
        /// Номер ответа
        /// </summary>
        public virtual string AnswerNumber { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime AppealDate { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}