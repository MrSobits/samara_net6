namespace Bars.Gkh.Overhaul.Hmao.Import.ImportDpkr
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import.Impl;
    using Ionic.Zip;

    /// <summary>
    /// Импорт ДПКР
    /// </summary>
    public class DpkrImport : GkhImportBase
    {
        /// <summary>
        /// IoC
        /// </summary>
        public new virtual IWindsorContainer Container { get; set; }

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType?.FullName;

        public override string Key => DpkrImport.Id;

        public override string CodeImport => "DpkrImport";

        // Импорт ДПКР(Камчатка)
        public override string Name => "Импорт ДПКР";

        public override string PossibleFileExtensions
        {
            get { return "xls,zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.Dpkr.View"; }
        }

        public new ILogImport LogImport { get; set; }

        public ILogImportManager LogManager { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoSeDomain { get; set; }

        public IRepository<Fias> FiasRepository { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain{ get; set; }

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        private Dictionary<string, List<RealtyObjectAddress>> realityObjectsByFiasGuidDict;

        private Dictionary<string, KeyValuePair<string, long>> fiasIdByMunicipalityNameDict;

        private Dictionary<string, Dictionary<string, StructuralElement>> structElByString;

        private Dictionary<long, Dictionary<long, long>> roSeByRealObj;

        private Dictionary<long, string> addressByRoId;

        private List<RealityObjectStructuralElement> roSeForSave = new List<RealityObjectStructuralElement>();

        private List<RealityObjectStructuralElementInProgramm> stage1ForSave = new List<RealityObjectStructuralElementInProgramm>();

        private List<RealityObjectStructuralElementInProgrammStage2> stage2ForSave = new List<RealityObjectStructuralElementInProgrammStage2>();

        private List<RealityObjectStructuralElementInProgrammStage3> stage3ForSave = new List<RealityObjectStructuralElementInProgrammStage3>();

        private FiasHelper fiasHelper;

        private Dictionary<string, RobjectProxy> robjectByAddressCache;

        private class RobjectProxy
        {
            public int Count { get; set; }

            public long RoId { get; set; }
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var bytes = baseParams.Files["FileImport"].Data;
                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] {this.PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                    return false;
                }

                if (extention == "xls")
                {

                    using (var memoryStreamFile = new MemoryStream(bytes))
                    {
                        var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                        using (this.Container.Using(excel))
                        {
                            if (excel == null)
                            {
                                throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                            }

                            excel.Open(memoryStreamFile);
                        }
                    }
                }

                message = null;
                return true;
            }
            catch (Exception exp)
            {
                this.Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;

            this.InitHeaders();

            this.InitDictionaries();

            using (var memoryStreamFile = this.GetFile(fileData))
            {
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);
                    var startImport = false;

                    for (var i = 0; i < rows.Count; i++)
                    {
                        if (rows[i][0].Value != "№ п/п" && !startImport)
                        {
                            continue;
                        }

                        if (rows[i][0].Value == "№ п/п" && !startImport)
                        {
                            startImport = true;
                            continue;
                        }

                        if (startImport)
                        {
                            this.ImportRow(rows[i], i + 1, startYear, endYear);
                        }
                    }

                    if (!startImport)
                    {
                        this.LogImport.Error("Ошибка", "В исходном файле отсутствует обязательный заголовок: \"№ п/п\"");
                    }

                    this.LogImport.IsImported = true;
                }
            }

            this.SaveResult();

            this.LogImport.SetFileName(fileData.FileName);
            this.LogImport.ImportKey = this.CodeImport;

            this.LogManager.FileNameWithoutExtention = fileData.FileName;
            this.LogManager.Add(fileData, this.LogImport);
            this.LogManager.Save();

            var message = string.Format(
                " Загружено строк: {0}. Ошибки: {1}", this.LogImport.CountAddedRows, this.LogImport.CountError);

            return new ImportResult(this.LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError,
                message, 
                string.Empty, this.LogManager.LogFileId);
        }

        private void ImportRow(GkhExcelCell[] row, int rowNumber, int startYear, int endYear)
        {
            var realObjId = this.GetRealObjId(row, rowNumber);

            if (realObjId == 0)
            {
                return;
            }

            var ceoName = this.GetValue(row, "CEO");
            var trimCeoName = ceoName.Trim().ToUpper();
            var structElName = this.GetValue(row, "STRUCTEL");
            var trimStructElName = structElName.Trim().ToUpper();

            if (!this.structElByString.ContainsKey(trimCeoName))
            {
                this.LogImport.Error("Ошибка", $"Не найден объект общего имущества: {ceoName}. Строка {rowNumber}");
                return;
            }

            if (!this.structElByString[trimCeoName].ContainsKey(trimStructElName))
            {
                this.LogImport.Error("Ошибка", $"Не найден конструктивный элемент: {structElName}, в ООИ: {ceoName}. Строка {rowNumber}");
                return;
            }

            var structEl = this.structElByString[trimCeoName][trimStructElName];

            var firstYear = this.GetValue(row, "FIRSTYEAR").ToInt();
            var secondYear = this.GetValue(row, "SECONDYEAR").ToInt();
            var thirdYear = this.GetValue(row, "THIRDYEAR").ToInt();

            var yearList = new List<string>();
            if (startYear <= firstYear && firstYear <= endYear)
            {
                this.AddDpkrRow(row, firstYear, realObjId, structEl);
                yearList.Add(firstYear.ToStr());
            }

            if (startYear <= secondYear && secondYear <= endYear)
            {
                this.AddDpkrRow(row, secondYear, realObjId, structEl);
                yearList.Add(secondYear.ToStr());
            }

            if (startYear <= thirdYear && thirdYear <= endYear)
            {
                this.AddDpkrRow(row, thirdYear, realObjId, structEl);
                yearList.Add(thirdYear.ToStr());
                
            }

            if (yearList.Any())
            {
                this.LogImport.CountAddedRows++;

                var desc = string.Format(
                    "Добавлена(-ы) запись(-и) в ДПКР. Адрес: {0}, ООИ: {1}, Конструктивный элемент: {2}, Год(-а): {3}", this.addressByRoId[realObjId],
                    structEl.Group.CommonEstateObject.Name,
                    structEl.Name,
                    yearList.AggregateWithSeparator(", "));
                this.LogImport.Info(string.Empty, desc);
            }
        }

        private void AddDpkrRow(GkhExcelCell[] row, int year, long realObjId, StructuralElement structEl)
        {      
            var sum = this.GetValue(row, "SUM").ToDecimal();

            var stage3 = new RealityObjectStructuralElementInProgrammStage3
                              {
                                  RealityObject = this.RealityObjectDomain.Load(realObjId),
                                  CommonEstateObjects = structEl.Group.CommonEstateObject.Name,
                                  Year = year,
                                  Sum = sum,
                                  IndexNumber = 0,
                                  Point = 0
                              };

            this.stage3ForSave.Add(stage3);

            var stage2 = new RealityObjectStructuralElementInProgrammStage2
            {
                RealityObject = this.RealityObjectDomain.Load(realObjId),
                Year = year,
                Sum = sum,
                CommonEstateObject = structEl.Group.CommonEstateObject,
                StructuralElements = structEl.Name,
                Stage3 = stage3
            };

            this.stage2ForSave.Add(stage2);

            var stage1 = new RealityObjectStructuralElementInProgramm
            {
                StructuralElement = this.GetRoStructEl(row, realObjId, structEl),
                Year = year,
                Sum = sum,
                ServiceCost = 0,
                Stage2 = stage2
            };

            this.stage1ForSave.Add(stage1);
        }

        private RealityObjectStructuralElement GetRoStructEl(GkhExcelCell[] row, long realObjId, StructuralElement structEl)
        {
            if (this.roSeByRealObj.ContainsKey(realObjId) && this.roSeByRealObj[realObjId].ContainsKey(structEl.Id))
            {
                return this.RoSeDomain.Load(this.roSeByRealObj[realObjId][structEl.Id]);
            }

            var roSe = new RealityObjectStructuralElement
            {
                RealityObject = this.RealityObjectDomain.Load(realObjId),
                StructuralElement = structEl,
                Name = structEl.Name,
                Volume = this.GetValue(row, "VOLUME").ToDecimal(),
                LastOverhaulYear = this.GetValue(row, "LASTOVERHAULYEAR").ToInt()
            };

            this.roSeForSave.Add(roSe);

            this.LogImport.Info(string.Empty, $"Добавлен конструктивный элемент '{structEl.Name}' по адресу: {this.addressByRoId[realObjId]}");

            return roSe;
        }

        private long GetRealObjId(GkhExcelCell[] row, int rowNumber)
        {
            var record = new Record {RowNumber = rowNumber};

            if (row.Length <= 1)
            {
                return 0;
            }

            var municipalityName = this.GetValue(row, "MUNICIPALITY").Trim().ToUpper();
            if (string.IsNullOrEmpty(municipalityName))
            {
                return 0;
            }

            record.LocalityName = DpkrImport.Simplified(this.GetValue(row, "LOCALITYNAME"));
            record.StreetName = DpkrImport.Simplified(this.GetValue(row, "STREETNAME"));
            record.Liter = DpkrImport.Simplified(this.GetValue(row, "LITER"));
            record.Housing = this.GetValue(row, "HOUSING");
            record.Building = DpkrImport.Simplified(this.GetValue(row, "BUILDING"));
            record.House = DpkrImport.Simplified(this.GetValue(row, "HOUSE"));

            if (string.IsNullOrEmpty(record.House))
            {
                this.LogImport.Error(string.Empty, $"Не задан номер дома. Строка: {rowNumber}");
                return 0;
            }

            if (!this.fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
            {
                var errText = $"В справочнике муниципальных образований не найдена запись: {this.GetValue(row, "MUNICIPALITY").Trim()}";
                this.LogImport.Error(string.Empty, errText);
                return 0;
            }

            var municipality = this.fiasIdByMunicipalityNameDict[municipalityName];
            var fiasGuid = municipality.Key;

            if (string.IsNullOrWhiteSpace(fiasGuid))
            {
                this.LogImport.Error(string.Empty, $"Муниципальное образование не привязано к ФИАС. Строка: {rowNumber}");
                return 0;
            }

            if (!this.fiasHelper.HasBranch(fiasGuid))
            {
                this.LogImport.Error(string.Empty, $"В структуре ФИАС не найдена актуальная запись для муниципального образования. Строка: {rowNumber}");
                return 0;
            }

            var faultReason = string.Empty;
            DynamicAddress address;

            if (string.IsNullOrEmpty(record.StreetName) && !string.IsNullOrEmpty(record.House))
            {
                var addressKey = string.Format(
                    "{0}#{1}#{2}#{3}#{4}",
                    this.ToLowerTrim(municipalityName), 
                    this.ToLowerTrim(record.LocalityName),
                    " ",
                    this.ToLowerTrim(record.House),
                    this.ToLowerTrim(record.Housing));

                if (this.robjectByAddressCache.ContainsKey(addressKey)
                    && this.robjectByAddressCache[addressKey].Count == 1)
                {
                    return this.robjectByAddressCache[addressKey].RoId;
                }
            }

            if (!this.fiasHelper.FindInBranch(fiasGuid, record.LocalityName, record.StreetName, ref faultReason, out address))
            {
                this.LogImport.Error(string.Empty, string.Format("{0}. Строка: {1}", faultReason, rowNumber));
                return 0;
            }

            record.Address = address;
            record.MunicipalityId = municipality.Value;

            // Проверяем есть ли дома на данной улице
            if (this.realityObjectsByFiasGuidDict.ContainsKey(record.Address.GuidId))
            {
                // Составляем список домов на данной улице (!) с проверкой привязки МО (это не бред, после разделения одного МО многое может произойти)
                var existingRealityObjects = this.realityObjectsByFiasGuidDict[record.Address.GuidId].Where(x => x.MunicipalityId == record.MunicipalityId).ToList();

                if (existingRealityObjects.Any())
                {
                    var result = existingRealityObjects.Where(x => x.House == record.House).ToList();

                    result = string.IsNullOrEmpty(record.Housing)
                                 ? result.Where(x => string.IsNullOrEmpty(x.Housing)).ToList()
                                 : result.Where(x => x.Housing == record.Housing).ToList();

                    result = string.IsNullOrEmpty(record.Building)
                                 ? result.Where(x => string.IsNullOrEmpty(x.Building)).ToList()
                                 : result.Where(x => x.Building == record.Building).ToList();

                    result = string.IsNullOrEmpty(record.Liter)
                                 ? result.Where(x => string.IsNullOrEmpty(x.Liter)).ToList()
                                 : result.Where(x => x.Liter == record.Liter).ToList();

                    if (result.Count == 0)
                    {
                        this.LogImport.Error(
                            string.Empty,
                            $"В системе не найден дом, соответствующий записи. Строка: {rowNumber}");
                    }
                    else if (result.Count > 1)
                    {
                        this.LogImport.Error(
                            string.Empty,
                            $"В системе найдено несколько домов, соответствующих записи. Строка: {rowNumber}");
                    }
                    else
                    {
                        return result.First().RealityObjectId;
                    }
                }
                else
                {
                    this.LogImport.Error(
                        string.Empty,
                        $"У данного адреса другое муниципальное образование. Строка: {rowNumber}");
                }
            }

            return 0;
        }

        private void SaveResult()
        {
            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3").ExecuteUpdate();

                    // сохраняем новые записи
                    this.roSeForSave.ForEach(x => session.Insert(x));
                    this.stage3ForSave.ForEach(x => session.Insert(x));
                    this.stage2ForSave.ForEach(x => session.Insert(x));
                    this.stage1ForSave.ForEach(x => session.Insert(x));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void InitDictionaries()
        {
            this.fiasHelper = new FiasHelper(this.Container);

            this.realityObjectsByFiasGuidDict = this.FiasRepository.GetAll()
                .Join(this.FiasAddressDomain.GetAll(),
                    x => x.AOGuid,
                    y => y.StreetGuidId,
                    (a, b) => new { fias = a, fiasAddress = b }).ToList()
                .Join(this.RealityObjectDomain.GetAll(),
                    x => x.fiasAddress.Id,
                    y => y.FiasAddress.Id,
                    (c, d) => new { c.fias, c.fiasAddress, realityObject = d }).ToList()
                .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new RealtyObjectAddress
                {
                    AoGuid = x.fias.AOGuid,
                    Id = x.fiasAddress.Id,
                    House = x.fiasAddress.House,
                    Liter = x.fiasAddress.Letter,
                    Housing = x.fiasAddress.Housing,
                    Building = x.fiasAddress.Building,
                    RealityObjectId = x.realityObject.Id,
                    MunicipalityId = x.realityObject.Municipality.Id
                })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.AoGuid))
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.ToList());
            
            var municipalities = this.Container.Resolve<IRepository<Municipality>>().GetAll().ToArray();

            this.fiasIdByMunicipalityNameDict = municipalities
                .Where(x => this.Name != null)
                .Select(x => new { x.Name, x.FiasId, x.Id })
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper().Trim())
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var first = x.First();
                        return new KeyValuePair<string, long>(first.FiasId, first.Id);
                    });

            this.structElByString = this.Container.Resolve<IDomainService<StructuralElement>>()
                                        .GetAll()
                                        .AsEnumerable()
                                        .GroupBy(x => x.Group.CommonEstateObject.Name.Trim(' ').ToUpper())
                                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Name.Trim(' ').ToUpper()).ToDictionary(x => x.Key, z => z.First()));

            this.roSeByRealObj = this.RoSeDomain.GetAll()
                .Select(x => new {RoId = x.RealityObject.Id, RoSeId = x.Id, SeId = x.StructuralElement.Id})
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.SeId).ToDictionary(z => z.Key, x => x.Select(z => z.RoSeId).First()));

            this.addressByRoId = this.RealityObjectDomain.GetAll()
                                               .Select(x => new { x.Id, x.Address })
                                               .ToDictionary(x => x.Id, y => y.Address);

            var fiasCache = this.Container.ResolveRepository<Fias>().GetAll()
                .Select(x => new
                {
                    x.AOGuid,
                    x.ShortName,
                    x.FormalName,
                    x.ActStatus
                })
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .ToList()
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, y => y.First());

            this.robjectByAddressCache = this.RealityObjectDomain.GetAll()
               .Where(x => x.FiasAddress != null)
               .Select(x => new
               {
                   RoId = x.Id,
                   Mr = x.Municipality.Name.ToLower().Trim(),
                   Mu = x.MoSettlement.Name.ToLower().Trim(),
                   x.FiasAddress.PlaceGuidId,
                   x.FiasAddress.StreetGuidId,
                   House = x.FiasAddress.House.ToLower().Trim(),
                   Housing = x.FiasAddress.Housing.ToLower().Trim(),
                   Letter = x.FiasAddress.Letter.ToLower().Trim(),
                   Building = x.FiasAddress.Building.ToLower().Trim(),
               })
               .AsEnumerable()
               .Select(x => new
               {
                   Key = string.Format("{0}#{1}#{2}#{3}#{4}{5}{6}",
                       x.Mr,
                       x.PlaceGuidId.IsEmpty()
                           ? string.Empty
                           : fiasCache.Get(x.PlaceGuidId)
                               .Return(y =>
                                   "{0} {1}".FormatUsing(this.ToLowerTrim(y.FormalName), this.ToLowerTrim(y.ShortName))),
                       x.StreetGuidId.IsEmpty()
                           ? " "
                           : fiasCache.Get(x.StreetGuidId)
                               .Return(y =>
                                   "{0} {1}".FormatUsing(this.ToLowerTrim(y.FormalName), this.ToLowerTrim(y.ShortName))),
                       x.House,
                       x.Housing,
                       x.Letter,
                       x.Building),
                   x.RoId
               })
               .GroupBy(x => x.Key)
               .ToDictionary(x => x.Key, y => new RobjectProxy
               {
                   Count = y.Count(),
                   RoId = y.First().RoId
               });
        }

        private string ToLowerTrim(string value)
        {
            return (value ?? string.Empty).ToLower().Trim(' ', '.');
        }

        private void InitHeaders()
        {
            this.headersDict["MUNICIPALITY"] = 1;
            this.headersDict["LOCALITYNAME"] = 2;
            this.headersDict["STREETNAME"] = 3;
            this.headersDict["HOUSE"] = 4;
            this.headersDict["LITER"] = 5;
            this.headersDict["HOUSING"] = 6;
            this.headersDict["BUILDING"] = 7;
            this.headersDict["CEO"] = 15;
            this.headersDict["STRUCTEL"] = 16;
            this.headersDict["VOLUME"] = 18;
            this.headersDict["LASTOVERHAULYEAR"] = 19;
            this.headersDict["SUM"] = 23;
            this.headersDict["FIRSTYEAR"] = 27;
            this.headersDict["SECONDYEAR"] = 28;
            this.headersDict["THIRDYEAR"] = 29;
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (this.headersDict.ContainsKey(field))
            {
                var index = this.headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }

        /// <summary>
        /// Returns a string that has space removed from the start and the end, and that has each sequence of internal space replaced with a single space.
        /// </summary>
        /// <param name="initialString"></param>
        /// <returns></returns>
        private static string Simplified(string initialString)
        {
            if (string.IsNullOrEmpty(initialString))
            {
                return initialString;
            }

            var trimmed = initialString.Trim();

            if (!trimmed.Contains(" "))
            {
                return trimmed;
            }

            var result = string.Join(" ", trimmed.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)));

            return result;
        }

        private MemoryStream GetFile(FileData fileData)
        {
            if (fileData.Extention == "xls")
            {
                return new MemoryStream(fileData.Data);
            }

            var result = new MemoryStream();

            if (fileData.Extention == "zip")
            {
                using (var zipFile = ZipFile.Read(new MemoryStream(fileData.Data)))
                {
                    var xlsFile = zipFile.FirstOrDefault(x => x.FileName.EndsWith(".xls"));
                    if (xlsFile != null)
                    {
                        xlsFile.Extract(result);
                        result.Seek(0, SeekOrigin.Begin);
                     }
                }
            }

            return result;
        }
    }
}