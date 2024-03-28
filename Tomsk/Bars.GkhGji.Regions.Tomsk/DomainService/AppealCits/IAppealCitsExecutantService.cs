namespace Bars.GkhGji.Regions.Tomsk.DomainService.AppealCits
{
	using Bars.B4;

	public interface IAppealCitsExecutantService
    {
        IDataResult AddExecutants(BaseParams baseParams);

        IDataResult RedirectExecutant(BaseParams baseParams);
    }
}