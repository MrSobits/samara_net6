namespace Bars.Gkh.Overhaul.Nso.Import.Program
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhExcel;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Gkh.Import.Impl;

    using Castle.Windsor;
    using NHibernate.Linq;

    public sealed class KpkrProgramImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IWindsorContainer Container { get; set; }

        public IRepository<RealityObject> RoRepo { get; set; }

        public IRepository<VersionRecordStage2> Version2Repo { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<Period> PeriodDomain { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "NsoKpkrImport"; }
        }

        public override string Name
        {
            get { return "Импорт программы краткосрочного капитального ремонта"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.KpkrForNsk.View"; }
        }

        public ILogImportManager LogManager { get; set; }

        public class ParsedRow
        {
            public int Number { get; set; }
            public string MO { get; set; }
            public string Area { get; set; }
            public string Street { get; set; }
            public string BuildingNum { get; set; }
            public string KorpNum { get; set; }
            public int OOIPercent { get; set; }
            public int EnterDate { get; set; }
            public string Work { get; set; }
            public int PlanFixYear { get; set; }
            public string LastFixYear { get; set; }
            public int LimitFixYear { get; set; }
            public decimal PriceForOOI { get; set; }
            public decimal KrMoney { get; set; }
            public decimal LocalMoney { get; set; }
            public decimal OblastMoney { get; set; }
            public decimal GkhMoney { get; set; }
            public decimal OtherMoney { get; set; }
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];

            var roCache = RoRepo.GetAll().Select(x => new
            {
                x.Id,
                x.FiasAddress.House,
                x.FiasAddress.Housing,
                x.FiasAddress.StreetName,
                x.FiasAddress.PlaceName
            }).ToList();

            var roDict = RoRepo.GetAll().ToDictionary(x => x.Id, x => x);

            var stage2Dict = Version2Repo.GetAll()
                .Fetch(x => x.CommonEstateObject)
                .Fetch(x => x.Stage3Version)
                .ThenFetch(x => x.RealityObject)
                .Select(x => new
                {
                    RoId = x.Stage3Version.RealityObject.Id,
                    CommonEstateId = x.CommonEstateObject.Id,
                    CommonEstateName = x.CommonEstateObject.Name,
                    x.Id
                })
                .AsEnumerable()
                .GroupBy(x => new { x.RoId, x.CommonEstateName }, x => x)
                .ToDictionary(x => "{0}#{1}".FormatUsing(x.Key.RoId, x.Key.CommonEstateName.ToLower()), x => x.First());

            var helper = new RealityObjectWorkHelper(Container);
            helper.InitDictionaries();

            /*var period = PeriodDomain.GetAll().FirstOrDefault(x => x.Name.ToLower() == "КПКР 2014 г.");
            if (period == null)
            {
                period = new Period
                {
                    Name = "КПКР 2014 г.",
                    DateStart = new DateTime(2014, 01, 01)
                };

                PeriodDomain.Save(period);
            }

            var program = new ProgramCr
            {
                Code = string.Empty,
                Description = "Импорт " + fileData.FileName + " от " + DateTime.Today.ToShortDateString(),
                Name = "Импорт " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Period = period,
                TypeProgramCr = TypeProgramCr.Primary,
                TypeProgramStateCr = TypeProgramStateCr.New,
                TypeVisibilityProgramCr = TypeVisibilityProgramCr.Full
            };

            ProgramCrDomain.Save(program);*/

            var program = ProgramCrDomain.GetAll().OrderByDescending(x => x.Id).First();
            var stateProvider = Container.Resolve<IStateProvider>();

            var objectCrDict = ObjectCrDomain.GetAll().Where(x => x.ProgramCr.Id == program.Id)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x.First());

            var listToSave = new List<ShortProgramRecord>();
            var objectCrToSave = new List<ObjectCr>();
            var typeWorkToSave = new List<TypeWorkCr>();
            var finResToSave = new List<FinanceSourceResource>();

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                using (var xlsMemoryStream = new MemoryStream(fileData.Data))
                {
                    excel.UseVersionXlsx();
                    excel.Open(xlsMemoryStream);

                    var dict = new Dictionary<string, string>();
                    var data = excel.GetRows(0, 0);

                    var startIndex = 0;
                    string[] header = null;

                    var rowNum = 0;
                    do
                    {
                        var row = data[rowNum].Where(x => x.Value != null).Select(x => x.Value.ToLower()).ToArray();

                        if (row.Contains("1"))
                        {
                            startIndex = rowNum + 1;
                            header = row;
                        }

                        rowNum++;
                    } while (startIndex == 0 && rowNum + 1 != data.Count);

                    if (startIndex == 0)
                    {
                        throw new ArgumentException("Формат шаблона не соответсвует импорту!\nНе найден столбец \"1\"");
                    }

                    for (var i = startIndex; i < data.Count; i++)
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
                            Number = dict.Get("1").ToInt(),
                            MO = dict.Get("2"),
                            Area = dict.Get("4"),
                            Street = FormatAddressPart(dict.Get("5")),
                            BuildingNum = dict.Get("6"),
                            KorpNum = dict.Get("7"),
                            OOIPercent = dict.Get("8").ToInt(),
                            EnterDate = dict.Get("9").ToInt(),
                            Work = dict.Get("10"),
                            PlanFixYear = dict.Get("11").ToInt(),
                            LastFixYear = dict.Get("12"),
                            LimitFixYear = dict.Get("13").ToInt(),
                            PriceForOOI = dict.Get("14").ToDecimal(),
                            KrMoney = dict.Get("15").ToDecimal(),
                            LocalMoney = dict.Get("16").ToDecimal(),
                            OblastMoney = dict.Get("17").ToDecimal(),
                            GkhMoney = dict.Get("18").ToDecimal(),
                            OtherMoney = dict.Get("19").ToDecimal()
                        };

                        entity.Area = entity.Area
                           .Replace("город", string.Empty)
                           .Replace("рабочий поселок", string.Empty)
                           .Replace("дачный поселок", string.Empty)
                           .Replace("поселок", string.Empty)
                           .Replace("железнодорожная станция ", string.Empty)
                           .Replace("село", string.Empty)
                           .Trim();

                        if (entity.Number == 0)
                        {
                            LogImport.Error(
                                "Строка не добавлена",
                                string.Format("Для строки {0} не найден порядковый номер", i + 1));
                            continue;
                        }

                        var roId =
                            roCache.FirstOrDefault(
                                x => x.House.ToLower() == entity.BuildingNum.ToLower() &&
                                     x.Housing == entity.KorpNum &&
                                     x.PlaceName.ToLower().Contains(entity.Area.ToLower()) &&
                                     x.StreetName.ToLower().Contains(entity.Street.ToLower()));

                        if (roId == null)
                        {
                            LogImport.Error(
                                "Строка не добавлена",
                                string.Format("Для строки {0} не найден дом", i + 1));
                            continue;
                        }

                        var ro = roDict[roId.Id];

                        var stage2Key = "{0}#{1}".FormatUsing(ro.Id, entity.Work.ToLower());
                        var stage2 = stage2Dict.ContainsKey(stage2Key) ? stage2Dict[stage2Key] : null;
                        if (stage2 == null)
                        {
                            LogImport.Error(
                                "Строка не добавлена",
                                string.Format("Для строки {0} не найден ДПКР по ключу {1}", i + 1, stage2Key));
                            continue;
                        }

                        var shortProgramRecord = new ShortProgramRecord
                        {
                            BudgetFcr = entity.GkhMoney,
                            BudgetMunicipality = entity.LocalMoney,
                            BudgetOtherSource = entity.OtherMoney,
                            BudgetRegion = entity.OblastMoney,
                            Difitsit = Math.Max(0, entity.PriceForOOI - entity.GkhMoney - entity.KrMoney - entity.LocalMoney - entity.OblastMoney - entity.OtherMoney),
                            OwnerSumForCr = entity.KrMoney,
                            Year = entity.PlanFixYear,
                            RealityObject = ro,
                            Stage2 = new VersionRecordStage2
                            {
                                Id = stage2.Id
                            }
                        };

                        listToSave.Add(shortProgramRecord);

                        ObjectCr objectCr;
                        if (objectCrDict.ContainsKey(ro.Id))
                        {
                            objectCr = objectCrDict[ro.Id];
                        }
                        else
                        {
                            objectCr = new ObjectCr(ro, program);
                            objectCrToSave.Add(objectCr);

                            objectCrDict.Add(ro.Id, objectCr);
                        }

                        string error;
                        var info = helper.GetWorkInfo(stage2.CommonEstateId, ro.Id, out error);
                        if (info == null)
                        {
                            LogImport.Error("Виды работы и фин. источники не добавлены", "Строка {0}. Причина: {1}".FormatUsing(i + 1, error));
                            continue;
                        }

                        var typeWork = new TypeWorkCr
                        {
                            ObjectCr = objectCr,
                            IsActive = true,
                            Work = new Work
                            {
                                Id = info.WorkId
                            },
                            YearRepair = entity.PlanFixYear,
                            Sum = entity.PriceForOOI,
                            FinanceSource = new FinanceSource
                            {
                                Id = info.FinSourceId
                            }
                        };

                        typeWorkToSave.Add(typeWork);

                        var financeSource = new FinanceSourceResource
                        {
                            BudgetMu = entity.LocalMoney,
                            BudgetSubject = entity.OblastMoney,
                            FundResource = entity.GkhMoney,
                            OwnerResource = entity.KrMoney,
                            ObjectCr = objectCr,
                            TypeWorkCr = typeWork,
                            Year = entity.PlanFixYear,
                            FinanceSource = new FinanceSource
                            {
                                Id = info.FinSourceId
                            }
                        };

                        finResToSave.Add(financeSource);
                    }
                }
            }

            var shift = 100;
            var shortRecordRepo = Container.ResolveRepository<ShortProgramRecord>();
            for (var index = 0; index < listToSave.Count; index += shift)
            {
                var part = listToSave.Skip(index).Take(shift);
                using(var tr = Container.Resolve<IDataTransaction>())
                {
                    foreach (var shortProgramRecord in part)
                    {
                        if (shortProgramRecord.Id != 0)
                        {
                            shortRecordRepo.Update(shortProgramRecord);
                        }
                        else
                        {
                            shortRecordRepo.Save(shortProgramRecord);
                        }
                    }

                    tr.Commit();
                }
            }

            var objectCrRepo = Container.ResolveRepository<ObjectCr>();
            var objectsToSave = objectCrToSave.GroupBy(x => x.RealityObject.Id).Select(x => x.First()).ToList();
            if (objectsToSave.Count > 0)
            {
                stateProvider.SetDefaultState(objectsToSave[0]);
            }

            for (var index = 0; index < objectsToSave.Count; index += shift)
            {
                var part = objectsToSave.Skip(index).Take(shift);
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    foreach (var objectCr in part)
                    {
                        objectCr.State = objectsToSave[0].State;
                        if (objectCr.Id != 0)
                        {
                            objectCrRepo.Update(objectCr);
                        }
                        else
                        {
                            objectCrRepo.Save(objectCr);
                        }
                    }

                    tr.Commit();
                }
            }

            var typeWorkGroups = typeWorkToSave.Where(x => x.ObjectCr.Id != 0).GroupBy(x => new { RoId = x.ObjectCr.Id, WorkId = x.Work.Id, FinId = x.FinanceSource.Id }).ToArray();
            typeWorkToSave = typeWorkGroups.Select(x => x.First()).ToList();

            var typeWorkRepo = Container.ResolveRepository<TypeWorkCr>();
            if (typeWorkToSave.Count > 0)
            {
                stateProvider.SetDefaultState(typeWorkToSave[0]);
            }

            for (var index = 0; index < typeWorkToSave.Count; index += shift)
            {
                var part = typeWorkToSave.Skip(index).Take(shift);
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    foreach (var typeWorkCr in part)
                    {
                        typeWorkCr.State = typeWorkToSave[0].State;
                        if (typeWorkCr.Id != 0)
                        {
                            typeWorkRepo.Update(typeWorkCr);
                        }
                        else
                        {
                            typeWorkRepo.Save(typeWorkCr);
                        }
                    }

                    tr.Commit();
                }
            }

            finResToSave.ForEach(x => x.TypeWorkCr = new TypeWorkCr{ Id = x.TypeWorkCr.Id});
            var finResGroups = finResToSave.Where(x => x.ObjectCr.Id != 0).Where(x => x.TypeWorkCr.Id != 0).GroupBy(x => new { RoId = x.ObjectCr.Id, WorkId = x.TypeWorkCr.Id, FinId = x.FinanceSource.Id }).ToArray();
            finResToSave = finResGroups.Select(x => x.First()).ToList();

            var finRepo = Container.ResolveRepository<FinanceSourceResource>();
            for (var index = 0; index < finResToSave.Count; index += shift)
            {
                var part = finResToSave.Skip(index).Take(shift);
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    foreach (var resource in part)
                    {
                        if (resource.Id != 0)
                        {
                            finRepo.Update(resource);
                        }
                        else
                        {
                            finRepo.Save(resource);
                        }
                    }

                    tr.Commit();
                }
            }

            LogImport.SetFileName(fileData.FileName);
            LogImport.ImportKey = CodeImport;

            LogImportManager.FileNameWithoutExtention = fileData.FileName;
            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            return new ImportResult(LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
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

        private List<string> _skipStreetList = new List<string>()
        {
            "лет Октября",
            "40 лет Комсомола",
            "9-го Ноября"
        }; 


        //страшная функция подгонки адреса из файла импорта под наш формат
        //писалась на коленке, по идее надо зарефакторить
        //возможны ситуации, когда в файле импорта часть названия улицы содержит цифры, у нас это в одном случае цифра, в другом римская цифра,
        //при том она может быть записана как в виде русских ХэХэ, так и в виде латинских I
        private string FormatAddressPart(string input)
        {
            //убираем из названия лишние части, такие как ул, пер и другие возможные, чтобы можно было найти в нашей базе
            var temp = input.Split(' ');
            if (temp.Length > 1)
            {
                input = string.Join(" ", temp.Take(temp.Length - 1));
            }
            //некоторые улицы лучше не трогать, они подходят почти один в один
            if (_skipStreetList.Contains(input))
            {
                return input;
            }
            //меняем цифры на римские
            if (input.Contains("Партсъезда"))
            {
                input = input.Replace("20", "ХХ"); //русские
            }
            else if (input.Contains("Интернационала"))
            {
                input = input.Replace("3", "III"); //латинские
            }
            //добавляем доп. символы после цифры (пока используется только для этой улицы)
            else if (input.Contains("4 Пятилетки"))
            {
                input = input.Replace("4", "4-й");
            }
            //меняем местами цифры в конце и название улицы в начале
            else if  (input.Any(char.IsDigit))
            {
                temp = input.Split(' ');
                if (temp.Length > 1)
                {
                    var last = temp.Last();
                    var first = temp.Take(temp.Length - 1).ToList();
                    first.Insert(0, last);
                    input = string.Join(" ", first);
                }
            }

            return input;
        }
    }
}