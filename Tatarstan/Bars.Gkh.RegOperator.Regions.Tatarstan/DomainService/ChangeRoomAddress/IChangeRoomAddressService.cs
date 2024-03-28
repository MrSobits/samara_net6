namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService.ChangeRoomAddress
{
    using Bars.B4;

    public interface IChangeRoomAddressService
    {
        /// <summary>
        /// Сохраняет изменения квартиры
        /// </summary>
        IDataResult SaveRoomAddress(BaseParams baseParams);
    }
}