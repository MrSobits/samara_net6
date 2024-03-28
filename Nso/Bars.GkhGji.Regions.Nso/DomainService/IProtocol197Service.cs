namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using Bars.B4;
	using System.Linq;
	using Bars.GkhGji.Regions.Nso.Entities;
	
	public interface IProtocol197Service
    {
		IDataResult ListView(BaseParams baseParams);
		IQueryable<ViewProtocol197> GetViewList();
		IDataResult GetInfo(long? documentId);
		IDataResult AddRequirements(BaseParams baseParams);
		IDataResult AddDirections(BaseParams baseParams);
    }
}