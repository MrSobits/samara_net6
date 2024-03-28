namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using B4.Modules.FIAS;
    using B4.Utils;

    using Bars.B4.IoC;

    using Castle.Windsor;
    using Distribution;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Enums.Import;
    using Gkh.Utils;
    using GkhExcel;
    using Import;
    using Import.Impl;
    using Overhaul.Import.CommonRealtyObjectImport;

    public class SubsidyIncomeImport : GkhImportBase
    {
        public virtual IWindsorContainer Container { get; set; }

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key => SubsidyIncomeImport.Id;

        public override string CodeImport => "SubsidyIncomeImport";

        public override string Name => "Импорт реестра субсидий";

        public override string PossibleFileExtensions => "xls,xlsx";

        public override string PermissionName => "Import.SubsidyIncome";

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogManager { get; set; }

        public IDomainService<SubsidyIncome> SubsidyIncomeDomain { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<FiasAddress> FiasAddressRepository { get; set; }

        public IRepository<Fias> fiasRepo { get; set; }

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        private Dictionary<string, List<RealtyObjectAddress>> realityObjectsByFiasGuidDict;

        private Dictionary<string, KeyValuePair<string, long>> fiasIdByMunicipalityNameDict;

        private Dictionary<long, string> addressByRoId;

        private FiasHelper _fiasHelper;

        private Dictionary<string, RobjectProxy> _robjectByAddressCache;

        private HashSet<long> _realObjIds;

        private HashSet<string> distrsCodes;

        private List<SubsidyIncomeDetail> subsidyIncomeDetailsList = new List<SubsidyIncomeDetail>();

        private class RobjectProxy
        {
            public int Count { get; set; }

            public long roId { get; set; }
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                message = null;
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

                using (var memoryStreamFile = new MemoryStream(bytes))
                {
                    var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                    using (this.Container.Using(excel))
                    {                           
                        if (excel == null)
                        {
                            throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                        }

                        if (extention == "xlsx")
                        {
                            excel.UseVersionXlsx();
                        }

                        excel.Open(memoryStreamFile);
                    }
                }

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

            this.InitHeaders();

            this.InitDictionaries();

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    if (fileData.Extention == "xlsx")
                    {
                        excel.UseVersionXlsx();
                    }

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);
                    var startImport = false;

                    var subsidyIncome = new SubsidyIncome();
                    for (var i = 0; i < rows.Count; i++)
                    {
                        if (rows[i][0].Value != "Номер п/п" && !startImport)
                        {
                            continue;
                        }

                        if (rows[i][0].Value == "Номер п/п" && !startImport)
                        {
                            startImport = true;
                            continue;
                        }

                        if (startImport && rows[i][0].Value.IsNotEmpty())
                        {
                            this.ImportRow(rows[i], i + 1, subsidyIncome);
                            this.LogImport.CountAddedRows++;
                        }
                    }

                    subsidyIncome.DetailsCount = this.subsidyIncomeDetailsList.Count();
                    subsidyIncome.DateReceipt = DateTime.Now;
                    subsidyIncome.MoneyDirection = MoneyDirection.Income;
                    subsidyIncome.Sum = this.subsidyIncomeDetailsList.SafeSum(x => x.Sum);
                    subsidyIncome.TypeSubsidyDistr = this.subsidyIncomeDetailsList.Select(x => x.TypeSubsidyDistr).Distinct().AggregateWithSeparator(",");
                    subsidyIncome.SubsidyIncomeDefineType = this.subsidyIncomeDetailsList.Any(x => x.RealityObject != null) ? this.subsidyIncomeDetailsList.All(x => x.RealityObject != null) ? SubsidyIncomeDefineType.Defined : SubsidyIncomeDefineType.PartiallyDefined : SubsidyIncomeDefineType.NotDefined;

                    if (this.LogImport.CountError == 0) 
                    {
                        this.SubsidyIncomeDomain.Save(subsidyIncome);
                        TransactionHelper.InsertInManyTransactions(this.Container, this.subsidyIncomeDetailsList, 1000, true, true);
                    }

                    this.LogImport.IsImported = true;
                }
            }

            this.LogImport.SetFileName(fileData.FileName);
            this.LogImport.ImportKey = this.CodeImport;

            this.LogManager.FileNameWithoutExtention = fileData.FileName;
            this.LogManager.Add(fileData, this.LogImport);
            this.LogManager.Save();

            var message = string.Format(
                " Загружено строк: {0}. Ошибки: {1}", this.LogImport.CountError > 0 ? 0 : this.LogImport.CountAddedRows, this.LogImport.CountError);

            return new ImportResult(this.LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError,
                message,
                string.Empty, this.LogManager.LogFileId);
        }

        private void ImportRow(GkhExcelCell[] row, int rowNumber, SubsidyIncome subsidyIncome)
        {
            var subsidyIncomeDetail = new SubsidyIncomeDetail
            {
                SubsidyIncome = subsidyIncome
            };

            var realObjId = this.GetRealObjId(row, rowNumber, subsidyIncomeDetail);

            if (realObjId > 0)
            {
                subsidyIncomeDetail.IsConfirmed = false;
                subsidyIncomeDetail.RealityObject = new RealityObject {Id = realObjId};
            }
            else
            {
                if (subsidyIncomeDetail.RealObjId == 0 && subsidyIncomeDetail.RealObjAddress.IsEmpty())
                {
                    return;
                }
            }

            var dateReceipt = this.GetValue(row, "DATERECEIPT").ToDateTime();
            var subsidyType = this.GetValue(row, "SUBSIDYTYPE");
            var sum = this.GetValue(row, "SUM").ToDecimal();

            if (dateReceipt == DateTime.MinValue)
            {
                this.LogImport.Error(string.Empty, string.Format("Не указана дата поступления. Строка: {0}", rowNumber));
            }

            if (sum < 0)
            {
                this.LogImport.Error(string.Empty, string.Format("Сумма должна быть больше нуля. Строка: {0}", rowNumber));
            }

            if (!this.distrsCodes.Contains(subsidyType))
            {
                this.LogImport.Error(string.Empty, string.Format("Не удалось определить тип субсилирования. Строка: {0}", rowNumber));
            }

            subsidyIncomeDetail.DateReceipt = dateReceipt;
            subsidyIncomeDetail.TypeSubsidyDistr = subsidyType;
            subsidyIncomeDetail.Sum = sum;

            this.subsidyIncomeDetailsList.Add(subsidyIncomeDetail);
        }

        private long GetRealObjId(GkhExcelCell[] row, int rowNumber, SubsidyIncomeDetail subsidyIncomeDetail)
        {
            var record = new Record { RowNumber = rowNumber };

            if (row.Length <= 1)
            {
                return 0;
            }

            var roId = this.GetValue(row, "IDDOMA").ToLong();
            subsidyIncomeDetail.RealObjId = roId;

            if (roId > 0 && this._realObjIds.Contains(roId))
            {
                return roId;
            }

            record.LocalityName = SubsidyIncomeImport.Simplified(this.GetValue(row, "LOCALITYNAME"));
            record.StreetName = SubsidyIncomeImport.Simplified(this.GetValue(row, "STREETNAME"));
            record.Liter = SubsidyIncomeImport.Simplified(this.GetValue(row, "LITER"));
            record.Housing = this.GetValue(row, "HOUSING");
            record.Building = SubsidyIncomeImport.Simplified(this.GetValue(row, "BUILDING"));
            record.House = SubsidyIncomeImport.Simplified(this.GetValue(row, "HOUSE"));

            var municipalityName = this.GetValue(row, "MUNICIPALITY").ToUpper();
            if (string.IsNullOrEmpty(municipalityName))
            {
                return 0;
            }

            subsidyIncomeDetail.RealObjAddress = this.CreateAddress(municipalityName, record);

            if (record.House.IsEmpty())
            {
                this.LogImport.Warn(string.Empty, string.Format("Не задан номер дома. Строка: {0}", rowNumber));
                return 0;
            }

            if (!this.fiasIdByMunicipalityNameDict.ContainsKey(municipalityName))
            {
                var errText = string.Format("В справочнике муниципальных образований не найдена запись: {0}", this.GetValue(row, "MUNICIPALITY").Trim());
                this.LogImport.Warn(string.Empty, errText);
                return 0;
            }

            var municipality = this.fiasIdByMunicipalityNameDict[municipalityName];
            var fiasGuid = municipality.Key;

            if (string.IsNullOrWhiteSpace(fiasGuid))
            {
                this.LogImport.Warn(string.Empty, string.Format("Муниципальное образование не привязано к ФИАС. Строка: {0}", rowNumber));
                return 0;
            }

            if (!this._fiasHelper.HasBranch(fiasGuid))
            {
                this.LogImport.Warn(string.Empty, string.Format("В структуре ФИАС не найдена актуальная запись для муниципального образования. Строка: {0}", rowNumber));
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
                    " ", this.ToLowerTrim(record.House),
                    this.ToLowerTrim(record.Housing));

                if (this._robjectByAddressCache.ContainsKey(addressKey)
                    && this._robjectByAddressCache[addressKey].Count == 1)
                {
                    return this._robjectByAddressCache[addressKey].roId;
                }
            }

            if (!this._fiasHelper.FindInBranch(fiasGuid, record.LocalityName, record.StreetName, ref faultReason, out address))
            {
                this.LogImport.Warn(string.Empty, string.Format("{0}. Строка: {1}", faultReason, rowNumber));
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
                        this.LogImport.Warn(
                            string.Empty,
                            string.Format("В системе не найден дом, соответствующий записи. Строка: {0}", rowNumber));
                    }
                    else if (result.Count > 1)
                    {
                        this.LogImport.Warn(
                            string.Empty,
                            string.Format("В системе найдено несколько домов, соответствующих записи. Строка: {0}", rowNumber));
                    }
                    else
                    {
                        return result.First().RealityObjectId;
                    }
                }
                else
                {
                    this.LogImport.Warn(
                        string.Empty,
                        string.Format("У данного адреса другое муниципальное образование. Строка: {0}", rowNumber));
                }
            }

            return 0;
        }

        private void InitDictionaries()
        {
            var fiasAddressDomain = this.Container.ResolveDomain<FiasAddress>();
            var muRepository = this.Container.ResolveRepository<Municipality>();
            var distrs = this.Container.ResolveAll<IDistribution>();

            try
            {
                this._fiasHelper = new FiasHelper(this.Container);


                var fiasAddressList = this.FiasAddressRepository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.House,
                        x.Housing,
                        x.Building,
                        x.Letter,
                        x.StreetGuidId
                    })
                    .ToList();
                var roList = this.RealityObjectRepository.GetAll()
                    .Select(x => new
                    {
                        FiasAddressId = x.FiasAddress.Id,
                        x.Id,
                        MunicipalityId = x.Municipality.Id
                    })
                    .ToList();
                this.realityObjectsByFiasGuidDict = this.fiasRepo.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .WhereNotEmptyString(x => x.AOGuid)
                    .Select(x => x.AOGuid)
                    .ToList()
                    .Join(fiasAddressList,
                        x => x,
                        y => y.StreetGuidId,
                        (a, b) => new { aoGuid = a, fiasAddress = b })
                    .Join(roList,
                        x => x.fiasAddress.Id,
                        y => y.FiasAddressId,
                        (c, d) => new { c.aoGuid, c.fiasAddress, realityObject = d })
                    .Select(x => new RealtyObjectAddress
                    {
                        AoGuid = x.aoGuid,
                        Id = x.fiasAddress.Id,
                        House = x.fiasAddress.House,
                        Housing = x.fiasAddress.Housing,
                        Building = x.fiasAddress.Building,
                        RealityObjectId = x.realityObject.Id,
                        MunicipalityId = x.realityObject.MunicipalityId,
                        Liter = x.fiasAddress.Letter
                    })
                    .GroupBy(x => x.AoGuid)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var municipalities = muRepository.GetAll().ToArray();

                this.fiasIdByMunicipalityNameDict = municipalities
                    .Where(x => this.Name != null)
                    .Select(x => new {x.Name, x.FiasId, x.Id})
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToUpper().Trim())
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var first = x.First();
                            return new KeyValuePair<string, long>(first.FiasId, first.Id);
                        });

                this.addressByRoId = this.RealityObjectRepository.GetAll()
                    .Select(x => new {x.Id, x.Address})
                    .ToDictionary(x => x.Id, y => y.Address);

                var fiasCache = this.fiasRepo.GetAll()
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

                this._robjectByAddressCache = this.RealityObjectRepository.GetAll()
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
                        roId = y.First().RoId
                    });

                this._realObjIds = this.RealityObjectRepository.GetAll()
                    .Select(x => x.Id)
                    .ToHashSet();

                this.distrsCodes = distrs
                    .Where(x => x.Code.Contains("Subsidy"))
                    .Select(x => x.Code)
                    .ToHashSet();

            }
            finally
            {
                this.Container.Release(fiasAddressDomain);
                this.Container.Release(muRepository);
                this.Container.Release(distrs);
            }
        }

        private string ToLowerTrim(string value)
        {
            return (value ?? string.Empty).ToLower().Trim(' ', '.');
        }

        private void InitHeaders()
        {
            this.headersDict["IDDOMA"] = 1;
            this.headersDict["MUNICIPALITY"] = 2;
            this.headersDict["SETTLEMENT"] = 3;
            this.headersDict["LOCALITYNAME"] = 4;
            this.headersDict["STREETNAME"] = 5;
            this.headersDict["HOUSE"] = 6;
            this.headersDict["LITER"] = 7;
            this.headersDict["HOUSING"] = 8;
            this.headersDict["BUILDING"] = 9;
            this.headersDict["DATERECEIPT"] = 10;
            this.headersDict["SUBSIDYTYPE"] = 11;
            this.headersDict["SUM"] = 12;
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

        private string CreateAddress(string municipality, Record record)
        {
            var addressName = new StringBuilder();

            addressName.Append(municipality);


            if (record.LocalityName.IsNotEmpty())
            {  
                var localityArray = record.LocalityName.Split(' ');
                if (localityArray.Length > 1)
                {
                    addressName.AppendFormat(", {0}. ",localityArray.Last());

                    localityArray[localityArray.Length - 1] = string.Empty;
                    addressName.Append(localityArray.AggregateWithSeparator(" "));
                }
            }

            if (record.StreetName.IsNotEmpty())
            {  
                var streetArray = record.StreetName.Split(' ');
                if (streetArray.Length > 1)
                {
                    addressName.AppendFormat(", {0}. ", streetArray.Last());

                    streetArray[streetArray.Length - 1] = string.Empty;
                    addressName.Append(streetArray.AggregateWithSeparator(" "));
                }
            }

            if (!string.IsNullOrEmpty(record.House))
            {
                addressName.Append(", д. ");
                addressName.Append(record.House);

                if (!string.IsNullOrEmpty(record.Housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(record.Housing);

                    if (!string.IsNullOrEmpty(record.Building))
                    {
                        addressName.Append(", секц. ");
                        addressName.Append(record.Building);
                    }
                }
            }

            return addressName.ToString();
        }
    }
}