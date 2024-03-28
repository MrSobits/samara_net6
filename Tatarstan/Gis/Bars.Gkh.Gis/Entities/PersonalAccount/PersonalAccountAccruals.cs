namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Начисление по дому
    /// </summary>
    public class PersonalAccountAccruals : PersistentObject
    {
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual long ApartmentId { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual string Supplier { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual decimal BalanceIn { get; set; }

        /// <summary>
        /// Начислено по тарифу
        /// </summary>
        public virtual decimal TariffAmount { get; set; }

        /// <summary>
        /// Недопоставка
        /// </summary>
        public virtual decimal BackorderAmount { get; set; }

        /// <summary>
        /// Сумма предыдущего перерасчета
        /// </summary>
        public virtual decimal RecalcAmount { get; set; }        

        /// <summary>
        /// Сумма оплаты через расчетный центр
        /// </summary>
        public virtual decimal ErcAmount { get; set; }

        /// <summary>
        /// Сумма оплаты через поставщика
        /// </summary>
        public virtual decimal SupplierAmount { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal BalanceOut { get; set; }

        /// <summary>
        /// Льготы, субсидии, скидки (руб)
        /// </summary>
        public virtual decimal Subsidy { get; set; }

        /// <summary>
        /// Объем услуги
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Идентификатор услуги
        /// </summary>
        public virtual long ServiceId { get; set; }

        /// <summary>
        /// Идентификатор поставщика
        /// </summary>
        public virtual long SupplierId { get; set; }
    }
}
