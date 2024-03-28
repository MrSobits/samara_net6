namespace Bars.GkhGji.Entities.Email
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Электронное письмо ГЖИ
    /// </summary>
    public class EmailGji : BaseEntity
    {
        /// <summary>
        /// От кого
        /// </summary>
        public virtual string From { get; set; }

        /// <summary>
        /// Подпись отправителя
        /// </summary>
        public virtual string SenderInfo { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public virtual string Theme { get; set; }

        /// <summary>
        /// Дата письма
        /// </summary>
        public virtual DateTime? EmailDate { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string GjiNumber { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string SystemNumber { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string LivAddress { get; set; }

        /// <summary>
        /// Тип письма
        /// </summary>
        public virtual EmailGjiType EmailType { get; set; }

        /// <summary>
        /// источник письма
        /// </summary>
        public virtual EmailGjiSource EmailGjiSource { get; set; }

        /// <summary>
        /// Зарегестрированно
        /// </summary>
        public virtual bool Registred { get; set; }

        /// <summary>
        /// Примечание(возможно, не все вложения закреплены)
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Pdf-файл письма
        /// </summary>
        public virtual FileInfo EmailPdf { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public virtual EmailDenailReason EmailDenailReason { get; set; }

        /// <summary>
        /// Комментарий к отклонению письма
        /// </summary>
        public virtual string DeclineReason { get; set; }
    }
}