namespace Bars.Gkh.Gis.DomainService.House.Claims
{
    using B4;
    using Entities.House.Claims;

    public interface IHouseClaimsService
    {
        IDataResult OrderList(BaseParams baseParams);

        AuthenticationInfoOk Authenticate();
    }
}
