namespace Bars.Gkh.RegOperator.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Интерфейс счета дома
    /// </summary>
    public interface IRealityObjectAccount
    {
        long Id { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        string AccountNumber { get; set; }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        RealityObject RealityObject { get; set; }
    }
}