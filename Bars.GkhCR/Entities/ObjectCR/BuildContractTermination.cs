namespace Bars.GkhCr.Entities
{
    using System;

    using B4.Modules.FileStorage;
    using B4.Modules.States;

    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    using Gkh.Entities;
    using Enums;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Расторжение договора подряда КР
    /// </summary>
    public class BuildContractTermination : BaseEntity
    {
        /// <summary>
        /// Договор подряда КР
        /// </summary>
        public virtual BuildContract BuildContract { get; set; }

        /// <summary>
        /// Дата расторжения
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Основание расторжения
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Причина расторжения из справочника
        /// </summary>
        public virtual TerminationReason DictReason { get; set; }
    }
}
