namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Договор рег. фонда
    /// </summary>
    public class ContractRf : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? DateBegin { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Номер прекращения контракта
        /// </summary>
        public virtual string TerminationContractNum { get; set; }

        /// <summary>
        /// Дата прекращения контракта
        /// </summary>
        public virtual DateTime? TerminationContractDate { get; set; }

        /// <summary>
        /// Файл прекращения контракта
        /// </summary>
        public virtual FileInfo TerminationContractFile { get; set; }
    }
}