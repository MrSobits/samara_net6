namespace Bars.Gkh.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    /// <summary>
    /// Информация о технических ошибках
    /// </summary>
    public class TechnicalMistake : BaseImportableEntity
    {
        /// <summary>
        /// Квалификационный аттестат
        /// </summary>
        public virtual PersonQualificationCertificate QualificationCertificate { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string StatementNumber { get; set; }

        /// <summary>
        /// Описание технической ошибки
        /// </summary>
        public virtual string FixInfo { get; set; }

        /// <summary>
        /// Дата исправления
        /// </summary>
        public virtual DateTime? FixDate { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public virtual DateTime? IssuedDate { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Номер решения
        /// </summary>
        public virtual string DecisionNumber { get; set; }

        /// <summary>
        /// Дата решения
        /// </summary>
        public virtual DateTime? DecisionDate { get; set; }
    }
}