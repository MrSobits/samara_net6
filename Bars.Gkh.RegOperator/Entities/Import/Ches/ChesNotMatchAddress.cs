namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Несопоставленный в периоде адрес
    /// </summary>
    public class ChesNotMatchAddress : PersistentObject
    {
        /// <summary>
        /// Адрес ЧЭС
        /// </summary>
        public virtual string ExternalAddress { get; set; }

        /// <summary>
        /// Гуид дома в ФИАС
        /// </summary>
        public virtual string HouseGuid { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }
    }
}