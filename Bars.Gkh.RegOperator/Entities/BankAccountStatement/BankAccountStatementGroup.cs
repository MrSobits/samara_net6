namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Соответсвует файлу, в котором находится информация по банковским выпискам <see cref="BankAccountStatement"/>
    /// </summary>
    public class BankAccountStatementGroup : BaseImportableEntity
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime ImportDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Sum { get; set; }
    }
}
