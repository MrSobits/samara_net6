namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using Bars.B4;

    public interface IResolutionArticleLawService
    {
        IDataResult GetListResolution(BaseParams baseParams);

        IDataResult GetListDisposal(BaseParams baseParams);
    }
}