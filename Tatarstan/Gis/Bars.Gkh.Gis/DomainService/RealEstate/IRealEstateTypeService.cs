using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.RealEstate
{
    public interface IRealEstateTypeService
    {
        IDataResult GroupedTypeList(BaseParams baseParams);        
        IDataResult Delete(BaseParams baseParams);
    }
}
