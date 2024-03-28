namespace Bars.GkhGji.Regions.Tyumen.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Оператор связи
    /// </summary>
    public class NetworkOperator : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}