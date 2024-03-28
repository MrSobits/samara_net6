namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Обращения граждан ФКР - Предписание
    /// </summary>
    public class AppealCitsPrescriptionFond : BaseGkhEntity
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
        /// BuildContranct
        /// </summary>
        public virtual MassBuildContract MassBuildContract { get; set; }

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
        /// Файл ответа
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }

        /// <summary>
        /// Подписанный файл ответа
        /// </summary>
        public virtual FileInfo SignedAnswerFile { get; set; }

        /// <summary>
        /// Подпись ответа
        /// </summary>
        public virtual FileInfo AnswerSignature { get; set; }

        /// <summary>
        /// Сертификат ответа
        /// </summary>
        public virtual FileInfo AnswerCertificate { get; set; }

        public virtual Inspector Inspector { get; set; }

        public virtual Inspector Executor { get; set; }

        public virtual KindKNDGJI KindKNDGJI { get; set; }
    }
}