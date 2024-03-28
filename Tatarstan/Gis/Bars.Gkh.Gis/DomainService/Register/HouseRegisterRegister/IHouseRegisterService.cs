namespace Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.Gkh.Gis.Entities.PersonalAccount;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;
    using Bars.Gkh.Gis.Entities.Register.HouseServiceRegister;

    public interface IHouseRegisterService
    {
        IDataResult SaveHouseTypes(BaseParams baseParams);
        IDataResult SaveHouseMunicipality(BaseParams baseParams);

        void RemoveDuplicateHouses(
            List<HouseRegister> houseRegisterUpdate,
            List<HouseRegister> houseRegisterDelete,
            List<HouseServiceRegister> houseServiceRegistersUpdate,
            List<PersonalAccountUic> personalAccountUicUpdate);

        IDataResult CopyHouseParams(List<HouseRegister> houseRegistersList = null);
        void CopyHouseParams(HouseRegister houseRegister);
    }
}
