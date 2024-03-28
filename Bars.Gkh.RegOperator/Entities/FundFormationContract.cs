namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Enum;

    /// <summary>
    /// Реестр договоров на формирование фонда капитального ремонта
    /// </summary>
    public class FundFormationContract : BaseImportableEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual LongTermPrObject LongTermPrObject { get; set; }

        /// <summary>
        /// Региональный оператор 
        /// </summary>
        public virtual RegOperator RegOperator { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public virtual FundFormationContractType TypeContract { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? ContractDate { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}