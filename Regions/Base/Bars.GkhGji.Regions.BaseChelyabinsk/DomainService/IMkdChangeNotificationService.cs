namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    public interface IMkdChangeNotificationService
	{
		IDataResult GetManagingOrgDetails(BaseParams baseParams);
		IDataResult GetManagingOrgByAddressName(BaseParams baseParams);
	}
}
