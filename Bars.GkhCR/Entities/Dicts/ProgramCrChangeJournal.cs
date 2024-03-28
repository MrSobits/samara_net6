namespace Bars.GkhCr.Entities
{
    using System;
    using Bars.GkhCr.Enums;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Журнал изменений программы КР
    /// </summary>
    public class ProgramCrChangeJournal : BaseImportableEntity
    {
        /// <summary>
        /// Программа
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Способ формирования
        /// </summary>
        public virtual TypeChangeProgramCr TypeChange { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime ChangeDate { get; set; }

        /// <summary>
        /// Количство МО
        /// </summary>
        public virtual int? MuCount { get; set; }

        /// <summary>
        /// Способ формирования
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
