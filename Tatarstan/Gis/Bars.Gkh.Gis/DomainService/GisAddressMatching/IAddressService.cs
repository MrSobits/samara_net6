namespace Bars.Gkh.Gis.DomainService.GisAddressMatching
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Gis.DomainService.GisAddressMatching.Impl;

    public interface IAddressService
    {
        IDataResult AreaListPaging(BaseParams baseParams);
        IDataResult PlaceListPaging(BaseParams baseParams);
        IDataResult StreetShortNameList(BaseParams baseParams);
        IDataResult StreetListNoGuidPaging(BaseParams baseParams);
        IQueryable<AddressProxy> GetAddresses(BaseParams baseParams);
        IQueryable<ImportedAddressProxy> GetImportAddresses();
    }
}
