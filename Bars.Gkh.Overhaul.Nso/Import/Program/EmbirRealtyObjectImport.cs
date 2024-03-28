namespace Bars.Gkh.Overhaul.Nso.Import.Program
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    /*
     * Запрос к бд ЕМБИР:
     * select fa.street_guid, fa.house, fa.housing, fa.post_code, fa.address_name, fa.place_address_name, fa.place_name, fa.place_guid, fa.place_code, fa.street_name, fa.street_code, fa.building, fa.flat, fa.coordinate, fa.letter, fa.address_guid, count(*) as roomscount, sum(r.totalsquare) as notlivingarea, ro.totalsquare, ro.nonresidentialsquare as livingsquare, ro.storeysnum, ro.blocknum, ro.liftnum, max(f.oktmo), max(f.kladrcode), max(f.okato) from realtyobject ro
join b4_fias_address fa on ro.fiasaddress=fa.id
join rooms r on r.realtyobjectid=ro.id
left join b4_fias f on f.aoguid=fa.place_guid
group by fa.street_guid, fa.house, fa.housing, fa.post_code, fa.address_name, fa.place_address_name, fa.place_name, fa.place_guid, fa.place_code, fa.street_name, fa.street_code, fa.building, fa.flat, fa.coordinate, fa.letter, fa.address_guid, ro.totalsquare, ro.nonresidentialsquare, ro.storeysnum, ro.blocknum, ro.liftnum
     */
    public sealed class EmbirRealtyObjectImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public IRepository<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<WallMaterial> WallMaterialDomain { get; set; }

        public IDomainService<Fias> FiasDomain { get; set; }

        public IFiasRepository FiasRepos { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "NsoRealtyObjectImport"; }
        }

        public override string Name
        {
            get { return "Импорт домов из ЕМБИР"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.NsoRealtyObjectImport.View"; }
        }

        public class ParsedRow
        {
            public string StreetGuid { get; set; }
            public string House { get; set; }
            public string Housing { get; set; }
            public int RoomsCount { get; set; }
            public decimal NotLivingArea { get; set; }
            public decimal TotalSquare { get; set; }
            public decimal LivingSquare { get; set; }
            public int StoreysNum { get; set; }
            public int BlockNum { get; set; }
            public int LiftNum { get; set; }
            public string PostCode { get; set; }
            public string AddressName { get; set; }
            public string PlaceAddressName { get; set; }
            public string PlaceName { get; set; }
            public string PlaceGuid { get; set; }
            public string PlaceCode { get; set; }
            public string StreetName { get; set; }
            public string StreetCode { get; set; }
            public string Building { get; set; }
            public string Flat { get; set; }
            public string Coordinate { get; set; }
            public string Letter { get; set; }
            public string AddressGuid { get; set; }

            public string Oktmo { get; set; }

            public string Klard { get; set; }

            public string Okato { get; set; }
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];

            var parsedRows = new List<ParsedRow>();

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                excel.UseVersionXlsx();
                using (var xlsMemoryStream = new MemoryStream(fileData.Data))
                {
                    excel.Open(xlsMemoryStream);
                    var dict = new Dictionary<string, string>();
                    var data = excel.GetRows(0, 0);

                    var startIndex = 0;
                    string[] header = null;

                    var rowNum = 0;
                    do
                    {
                        var row = data[rowNum].Where(x => x.Value != null).Select(x => x.Value.ToLower()).ToArray();

                        if (row.Contains("street_guid"))
                        {
                            startIndex = rowNum + 1;
                            header = row;
                        }

                        rowNum++;
                    } while (startIndex == 0 && rowNum + 1 != data.Count);

                    if (startIndex == 0)
                    {
                        throw new ArgumentException("Формат шаблона не соответсвует импорту!\nНе найден столбец \"street_guid\"");
                    }

                    for (var i = startIndex; i < data.Count; i++)
                    {
                        try
                        {
                            var row = data[i];
                            dict.Clear();

                            for (var j = 0; j < row.Length; j++)
                            {
                                if (!string.IsNullOrWhiteSpace(header[j]))
                                {
                                    dict.Add(header[j], row[j].Value);
                                }
                            }

                            var entity = new ParsedRow
                            {
                                StreetGuid = dict.Get("street_guid"),
                                House = dict.Get("house"),
                                Housing = dict.Get("housing"),
                                RoomsCount = dict.Get("roomscount").ToInt(),
                                NotLivingArea = dict.Get("notlivingarea").ToDecimal(),
                                TotalSquare = dict.Get("totalsquare").ToDecimal(),
                                LivingSquare = dict.Get("livingsquare").ToDecimal(),
                                StoreysNum = dict.Get("storeysnum").ToInt(),
                                BlockNum = dict.Get("blocknum").ToInt(),
                                LiftNum = dict.Get("liftnum").ToInt(),
                                PostCode = dict.Get("post_code"),
                                AddressName = dict.Get("address_name"),
                                PlaceAddressName = dict.Get("place_address_name"),
                                PlaceName = dict.Get("place_name"),
                                PlaceGuid = dict.Get("place_guid"),
                                PlaceCode = dict.Get("place_code"),
                                StreetName = dict.Get("street_name"),
                                StreetCode = dict.Get("street_code"),
                                Building = dict.Get("building"),
                                Flat = dict.Get("flat"),
                                Coordinate = dict.Get("coordinate"),
                                Letter = dict.Get("letter"),
                                AddressGuid = dict.Get("address_guid"),
                                Oktmo = dict.Get("oktmo"),
                                Klard = dict.Get("kladrcode"),
                                Okato = dict.Get("okato")
                            };

                            parsedRows.Add(entity);
                        }
                        catch (Exception exception)
                        {
                            LogImport.Error(
                                    "Строка не добавлена",
                                    string.Format("Для строки {0} произошла ошибка: {1} c типом {2}", i + 1, exception.Message, exception.GetType().FullName));
                        }
                    }
                }
            }

            try
            {
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
                    .Where(x => x.FiasId != null)
                    .GroupBy(x => x.FiasId)
                    .ToDictionary(x => x.Key, y => y.First());

                var muByOktmo = MunicipalityDomain.GetAll()
                    .Where(x => x.Oktmo != string.Empty)
                    .AsEnumerable()
                    .GroupBy(x => x.Oktmo)
                    .ToDictionary(x => x.Key, y => y.First());

                var muByOkato = MunicipalityDomain.GetAll()
                    .Where(x => x.Okato != null && x.Okato != "")
                    .AsEnumerable()
                    .GroupBy(x => x.Okato)
                    .ToDictionary(x => x.Key, y => y.First());

                var muByKladr = (from municipality in MunicipalityDomain.GetAll()
                    join fias in FiasRepos.GetAll() on municipality.FiasId equals fias.AOGuid
                    select new
                    {
                        fias.KladrCode,
                        municipality.Id
                    }).AsEnumerable()
                    .Where(x => x.KladrCode != null && x.KladrCode != "")
                    .GroupBy(x => x.KladrCode)
                    .ToDictionary(x => x.Key, x => x.First().Id);

                var muById = MunicipalityDomain.GetAll().AsEnumerable().ToDictionary(x => x.Id, x => x);

                var oktmoByPlaceGuid = FiasDomain.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(x => new { x.AOGuid, x.OKTMO })
                    .AsEnumerable()
                    .GroupBy(x => x.AOGuid)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.OKTMO).FirstOrDefault());

                var listFiasAddressToSave = new List<FiasAddress>();
                var listRealObjToSave = new List<RealityObject>();

                foreach (var parsedRow in parsedRows)
                {
                    try
                    {
                        var addressKey = "{0}_{1}_{2}".FormatUsing(parsedRow.StreetGuid, parsedRow.House,
                            parsedRow.Housing);
                        var realObject = realObjByAddressGuid.Get(addressKey);

                        if (realObject == null)
                        {
                            FiasAddress fiasAddress;
                            if (fiasAddressByAddressGuid.ContainsKey(addressKey))
                            {
                                fiasAddress = fiasAddressByAddressGuid.Get(addressKey);
                                listFiasAddressToSave.Add(fiasAddress);
                            }
                            else
                            {
                                fiasAddress = new FiasAddress
                                {
                                    AddressGuid = parsedRow.AddressGuid,
                                    AddressName = parsedRow.AddressName,
                                    Building = parsedRow.Building.Substring(0, Math.Min(parsedRow.Building.Length, 10)),
                                    Coordinate = parsedRow.Coordinate,
                                    Flat = parsedRow.Flat.Substring(0, Math.Min(parsedRow.Flat.Length, 10)),
                                    House = parsedRow.House.Substring(0, Math.Min(parsedRow.House.Length, 10)),
                                    Housing = parsedRow.Housing.Substring(0, Math.Min(parsedRow.Housing.Length, 10)),
                                    Letter = parsedRow.Letter.Substring(0, Math.Min(parsedRow.Letter.Length, 10)),
                                    PlaceAddressName = parsedRow.PlaceAddressName,
                                    PlaceCode = parsedRow.PlaceCode,
                                    PlaceGuidId = parsedRow.PlaceGuid,
                                    PlaceName = parsedRow.PlaceName,
                                    PostCode = parsedRow.PostCode,
                                    StreetCode = parsedRow.StreetCode,
                                    StreetGuidId = parsedRow.StreetGuid,
                                    StreetName = parsedRow.StreetName
                                };
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

                            if (parsedRow.Oktmo.Length != 0)
                            {
                                realObject.MoSettlement = muByOktmo.ContainsKey(parsedRow.Oktmo) ? muByOktmo[parsedRow.Oktmo] : null;
                            }

                            if (realObject.MoSettlement == null && !string.IsNullOrWhiteSpace(parsedRow.Okato))
                            {
                                realObject.MoSettlement = muByOkato.ContainsKey(parsedRow.Okato) ? muByOkato[parsedRow.Okato] : null;
                            }

                            if (realObject.MoSettlement == null && !string.IsNullOrWhiteSpace(parsedRow.Klard))
                            {
                                var muId = muByKladr.ContainsKey(parsedRow.Klard) ? muByKladr[parsedRow.Klard] : (long?) null;
                                if (muId.HasValue)
                                {
                                    realObject.MoSettlement = muById[muId.Value];
                                }
                            }

                            if (realObject.MoSettlement == null)
                            {
                                realObject.MoSettlement = GetSettlementForReality(oktmoByPlaceGuid, fiasAddress, muByOktmo);
                            }

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

                            realObject.FiasAddress = fiasAddress;
                            realObject.Municipality = mu;


                            realObject.Address = realObject.Municipality != null
                                ? GetAddressForMunicipality(realObject.Municipality, realObject.FiasAddress)
                                : realObject.FiasAddress.AddressName;

                            LogImport.Info("Добавлен",
                                "Добавлен жилой дом. Муниципальный район: {0}. Адрес: {1}".FormatUsing(
                                    realObject.Municipality.Name, realObject.Address));

                            listFiasAddressToSave.Add(fiasAddress);
                        }

                        var roomsCount = parsedRow.RoomsCount;
                        if (roomsCount != 0)
                        {
                            if (realObject.NumberApartments.ToInt() == 0)
                            {
                                realObject.NumberApartments = roomsCount;
                            }
                        }

                        var roomsArea = parsedRow.NotLivingArea;
                        if (roomsArea != 0)
                        {
                            if (realObject.AreaLivingNotLivingMkd.ToDecimal() == 0)
                            {
                                realObject.AreaLivingNotLivingMkd = roomsArea;
                            }
                        }

                        var areaMkd = parsedRow.TotalSquare;
                        if (areaMkd != 0)
                        {
                            if (realObject.AreaMkd.ToDecimal() == 0)
                            {
                                realObject.AreaMkd = areaMkd;
                            }
                        }

                        var areaLiving = parsedRow.LivingSquare;
                        if (areaLiving != 0)
                        {
                            if (realObject.AreaLiving.ToDecimal() == 0)
                            {
                                realObject.AreaLiving = areaLiving;
                            }
                        }

                        var maxFloors = parsedRow.StoreysNum;
                        if (maxFloors != 0)
                        {
                            if (realObject.MaximumFloors.ToInt() == 0)
                            {
                                realObject.MaximumFloors = maxFloors;
                            }
                        }

                        var numberEntrances = parsedRow.BlockNum;
                        if (numberEntrances != 0)
                        {
                            if (realObject.NumberEntrances.ToInt() == 0)
                            {
                                realObject.NumberEntrances = numberEntrances;
                            }
                        }

                        var liftNum = parsedRow.LiftNum;
                        if (liftNum != 0)
                        {
                            if (realObject.NumberLifts.ToInt() == 0)
                            {
                                realObject.NumberLifts = liftNum;
                            }
                        }

                        listRealObjToSave.Add(realObject);

                        LogImport.CountAddedRows++;
                    }
                    catch
                    {
                        LogImport.Error("Ошибка", "Непредвиденная ошибка");
                    }
                }

                var fiasRepo = Container.ResolveRepository<FiasAddress>();

                var partitioner = Partitioner.Create(0, listFiasAddressToSave.Count, 5000);
                foreach (var partition in partitioner.GetDynamicPartitions())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            for (var index = partition.Item1; index < partition.Item2; index++)
                            {
                                var fiasAddress = listFiasAddressToSave[index];
                                fiasRepo.Save(fiasAddress);
                            }

                            tr.Commit();
                        }
                        catch (Exception exception)
                        {
                            LogImport.Error("Ошибка сохранения", "При сохранении фиас части с {0} по {1}. Произошла ошибка: {2}".FormatUsing(partition.Item1, partition.Item2, exception.Message));
                            tr.Rollback();
                        }
                    }
                }

                partitioner = Partitioner.Create(0, listRealObjToSave.Count, 5000);
                foreach (var partition in partitioner.GetDynamicPartitions())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            for (var index = partition.Item1; index < partition.Item2; index++)
                            {
                                var realityObject = listRealObjToSave[index];
                                if (realityObject.Id == 0)
                                {
                                    RealityObjectDomain.Save(realityObject);
                                }
                                else
                                {
                                    RealityObjectDomain.Update(realityObject);
                                }
                            }

                            tr.Commit();
                        }
                        catch (Exception exception)
                        {
                            LogImport.Error("Ошибка сохранения", "При сохранении части МКД с {0} по {1}. Произошла ошибка: {2}".FormatUsing(partition.Item1, partition.Item2, exception.Message));
                            tr.Rollback();
                        }
                    }
                }

                LogImport.Info(string.Empty, "Общее количество полученных записей: {0}".FormatUsing(parsedRows.Count));
                LogImport.Info(string.Empty, "Общее количество записей, с измененными в ЕМБИР значениями: {0}".FormatUsing(listRealObjToSave.Count));
            }
            catch (Exception exception)
            {
                this.LogImport.Error("Ошибка сохранения", $"{exception.Message}.\r\n{exception.StackTrace}");
            }

            LogImport.SetFileName(fileData.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogManager.FileNameWithoutExtention = fileData.FileName;
            LogManager.Add(fileData, LogImport);
            LogManager.Save();

            return new ImportResult(LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
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

            var dinamicAddress = (DinamicAddress)FiasRepos.GetDinamicAddress(mo.FiasId);

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

        private Municipality GetSettlementForReality(IDictionary<string, string> oktmoByAoGuid, FiasAddress fiasAddress, Dictionary<string, Municipality> muByOktmo)
        {
            var oktmo = oktmoByAoGuid.Get(fiasAddress.PlaceGuidId);

            var mo = muByOktmo.Get(oktmo);

            return mo;
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }
    }
}