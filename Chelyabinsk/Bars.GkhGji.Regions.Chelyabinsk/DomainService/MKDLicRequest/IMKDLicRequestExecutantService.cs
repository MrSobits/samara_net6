namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService.MKDLicRequest
{
    using Bars.B4;

    public interface IMKDLicRequestExecutantService
    {
        IDataResult AddExecutants(BaseParams baseParams);

        IDataResult RedirectExecutant(BaseParams baseParams);

        IDataResult ListAppealOrderExecutant(BaseParams baseParams);
    }
}