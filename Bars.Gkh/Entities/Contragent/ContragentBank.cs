namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Банк контрагента
    /// </summary>
    public class ContragentBank : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// БИК
        /// </summary>
        public virtual string Bik { get; set; }

        /// <summary>
        /// ОКОНХ
        /// </summary>
        public virtual string Okonh { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        public virtual string Okpo { get; set; }

        /// <summary>
        /// Корреспондентский счет
        /// </summary>
        public virtual string CorrAccount { get; set; }

        /// <summary>
        /// Расчетный счет
        /// </summary>
        public virtual string SettlementAccount { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
