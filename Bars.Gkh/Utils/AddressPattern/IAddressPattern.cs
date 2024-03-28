namespace Bars.Gkh.Utils.AddressPattern
{
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис форматирования адресов.
    /// </summary>
    public interface IAddressPattern
    {
        /// <summary>
        /// Формирует краткий адрес дома, основываясь на настройке GeneralConfig.ShortAddressFormat.
        /// </summary>
        /// <param name="mo">Муниципальное образование.</param>
        /// <param name="address">Полный адрес.</param>
        /// <returns>Краткий адрес.</returns>
        string FormatShortAddress(Municipality mo, FiasAddress address);
    }
}