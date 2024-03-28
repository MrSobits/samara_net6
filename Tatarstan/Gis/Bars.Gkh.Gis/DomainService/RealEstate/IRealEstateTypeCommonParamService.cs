namespace Bars.Gkh.Gis.DomainService.RealEstate
{
    using System.Linq;
    using Entities.RealEstate.GisRealEstateType;
    using Entities.Register.HouseRegister;

    public interface IRealEstateTypeCommonParamService
    {
        IQueryable<HouseRegister> GetHouseRegistersByRealEstateType(GisRealEstateType realEstateType);
    }
}
