namespace Bars.GkhCr.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Протокол конкурса
    /// </summary>
    public class CompetitionProtocol : BaseImportableEntity
    {
        /// <summary>
        /// Конкурс
        /// </summary>
        public virtual Competition Competition { get; set; }

        /// <summary>
        /// Тип протокола
        /// </summary>
        public virtual TypeCompetitionProtocol TypeProtocol { get; set; }

        /// <summary>
        /// Дата подписания протокола
        /// </summary>
        public virtual DateTime SignDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата проведения процедуры
        /// </summary>
        public virtual DateTime? ExecDate { get;set; }

        /// <summary>
        /// Время проведения процедуры
        /// </summary>
        public virtual DateTime? ExecTime { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Конкурс признан несостоявшимся
        /// </summary>
        public virtual bool IsCancelled { get; set; }
    }
}
