namespace Bars.GkhGji.DomainService
{
    using B4;

    public interface IStatSubsubjectGjiService
    {
        IDataResult AddFeature(BaseParams baseParams);

        IDataResult AddSubject(BaseParams baseParams);
    }
}