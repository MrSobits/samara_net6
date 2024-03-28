namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Enums;

    /// <summary>
    /// Заявка на перечисление средств
    /// </summary>
    public class RequestTransferRf : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Договор рег. фонда
        /// </summary>
        public virtual ContractRf ContractRf { get; set; }

        /// <summary>
        /// Программа кап. ремонта
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Банк контрагента
        /// </summary>
        public virtual ContragentBank ContragentBank { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Тип программы заявки перечисления рег.фонда
        /// </summary>
        public virtual TypeProgramRequest TypeProgramRequest { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual string Perfomer { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual int DocumentNum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual int? TransferFundsCount { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? TransferFundsSum { get; set; }
    }
}