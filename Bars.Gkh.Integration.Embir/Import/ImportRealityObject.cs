namespace Bars.Gkh.Integration.Embir.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Integration.DataFetcher;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;
    using Gkh.Import.Impl;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ImportRealityObject : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key { get { return Id; } }

        public override string CodeImport { get { return "ImportEmbir"; } }

        public override string Name { get { return "Импорт жилых домов с ЕМБИР"; } }

        public override string PossibleFileExtensions { get { return string.Empty; } }

        public override string PermissionName { get { return "Import.Embir.View"; } }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }

        public IDomainService<TehPassport> TehPassportDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<WallMaterial> WallMaterialDomain { get; set; }

        public IDomainService<Fias> FiasDomain { get; set; }

        public IFiasRepository FiasRepos { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            Exception thrown = null;
            try
            {
                var webClientFetcher = new WebClientFetcher();
                var importHelper = new ImportIntegrationHelper(Container);
                var httpQueryBuilder = importHelper.GetHttpQueryBuilder();

                if (httpQueryBuilder.ToString().Length == 0)
                {
                    throw new Exception("В конфиге не задан адрес системы ЕМБИР");
                }

                var take = 50000;
                httpQueryBuilder.AddParameter("type", "RealtyObject");
                httpQueryBuilder.AddParameter("take", take);

                dynamic[] dynRealObjs = Enumerable.ToArray(webClientFetcher.GetData(httpQueryBuilder));

                httpQueryBuilder = importHelper.GetHttpQueryBuilder();

                var select = new DynamicDictionary
                {
                    {"RealtyObjectId", "RealtyObjectId.Id"},
                    {"TotalSquare", "TotalSquare"}
                };

                httpQueryBuilder.AddParameter("type", "Rooms");
                httpQueryBuilder.AddDictionary("select", select);
                httpQueryBuilder.AddParameter("take", take);

                var roomsInfo = webClientFetcher.GetData<IEnumerable<RoomInfoProxy>>(httpQueryBuilder)
                    .GroupBy(x => x.RealtyObjectId)
                    .ToDictionary(x => x.Key, y => new
                    {
                        Area = y.SafeSum(x => x.TotalSquare),
                        Count = y.Count()
                    });


                httpQueryBuilder = importHelper.GetHttpQueryBuilder();

                select = new DynamicDictionary
                {
                    {"RealtyObjectId", "RealtyObjectId.Id"},
                    {"InventoryNumber", "InventoryNumber"},
                    {"CadastralNumber", "CadastralNumber"}
                };

                httpQueryBuilder.AddParameter("type", "TechnicalPassport");
                httpQueryBuilder.AddDictionary("select", select);
                httpQueryBuilder.AddParameter("take", take);

                var techPassportInfo = webClientFetcher.GetData<IEnumerable<TechPassportProxy>>(httpQueryBuilder)
                    .GroupBy(x => x.RealtyObjectId)
                    .ToDictionary(x => x.Key, y => y.First());

                var invNumTechPaspValue = TehPassportValueDomain.GetAll()
                    .Where(x => x.CellCode == "18:1")
                    .GroupBy(x => x.TehPassport.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var tehPaspIdByRoId = TehPassportDomain.GetAll()
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                var realObjByAddressGuid = RealityObjectDomain.GetAll()
                    .Where(x => x.FiasAddress != null)
                    .Select(x => new
                    {
                        x.FiasAddress.StreetGuidId,
                        x.FiasAddress.House,
                        x.FiasAddress.Housing,
                        RealObj = x
                    })
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.StreetGuidId, x.House, x.Housing))
                    .ToDictionary(x => x.Key, y => y.Select(x => x.RealObj).First());

                var fiasAddressByAddressGuid = FiasAddressDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.StreetGuidId, x.House, x.Housing))
                    .ToDictionary(x => x.Key, y => y.First());

                var muByGuid = MunicipalityDomain.GetAll()
                    .GroupBy(x => x.FiasId)
                    .ToDictionary(x => x.Key, y => y.First());

                var muByOktmo = MunicipalityDomain.GetAll()
                    .Where(x => x.Oktmo.HasValue)
                    .AsEnumerable()
                    .GroupBy(x => x.Oktmo.ToLong())
                    .ToDictionary(x => x.Key, y => y.First());

                var oktmoByPlaceGuid = FiasDomain.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(x => new {x.AOGuid, x.OKTMO})
                    .AsEnumerable()
                    .GroupBy(x => x.AOGuid)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.OKTMO).FirstOrDefault());

                var wallMaterials = WallMaterialDomain.GetAll().AsEnumerable().GroupBy(x => x.Name.Trim().ToUpper()).ToDictionary(x => x.Key, y => y.First());

                var listFiasAddressToSave = new List<FiasAddress>();
                var listRealObjToSave = new List<RealityObject>();
                var listTehPassportValueToSave = new List<TehPassportValue>();
                var listTehPassportToSave = new List<TehPassport>();

                foreach (var dynRealObj in dynRealObjs)
                {
                    var isRealObjChange = false;

                    try
                    {
                        var realObjProxy = (JObject) (dynRealObj);
                        var embirRoId = realObjProxy["Id"].ToLong();

                        var fiasAddress = JsonConvert.DeserializeObject<FiasAddress>(realObjProxy["FiasAddress"].ToStr());
                        var addressKey = "{0}_{1}_{2}".FormatUsing(fiasAddress.StreetGuidId, fiasAddress.House,
                            fiasAddress.Housing);
                        var realObject = realObjByAddressGuid.Get(addressKey);

                        if (realObject == null)
                        {
                            fiasAddress.Id = 0;
                            var isFiasNew = false;
                            if (fiasAddressByAddressGuid.ContainsKey(addressKey))
                            {
                                fiasAddress = fiasAddressByAddressGuid.Get(addressKey);
                            }
                            else
                            {
                                isFiasNew = true;
                            }

                            realObject = new RealityObject
                            {
                                ConditionHouse = ConditionHouse.Serviceable,
                                HavingBasement = YesNoNotSet.NotSet,
                                HeatingSystem = HeatingSystem.Individual,
                                TypeHouse = TypeHouse.NotSet,
                                TypeRoof = TypeRoof.Plane,
                                ResidentsEvicted = false,
                                IsBuildSocialMortgage = YesNo.No,
                                NecessaryConductCr = YesNoNotSet.NotSet,
                                MethodFormFundCr = MethodFormFundCr.NotSet,
                                HasJudgmentCommonProp = YesNo.No,
                                ProjectDocs = TypePresence.NotSet,
                                EnergyPassport = TypePresence.NotSet,
                                ConfirmWorkDocs = TypePresence.NotSet,
                                IsNotInvolvedCr = false,
                                HasPrivatizedFlats = false,
                                IsRepairInadvisable = false,
                                IsInsuredObject = false
                            };


                            realObject.MoSettlement = GetSettlementForReality(oktmoByPlaceGuid, fiasAddress, muByOktmo);

                            var mu = realObject.MoSettlement != null
                                ? realObject.MoSettlement.ParentMo ?? realObject.MoSettlement
                                : GetMunicipality(muByGuid, fiasAddress);

                            if (mu == null)
                            {
                                LogImport.Error("Ошибка",
                                    "По адресу не удалось определить муниципальное образование.{0}".FormatUsing(
                                        fiasAddress.AddressName));
                                continue;
                            }

                            if (isFiasNew)
                            {
                                listFiasAddressToSave.Add(fiasAddress);
                            }

                            realObject.FiasAddress = fiasAddress;
                            realObject.Municipality = mu;


                            realObject.Address = realObject.Municipality != null
                                ? GetAddressForMunicipality(realObject.Municipality, realObject.FiasAddress)
                                : realObject.FiasAddress.AddressName;

                            LogImport.Info("Добавлен",
                                "Добавлен жилой дом. Муниципальный район: {0}. Адрес: {1}".FormatUsing(
                                    realObject.Municipality.Name, realObject.Address));

                            listFiasAddressToSave.Add(fiasAddress);

                            isRealObjChange = true;
                        }

                        var addressLog = "Муниципальный район: {0}. Адрес: {1}.".FormatUsing(realObject.Municipality.Name,
                            realObject.Address);

                        var wallMaterialStr = realObjProxy["WallMaterial"].ToStr();
                        var wallMaterial = !wallMaterialStr.IsEmpty()
                            ? JsonConvert.DeserializeObject<DictProxy>(realObjProxy["WallMaterial"].ToStr())
                            : null;
                        if (wallMaterial != null && !wallMaterial.Name.IsEmpty())
                        {
                            if (realObject.WallMaterial != null)
                            {
                                LogImport.Info("Информация",
                                    "{0} Поле 'Материал стен' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                                        .FormatUsing(addressLog, realObject.WallMaterial.Name, wallMaterial.Name));
                            }
                            else
                            {
                                var val = wallMaterials.Get(wallMaterial.Name.Trim().ToUpper());
                                if (val != null)
                                {
                                    realObject.WallMaterial = val;
                                    LogImport.Info("Изменено",
                                        "{0} В поле 'Материал стен' записано новое значение. Значение: {1}".FormatUsing(
                                            addressLog, val));
                                }
                                else
                                {
                                    LogImport.Info("Предупреждение",
                                        "В справочнике 'Материал стен' запись не найдена. Значение: {1}".FormatUsing(
                                            addressLog, wallMaterial.Name));
                                }
                            }
                        }

                        var roomsCount = roomsInfo.Get(embirRoId).Return(x => x.Count);
                        if (roomsCount != 0)
                        {
                            if (realObject.NumberApartments.ToInt() != 0)
                            {
                                LogImport.Info("Информация",
                                    "{0} Поле 'Количество квартир' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                                        .FormatUsing(addressLog, realObject.NumberApartments, roomsCount));
                            }
                            else
                            {
                                realObject.NumberApartments = roomsCount;
                                isRealObjChange = true;
                                LogImport.Info("Изменено",
                                    "{0} В поле 'Количество квартир' записано новое значение. Значение: {1}".FormatUsing(
                                        addressLog, roomsCount));
                            }
                        }

                        var roomsArea = roomsInfo.Get(embirRoId).Return(x => x.Area);
                        if (roomsArea != 0)
                        {
                            if (realObject.AreaLivingNotLivingMkd.ToDecimal() != 0)
                            {
                                LogImport.Info("Информация",
                                    "{0} Поле 'Общая площадь жилых и не жилых помещений' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                                        .FormatUsing(addressLog, realObject.AreaLivingNotLivingMkd, roomsArea));
                            }
                            else
                            {
                                realObject.AreaLivingNotLivingMkd = roomsArea;
                                isRealObjChange = true;
                                LogImport.Info("Изменено",
                                    "{0} В поле 'Общая площадь жилых и не жилых помещений' записано новое значение. Значение: {1}"
                                        .FormatUsing(addressLog, roomsArea));
                            }
                        }

                        var cadastralNum = techPassportInfo.Get(embirRoId).Return(x => x.CadastralNumber);
                        if (!cadastralNum.IsEmpty())
                        {
                            if (!realObject.CadastreNumber.IsEmpty())
                            {
                                LogImport.Info("Информация",
                                    "{0} Поле 'Кадастровый номер земельного участка' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                                        .FormatUsing(addressLog, realObject.CadastreNumber, cadastralNum));
                            }
                            else
                            {
                                realObject.CadastreNumber = cadastralNum;
                                isRealObjChange = true;
                                LogImport.Info("Изменено",
                                    "{0} В поле 'Кадастровый номер земельного участка' записано новое значение. Значение: {1}"
                                        .FormatUsing(addressLog, cadastralNum));
                            }
                        }

                        var invNum = techPassportInfo.Get(embirRoId).Return(x => x.InventoryNumber);

                        if (!invNum.IsEmpty())
                        {
                            var techPaspId = tehPaspIdByRoId.Get(realObject.Id);

                            if (techPaspId > 0)
                            {
                                var value = invNumTechPaspValue.Get(techPaspId);
                                if (value != null)
                                {
                                    if (value.Value.IsEmpty())
                                    {
                                        value.Value = invNum;
                                        listTehPassportValueToSave.Add(value);
                                        LogImport.Info("Информация",
                                            "{0} В поле 'Инвентарный номер' записано новое значение. Значение: {1}"
                                                .FormatUsing(addressLog, invNum));
                                    }
                                    else
                                    {
                                        LogImport.Info("Изменено",
                                            "{0} Поле 'Инвентарный номер' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                                                .FormatUsing(addressLog, value.Value, invNum));
                                    }
                                }
                                else
                                {
                                    listTehPassportValueToSave.Add(new TehPassportValue
                                    {
                                        TehPassport = TehPassportDomain.Load(techPaspId),
                                        FormCode = "Form_1",
                                        CellCode = "18:1",
                                        Value = invNum
                                    });
                                    LogImport.Info("Изменено",
                                        "{0} В поле 'Инвентарный номер' записано новое значение. Значение: {1}".FormatUsing(
                                            addressLog, invNum));
                                }
                            }
                            else
                            {
                                var tehPasp = new TehPassport
                                {
                                    RealityObject = realObject
                                };

                                listTehPassportToSave.Add(tehPasp);

                                listTehPassportValueToSave.Add(new TehPassportValue
                                {
                                    TehPassport = tehPasp,
                                    FormCode = "Form_1",
                                    CellCode = "18:1",
                                    Value = invNum
                                });
                                LogImport.Info("Изменено",
                                    "{0} В поле 'Инвентарный номер' записано новое значение. Значение: {1}".FormatUsing(
                                        addressLog, invNum));
                            }
                        }


                        ImportSimpleRows(realObjProxy, realObject, addressLog, ref isRealObjChange);

                        if (isRealObjChange)
                        {
                            listRealObjToSave.Add(realObject);
                        }

                        LogImport.CountAddedRows++;
                    }
                    catch
                    {
                        LogImport.Error("Ошибка", "Непредвиденная ошибка");
                    }
                }

                TransactionHelper.InsertInManyTransactions(Container, listFiasAddressToSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(Container, listRealObjToSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(Container, listTehPassportToSave, 10000, false, true);
                TransactionHelper.InsertInManyTransactions(Container, listTehPassportValueToSave, 10000, false, true);

                LogImport.Info(string.Empty, "Общее количество полученных записей: {0}".FormatUsing(dynRealObjs.Length));
                LogImport.Info(string.Empty, "Общее количество записей, с измененными в ЕМБИР значениями: {0}".FormatUsing(listRealObjToSave.Count));
            }
            catch (Exception exception)
            {
                thrown = exception;
            }

            SaveLog();

            return thrown == null ? new ImportResult() : new ImportResult(StatusImport.CompletedWithError, thrown.GetType().FullName + "\t" + thrown.Message);
        }

        private void SaveLogInternal()
        {
            var repo = Container.ResolveRepository<LogImport>();
            repo.Save(new LogImport
            {
                CountChangedRows = LogImport.CountChangedRows,
                CountError = LogImport.CountError,
                CountWarning = LogImport.CountWarning,
                ImportKey = Key,
                UploadDate = DateTime.Now,
            });
        }

        private void SaveLog()
        {
            var success = false;
            var tryIndex = 0;
            while (!success && tryIndex < 5)
            {
                try
                {
                    LogImport.SetFileName(Name);
                    LogImport.ImportKey = Key;
                    LogManager.AddLog(LogImport);
                    LogManager.FileNameWithoutExtention = Name;
                    LogManager.Save();
                    success = true;
                }
                catch (Exception exception)
                {
                    tryIndex++;
                }
            }

            if (!success)
            {
                try
                {
                    SaveLogInternal();
                }
                catch (Exception exception)
                {
                }
            }
        }

        private void ImportSimpleRows(JObject realObjProxy, RealityObject realObject, string addressLog, ref bool isRealObjChange)
        {
            var buildYear = realObjProxy["BuildDate"].ToInt();
            if (buildYear != 0)
            {
                if (realObject.BuildYear.ToInt() != 0)
                {
                    LogImport.Info("Информация",
                        "{0} Поле 'Год постройки' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                            .FormatUsing(addressLog, realObject.BuildYear, buildYear));
                }
                else
                {
                    realObject.BuildYear = buildYear;
                    isRealObjChange = true;
                    LogImport.Info("Изменено",
                        "{0} В поле 'Год постройки' записано новое значение. Значение: {1}".FormatUsing(addressLog,
                            buildYear));
                }
            }

            var areaMkd = realObjProxy["TotalSquare"].ToDecimal();
            if (areaMkd != 0)
            {
                if (realObject.AreaMkd.ToDecimal() != 0)
                {
                    LogImport.Info("Информация",
                        "{0} Поле 'Общая площадь МКД' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                            .FormatUsing(addressLog, realObject.AreaMkd, areaMkd));
                }
                else
                {
                    isRealObjChange = true;
                    realObject.AreaMkd = areaMkd;
                    LogImport.Info("Изменено",
                        "{0} В поле 'Общая площадь МКД' записано новое значение. Значение: {1}".FormatUsing(addressLog,
                            areaMkd));
                }
            }

            var areaLiving = realObjProxy["LivingSquare"].ToDecimal();
            if (areaLiving != 0)
            {
                if (realObject.AreaLiving.ToDecimal() != 0)
                {
                    LogImport.Info("Информация",
                        "{0} Поле 'Площадь жилая, всего' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                            .FormatUsing(addressLog, realObject.AreaLiving, areaLiving));
                }
                else
                {
                    isRealObjChange = true;
                    realObject.AreaLiving = areaLiving;
                    LogImport.Info("Изменено",
                        "{0} В поле 'Площадь жилая, всего' записано новое значение. Значение: {1}".FormatUsing(
                            addressLog, areaLiving));
                }
            }

            var maxFloors = realObjProxy["StoreysNum"].ToInt();
            if (maxFloors != 0)
            {
                if (realObject.MaximumFloors.ToInt() != 0)
                {
                    LogImport.Info("Информация",
                        "{0} Поле 'Максимальная этажность' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                            .FormatUsing(addressLog, realObject.MaximumFloors, maxFloors));
                }
                else
                {
                    isRealObjChange = true;
                    realObject.MaximumFloors = maxFloors;
                    LogImport.Info("Изменено",
                        "{0} В поле 'Максимальная этажность' записано новое значение. Значение: {1}".FormatUsing(
                            addressLog, maxFloors));
                }
            }

            var numberEntrances = realObjProxy["BlockNum"].ToInt();
            if (numberEntrances != 0)
            {
                if (realObject.NumberEntrances.ToInt() != 0)
                {
                    LogImport.Info("Информация",
                        "{0} Поле 'Количество подъездов' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                            .FormatUsing(addressLog, realObject.NumberEntrances, numberEntrances));
                }
                else
                {
                    isRealObjChange = true;
                    realObject.NumberEntrances = numberEntrances;
                    LogImport.Info("Изменено",
                        "{0} В поле 'Количество подъездов' записано новое значение. Значение: {1}".FormatUsing(
                            addressLog, numberEntrances));
                }
            }

            var liftNum = realObjProxy["LiftNum"].ToInt();
            if (liftNum != 0)
            {
                if (realObject.NumberLifts.ToInt() != 0)
                {
                    LogImport.Info("Информация",
                        "{0} Поле 'Количество лифтов' уже имеет значение. Значение: {1}. Значение из ЕМБИР: {2}"
                            .FormatUsing(addressLog, realObject.NumberLifts, liftNum));
                }
                else
                {
                    isRealObjChange = true;
                    realObject.NumberLifts = liftNum;
                    LogImport.Info("Изменено",
                        "{0} В поле 'Количество лифтов' записано новое значение. Значение: {1}".FormatUsing(addressLog,
                            liftNum));
                }
            }
        }

        /// <summary>
        ///     Метод получения Муниципального образования по переданному Адресу Fias
        /// </summary>
        private Municipality GetMunicipality(IDictionary<string, Municipality> muByFiasId, FiasAddress address)
        {
            if (address == null || string.IsNullOrEmpty(address.AddressGuid))
            {
                return null;
            }

            var guidMass = address.AddressGuid.Split('#');

            Municipality result = null;

            foreach (var s in guidMass)
            {
                var t = s.Split('_');

                Guid g;

                if (Guid.TryParse(t[1], out g) && g != Guid.Empty)
                {
                    var mcp = muByFiasId.Get(g.ToString());
                    if (mcp != null)
                    {
                        result = mcp;
                    }
                }
            }

            return result != null && result.ParentMo != null ? result.ParentMo : result;
        }

        /// <summary>
        ///     Метод получения строки адреса Fias ограничченный по МО
        /// </summary>
        private string GetAddressForMunicipality(Municipality mo, FiasAddress address)
        {
            if (address == null)
            {
                return string.Empty;
            }

            if (mo == null)
            {
                return string.Empty;
            }

            var result = address.AddressName ?? string.Empty;

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(mo.FiasId))
            {
                return string.Empty;
            }

            var dinamicAddress = (DinamicAddress) FiasRepos.GetDinamicAddress(mo.FiasId);

            if (dinamicAddress == null)
            {
                return string.Empty;
            }

            if (result.StartsWith(dinamicAddress.AddressName))
            {
                if (mo.Level == TypeMunicipality.UrbanArea)
                {
                    var urbanAreaName = dinamicAddress.AddressName.Contains("г.")
                        ? dinamicAddress.AddressName.Split("г.")[0]
                        : dinamicAddress.AddressName;

                    result = result.Replace(urbanAreaName, string.Empty).Trim();
                }
                else
                {
                    result = result.Replace(dinamicAddress.AddressName, string.Empty).Trim();
                }
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }

        private Municipality GetSettlementForReality(IDictionary<string, string> oktmoByAoGuid, FiasAddress fiasAddress, IDictionary<long, Municipality> muByOktmo)
        {
            var oktmo = oktmoByAoGuid.Get(fiasAddress.PlaceGuidId).ToInt();

            var mo = muByOktmo.Get(oktmo);

            return mo;
        }

        private class RoomInfoProxy
        {
            public long RealtyObjectId { get; set; }
            public decimal TotalSquare { get; set; }
        }

        private class TechPassportProxy
        {
            public long RealtyObjectId { get; set; }
            public string InventoryNumber { get; set; }
            public string CadastralNumber { get; set; }
        }

        private class DictProxy
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}