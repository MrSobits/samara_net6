namespace Bars.Gkh.Gis.DomainService.Register.ServiceSubsidyRegister
{
    using B4;

    public interface IServiceSubsidyRegisterService
    {
        IDataResult ListByApartmentId(BaseParams baseParams);
    }
}
