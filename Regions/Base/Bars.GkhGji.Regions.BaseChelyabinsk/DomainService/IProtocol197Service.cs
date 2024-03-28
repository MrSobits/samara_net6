namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views;

    public interface IProtocol197Service
    {
		IDataResult ListView(BaseParams baseParams);
		IQueryable<ViewProtocol197> GetViewList();
		IDataResult GetInfo(long? documentId);
		IDataResult AddRequirements(BaseParams baseParams);
		IDataResult AddDirections(BaseParams baseParams);
    }
}