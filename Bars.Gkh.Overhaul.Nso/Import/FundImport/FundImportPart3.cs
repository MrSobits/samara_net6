namespace Bars.Gkh.Overhaul.Nso.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4.DataAccess;
    using Castle.Windsor;

    using Bars.B4;
    using B4.Utils;

    using Bars.Gkh.Import;
    using Enums.Import;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import.Impl;
    using Overhaul.Entities;

    /// <summary>
    /// Импорт конструктивных элементов, выгрузка из фонда часть 3
    /// </summary>
    public class FundImportPart3 : GkhImportBase
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
            get { return "Импорт конструктивных элементов (часть 3)"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.FundRealtyObjectsImport3.View"; }
        }

        public IRepository<RealityObjectStructuralElement> RoStructElemRep { get; set; }

        public IRepository<StructuralElement> StructElemRep { get; set; }

        public IRepository<RealityObject> RobjectRep { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion Properties

        private readonly Dictionary<string, string> dictCodes = new Dictionary<string, string>
            {
                {"COUNT_KV_R", "1"},
                {"COUNT_L_R", "1"},
                {"COUNT_SV", "2"},
                {"COUNT_SV_DRL", "3"},
                {"COUNT_SV_N", "4"},
                {"ELEV", "5"},
                {"GR_E_C", "6"},
                {"HSV_LKV", "7"},
                {"HSV_LPODV", "7"},
                {"HSV_LONG", "8"},
                {"HSV_LRAZV", "8"},
                {"HSV_VODOM", "9"},
                {"KAL", "13"},
                {"KONV", "10"},
                {"KOR", "11"},
                {"KRAN", "12"},
                {"LONG_G", "13"},
                {"LONG_GOR", "14"},
                {"LONG_KV_GOR", "15"},
                {"LONG_LIFT", "16"},
                {"LONG_LIGHT", "17"},
                {"MUS_COUNT", "18"},
                {"MUS_KAM", "19"},
                {"MUS_KAM_COUNT", "20"},
                {"SEW_LONG", "22"},
                {"SEW_LSTOYAK", "21"},
                {"VENT", "22"},
                {"ZADV", "23"},
                {"ZAPOR", "24"}
            };

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

            var dictHeaders = dictCodes.Keys.ToDictionary(x => x, y => 0);

            //словарь id жилого дома - коды конструктивных элементов
            var dictRoStructEls = RoStructElemRep.GetAll()
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
            var dictStructElements = StructElemRep.GetAll()
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
            var dictRobject = RobjectRep.GetAll()
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

            var records = new List<Part3>();

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
                            continue;
                        }

                        var row = line.Split(';').Select(x => x ?? "").ToArray();

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

                        var roStrElems = dictRoStructEls.ContainsKey(row[0]) ? dictRoStructEls[row[0]] : new Dictionary<string, string>();

                        foreach (var header in dictHeaders.Where(x => x.Value > 0))
                        {
                            if (row[header.Value].ToDecimal() == 0)
                            {
                                LogImport.Warn("Предупреждение", string.Format("Неверное значение объема, строка {0}, ячейка {1}", rowNumber, header.Value));
                                continue;
                            }

                            var strElCode = dictCodes[header.Key];

                            //если в доме уже есть конструктивный элемент с таким кодом - пролетаем
                            if (roStrElems.ContainsKey(strElCode))
                            {
                                continue;
                            }

                            var ro = dictRobject[row[0]];

                            if (dictStructElements.ContainsKey(strElCode))
                            {
                                records.Add(new Part3
                                {
                                    RoId = ro.Id,
                                    StrElCode = strElCode,
                                    StrElId = dictStructElements[strElCode],
                                    Volume = row[header.Value].ToDecimal(),
                                    LastOverhaulYear = ro.DateCommissioning.HasValue ? ro.DateCommissioning.Value.Year : -1
                                });
                            }
                        }
                    }
                }
            }

            var newStructElems = new List<RealityObjectStructuralElement>();

            var groupedRecords =
                records.GroupBy(x => new { x.RoId, x.StrElCode })
                    .Select(x => new Part3
                    {
                        RoId = x.Key.RoId,
                        LastOverhaulYear = x.Select(y => y.LastOverhaulYear).FirstOrDefault(),
                        StrElId = x.Select(y => y.StrElId).FirstOrDefault(),
                        Volume = x.Sum(y => y.Volume)
                    });

            foreach (var item in groupedRecords)
            {
                LogImport.CountAddedRows++;

                var newRec = new RealityObjectStructuralElement
                {
                    RealityObject = RobjectRep.Load(item.RoId),
                    StructuralElement = StructElemRep.Load(item.StrElId),
                    Volume = item.Volume,
                    Wearout = 0,
                    LastOverhaulYear = item.LastOverhaulYear
                };

                newStructElems.Add(newRec);
            }

            SaveOrUpdate(newStructElems, RoStructElemRep);

            LogImport.SetFileName(fileData.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = fileData.FileName;
            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            return new ImportResult(LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
        }

        protected void InTransaction(Action action)
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

        protected void SaveOrUpdate(IEnumerable<IEntity> entities, IRepository repository)
        {
            this.InTransaction(() =>
            {
                foreach (var entity in entities)
                {
                    if ((long)entity.Id > 0)
                        repository.Update(entity);
                    else
                        repository.Save(entity);
                }
            });
        }

        private class Part3
        {
            public long RoId;
            public string StrElCode;
            public long StrElId;
            public decimal Volume;
            public int LastOverhaulYear;
        }
    }
}