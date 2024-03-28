namespace Bars.GkhRf.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Объект записи перечисления средств рег. фонда
    /// </summary>
    public class TransferRfRecObj : BaseDocument
    {
        /// <summary>
        /// Запись договора рег. фонда
        /// </summary>
        public virtual TransferRfRecord TransferRfRecord { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}
