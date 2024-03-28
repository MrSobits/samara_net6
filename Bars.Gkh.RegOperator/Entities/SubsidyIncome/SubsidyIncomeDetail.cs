namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Запись реестр субсидий 
    /// </summary>
    public class SubsidyIncomeDetail : BaseImportableEntity
    {
        /// <summary>
        /// SubsidyIncome
        /// </summary>
        public virtual SubsidyIncome SubsidyIncome { get; set; }

        /// <summary>
        /// ID дома в файле
        /// </summary>
        public virtual long RealObjId { get; set; }

        /// <summary>
        /// Адрес в файле
        /// </summary>
        public virtual string RealObjAddress { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Тип субсидии
        /// </summary>
        public virtual string TypeSubsidyDistr { get; set; }

        /// <summary>
        /// Сумма 
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime DateReceipt { get; set; }

        /// <summary>
        /// Определен/Не определен 
        /// </summary>
        public virtual bool IsConfirmed { get; set; }
    }
}