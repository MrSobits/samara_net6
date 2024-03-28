namespace Bars.Gkh.DomainService
{
    using B4;

    /// <summary>
    /// Интерфейс сервиса для ManOrgContractOwnersController
    /// </summary>
    public interface IManOrgContractOwnersService
    {
        /// <summary>
        /// Возвращает объект недвижимости договора
        /// </summary>
        /// <returns></returns>
        IDataResult GetInfo(BaseParams baseParams);
    }
}