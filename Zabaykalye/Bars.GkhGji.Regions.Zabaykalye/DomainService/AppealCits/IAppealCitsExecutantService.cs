namespace Bars.GkhGji.Regions.Zabaykalye.DomainService
{
    using Bars.B4;

    public interface IAppealCitsExecutantService
    {
        IDataResult AddExecutants(BaseParams baseParams);

        IDataResult RedirectExecutant(BaseParams baseParams);
    }
}