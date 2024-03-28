namespace Bars.GkhGji.Regions.Tula.DomainService
{
    using Bars.B4;

    public interface IAppealCitsExecutantService
    {
        IDataResult AddExecutants(BaseParams baseParams);

        IDataResult RedirectExecutant(BaseParams baseParams);
    }
}