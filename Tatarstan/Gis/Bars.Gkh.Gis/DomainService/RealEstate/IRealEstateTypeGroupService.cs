using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.RealEstate
{
    public interface IRealEstateTypeGroupService
    {
        /// <summary>
        /// Список групп без пейджинга
        /// </summary>
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}
