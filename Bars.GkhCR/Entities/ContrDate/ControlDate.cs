namespace Bars.GkhCr.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Контрольные сроки вида работ
    /// </summary>
    public class ControlDate : BaseImportableEntity
    {
        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? Date { get; set; }
    }
}
