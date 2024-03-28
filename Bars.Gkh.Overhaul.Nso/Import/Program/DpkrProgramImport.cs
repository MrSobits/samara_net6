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
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhExcel;

    using Castle.Windsor;

    public sealed class DpkrProgramImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IWindsorContainer Container { get; set; }

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
            get { return "Импорт программы капитального ремонта"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.NsoRealtyObjectImport.View"; }
        }

        public ILogImportManager LogManager { get; set; }

        public class ParsedRow
        {
            public int Number { get; set; }
            public string HouseId { get; set; }
            public string WorkId { get; set; }
            public DateTime EnterDate { get; set; }
            public string Kladr { get; set; }
            public string MR { get; set; }
            public string MO { get; set; }
            public string Area { get; set; }
            public string Street { get; set; }
            public string BuildingNum { get; set; }
            public string KorpNum { get; set; }
            public string OOI { get; set; }
            public decimal Volume { get; set; }
            public string ConstructEl { get; set; }
            public int Year { get; set; }
            public int LastFix { get; set; }
            public int PeriodBetweenFix { get; set; }
            public int PrivYear { get; set; }
            public int PlanFixYear { get; set; }
            public int RequiredFixYear { get; set; }
            public int RequiredFixPercent { get; set; }
            public int FixPercent { get; set; }
            public bool IsPrivate { get; set; }
            public int DpkrFixYear { get; set; }
            public int FactFixYear { get; set; }
            public string Reason { get; set; }
            public decimal Price { get; set; }
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];

            var roRepo = Container.ResolveRepository<RealityObject>();
            var stage1Repo = Container.ResolveRepository<VersionRecordStage1>();
            var stage2Repo = Container.ResolveRepository<VersionRecordStage2>();
            var stage3Repo = Container.ResolveRepository<VersionRecord>();
            var strElRepo = Container.ResolveRepository<StructuralElement>();
            var realStrElRepo = Container.ResolveRepository<RealityObjectStructuralElement>();
            var commonEstateRepo = Container.ResolveRepository<CommonEstateObject>();
            var publishPrgRecRepo = Container.ResolveRepository<PublishedProgramRecord>();
            var structElWorkRepo = Container.ResolveRepository<StructuralElementWork>();

            var programVerRepo = Container.ResolveRepository<ProgramVersion>();
            var programVer = programVerRepo.GetAll().OrderBy(x => x.Id).FirstOrDefault();
            if (programVer == null)
            {
                programVer = new ProgramVersion
                {
                    Name = "Импортировано " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    VersionDate = DateTime.Now
                };
                programVerRepo.Save(programVer);
            }


            var publishPrgRepo = Container.ResolveRepository<PublishedProgram>();
            var lastProgram = publishPrgRepo.GetAll().OrderBy(x => x.Id).FirstOrDefault();
            if (lastProgram == null)
            {
                lastProgram = new PublishedProgram
                {
                    ProgramVersion = programVer
                };

                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(lastProgram);
                publishPrgRepo.Save(lastProgram);
            }

            var realityCache = roRepo.GetAll().Where(x => x.ExternalId != null).GroupBy(x => x.ExternalId).ToDictionary(x => x.Key, x => x.First());
            var strElCache = strElRepo.GetAll().GroupBy(x => x.Group.CommonEstateObject.Id).ToDictionary(x => x.Key, x => x.ToArray());
            var realStrElCache = realStrElRepo.GetAll().GroupBy(x => x.RealityObject.Id).ToDictionary(x => x.Key, x => x.GroupBy(y => y.StructuralElement.Id).ToDictionary(y => y.Key, y => y.First()));

            var stage3List = new List<VersionRecord>();
            var stage2List = new List<VersionRecordStage2>();
            var stage1List = new List<VersionRecordStage1>();

            var publishedProgramRecordList = new List<PublishedProgramRecord>();
            var structElWorkCache = structElWorkRepo.GetAll().GroupBy(x => x.StructuralElement.Id).ToDictionary(x => x.Key, x => x.First());
            var commonEstateCache = commonEstateRepo.GetAll().ToDictionary(x => x.Name.ToLower(), x => x);

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                excel.UseVersionXlsx();
                using(var xlsMemoryStream = new MemoryStream(fileData.Data))
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

                        if (row.Contains("num"))
                        {
                            startIndex = rowNum + 1;
                            header = row;
                        }

                        rowNum++;
                    } while (startIndex == 0 && rowNum + 1 != data.Count);

                    if (startIndex == 0)
                    {
                        throw new ArgumentException("Формат шаблона не соответсвует импорту!\nНе найден столбец \"num\"");
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
                                Number = dict.Get("num").ToInt(),
                                HouseId = dict.Get("id"),
                                WorkId = dict.Get("id_work"),
                                EnterDate = dict.Get("plus").ToDateTime(),
                                Kladr = dict.Get("kla"),
                                MR = dict.Get("3(1)"),
                                MO = dict.Get("4"),
                                Area = dict.Get("5"),
                                Street = dict.Get("6"),
                                BuildingNum = dict.Get("7"),
                                KorpNum = dict.Get("8"),
                                OOI = dict.Get("obj"),
                                Volume = dict.Get("vol").ToDecimal(),
                                ConstructEl = dict.Get("name_ke"),
                                Year = dict.Get("17").ToInt(),
                                LastFix = dict.Get("last_rem").ToInt(),
                                PeriodBetweenFix = dict.Get("mrs").ToInt(),
                                PrivYear = dict.Get("date_priv").ToInt(),
                                PlanFixYear = dict.Get("god_plan").ToInt(),
                                RequiredFixYear = dict.Get("god_neob").ToInt(),
                                RequiredFixPercent = dict.Get("ndr").ToInt(),
                                FixPercent = dict.Get("det").ToInt(),
                                IsPrivate = dict.Get("priv").ToBool(),
                                DpkrFixYear = dict.Get("god_pre").ToInt(),
                                FactFixYear = dict.Get("god").ToInt(),
                                Reason = dict.Get("hand"),
                                Price = dict.Get("cost_5").ToDecimal()
                            };

                            if (entity.Number == 0)
                            {
                                LogImport.Error(
                                    "Строка не добавлена",
                                    string.Format("Для строки {0} не найден порядковый номер", i + 1));
                                continue;
                            }

                            var realityEntity = realityCache.ContainsKey(entity.HouseId) ? realityCache[entity.HouseId] : null;
                            if (realityEntity == null)
                            {
                                LogImport.Error(
                                    "Строка не добавлена",
                                    string.Format("Для строки {0} не найден дом с id {1}", i + 1, entity.HouseId));
                                continue;
                            }

                            var commonEstateEntity = commonEstateCache.ContainsKey(entity.OOI.ToLower()) ? commonEstateCache[entity.OOI.ToLower()] : null;
                            if (commonEstateEntity == null)
                            {
                                LogImport.Error(
                                    "Строка не добавлена",
                                    string.Format("Для строки {0} не найден ООИ {1}", i + 1, entity.OOI));
                                continue;
                            }

                            var stage3 = new VersionRecord
                            {
                                RealityObject = realityEntity,
                                Sum = entity.Price,
                                Year = entity.FactFixYear,
                                ProgramVersion = programVer,
                                CommonEstateObjects = commonEstateEntity.Name
                            };

                            stage3List.Add(stage3);

                            var stage2 = new VersionRecordStage2
                            {
                                Sum = entity.Price,
                                CommonEstateObject = commonEstateEntity,
                                Stage3Version = stage3
                            };

                            stage2List.Add(stage2);

                            if (strElCache.ContainsKey(commonEstateEntity.Id))
                            {
                                var strElEntities = strElCache[commonEstateEntity.Id];

                                var realGroup = realStrElCache.ContainsKey(realityEntity.Id) ? realStrElCache[realityEntity.Id] : null;
                                if (realGroup != null)
                                {
                                    StructuralElement strElEntity = null;
                                    RealityObjectStructuralElement realStrElEntity = null;
                                    StructuralElementWork structElWork = null;
                                    for (var index = 0; index < strElEntities.Length; index++)
                                    {
                                        strElEntity = strElEntities[index];

                                        realStrElEntity = realGroup.ContainsKey(strElEntity.Id) ? realGroup[strElEntity.Id] : null;
                                        if (realStrElEntity == null)
                                        {
                                            continue;
                                        }

                                        structElWork = structElWorkCache.ContainsKey(strElEntity.Id) ? structElWorkCache[strElEntity.Id] : null;
                                        if (structElWork == null)
                                        {
                                            continue;
                                        }

                                        break;
                                    }

                                    if (strElEntity != null && realStrElEntity != null && structElWork != null)
                                    {
                                        var stage1 = new VersionRecordStage1
                                        {
                                            LastOverhaulYear = entity.LastFix,
                                            RealityObject = realityEntity,
                                            Sum = entity.Price,
                                            Volume = entity.Volume,
                                            Wearout = entity.FixPercent,
                                            Year = entity.PlanFixYear,
                                            StructuralElement = realStrElEntity,
                                            Stage2Version = stage2
                                        };

                                        stage1List.Add(stage1);
                                    }
                                    else
                                    {
                                        LogImport.Error(
                                            "Строка не добавлена",
                                            string.Format("Для строки {0} не найден связка дом - структурный элемент", i + 1));
                                    }
                                }
                                else
                                {
                                    LogImport.Error(
                                        "Строка не добавлена",
                                        string.Format("Для строки {0} не найден связка дом - структурный элемент", i + 1));
                                }
                            }

                            var address = entity.Area + ", " + entity.Street + " " + entity.BuildingNum;
                            if (!string.IsNullOrWhiteSpace(entity.KorpNum))
                            {
                                address += ", корп. " + entity.KorpNum;
                            }

                            var publishProgramRec = new PublishedProgramRecord
                            {
                                Address = address,
                                CommissioningYear = entity.Year,
                                CommonEstateobject = entity.OOI,
                                House = entity.BuildingNum,
                                Housing = entity.KorpNum,
                                LastOverhaulYear = entity.LastFix,
                                Locality = entity.Area,
                                PublishedProgram = lastProgram,
                                PublishedYear = entity.PlanFixYear,
                                Stage2 = stage2,
                                Street = entity.Street,
                                Sum = entity.Price,
                                Wear = entity.FixPercent
                            };

                            publishedProgramRecordList.Add(publishProgramRec);
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

            for (var index = 0; index < stage3List.Count; index += 1000)
            {
                using(var tr = Container.Resolve<IDataTransaction>())
                {
                    for (var indexItem = index; indexItem < (stage3List.Count < index + 1000 ? stage3List.Count : index + 1000); indexItem++)
                    {
                        stage3Repo.Save(stage3List[indexItem]);
                    }

                    tr.Commit();
                }
            }

            for (var index = 0; index < stage2List.Count; index += 1000)
            {
                using(var tr = Container.Resolve<IDataTransaction>())
                {
                    for (var indexItem = index; indexItem < (stage2List.Count < index + 1000 ? stage2List.Count : index + 1000); indexItem++)
                    {
                        stage2Repo.Save(stage2List[indexItem]);
                    }

                    tr.Commit();
                }
            }

            for (var index = 0; index < stage1List.Count; index += 1000)
            {
                using(var tr = Container.Resolve<IDataTransaction>())
                {
                    for (var indexItem = index; indexItem < (stage1List.Count < index + 1000 ? stage1List.Count : index + 1000); indexItem++)
                    {
                        stage1Repo.Save(stage1List[indexItem]);
                    }

                    tr.Commit();
                }
            }

            for (var index = 0; index < publishedProgramRecordList.Count; index += 1000)
            {
                using(var tr = Container.Resolve<IDataTransaction>())
                {
                    for (var indexItem = index; indexItem < (publishedProgramRecordList.Count < index + 1000 ? publishedProgramRecordList.Count : index + 1000); indexItem++)
                    {
                        publishPrgRecRepo.Save(publishedProgramRecordList[indexItem]);
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
    }
}