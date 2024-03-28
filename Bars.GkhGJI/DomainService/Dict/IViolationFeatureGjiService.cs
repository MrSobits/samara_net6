namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IViolationFeatureGjiService
    {
        IDataResult AddFeatureViolations(BaseParams baseParams);

        IDataResult SaveViolationGroups(BaseParams baseParams);
    }
}