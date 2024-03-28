namespace Bars.Gkh.Gis.DomainService.Register.TenantSubsidyRegister
{
    using B4;

    public interface ITenantSubsidyRegisterService
    {
        IDataResult ListByApartmentId(BaseParams baseParams);
    }
}
