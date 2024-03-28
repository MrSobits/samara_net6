using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FIAS;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums.Import;
using Bars.Gkh.Import;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.DomainService;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.GkhExcel;
using Castle.Windsor;
using Ionic.Zip;

namespace Bars.Gkh.Overhaul.Hmao.Import.ImportDpkr
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import.Impl;
    using Gkh.Utils;

    class Dpkr1cImport : GkhImportBase
    {
        public virtual IWindsorContainer Container { get; set; }

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key
        {
            get { return Id; }
        }
        public override string CodeImport
        {
            get { return "DpkrImport"; }
        }
        public override string Name
        {
            get { return "Импорт ДПКР из 1С"; }
        }
        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }
        public override string PermissionName
        {
            get { return "Import.Dpkr1C.View"; }
        }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogManager { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoSeDomain { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public IStateProvider StateProvider { get; set; }

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        private readonly Dictionary<StructuralElement, int> structHeadersDict = new Dictionary<StructuralElement, int>();

        private  Dictionary<string, StructuralElement> structElByString;

        private Dictionary<long, string> addressByRoId;

        private Dictionary<string, long> addressByRoAddress;

        private Dictionary<long, RealityObject> realityObjDict;

        private Dictionary<long, Dictionary<long, long>> roSeByRealObj;

        private List<RealityObjectStructuralElement> roSeForSave = new List<RealityObjectStructuralElement>();

        private List<RealityObjectStructuralElementInProgramm> stage1ForSave = new List<RealityObjectStructuralElementInProgramm>();

        private List<RealityObjectStructuralElementInProgrammStage2> stage2ForSave = new List<RealityObjectStructuralElementInProgrammStage2>();

        private List<RealityObjectStructuralElementInProgrammStage3> stage3ForSave = new List<RealityObjectStructuralElementInProgrammStage3>();
        public ILongProgramService LongProgramService { get; set; }
        private bool addStructEl = false;
        private State defaultRoSeState = null;

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

                var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
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
                Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }


        private void InitHeaders()
        {
            headersDict["ROID"] = 0;
            headersDict["ADDRESS"] = 1;
            headersDict["NUM"] = 2;
            headersDict["POINT"] = 3;
        }
        
        private void InitDictionaries()
        {
            //_fiasHelper = new FiasHelper(Container);

            var roSe = new RealityObjectStructuralElement();

            StateProvider.SetDefaultState(roSe);

            defaultRoSeState = roSe.State;

            realityObjDict = RealityObjectDomain.GetAll()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            structElByString = Container.Resolve<IDomainService<StructuralElement>>()
                                        .GetAll()
                                        .AsEnumerable()
                                        .GroupBy(x => x.Name.Trim(' ').ToUpper())
                                        .ToDictionary(x => x.Key, z => z.First());

            roSeByRealObj = RoSeDomain.GetAll()
                .Select(x => new { RoId = x.RealityObject.Id, RoSeId = x.Id, SeId = x.StructuralElement.Id })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.SeId).ToDictionary(z => z.Key, x => x.Select(z => z.RoSeId).First()));

            addressByRoId = RealityObjectDomain.GetAll()
                                               .Select(x => new { x.Id, x.Address })
                                               .ToDictionary(x => x.Id, y => y.Address);

            this.addressByRoAddress = this.RealityObjectDomain.GetAll()
                                   .Select(x => new { x.Id, x.Address })
                                   .AsEnumerable()
                                   .GroupBy(x => x.Address)
                                   .ToDictionary(x => x.Key, y => y.First().Id);
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];
            addStructEl = baseParams.Params.GetAs<bool>("AddStructEl");

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;

           InitHeaders();

           InitDictionaries();

            using (var memoryStreamFile = GetFile(fileData))
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
                        if (rows[i][0].Value != "ID жилого дома" && !startImport)
                        {
                            continue;
                        }

                        if (rows[i][0].Value == "ID жилого дома" && !startImport)
                        {
                            startImport = true;

                            FillCeoHeaders(rows[i]);

                            i++;
                        }

                        if (startImport)
                        {
                            ImportRow(rows[i], i + 1, startYear, endYear);
                        }
                    }

                    if (!startImport)
                    {
                        this.LogImport.Error("Ошибка", "В исходном файле отсутствует обязательный заголовок: \"ID жилого дома\"");
                    }

                    LogImport.IsImported = true;
                }
            }

            SaveResult();

            LogImport.SetFileName(fileData.FileName);
            LogImport.ImportKey = CodeImport;

            LogManager.FileNameWithoutExtention = fileData.FileName;
            LogManager.Add(fileData, LogImport);
            LogManager.Save();

            var message = string.Format(
                " Загружено строк: {0}. Ошибки: {1}", LogImport.CountAddedRows, LogImport.CountError);

            return new ImportResult(LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError,
                message, string.Empty, LogManager.LogFileId);
        }

        private void AddDpkrRow(GkhExcelCell[] row, int year, long realObjId, StructuralElement structEl, RealityObjectStructuralElement roSe)
        {
            var ro = realityObjDict.ContainsKey(realObjId) ? realityObjDict[realObjId] : null;

            var num = GetValue(row, "NUM").ToInt();
            var point = GetValue(row, "POINT").ToInt();

            if (ro == null)
            {
                return;
            }

            var sum = LongProgramService.GetDpkrCost(ro.Municipality.Id,
                                                     ro.MoSettlement.ReturnSafe(x => x.Id),
                                                        year,
                                                        structEl.Id,
                                                        ro.Id,
                                                        ro.CapitalGroup != null ? ro.CapitalGroup.Id : 0,
                                                        structEl.CalculateBy,
                                                        0m,
                                                        ro.AreaLiving,
                                                        ro.AreaMkd,
                                                        ro.AreaLivingNotLivingMkd);

            if (!sum.HasValue || sum == 0)
            {
                LogImport.Warn("Отсутствуют расценки", string.Format("Отсутствуют расценки: {0}, адрес: {1}", structEl.Name, GetValue(row, "ADDRESS")));
                sum = 0;
            }

            var stage3 = new RealityObjectStructuralElementInProgrammStage3
            {
                RealityObject = new RealityObject { Id = realObjId },
                CommonEstateObjects = structEl.Group.CommonEstateObject.Name,
                Year = year,
                Sum = sum.Value,
                IndexNumber = num,//row[0].Value.ToInt(),
                Point = point
            };

            stage3ForSave.Add(stage3);

            var stage2 = new RealityObjectStructuralElementInProgrammStage2
            {
                RealityObject = new RealityObject { Id = realObjId },
                Year = year,
                Sum = sum.Value,
                CommonEstateObject = structEl.Group.CommonEstateObject,
                StructuralElements = structEl.Name,
                Stage3 = stage3
            };

            stage2ForSave.Add(stage2);

            var stage1 = new RealityObjectStructuralElementInProgramm
            {
                StructuralElement = roSe,
                Year = year,
                Sum = sum.Value,
                ServiceCost = 0,
                Stage2 = stage2
            };

            stage1ForSave.Add(stage1);

            LogImport.CountAddedRows++;

            LogImport.Info(string.Empty, string.Format("Добавлена запись в ДПКР. Адрес: {0}, ООИ: {1}, Конструктивный элемент: {2}, Год: {3}",
                                                        GetValue(row, "ADDRESS"),
                                                        structEl.Group.CommonEstateObject.Name,
                                                        structEl.Name,
                                                        year));
        }

        private void FillCeoHeaders(GkhExcelCell[] row)
        {
            var i = headersDict.Values.Max() + 1;

            for (; i < row.Length; i++)
            {
                var structElName = row[i].Value;
                var trimStructElName = structElName.Trim().ToUpper();

                if (!structElByString.ContainsKey(trimStructElName))
                {
                    LogImport.Error("Ошибка", string.Format("Не найден конструктивный элемент: {0}", structElName));
                    continue;
                }

                var structEl = structElByString[trimStructElName];


                structHeadersDict.Add(structEl, i);
            }
        }

        private void SaveResult()
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2").ExecuteUpdate();
                    session.CreateSQLQuery("DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3").ExecuteUpdate();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, roSeForSave, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(Container, stage3ForSave, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(Container, stage2ForSave, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(Container, stage1ForSave, 10000, true, true);
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (headersDict.ContainsKey(field))
            {
                var index = headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result != null ? result.Trim() : null;
        }

        private void ImportRow(GkhExcelCell[] row, int rowNumber, int startYear, int endYear)
        {
            var realObjId = GetValue(row, "ROID").ToLong();
            var realObjAddr = GetValue(row, "ADDRESS");

            if (realObjId == 0 && !this.addressByRoAddress.TryGetValue(realObjAddr, out realObjId))
            {
                LogImport.Warn("Предупреждение", string.Format("Не задан либо не найден ID дома, строка: {0}, адрес: {1}", rowNumber, realObjAddr));
                return;
            }

            if (!addressByRoId.ContainsKey(realObjId))
            {
                LogImport.Error("Ошибка", string.Format("Не найден дом по ID или адресу: {0}, адрес: {1}", realObjId, realObjAddr));
                return;
            }

            var roSeElements = roSeByRealObj.ContainsKey(realObjId) ? roSeByRealObj[realObjId] : new Dictionary<long, long>();

            if (!addStructEl && roSeElements.Count == 0)
            {
                LogImport.Error("Ошибка", string.Format("У дома отсутствуют конструктивные элементы, id: {0}, адрес: {1}", realObjId, realObjAddr));
                return;
            }

            foreach (var seHeader in structHeadersDict)
            {
                var strElem = seHeader.Key;
                var index = seHeader.Value;

                var yearValue = row[index].Value.Length > 4
                    ? row[index].Value.ToDateTime().Year
                    : row[index].Value.ToInt();

                if (yearValue > 0)
                {
                    if (roSeElements.ContainsKey(strElem.Id))
                    {
                        if (yearValue >= startYear && yearValue <= endYear)
                        {
                            AddDpkrRow(row, yearValue, realObjId, strElem, new RealityObjectStructuralElement{ Id = roSeElements[strElem.Id] });
                        }
                        else
                        {
                            LogImport.Warn("Предупреждение", string.Format("Плановый год не входит в период программы {4}-{5}, дом id: {0}, адрес: {1}, элемент: {2}, год: {3}", realObjId, realObjAddr, strElem.Name, yearValue, startYear, endYear));
                        }
                    }
                    else
                    {
                        if (addStructEl)
                        {
                            var newRoSe = new RealityObjectStructuralElement
                            {
                                RealityObject = new RealityObject{ Id = realObjId },
                                StructuralElement = strElem,
                                State = defaultRoSeState
                            };

                            roSeForSave.Add(newRoSe);

                            AddDpkrRow(row, yearValue, realObjId, strElem, newRoSe);

                            LogImport.Debug("Информация", string.Format("В дом добавлен конструктивный элемент: {0}, id: {1}, адрес: {2}", strElem.Name, realObjId, realObjAddr));
                        }
                        else
                        {
                            LogImport.Error("Ошибка", string.Format("У дома отсутствует конструктивный элемент: {0}, id: {1}, адрес: {2}", strElem.Name, realObjId, realObjAddr));
                        }                     
                    }
                }
            }
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
