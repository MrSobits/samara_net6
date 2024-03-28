namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBaseLicenseApplicantsService
    {
        IDataResult ListByLicenseReq(BaseParams baseParams);
    }
}