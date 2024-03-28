namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Bars.B4;

    public interface IResolutionArticleLawService
    {
        IDataResult AddArticles(BaseParams baseParams);

        IDataResult GetListResolution(BaseParams baseParams);

        IDataResult GetListDisposal(BaseParams baseParams);
        IDataResult GetListDecision(BaseParams baseParams);
    }
}