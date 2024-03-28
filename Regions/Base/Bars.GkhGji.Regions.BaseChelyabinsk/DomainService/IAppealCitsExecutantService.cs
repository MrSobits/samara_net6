namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    public interface IAppealCitsExecutantService
    {
        IDataResult AddExecutants(BaseParams baseParams);

        IDataResult RedirectExecutant(BaseParams baseParams);

        IDataResult ListAppealOrderExecutant(BaseParams baseParams);

        object GetSOPRId(BaseParams baseParams);
    }
}