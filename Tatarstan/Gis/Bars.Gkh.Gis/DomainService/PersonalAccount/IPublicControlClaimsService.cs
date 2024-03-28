namespace Bars.Gkh.Gis.DomainService.House.Claims
{
    using B4;
    using Entities.PersonalAccount.PublicControlClaims;

    public interface IPublicControlClaimsService
    {
        IDataResult OrderList(BaseParams baseParams);

        AuthenticationInfoPublicControl Authenticate(); 
    }
}