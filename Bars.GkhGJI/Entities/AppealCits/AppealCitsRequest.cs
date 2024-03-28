namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Обращениям граждан - Запрос
    /// </summary>
    public class AppealCitsRequest : BaseGkhEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? PerfomanceDate { get; set; }

        /// <summary>
        /// Дата фактического исполнения
        /// </summary>
        public virtual DateTime? PerfomanceFactDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }

        /// <summary>
        /// Инспектор запроса ГЖИ
        /// </summary>
        public virtual Inspector SenderInspector { get; set; }

        /// <summary>
        /// Подписант
        /// </summary>
        public virtual Inspector Signer { get; set; }
    }
}