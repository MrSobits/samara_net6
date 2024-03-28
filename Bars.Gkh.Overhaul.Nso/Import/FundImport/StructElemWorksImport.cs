namespace Bars.Gkh.Overhaul.Nso.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;

    using Castle.Windsor;
    using Enums.Import;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Import;
    using Gkh.Import.Impl;
    using GkhExcel;
    using Overhaul.Entities;

    public class StructElemWorksImport : GkhImportBase
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
            get { return "Импорт дат ремонта"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.StructElemWorksImport.View"; }
        }

        public IRepository<RealityObjectStructuralElement> RoStructElemRep { get; set; }

        public IRepository<StructuralElement> StructElemRep { get; set; }

        public IRepository<RealityObject> RobjectRep { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion Properties

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

        private readonly Dictionary<string, string[]> dictCodes = new Dictionary<string, string[]>
            {
                {"ID_MKD", new string[0]},
                {"PLAN_DATE", new string[0]},
                {"TEPL_F", new[] {"1", "5", "10", "11", "12", "13", "22", "23", "24"}},
                {"EL_F", new[] {"2", "3", "4", "6", "16", "17"}},
                {"VS_COLD_F", new[] {"7", "8"}},
                {"VS_HOT_F", new[] {"14", "15"}},
                {"PU_HVS_RF", new[] {"9"}},
                {"GAZ_F", new[] {"13"}},
                {"SEW_F", new[] {"21", "22", "32", "34", "35", "37"}},
                {"ROOF_RF", new[] {"26", "28", "29", "30", "36", "38", "45", "46", "50", "56"}},
                {"PODVAL_RF", new[] {"31"}},
                {"FASAD_RFR", new[] {"39", "40", "41", "44", "52"}},
                {"FUNDAM_F_RUB", new[] {"49"}}
            };

        private Dictionary<string, int> dictHeaders;

        public override ImportResult Import(BaseParams baseParams)
        {
            dictHeaders = dictCodes.ToDictionary(x => x.Key, y => -1);

            var fileData = baseParams.Files["FileImport"];

            //словарь id жилого дома - коды конструктивных элементов
            var dictRoStructEls = RoStructElemRep.GetAll()
                .Where(x => x.RealityObject.FederalNum != null)
                .Where(x => x.StructuralElement.Code != null)
                .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.RealityObject.FederalNum,
                        x.StructuralElement.Code
                    })
                .AsEnumerable()
                .GroupBy(x => x.FederalNum)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Code).ToDictionary(x => x.Key, z => z.Select(x => new { x.Id, x.RoId }).ToArray()));

            //словарь соответствия федерального номера и идентификатора жилого дома в нашей системе
            var dictRobject = RobjectRep.GetAll()
                .Where(x => x.FederalNum != null)
                .Select(x => new
                    {
                        x.Id,
                        x.FederalNum
                    })
                .AsEnumerable()
                .GroupBy(x => x.FederalNum)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            var records = new List<StructElemWork>();

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData.Data))
                {
                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    var headerRow = rows[0];

                    for (int i = 0; i < headerRow.Length; i++)
                    {
                        if (dictHeaders.ContainsKey(headerRow[i].Value))
                        {
                            dictHeaders[headerRow[i].Value] = i;
                        }
                    }

                    if (dictHeaders["ID_MKD"] == -1)
                    {
                        LogImport.Error("Ошибка", "Отсутствует столбец с федеральным номером дома");
                        return new ImportResult(StatusImport.CompletedWithError);
                    }

                    for (var i = 1; i < rows.Count; i++)
                    {
                        var row = rows[i];

                        var federalNumber = row[dictHeaders["ID_MKD"]].Value;

                        if (string.IsNullOrEmpty(federalNumber))
                        {
                            LogImport.Warn("Предупреждение", string.Format("Не указан федеральный номер дома, строка {0}", i));
                            continue;
                        }

                        if (!dictRobject.ContainsKey(federalNumber))
                        {
                            LogImport.Warn("Предупреждение", string.Format("Не удалось получить дом по федеральному номеру, строка {0}", i));
                            continue;
                        }

                        if (!dictRoStructEls.ContainsKey(federalNumber))
                        {
                            LogImport.Info("Предупреждение",
                                string.Format("В жилом доме отсутствуют конструктивные элементы, федеральный номер {0}, строка {1}", federalNumber, i));
                            continue;
                        }

                        var planDate = row[dictHeaders["PLAN_DATE"]].Value.ToDateTime();

                        if (planDate == DateTime.MinValue)
                        {
                            LogImport.Warn("Предупреждение", string.Format("Не указана плановая дата ремонта, строка {0}", i));
                            continue;
                        }

                        var roStructElems = dictRoStructEls[federalNumber];

                        foreach (var header in dictHeaders.Where(x => x.Value > -1))
                        {
                            var value = row[header.Value].Value.ToDecimal();

                            if (value == 0)
                            {
                                continue;
                            }

                            var codes = dictCodes[header.Key];

                            foreach (var code in codes.Where(roStructElems.ContainsKey))
                            {
                                foreach (var item in roStructElems[code])
                                {
                                    records.Add(new StructElemWork
                                    {
                                        Id = item.Id,
                                        RoId = item.RoId,
                                        Code = code,
                                        PlanDate = planDate
                                    });
                                }
                            }
                        }
                    }
                }
            }

            var newStructElems = new List<RealityObjectStructuralElement>();

            var recordsForUpdate = records
                .GroupBy(x => new {x.RoId, x.Code})
                .Select(x => new
                    {
                        PlanDate = x.Max(y => y.PlanDate),
                        Ids = x.Select(y => y.Id).ToArray()
                    })
                .ToArray();

            foreach (var item in recordsForUpdate)
            {
                foreach (var id in item.Ids)
                {
                    var newRec = RoStructElemRep.Load(id);
                    newRec.LastOverhaulYear = item.PlanDate.Year;
                    newStructElems.Add(newRec);
                }
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
            var transaction = this.Container.Resolve<IDataTransaction>();
            using (this.Container.Using(transaction))
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

        private class StructElemWork
        {
            public DateTime PlanDate;
            public string Code;
            public long Id;
            public long RoId;
        }
    }
}