namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IInspectorService
    {
        IDataResult SubcribeToInspectors(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult AddZonalInspection(BaseParams baseParams);

        IDataResult ListZonalInspection(BaseParams baseParams);
        
    }
}