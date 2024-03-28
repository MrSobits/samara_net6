namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using B4;

    public interface IActVisualService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);
    }
}