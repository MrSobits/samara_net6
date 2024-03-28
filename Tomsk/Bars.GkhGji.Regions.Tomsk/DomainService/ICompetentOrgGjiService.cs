namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using B4;

    public interface ICompetentOrgGjiService
    {
        IDataResult AddRevenueSource(BaseParams baseParams);

        IDataResult AddContragents(BaseParams baseParams);
    }
}
