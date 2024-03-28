using Bars.B4.DataAccess.ByCode;

namespace Bars.B4.Modules.FIAS.Map
{
    public class FiasAddressMap : BaseEntityMap<FiasAddress>
    {
        public FiasAddressMap()
            : base("B4_FIAS_ADDRESS")
        {
            Map(x => x.AddressGuid, "ADDRESS_GUID", false, 1000);
            Map(x => x.AddressName, "ADDRESS_NAME", false, 1000);
            Map(x => x.PostCode, "POST_CODE", false, 6);
            Map(x => x.PlaceAddressName, "PLACE_ADDRESS_NAME", false, 1000);
            Map(x => x.PlaceName, "PLACE_NAME", false, 50);
            Map(x => x.PlaceGuidId, "PLACE_GUID", false, 36);
            Map(x => x.PlaceCode, "PLACE_CODE", false, 30);
            Map(x => x.StreetName, "STREET_NAME", false, 50);
            Map(x => x.StreetGuidId, "STREET_GUID", false, 36);
            Map(x => x.StreetCode, "STREET_CODE", false, 30);
            Map(x => x.House, "HOUSE", false, 10);
            Map(x => x.Letter, "LETTER", false, 10);
            Map(x => x.Housing, "HOUSING", false, 10);
            Map(x => x.Building, "BUILDING", false, 10);
            Map(x => x.Flat, "FLAT", false, 10);
            Map(x => x.Coordinate, "COORDINATE", false, 50);
			Map(x => x.HouseGuid, "HOUSE_GUID", false);
            Map(x => x.EstimateStatus, "EST_STATUS", true, Enums.FiasEstimateStatusEnum.House);
        }
    }
}