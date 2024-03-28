namespace Bars.Gkh.Overhaul.Nso.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using B4.Utils;
    using B4.DataAccess;

    using Castle.Windsor;
    using Enums.Import;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import;
    using Gkh.Import.Impl;
    using Overhaul.Entities;

    public class FundImportPart5 : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "StructElemFundImport"; }
        }

        public override string Name
        {
            get { return "Импорт конструктивных элементов (часть 5)"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.FundRealtyObjectsImport5.View"; }
        }

        public IRepository<RealityObjectStructuralElement> RoStructElemService { get; set; }

        public IRepository<StructuralElement> StructElemService { get; set; }

        public IRepository<RealityObject> RobjectService { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion Properties

        #region Fields

        private readonly Dictionary<string, string> dictCodes = new Dictionary<string, string>
            {
                {"COUNT_KOLP", "25"},
                {"COUNT_KR_F", "26"},
                {"COUNT_KR_LAZ", "28"},
                {"COUNT_KR_P", "29"},
                {"COUNT_KR_VIH", "30"},
                {"COUNT_PODV_WIN", "31"},
                {"LONG_LOT", "32"},
                {"LONG_ST", "34"},
                {"LONG_STOK", "35"},
                {"LONG_SV", "36"},
                {"LONG_VOD", "37"},
                {"SL_WIN", "38"},
                {"SQL_PVH", "39"},
                {"SQV_BAL", "40"},
                {"SQV_COK", "41"},
                {"SQV_KERAM", "42"},
                {"SQV_KR_K", "44"},
                {"SQV_KR_MET", "45"},
                {"SQV_KR_O", "56"},
                {"SQV_KR_RUL", "46"},
                {"SQV_LK", "47"},
                {"SQV_MR", "48"},
                {"SQV_MS_OP", "43"},
                {"SQV_OTM", "49"},
                {"SQV_PAR", "50"},
                {"SQV_POST", "51"},
                {"SQV_STUK", "52"},
                {"WALL_TYPE", ""},
                {"SQV_ST", ""}
            };

        private Dictionary<string, int> dictHeaders;

        private Dictionary<string, long> dictStructElements;

        private Dictionary<string, Dictionary<string, string>> dictRoStructEls;

        #endregion Fields

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

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];

            dictHeaders = dictCodes.Keys.ToDictionary(x => x, y => 0);

            //словарь id жилого дома - коды конструктивных элементов
            dictRoStructEls = RoStructElemService.GetAll()
                .Where(x => x.RealityObject.FederalNum != null)
                .Where(x => x.StructuralElement.Code != null)
                .Select(x => new
                {
                    x.RealityObject.FederalNum,
                    x.StructuralElement.Code
                })
                .AsEnumerable()
                .GroupBy(x => x.FederalNum)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Code).Distinct().ToDictionary(x => x));

            //словарь соответствия кода и идентификатора конструктивного элемента
            dictStructElements = StructElemService.GetAll()
                .Where(x => x.Code != null)
                .Select(x => new
                {
                    x.Id,
                    x.Code
                })
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            //словарь соответствия федерального номера и идентификатора жилого дома в нашей системе
            var dictRobject = RobjectService.GetAll()
                .Where(x => x.FederalNum != null)
                .Select(x => new
                {
                    x.Id,
                    x.DateCommissioning,
                    x.FederalNum
                })
                .AsEnumerable()
                .GroupBy(x => x.FederalNum)
                .ToDictionary(x => x.Key, y => y.Select(x => new { x.Id, x.DateCommissioning }).First());

            var records = new List<Part5>();

            using (var ms = new MemoryStream(fileData.Data))
            {
                using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    //считывание заголовков
                    var headers = sr.ReadLine().Split(';').Select(x => x.Trim('"')).ToArray();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (dictHeaders.ContainsKey(headers[i]))
                        {
                            dictHeaders[headers[i]] = i;
                        }
                    }

                    int rowNumber = 1;

                    //читаем данные
                    while (!sr.EndOfStream)
                    {
                        rowNumber++;

                        var line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            LogImport.Warn("Предупреждение", string.Format("Пустая строка, номер {0}", rowNumber));
                            continue;
                        }

                        var row = line.Split(';');

                        if (string.IsNullOrEmpty(row[0]))
                        {
                            LogImport.Warn("Предупреждение", string.Format("Не указан федеральный номер дома, строка {0}", rowNumber));
                            continue;
                        }

                        if (!dictRobject.ContainsKey(row[0]))
                        {
                            LogImport.Warn("Предупреждение", string.Format("Не удалось получить дом по федеральному номеру, строка {0}", rowNumber));
                            continue;
                        }

                        var roStrElems = dictRoStructEls.ContainsKey(row[0])
                            ? dictRoStructEls[row[0]]
                            : new Dictionary<string, string>();

                        if (dictHeaders["WALL_TYPE"] > 0 && dictHeaders["SQV_ST"] > 0)
                        {
                            var code = this.GetCodeByWallType(row[dictHeaders["WALL_TYPE"]].ToInt());

                            if (string.IsNullOrEmpty(code))
                            {
                                LogImport.Warn("Предупреждение", string.Format("Неверное значение WALL_TYPE, строка {0}", rowNumber));
                            }
                            else if (row[dictHeaders["SQV_ST"]].ToDecimal() == 0)
                            {
                                LogImport.Warn("Предупреждение", string.Format("Неверное значение SQV_ST, строка {0}", rowNumber));
                            }
                            else if (!roStrElems.ContainsKey(code))
                            {
                                var ro = dictRobject[row[0]];

                                if (!dictStructElements.ContainsKey(code))
                                {
                                    LogImport.Warn("Предупреждение", string.Format("В справочнике отсутствует конструктивный элемент с кодом {0}", code));
                                    continue;
                                }

                                records.Add(new Part5
                                {
                                    RoId = ro.Id,
                                    StrElId = dictStructElements[code],
                                    Volume = row[dictHeaders["SQV_ST"]].ToDecimal(),
                                    LastOverhaulYear = ro.DateCommissioning.HasValue ? ro.DateCommissioning.Value.Year : -1
                                });
                            }
                        }

                        foreach (var header in dictHeaders.Where(x => x.Value > 0))
                        {
                            if (row[header.Value].ToDecimal() <= 0)
                            {
                                LogImport.Warn("Предупреждение", string.Format("Неверное значение объема, строка {0}, ячейка {1}", rowNumber, header.Value));
                                continue;
                            }

                            var strElCode = dictCodes[header.Key];

                            if (string.IsNullOrWhiteSpace(strElCode))
                            {
                                continue;
                            }

                            //если в доме уже есть конструктивный элемент с таким кодом - пролетаем
                            if (roStrElems.ContainsKey(strElCode))
                            {
                                continue;
                            }

                            var ro = dictRobject[row[0]];

                            if (!dictStructElements.ContainsKey(strElCode))
                            {
                                LogImport.Warn("Предупреждение", string.Format("В справочнике отсутствует конструктивный элемент с кодом {0}", strElCode));
                                continue;
                            }

                            records.Add(new Part5
                            {
                                RoId = ro.Id,
                                StrElId = dictStructElements[strElCode],
                                Volume = row[header.Value].ToDecimal(),
                                LastOverhaulYear = ro.DateCommissioning.HasValue ? ro.DateCommissioning.Value.Year : -1
                            });
                        }
                    }
                }
            }

            var newStructElems = new List<RealityObjectStructuralElement>();

            foreach (var item in records)
            {
                LogImport.CountAddedRows++;

                var newRec = new RealityObjectStructuralElement
                {
                    RealityObject = RobjectService.Load(item.RoId),
                    StructuralElement = StructElemService.Load(item.StrElId),
                    Volume = item.Volume,
                    Wearout = 0,
                    LastOverhaulYear = item.LastOverhaulYear
                };

                newStructElems.Add(newRec);
            }

            SaveOrUpdate(newStructElems, RoStructElemService);

            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = fileData.FileName;
            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            return new ImportResult(LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
        }

        #region Utils

        private void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        private void SaveOrUpdate(IEnumerable<IEntity> entities, IRepository repository)
        {
            this.InTransaction(() =>
            {
                foreach (var entity in entities)
                {
                    if ((int)entity.Id > 0)
                        repository.Update(entity);
                    else
                        repository.Save(entity);
                }
            });
        }

        private string GetCodeByWallType(int wallType)
        {
            switch (wallType)
            {
                case 1:
                    return "53";
                case 2:
                    return "54";
                case 7:
                    return "55";
                default:
                    return "";
            }
        }

        #endregion Utils

        private class Part5
        {
            public long RoId;
            public long StrElId;
            public decimal Volume;
            public int LastOverhaulYear;
        }
    }
}