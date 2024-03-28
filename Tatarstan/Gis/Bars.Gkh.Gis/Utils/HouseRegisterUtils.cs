namespace Bars.Gkh.Gis.Utils
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using Entities.Register.HouseRegister;
    using Gkh.Entities;
    using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;

    public static class HouseRegisterUtils
    {
        public static void FillHouseByFiasAddress(HouseRegister house, string guid, IRepository<Fias> fiasRepository)
        {
            if (CollectionExtensions.IsNullOrEmpty(guid))
            {
                return;
            }

            if (fiasRepository.GetAll().FirstOrDefault(x => x.AOGuid == guid
                                                            && x.ActStatus == FiasActualStatusEnum.Actual
                                                            && x.KladrCurrStatus == 0) == null)
            {
                return;
            }

            //clear fields
            house.Region = house.Area = house.City = house.Street = "";

            FiasLevelEnum level;

            do
            {
                var parentGuids = fiasRepository
                    .GetAll()
                    .Where(
                        x =>
                            x.AOGuid == guid

                            //&& x.ActStatus == FiasActualStatusEnum.Actual
                            &&
                            x.KladrCurrStatus == 0);
                var parent = parentGuids.FirstOrDefault(x => x.ActStatus == FiasActualStatusEnum.Actual) ??
                             parentGuids.FirstOrDefault(x => x.AOGuid == "93b3df57-4c89-44df-ac42-96f05e9cd3b9");
                if (parent == null)
                {
                    break;
                }

                var name = string.Format("{0}. {1}", parent.ShortName, parent.OffName);
                level = parent.AOLevel;
                switch (level)
                {
                    case FiasLevelEnum.Street:
                        house.Street = name;
                        break;
                    case FiasLevelEnum.Place:
                    case FiasLevelEnum.City:
                        house.City = name;
                        break;
                    case FiasLevelEnum.Raion:
                        house.Area = name;
                        break;
                    case FiasLevelEnum.Region:
                        house.Region = name;
                        break;
                }
                guid = parent.ParentGuid;
            } while (level != FiasLevelEnum.Region);
        }

        public static void CopyParam(HouseRegister house, RealityObject realityObject)
        {
            if (house == null || realityObject == null)
            {
                return;
            }

            house.AreaLivingNotLivingMkd = realityObject.AreaLivingNotLivingMkd;
            house.AreaOwned = realityObject.AreaOwned;
            if (realityObject.BuildYear.HasValue)
            {
                house.BuildDate = new DateTime(realityObject.BuildYear.Value, 1, 1);
            }
            house.HeatingSystem = realityObject.HeatingSystem;
            if (realityObject.MaximumFloors.HasValue)
            {
                house.MaximumFloors = realityObject.MaximumFloors.Value;
            }
            if (realityObject.Floors.HasValue)
            {
                house.MinimumFloors = realityObject.Floors.Value;
            }
            house.Municipality = realityObject.Municipality;
            house.NumberApartments = realityObject.NumberApartments;
            if (realityObject.NumberEntrances.HasValue)
            {
                house.NumberEntrances = realityObject.NumberEntrances.Value;
            }
            if (realityObject.NumberLifts.HasValue)
            {
                house.NumberLifts = realityObject.NumberLifts.Value;
            }
            if (realityObject.NumberLiving.HasValue)
            {
                house.NumberLiving = realityObject.NumberLiving.Value;
            }
            if (realityObject.PhysicalWear.HasValue)
            {
                house.PhysicalWear = Convert.ToDecimal(realityObject.PhysicalWear.Value);
            }
            if (realityObject.PrivatizationDateFirstApartment.HasValue)
            {
                house.PrivatizationDate = realityObject.PrivatizationDateFirstApartment.Value;
            }

            house.RoofingMaterial = realityObject.RoofingMaterial;
            house.TypeHouse = realityObject.TypeHouse;
            house.TypeProject = realityObject.TypeProject;
            house.TypeRoof = realityObject.TypeRoof;
            house.WallMaterial = realityObject.WallMaterial;
            if (realityObject.AreaMkd.HasValue)
            {
                house.TotalSquare = Convert.ToDecimal(realityObject.AreaMkd.Value);
            }
        }
    }
}
