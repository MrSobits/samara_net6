namespace Bars.Gkh.Gis.Entities.House
{
    using B4.DataAccess;

    /// <summary>
    /// Начисление по дому
    /// </summary>
    public class HouseAccruals : PersistentObject
    {
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long HouseId { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual long Month { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual long Year { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual string Service { get; set; }

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
    }
}
