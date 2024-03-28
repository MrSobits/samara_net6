namespace Bars.Gkh.Services.Impl.GlonassIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.IoC;
    using B4.Utils;
    using Castle.Core.Internal;
    using DataContracts;
    using DataContracts.GlonassIntegration;
    using Domain.TechPassport;
    using DomainService.TechPassport;
    using Entities;
    using B4.DataAccess;
    using Enums;
    using PassportProvider;

    public partial class GlonassIntegService
    {
        private IList<SerializePassportValue> _techPassportValues;

        /// <summary>
        /// Получить информацию по дому
        /// </summary>
        /// <param name="id">Идентификатор дома</param>
        /// <returns>Ответ от сервиса с результатом выполнения запроса</returns>
        public GetHouseResponse GetHouse(string id)
        {
            var response = new GetHouseResponse
            {
                Result = Result.NoErrors
            };

            long houseId;
            try
            {
                houseId = id.ToLong();
            }
            catch
            {
                response.Result = IncorrectRequest();

                return response;
            }
            
            var realityObjectDomain = Container.ResolveDomain<RealityObject>();
            var roomDomain = Container.ResolveDomain<Room>();
            var servorgRoContractDomain= Container.ResolveDomain<ServiceOrgRealityObjectContract>();
            var moContractRoDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            using (Container.Using(realityObjectDomain, roomDomain, servorgRoContractDomain,
                moContractRoDomain))
            {
                var realityObject = realityObjectDomain
                    .Get(houseId);
                if (realityObject == null)
                {
                    response.Result = Result.DataNotFound;

                    return response;
                }

                response.HouseInfo = realityObjectDomain
                    .Get(houseId)
                    .Return(GetHouseInfo);

                response.FlatsInfo = GetFlatsInfo(roomDomain.GetAll()
                    .Where(x => x.RealityObject.Id == houseId));

                response.ProvidersInfo = GetProvidersInfo(servorgRoContractDomain.GetAll()
                    .Where(x => x.RealityObject.Id == houseId));

                var manOrgContractRo = GetManOrgContractRo(moContractRoDomain, houseId);

                response.HouseInfo.ManOrg = GetManOrgContractRoName(manOrgContractRo);

                response.UoInfo = GetUoInfo(manOrgContractRo);
            }
            
            return response;
        }

        private HouseInfo GetHouseInfo(RealityObject realityObject)
        {
            var belayPolicyMkdDomain = Container.ResolveDomain<BelayPolicyMkd>();
            var passportProvider = Container.Resolve<IPassportProvider>();
            var techPassportElementDescriptorService = Container.Resolve<ITechPassportElementDescriptorService>();
            using (Container.Using(passportProvider, belayPolicyMkdDomain,
                techPassportElementDescriptorService))
            {
                _techPassportValues = passportProvider.GetPassportValuesByRo(realityObject.Id);

                return new HouseInfo
                {
                    Id = realityObject.Id,
                    ExplYear = realityObject.BuildYear.HasValue
                        ? realityObject.BuildYear.Value.ToString(NumberFormatInfo)
                        : string.Empty,
                    FlatNumber = realityObject.NumberApartments.HasValue
                        ? realityObject.NumberApartments.Value.ToString(NumberFormatInfo)
                        : string.Empty,
                    Floor = realityObject.MaximumFloors.HasValue
                        ? realityObject.MaximumFloors.Value.ToString(NumberFormatInfo)
                        : string.Empty,
                    LiveArea = realityObject.AreaLiving.HasValue
                        ? realityObject.AreaLiving.Value.RoundDecimal(2).ToString(NumberFormatInfo)
                        : string.Empty,
                    LiveUnliveArea = realityObject.AreaLivingNotLivingMkd.HasValue
                        ? realityObject.AreaLivingNotLivingMkd.Value.RoundDecimal(2).ToString(NumberFormatInfo)
                        : string.Empty,
                    TotalArea = realityObject.AreaMkd.HasValue
                        ? realityObject.AreaMkd.Value.RoundDecimal(2).ToString(NumberFormatInfo)
                        : string.Empty,
                    NotLivingArea = (realityObject.AreaLivingNotLivingMkd.HasValue && realityObject.AreaLiving.HasValue)
                        ? (realityObject.AreaLivingNotLivingMkd.Value - realityObject.AreaLiving.Value).RoundDecimal(2)
                            .ToString(NumberFormatInfo)
                        : string.Empty,
                    EntranceNumber = realityObject.NumberEntrances.HasValue
                        ? realityObject.NumberEntrances.Value.ToString(NumberFormatInfo)
                        : string.Empty,
                    RoofMaterial = realityObject.RoofingMaterial.Name,
                    RoofType = realityObject.TypeRoof.GetEnumMeta().Display,
                    WallMaterial = realityObject.WallMaterial.Name,
                    Address = realityObject.Address,
                    FlatMaterial = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeFloorType))
                        .Return(y => y.EditorValue, ""),
                    Insurance = realityObject.IsInsuredObject
                        ? "Да"
                        : "Нет",
                    HeatingType = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeHeatingType))
                        .Return(y => y.EditorValue, ""),
                    HotWaterType = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeHotWaterType))
                        .Return(y => y.EditorValue, ""),
                    GasType = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeGas))
                        .Return(y => y.EditorValue, ""),
                    RoofEntrance = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeRoofEntrance))
                        .Return(y => y.Value, ""),
                    HeatingEntrance = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeHeatingEntrance))
                        .Return(y => y.Value, ""),
                    WaterEntrance = GetWaterEntrance(
                       techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeHotWaterEntrance),
                       techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeColdWaterEntrance)),
                    ElectroEntrance = GetPassportValue(
                        techPassportElementDescriptorService.GetComponent(TypeTechPassportComponent.TypeElectroEntrance))
                        .Return(y => y.Value, "") 
                };
            }
        }

        private static FlatInfo[] GetFlatsInfo(IQueryable<Room> rooms)
        {
            return rooms
                .Select(x => new FlatInfo
                {
                    Num = x.RoomNum,
                    Area = x.Area.ToString(NumberFormatInfo),
                    TypeRoom = x.Type.GetEnumMeta().Display,
                    Type = x.OwnershipType.GetEnumMeta().Display,
                    Owner = "",
                    Telephone = ""
                })
                .ToArray();
        }

        private static ManOrgContractRealityObject GetManOrgContractRo(IDomainService<ManOrgContractRealityObject> moContractRoDomain, 
            long realityObjectId)
        {
            return moContractRoDomain.GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Where(x => x.ManOrgContract != null)
                .Where(x => x.ManOrgContract.StartDate == null
                            || x.ManOrgContract.StartDate <= DateTime.Now)
                .FirstOrDefault(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now);    
        }

        private string GetManOrgContractRoName(ManOrgContractRealityObject moContractRo)
        {
            var manOrgName = "";

            Container.UsingForResolved<IDomainService<RealityObjectDirectManagContract>>(
                (c, moContractDirectManagServ) =>
                {
                    manOrgName = moContractRo
                        .ReturnSafe(x =>
                        {
                            if (x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                            {
                                return
                                    moContractDirectManagServ.GetAll()
                                        .Any(y => y.Id == x.ManOrgContract.Id && y.IsServiceContract)
                                        ? ManOrgBaseContract.DirectManagementWithContractText
                                        : ManOrgBaseContract.DirectManagementText;
                            }

                            return x.ManOrgContract
                                .ReturnSafe(z => z.ManagingOrganization)
                                .ReturnSafe(z => z.Contragent)
                                .ReturnSafe(z => z.Name);
                        });
                });

            return manOrgName;
        }

        private UoInfo GetUoInfo(ManOrgContractRealityObject moContractRo)
        {
            var uoInfo = new UoInfo();

            var contragent = moContractRo.ManOrgContract
                    .ReturnSafe(x => x.ManagingOrganization)
                    .ReturnSafe(x => x.Contragent);

            uoInfo.UOAdress = contragent.ReturnSafe(x => x.FactAddress);
            uoInfo.UOName = GetManOrgContractRoName(moContractRo);
            uoInfo.UOEmergency = contragent.PhoneDispatchService;
            uoInfo.UOPhone = contragent.Phone;
            uoInfo.UOType = moContractRo.ManOrgContract
                .ReturnSafe(x => x.ManagingOrganization)
                .ReturnSafe(x => x.TypeManagement)
                .GetEnumMeta().Display;

            return uoInfo;
        }

        private static ProviderInfo[] GetProvidersInfo(IQueryable<ServiceOrgRealityObjectContract> contracts)
        {
            return contracts.Select(x => new ProviderInfo
            {
                ProviderName = x.ServOrgContract.ServOrg.Contragent.Name,
                ProviderAddress = x.ServOrgContract.ServOrg.Contragent.FactAddress,
                ProviderPhone = x.ServOrgContract.ServOrg.Contragent.Phone,
                ProviderEmergency = x.ServOrgContract.ServOrg.Contragent.PhoneDispatchService
            })
                .ToArray();
        }

        private SerializePassportValue GetPassportValue(TechPassportComponent techPassportComponent)
        {
            return _techPassportValues
                .FirstOrDefault(y => y.ComponentCode == techPassportComponent.FormCode
                                     && y.CellCode == techPassportComponent.CellCode);
        }

        private string GetWaterEntrance(TechPassportComponent hotWaterCmp, TechPassportComponent coldWaterCmp)
        {
            var hotWaterEntranceValue = 0;
            var coldWaterEntranceValue = 0;

            var strHotWaterEntValue = GetPassportValue(hotWaterCmp).Return(y => y.Value);
            var strColdWaterEntValue = GetPassportValue(coldWaterCmp).Return(y => y.Value);

            if (!strHotWaterEntValue.IsNullOrEmpty())
            {
                hotWaterEntranceValue = strHotWaterEntValue.ToInt();
            }

            if (!strColdWaterEntValue.IsNullOrEmpty())
            {
                coldWaterEntranceValue = strHotWaterEntValue.ToInt();
            }

            return (hotWaterEntranceValue + coldWaterEntranceValue).ToStr();
        }
    }
}
