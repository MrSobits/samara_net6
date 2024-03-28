namespace Bars.GkhDi.DomainService
{
    using Services.DataContracts.GetManOrgRealtyObjectInfo;

    /// <summary>
    /// Сервис получение информации о лифтах Жилого дома 
    /// </summary>
    public interface IRealtyObjectLiftService
    {
        /// <summary>
        /// Получение информации о лифтах Жилого дома 
        /// </summary>
        /// <param name="realtyObjectId">Id жилого дома</param>
        /// <returns>Информация и лифтах жилого дома</returns>
        HouseLift[] GetRealtyObjectLift(long realtyObjectId);
    }
}