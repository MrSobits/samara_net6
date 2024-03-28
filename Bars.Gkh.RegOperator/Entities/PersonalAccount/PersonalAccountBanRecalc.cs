namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Запрет перерасчета за период
    /// </summary>
    public class PersonalAccountBanRecalc : BaseImportableEntity
    {
         /// <summary>
         /// Лицевой счет
         /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Тип запрета перерасчета
        /// </summary>
        public virtual BanRecalcType Type { get; set; }
    }
}