namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса адреса ФССП
    /// </summary>
    public interface IFsspAddressService
    {
        /// <summary>
        /// Сопоставить адрес ФССП с адресом ПГМУ
        /// </summary>
        /// <param name="fsspAddressId">Id адреса ФССП</param>
        /// <param name="pgmuAddressId">Id адреса ПГМУ</param>
        IDataResult AddressMatch(long fsspAddressId, long pgmuAddressId);
        
        /// <summary>
        /// Разорвать связь адреса ФССП с адресом ПГМУ
        /// </summary>
        /// <param name="fsspAddressId">Id адреса ФССП</param>
        /// <returns></returns>
        IDataResult AddressMismatch(long fsspAddressId);
    }
}