namespace Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Gis.DomainService.GisAddressMatching;
    using Bars.Gkh.Gis.Entities.PersonalAccount;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;
    using Bars.Gkh.Gis.Entities.Register.HouseServiceRegister;
    using Castle.Windsor;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Enum;

    using Enum = System.Enum;

    public class HouseRegisterService : IHouseRegisterService
    {
        public IWindsorContainer Container { get; set; }
        public IRepository<HouseRegister> HouseRepository { get; set; }
        public IAddressService AddressService { get; set; }
        public IRepository<Municipality> MunicipalityRepository { get; set; }
        public IRepository<RealityObject> RealityObjectRepository { get; set; }
        public IRepository<WallMaterial> WallMaterialRepository { get; set; }
        public IRepository<TypeProject> TypeProjectRepository { get; set; }
        public ISessionProvider SessionProvider { get; set; }
        public IRepository<FiasAddress> FiasAddressRepository { get; set; }
        public IRepository<HouseServiceRegister> HouseServiceRegisterRepository { get; set; }
        public IRepository<PersonalAccountUic> PersonalAccountUicRepository { get; set; }

        /// <summary>
        /// Присвоить типы домам
        /// </summary>
        /// <param name="baseParams">список домов, тип</param>
        /// <returns>результат</returns>
        public IDataResult SaveHouseTypes(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var idList = baseParams.Params.GetAs<List<long>>("houseIdList");
            var all = baseParams.Params.GetAs<bool>("all");
            var houseType = baseParams.Params.GetAs<TypeHouse>("houseType");

            if (all)
            {
                idList =
                    AddressService.GetAddresses(baseParams)
                        .Where(x => x.TypeAddressMatched == TypeAddressMatched.MatchedFound)
                        .AsQueryable()
                        .Filter(loadParams, Container)
                        .Select(x => x.Id)
                        .AsQueryable()
                        .ToList();
            }

            var housesById = HouseRepository
                .GetAll()
                .Where(x => idList.Contains(x.Id))
                .ToList();
            housesById.ForEach(x => x.TypeHouse = houseType);
            //попытка найти МО
            housesById.ForEach(
                x =>
                {
                    if (x.Municipality == null)
                    {
                        x.Municipality =
                            MunicipalityRepository.FirstOrDefault(y => y.FiasId == x.FiasAddress.PlaceGuidId);
                    }
                });
            //дома для РЖД
            //т.к. в b4_fias_address почему то дублируются записи - проверяем сложно, а не по id
            var realityObjectSave = housesById
                .Where(
                    x =>
                        !RealityObjectRepository.GetAll()
                            .Any(
                                y =>
                                    y.FiasAddress.StreetGuidId == x.FiasAddress.StreetGuidId
                                    &&
                                    y.FiasAddress.House.ToUpper() == x.FiasAddress.House.ToUpper()
                                    &&
                                    (y.FiasAddress.Housing ?? "").ToUpper() == (x.FiasAddress.Housing ?? "").ToUpper())
                        &&
                        x.Municipality != null)

                .Select(GetRealityObjectByHouseRegister)
                .ToList();
            SaveData(housesById, realityObjectSave);
            return new BaseDataResult();
        }

        /// <summary>
        /// Присвоить МО домам
        /// </summary>        >
        public IDataResult SaveHouseMunicipality(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var idList = baseParams.Params.GetAs<List<long>>("houseIdList");
            var all = baseParams.Params.GetAs<bool>("all");
            var municipality = MunicipalityRepository.Get(baseParams.Params.GetAs<long>("municipality"));
            if (municipality == null)
            {
                return BaseDataResult.Error("Выбранное муниципальное образование не найдено");
            }

            //если выбрали все - выбрать дома с фиас(возможно которых нет в РЖД)
            if (all)
            {
                idList =
                    AddressService.GetAddresses(baseParams)
                        .Where(x => x.TypeAddressMatched == TypeAddressMatched.MatchedFound)
                        .AsQueryable()
                        .Filter(loadParams, Container)
                        .Select(x => x.Id)
                        .AsQueryable()
                        .ToList();
            }
            var housesById = HouseRepository
                .GetAll()
                .Where(x => idList.Contains(x.Id))
                .ToList();
            housesById.ForEach(x => x.Municipality = municipality);

            //дома для РЖД
            //т.к. в b4_fias_address почему то дублируются записи - проверяем сложно, а не по id
            var realityObjectSave = housesById
                .Where(
                    x =>
                        !RealityObjectRepository.GetAll()
                            .Any(
                                y =>
                                    y.FiasAddress.StreetGuidId == x.FiasAddress.StreetGuidId
                                    &&
                                    y.FiasAddress.House.ToUpper() == x.FiasAddress.House.ToUpper()
                                    &&
                                    (y.FiasAddress.Housing ?? "").ToUpper() == (x.FiasAddress.Housing ?? "").ToUpper())
                )
                .Select(GetRealityObjectByHouseRegister)
                .ToList();
            SaveData(housesById, realityObjectSave);
            return new BaseDataResult();
        }


        /// <summary>
        /// Оставить только один сопоставленный дом. 
        /// Удалить дубли домов
        /// </summary>
        /// <param name="houseRegisterUpdate"></param>
        /// <param name="houseRegisterDelete"></param>
        /// <param name="houseServiceRegistersUpdate"></param>
        /// <param name="personalAccountUicUpdate"></param>
        /// <returns></returns>
        public void RemoveDuplicateHouses(
            List<HouseRegister> houseRegisterUpdate,
            List<HouseRegister> houseRegisterDelete,
            List<HouseServiceRegister> houseServiceRegistersUpdate,
            List<PersonalAccountUic> personalAccountUicUpdate)
        {            

            //дома с одинаковыми адресами
            var housesGroupsDict = houseRegisterUpdate
                .GroupBy(x => x.FiasAddress)
                .ToDictionary(x => x.Key, x => x);

            foreach (var groupKvp in housesGroupsDict)
            {
                var kvp = groupKvp;

                //поиск ранее сопотсавленных адресов
                var earlyMatchedList = HouseRepository
                    .GetAll()
                    .Where(x => x.FiasAddress == kvp.Key)
                    .ToList();
                
                var standard = earlyMatchedList.FirstOrDefault();                

                if (standard != null)
                {
                    earlyMatchedList.Remove(standard);

                    //обновляем ссылки услуг дома + несопоставленные дома
                    var houseServiceList = HouseServiceRegisterRepository
                        .GetAll()
                        .Where(x => earlyMatchedList.Contains(x.House) || kvp.Value.ToList().Contains(x.House))
                        .ToList();
                    houseServiceList.ForEach(x => x.House = standard);
                    houseServiceRegistersUpdate.AddRange(houseServiceList);

                    //обновляем ссылки uic + несопоставленные дома
                    var uicPersonalAccount = PersonalAccountUicRepository
                        .GetAll()
                        .Where(x => earlyMatchedList.Contains(x.HouseRegister) || kvp.Value.ToList().Contains(x.HouseRegister))
                        .ToList();
                    uicPersonalAccount.ForEach(x => x.HouseRegister = standard);
                    personalAccountUicUpdate.AddRange(uicPersonalAccount);


                    //ранее сопоставленные дома удаляются
                    houseRegisterDelete.AddRange(earlyMatchedList);
                    //удалить из списка на обновление
                    houseRegisterUpdate.RemoveAll(x => kvp.Value.Contains(x));
                    //удаляются еще не сопоставленные
                    houseRegisterDelete.AddRange(kvp.Value);
                }
                //определяем среди домов на обновление
                else
                {
                    standard = kvp.Value.FirstOrDefault();
                    var housesDelete = kvp.Value.Where(x => x != standard).ToList();

                    //обновляем ссылки услуг дома
                    var houseServiceList = HouseServiceRegisterRepository
                        .GetAll()
                        .Where(x => housesDelete.Contains(x.House))
                        .ToList();
                    houseServiceList.ForEach(x => x.House = standard);
                    houseServiceRegistersUpdate.AddRange(houseServiceList);

                    //обновляем ссылки uic
                    var uicPersonalAccount = PersonalAccountUicRepository
                        .GetAll()
                        .Where(x => housesDelete.Contains(x.HouseRegister))
                        .ToList();
                    uicPersonalAccount.ForEach(x => x.HouseRegister = standard);
                    personalAccountUicUpdate.AddRange(uicPersonalAccount);

                    //удалить из списка на обновление, кроме стандарта
                    houseRegisterUpdate.RemoveAll(x => housesDelete.Contains(x));
                    //удаляются еще не сопоставленные
                    houseRegisterDelete.AddRange(housesDelete);
                }
            }
        }

        /// <summary>
        /// Скопировать параметры домов РЖД в дома ГИС
        /// </summary>
        public IDataResult CopyHouseParams(List<HouseRegister> houseRegistersList = null)
        {
            try
            {
                List<HouseProxy> houseProxyList;
                if (houseRegistersList != null)
                {
                    var houseFiasesList = houseRegistersList.Select(x => x.FiasAddress).ToList();
                    var roList =
                        RealityObjectRepository
                            .GetAll()
                            .Where(x => houseFiasesList.Contains(x.FiasAddress))
                            .ToList();
                    houseProxyList =
                        houseRegistersList.Select(
                            x =>
                                new HouseProxy
                                {
                                    HouseRegister = x,
                                    RealityObject = roList.FirstOrDefault(y => y.FiasAddress == x.FiasAddress)
                                })
                            .ToList();

                }
                else
                {
                    houseProxyList =
                        HouseRepository.GetAll()
                            .Join(RealityObjectRepository.GetAll(),
                                x => x.FiasAddress, y => y.FiasAddress,
                                (x, y) => new HouseProxy {HouseRegister = x, RealityObject = y})
                            .ToList();
                }



                houseProxyList
                    .Where(x => x.RealityObject != null)
                    .ForEach(proxy =>
                    {
                        proxy.HouseRegister.TotalSquare = proxy.RealityObject.AreaMkd ?? 0;
                        proxy.HouseRegister.BuildDate = proxy.RealityObject.BuildYear.HasValue &&
                                                        proxy.RealityObject.BuildYear.Value > 0 &&
                                                        proxy.RealityObject.BuildYear.Value <= DateTime.MaxValue.Year
                            ? new DateTime(proxy.RealityObject.BuildYear.Value, 1, 1)
                            : DateTime.MinValue;
                        proxy.HouseRegister.TypeHouse = proxy.RealityObject.TypeHouse;
                        proxy.HouseRegister.MinimumFloors = proxy.RealityObject.Floors ?? 0;
                        proxy.HouseRegister.MaximumFloors = proxy.RealityObject.MaximumFloors ?? 0;
                        proxy.HouseRegister.NumberLiving = proxy.RealityObject.NumberLiving ?? 0;
                        proxy.HouseRegister.PrivatizationDate = proxy.RealityObject.PrivatizationDateFirstApartment ??
                                                                DateTime.MinValue;
                        proxy.HouseRegister.NumberLifts = proxy.RealityObject.NumberLifts ?? 0;
                        proxy.HouseRegister.TypeRoof = proxy.RealityObject.TypeRoof;
                        proxy.HouseRegister.WallMaterial = proxy.RealityObject.WallMaterial;
                        proxy.HouseRegister.PhysicalWear = proxy.RealityObject.PhysicalWear ?? 0;
                        proxy.HouseRegister.NumberEntrances = proxy.RealityObject.NumberEntrances ?? 0;
                        proxy.HouseRegister.RoofingMaterial = proxy.RealityObject.RoofingMaterial;
                        proxy.HouseRegister.TypeProject = proxy.RealityObject.TypeProject;
                        proxy.HouseRegister.HeatingSystem = proxy.RealityObject.HeatingSystem;
                        proxy.HouseRegister.AreaLivingNotLivingMkd = proxy.RealityObject.AreaLivingNotLivingMkd;
                        proxy.HouseRegister.AreaOwned = proxy.RealityObject.AreaOwned;
                        proxy.HouseRegister.NumberApartments = proxy.RealityObject.NumberApartments;
                    });

                var result = houseProxyList                    
                    .Select(x => x.HouseRegister)
                    .ToList();

                //если вызвали напрямую - обновляем все дома                
                if (houseRegistersList == null)
                {
                    SaveData(result);
                }
                return new ListDataResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return BaseDataResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// Скопировать параметры 1 дома РЖД в дома ГИС
        /// </summary>
        /// <param name="houseRegister">дом ГИС</param>
        public void CopyHouseParams(HouseRegister houseRegister)
        {
            CopyHouseParams(new List<HouseRegister> {houseRegister});
        }
        
        #region private         
        /// <summary>
        /// Получить аналогичный объект дома РЖД
        /// </summary>
        /// <param name="houseRegister">дом</param>
        /// <returns>дом для РЖД</returns>
        private RealityObject GetRealityObjectByHouseRegister(HouseRegister houseRegister)
        {
            var ro =
                new RealityObject
                {
                    Municipality = houseRegister.Municipality,
                    FiasAddress = houseRegister.FiasAddress,
                    //Address
                    AreaLiving = houseRegister.AreaLivingNotLivingMkd,
                    //AreaLivingOwned = houseRegister.TotalSquare,
                    AreaOwned = houseRegister.AreaOwned,
                    //AreaMunicipalOwned
                    //AreaGovernmentOwned
                    //AreaNotLivingFunctional
                    AreaLivingNotLivingMkd = houseRegister.AreaLivingNotLivingMkd,
                    AreaMkd = (decimal?) houseRegister.TotalSquare,
                    //AreaBasement
                    //AreaCommonUsage
                    //DateLastOverhaul
                    //DateCommissioning
                    //CapitalGroup
                    CodeErc = houseRegister.CodeErc,
                    //DateDemolition
                    MaximumFloors = houseRegister.MaximumFloors,
                    //RoofingMaterial
                    WallMaterial = WallMaterialRepository.Get(houseRegister.WallMaterial),
                    HavingBasement = YesNoNotSet.NotSet,
                    IsInsuredObject = false,
                    //Notation
                    //SeriesHome
                    TypeProject = TypeProjectRepository.Get(houseRegister.TypeProject),
                    HeatingSystem =
                        (HeatingSystem) Enum.Parse(typeof (HeatingSystem), houseRegister.HeatingSystem.ToString()),
                    ConditionHouse = ConditionHouse.Serviceable,
                    TypeHouse = (TypeHouse) Enum.Parse(typeof (TypeHouse), houseRegister.TypeHouse.ToString()),
                    TypeRoof = (TypeRoof) Enum.Parse(typeof (TypeRoof), houseRegister.TypeRoof.ToString()),
                    //FederalNum,
                    //PhysicalWear,
                    //TypeOwnership,
                    Floors = houseRegister.MaximumFloors,
                    NumberApartments = houseRegister.NumberApartments,
                    NumberEntrances = houseRegister.NumberEntrances,
                    NumberLifts = houseRegister.NumberLifts,
                    NumberLiving = houseRegister.NumberLiving,
                    //Description
                    //GkhCode
                    //WebCameraUrl
                    //DateTechInspection
                    //ResidentsEvicted
                    //DeleteAddressId
                    //IsBuildSocialMortgage
                    //TotalBuildingVolumeIsInsuredObject
                    //CadastreNumber
                    //NecessaryConductCr
                    //FloorHeight
                    //PercentDebt
                    PrivatizationDateFirstApartment = houseRegister.PrivatizationDate,
                    HasPrivatizedFlats = false,
                    BuildYear = houseRegister.BuildDate.Year,
                    //State
                    //MethodFormFundCr
                    //HasJudgmentCommonProp
                    //IsRepairInadvisable
                    IsNotInvolvedCr = false,
                    ProjectDocs = TypePresence.NotSet,
                    EnergyPassport = TypePresence.NotSet,
                    ConfirmWorkDocs = TypePresence.NotSet,
                    //MoSettlement
                    ManOrgs = houseRegister.ManOrgs,
                    //TypesContract
                };
            ro.Address = ro.GetLocalAddress();
            return ro;
        }

        /// <summary>
        /// Сохранить дома
        /// </summary>
        /// <param name="houseRegistersToUpdate">Список домов для сохранения</param>
        /// <param name="realityObjectsSave">Список для домов для реестра жилых домов</param>
        private void SaveData(List<HouseRegister> houseRegistersToUpdate, List<RealityObject> realityObjectsSave = null)
        {
            var session = SessionProvider.OpenStatelessSession();
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        houseRegistersToUpdate.ForEach(session.Update);
                        if (realityObjectsSave != null)
                        {
                            realityObjectsSave.ForEach(x => session.Insert(x));
                        }

                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, exc);
                        }
                    }
                }
            }
            finally
            {
                SessionProvider.CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #endregion
    }

    public class HouseProxy
    {
        public RealityObject RealityObject { set; get; }
        public HouseRegister HouseRegister { set; get; }
    }
}
