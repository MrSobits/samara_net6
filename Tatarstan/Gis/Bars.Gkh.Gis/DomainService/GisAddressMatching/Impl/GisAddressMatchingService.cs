namespace Bars.Gkh.Gis.DomainService.GisAddressMatching.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister;
    using Bars.Gkh.Gis.Entities.PersonalAccount;
    using Bars.Gkh.Gis.Entities.Register.HouseServiceRegister;
    using Bars.Gkh.Gis.Utils;
    using Entities.Register.HouseRegister;
    using Enums;
    using UicHouse;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Gis.Entities.ImportAddressMatching;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.Domain;
    using Castle.Windsor;

    public class GisAddressMatchingService : IGisAddressMatchingService
    {        
        public ISessionProvider SessionProvider { get; set; }
        public IWindsorContainer Container { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }
        public IRepository<HouseRegister> HouseRepository { get; set; }                                    
        public IRepository<ImportedAddressMatch> ImportedAddressRepository { get; set; }                                    
        public IDomainService<Fias> FiasDomain { get; set; }
        public IRepository<Fias> FiasRepository { get; set; }
        public IAddressMatcherService AddressMatcherService { get; set; }
        public IUicHouseService UicHouseService { get; set; }
        public IHouseRegisterService HouseRegisterService { get; set; }
        public IRepository<FiasAddressUid> FiasAddressRepository { get; set; }
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Иерархия адресных объектов
        /// </summary>
        private readonly FiasLevelEnum[] AddressObjectsOrder =
        {
            FiasLevelEnum.Street
            //FiasLevelEnum.Place,
            //FiasLevelEnum.City,
            //FiasLevelEnum.Raion,
            //FiasLevelEnum.Region
        };

        /// <summary>
        /// Единичное ручное сопоставление адресов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ManualMathAddress(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("addrMatchId");
            var fiasId = baseParams.Params.GetAs<long>("fiasId");            

            var house = HouseRepository.Get(id);
            if (house == null)
            {
                return BaseDataResult.Error("Выбранный дом не был найден");
            }

            var fias = FiasAddressDomain.Get(fiasId);
            if (fias == null)
            {
                return BaseDataResult.Error("Выбранный ФИАС адрес не был найден");
            }

            AddFiasAddressToHouse(house, fias);
            HouseRegisterUtils.FillHouseByFiasAddress(
                house,
                fias.StreetGuidId,
                FiasRepository);

            var houseRegisterUpdate = new List<HouseRegister> {house};
            return SaveMatchedHouses(houseRegisterUpdate);            
        }

        public IDataResult ManualBillingAddressMatch(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("addrMatchId");
            var fiasId = baseParams.Params.GetAs<long>("fiasId");            

            var match = ImportedAddressRepository.Get(id);
            if (match == null)
            {
                return BaseDataResult.Error("Выбранный дом не был найден");
            }

            var fias = FiasAddressDomain.Get(fiasId);
            if (fias == null)
            {
                return BaseDataResult.Error("Выбранный ФИАС адрес не был найден");
            }

            var fiasAddressUid = new FiasAddressUid{Address = fias, BillingId = match.AddressCode, Uid = Guid.NewGuid().ToString("N").ToUpper()};
            var existed = FiasAddressRepository.GetAll()
                .FirstOrDefault(x => x.BillingId == match.AddressCode && x.Address.Id == fias.Id);
            if (existed == null)
            {
                FiasAddressRepository.Save(fiasAddressUid);
            }
            match.FiasAddress = existed ?? fiasAddressUid;
            ImportedAddressRepository.Save(match);

            return new BaseDataResult(true, "Данные успешно сопоставлены");
        }

        /// <summary>
        /// Массовое ручное сопоставление адресов
        /// </summary>        
        public IDataResult MassManualMathAddress(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("addrMatchId");
            var fiasId = baseParams.Params.GetAs<long>("fiasId");
            var house = HouseRepository.Get(id);
            if (house == null)
            {
                return BaseDataResult.Error("Выбранный дом не был найден");
            }
            var fiasAddress = FiasAddressDomain.Get(fiasId);
            if (fiasAddress == null)
            {
                return BaseDataResult.Error("Выбранный адресс не был найден");
            }

            //определяем адрес добавлен вручную или выбран из ФИАС
            IDataResult result = new BaseDataResult();
            if (
                FiasDomain
                    .GetAll()
                    .Any(x =>
                        x.AOGuid == fiasAddress.StreetGuidId
                        &&
                        x.ParentGuid == fiasAddress.PlaceGuidId
                        &&
                        x.ActStatus == FiasActualStatusEnum.Actual)
                )
            {
                result = MatchAddressWithFias(house, fiasAddress);
            }
            else
            {
                result = MatchAddressWithFiasAddress(house, fiasAddress);
            }
            return result;
        }

        /// <summary>
        /// Спосоставить все похожие адреса с адресои из ФИАС(адрес уже был в ФИАС)
        /// </summary>
        /// <param name="house">дом</param>
        /// <param name="fiasAddress">выбранный адрес</param>
        private IDataResult MatchAddressWithFias(HouseRegister house, FiasAddress fiasAddress)
        {
            var result = new BaseDataResult();
            //сохраняем эталонные пары адресных объектов
            var referenceAo = new Dictionary<FiasLevelEnum, Tuple<string, string>>
            {
                {
                    FiasLevelEnum.Region,
                    new Tuple<string, string>(house.Region,
                        GetFiasByLevel(fiasAddress.StreetGuidId, FiasLevelEnum.Region, FiasDomain)
                            .Return(x => x.FormalName))
                },
                {
                    FiasLevelEnum.Raion,
                    new Tuple<string, string>(house.Area,
                        GetFiasByLevel(fiasAddress.StreetGuidId, FiasLevelEnum.Raion, FiasDomain)
                            .Return(x => x.FormalName))
                },
                {
                    FiasLevelEnum.City,
                    new Tuple<string, string>(house.City,
                        GetFiasByLevel(fiasAddress.StreetGuidId, FiasLevelEnum.City, FiasDomain)
                            .Return(x => x.FormalName))
                },
                {
                    FiasLevelEnum.Place,
                    new Tuple<string, string>(house.City,
                        GetFiasByLevel(fiasAddress.StreetGuidId, FiasLevelEnum.Place, FiasDomain)
                            .Return(x => x.FormalName))
                },
                {
                    FiasLevelEnum.Street,
                    new Tuple<string, string>(house.Street,
                        GetFiasByLevel(fiasAddress.StreetGuidId, FiasLevelEnum.Street, FiasDomain)
                            .Return(x => x.FormalName))
                }
            };

            var addressMatched = false;
            //по адресным уровням ФИАС
            foreach (var fiasLevelEnum in AddressObjectsOrder)
            {                
                var similarHouses = GetSimilarHouses(house, fiasLevelEnum);
                var houseRegisterUpdate = new List<HouseRegister>();                
                //похожие дома(степень похожести до определенного уровня)
                foreach (var similarHouse in similarHouses)
                {
                    HouseRegister similarHouseCopy;
                    EntityUtils.CopyEntityProperties(similarHouse, out similarHouseCopy);

                    //попытаться получить фиас для дома с модифицированным адресом
                    switch (fiasLevelEnum)
                    {
                        case FiasLevelEnum.Region:
                            {
                                similarHouseCopy.Region = referenceAo[FiasLevelEnum.Region].Item2;
                                break;
                            }
                        case FiasLevelEnum.Raion:
                            {
                                similarHouseCopy.Area = referenceAo[FiasLevelEnum.Raion].Item2;
                                similarHouseCopy.Region = referenceAo[FiasLevelEnum.Region].Item2;
                                break;
                            }
                        case FiasLevelEnum.City:
                        case FiasLevelEnum.Place:
                            {
                                similarHouseCopy.City = string.IsNullOrEmpty(referenceAo[FiasLevelEnum.City].Item2)
                                    ? referenceAo[FiasLevelEnum.Place].Item2
                                    : referenceAo[FiasLevelEnum.City].Item2;
                                similarHouseCopy.Area = referenceAo[FiasLevelEnum.Raion].Item2;
                                similarHouseCopy.Region = referenceAo[FiasLevelEnum.Region].Item2;
                                break;
                            }
                        case FiasLevelEnum.Street:
                            {
                                similarHouseCopy.Street = referenceAo[FiasLevelEnum.Street].Item2;
                                similarHouseCopy.City = string.IsNullOrEmpty(referenceAo[FiasLevelEnum.City].Item2)
                                    ? referenceAo[FiasLevelEnum.Place].Item2
                                    : referenceAo[FiasLevelEnum.City].Item2;
                                similarHouseCopy.Area = referenceAo[FiasLevelEnum.Raion].Item2;
                                similarHouseCopy.Region = referenceAo[FiasLevelEnum.Region].Item2;
                                break;
                            }
                    }

                    var fiasAddressFound = AddressMatcherService.MatchAddress(similarHouseCopy);
                    if (fiasAddressFound != null)
                    {
                        addressMatched = true;
                        AddFiasAddressToHouse(similarHouse, fiasAddressFound);
                        HouseRegisterUtils.FillHouseByFiasAddress(
                            similarHouse,
                            fiasAddressFound.StreetGuidId,
                            FiasRepository);
                        houseRegisterUpdate.Add(similarHouse);
                    }
                }
                result = (BaseDataResult) SaveMatchedHouses(houseRegisterUpdate);
                if (!result.Success) return result;
            }

            //сообщение о том что ни один адрес не был сопоставлен            
            if (!addressMatched)
            {
                result.Message = "Не найдены соответствующие адреса или найденные адреса не актуальны";
            }
            return result;
        }

        /// <summary>
        /// Спосоставить все похожие адреса с адресои из ФИАС(адрес добавлен вручную в ФИАС)
        /// </summary>
        /// <param name="house">дом</param>
        /// <param name="fiasAddress">выбранный адрес</param>
        private IDataResult MatchAddressWithFiasAddress(HouseRegister house, FiasAddress fiasAddress)
        {
            var houseRegisterUpdate = new List<HouseRegister>();            
            var similarHouses = GetSimilarHouses(house, FiasLevelEnum.Street);
            foreach (var similarHouse in similarHouses)
            {
                var sHouse = similarHouse;

                var fiasAddressFound = FiasAddressDomain.FirstOrDefault(x =>
                    x.PlaceGuidId == fiasAddress.PlaceGuidId
                    &&
                    x.StreetGuidId == fiasAddress.StreetGuidId
                    &&
                    x.House.ToUpper().Trim() == sHouse.HouseNum.ToUpper().Trim()
                    &&
                    (x.Housing ?? "").ToUpper().Trim() ==
                    (sHouse.BuildNum == "-" ? "" : (sHouse.BuildNum ?? "")).ToUpper().Trim()
                    );

                if (fiasAddressFound != null)
                {
                    AddFiasAddressToHouse(
                        similarHouse,
                        fiasAddressFound);
                    HouseRegisterUtils.FillHouseByFiasAddress(similarHouse, fiasAddressFound.StreetGuidId,
                        FiasRepository);
                    houseRegisterUpdate.Add(similarHouse);
                }
            }
            return SaveMatchedHouses(houseRegisterUpdate);
        }

        /// <summary>
        /// Определить похожие адреса
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>список домов с похожими адресами</returns>
        public IDataResult DetectSimilarAddresses(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("addrMatchId");            
            var house = HouseRepository.Get(id);
            if (house == null)
            {
                return BaseDataResult.Error("Выбранный дом не был найден");
            }            
            return new BaseDataResult(GetSimilarHouses(house, FiasLevelEnum.Street).Count() > 1);
        }

        /// <summary>
        /// Получить список похожих адресов
        /// </summary>
        /// <param name="houseRegister">дом</param>
        /// <param name="fiasLevel">уровень, до которого ищутся похожие дома</param>
        /// <returns>список домов с похожими адресами </returns>
        private IQueryable<HouseRegister> GetSimilarHouses(HouseRegister houseRegister, FiasLevelEnum fiasLevel)
        {
            //все несопоставленные
            var query = HouseRepository
                .GetAll()
                .Where(x =>
                    x.FiasAddress == null
                    &&
                    x.Region != null && x.Region.Trim() != ""
                    &&
                    (x.Area != null && x.Area.Trim() != "" || x.City != null && x.City.Trim() != "")
                    &&
                    x.Street != null && x.Street.Trim() != ""
                );

            switch (fiasLevel)
            {
                case FiasLevelEnum.Region:
                {
                    query = query
                        .Where(x =>                            
                            (x.Region ?? "").ToUpper() == (houseRegister.Region ?? "").ToUpper()
                        );
                    break;
                }
                case FiasLevelEnum.Raion:
                {
                    query = query
                        .Where(x =>                            
                            (x.Region ?? "").ToUpper() == (houseRegister.Region ?? "").ToUpper()
                            &&
                            (x.Area ?? "").ToUpper() == (houseRegister.Area ?? "").ToUpper()
                        );
                    break;
                }                                
                case FiasLevelEnum.City:
                case FiasLevelEnum.Place:
                {
                    query = query
                        .Where(x =>
                            (x.Region ?? "").ToUpper() == (houseRegister.Region ?? "").ToUpper()
                            &&
                            (x.Area ?? "").ToUpper() == (houseRegister.Area ?? "").ToUpper()
                            &&
                            (x.City ?? "").ToUpper() == (houseRegister.City ?? "").ToUpper()
                        );
                    break;
                }
                case FiasLevelEnum.Street:
                {
                    query = query
                        .Where(x =>
                            (x.Region ?? "").ToUpper() == (houseRegister.Region ?? "").ToUpper()
                            &&
                            (x.Area ?? "").ToUpper() == (houseRegister.Area ?? "").ToUpper()
                            &&
                            (x.City ?? "").ToUpper() == (houseRegister.City ?? "").ToUpper()
                            &&
                            (x.Street ?? "").ToUpper() == (houseRegister.Street ?? "").ToUpper()
                        );
                    break;
                }
            }

            return query;
        }

        /// <summary>
        /// Получить адресный объект определенного уровня
        /// </summary>
        /// <param name="guid">стартовый гуид</param>
        /// <param name="level">какой уровень нужен</param>
        /// <param name="fiasRepository">репозиторий фиас</param>
        /// <returns>искомый объект</returns>
        public Fias GetFiasByLevel(string guid, FiasLevelEnum level, IDomainService<Fias> fiasRepository)
        {
            var ao = fiasRepository.GetAll()
                .FirstOrDefault(x => x.AOGuid == guid);
            if (ao == null)
            {
                return null;
            }

            while (ao.AOLevel != level && ao.AOLevel != FiasLevelEnum.Region)
            {
                ao = fiasRepository.GetAll()
                .FirstOrDefault(x => x.AOGuid == ao.ParentGuid);

                if (ao == null)
                {
                    return null;
                }
            }

            if (level != FiasLevelEnum.Region && ao.AOLevel == FiasLevelEnum.Region)
            {
                return null;
            }

            return ao;
        }

        /// <summary>
        /// Сохранить сопоставленные дома
        /// </summary>
        /// <param name="houseRegisterUpdate"></param>
        private IDataResult SaveMatchedHouses(List<HouseRegister> houseRegisterUpdate)
        {
            var houseServiceRegisterUpdate = new List<HouseServiceRegister>();
            var personalAccountUicUpdate = new List<PersonalAccountUic>();
            var houseRegisterDelete = new List<HouseRegister>();
            
            //удалить дубли
            HouseRegisterService.RemoveDuplicateHouses(
                houseRegisterUpdate,
                houseRegisterDelete,
                houseServiceRegisterUpdate,
                personalAccountUicUpdate);

            //обновить параметры
            houseRegisterUpdate =
                ((ListDataResult)HouseRegisterService.CopyHouseParams(houseRegisterUpdate)).Data as List<HouseRegister>;

            //сохранить
            return SaveData(houseRegisterUpdate, houseServiceRegisterUpdate, houseRegisterDelete,
                personalAccountUicUpdate);
        }

        /// <summary>
        /// Сохранить дома
        /// </summary>
        /// <param name="houseRegistersToUpdate">Список домов для сохранения</param>        
        /// <param name="houseServiceRegisterUpdate">Список услуг для обновления ссылок</param>
        /// <param name="houseRegistersDelete">Список повторяющихся домов для удаления</param>
        /// <param name="personalAccountUicsUpdate"></param>
        private IDataResult SaveData(
            List<HouseRegister> houseRegistersToUpdate,
            List<HouseServiceRegister> houseServiceRegisterUpdate,
            List<HouseRegister> houseRegistersDelete,
            List<PersonalAccountUic> personalAccountUicsUpdate)
        {
            var session = SessionProvider.OpenStatelessSession();
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        houseServiceRegisterUpdate.ForEach(session.Update);
                        personalAccountUicsUpdate.ForEach(session.Update);
                        houseRegistersDelete.ForEach(session.Delete);
                        houseRegistersToUpdate.ForEach(session.Update);
                        transaction.Commit();
                        return new BaseDataResult(true, "Данные успешно сопоставлены");
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            transaction.Rollback();
                            return BaseDataResult.Error(exc.Message);
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

        private void AddFiasAddressToHouse(HouseRegister houseRegister, FiasAddress fiasAddress)
        {
            houseRegister.FiasAddress = fiasAddress;

            // При сопоставлении адреса - вызываем сервис генерации УИК
            UicHouseService.GenerateUic(houseRegister);
        }

        public IDataResult ManualBillingAddressMismatch(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var importedAddressMatch = ImportedAddressRepository.Get(id);

            if (importedAddressMatch.IsNotNull())
            {
                var fiasAddressUid = importedAddressMatch.FiasAddress;

                if (fiasAddressUid.IsNotNull())
                {
                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            importedAddressMatch.FiasAddress = null;
                            ImportedAddressRepository.Save(importedAddressMatch);
                            FiasAddressRepository.Delete(fiasAddressUid.Id);

                            var realityObjects = RealityObjectRepository.GetAll()
                                .Where(x => x.FiasAddress == fiasAddressUid.Address)
                                .ToList();

                            var ercCodes = FiasAddressRepository.GetAll()
                                .Where(x => x.Address == fiasAddressUid.Address && x.Id != fiasAddressUid.Id)
                                .Select(y => y.BillingId);

                            foreach (var ro in realityObjects)
                            {
                                ro.CodeErc = string.Join(", ", ercCodes);
                                RealityObjectRepository.Save(ro);
                            }

                            transaction.Commit();
                            return new BaseDataResult(true, "Связь дом - код ЕРЦ удалена");
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return new BaseDataResult(false, "Не удалось удалить связь дом - код ЕРЦ");
                        }
                    }
                }

                return new BaseDataResult(false, "Выбранная запись не имеет связи дом - код ЕРЦ");
            }

            return new BaseDataResult(false, "Сопоставленного импортированного адреса и адреса из ФИАС не существует");
        }
    }

    public class AddressProxy
    {
        public long Id { set; get; }
        public string RegionName { set; get; }
        public string Supplier { set; get; }
        public string CityName { set; get; }
        public string StreetName { set; get; }
        public string Number { set; get; }
        public string Address { set; get; }
        public TypeAddressMatched TypeAddressMatched { set; get; }
        public TypeHouse HouseType { set; get; }
        public string Municipality { set; get; }
    }
    
    public class ImportedAddressProxy
    {
        public long Id { set; get; }
        public string ImportType { get; set; }
        public string ImportFilename { get; set; }
        public string AddressCodeRemote { get; set; }
        public string AddressRemote { get; set; }
        public long? AddressCode { get; set; }
        public string Address { get; set; }
        public bool IsMatched { get; set; }
    }
}