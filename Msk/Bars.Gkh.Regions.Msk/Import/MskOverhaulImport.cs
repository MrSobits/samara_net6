namespace Bars.Gkh.Regions.Msk.Import.CommonRealtyObjectImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Domain;
    using Enums.Import;
    using Gkh.Entities;
    using Gkh.Import;
    using Gkh.Import.Impl;

    using GkhExcel;
    using NHibernate.Linq;
    using Overhaul.Entities;
    using Overhaul.Nso.Entities;
    using Utils;

    public class MskOverhaulImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public virtual IDomainService<RealityObjectStructuralElement> RoSeDomain { get; set; }

        public virtual IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public virtual IDomainService<TypeProject> TypeProjectDomain { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "MskOverhaulImport"; }
        }

        public override string Name
        {
            get { return "Импорт ДПКР (Москва)"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.MskDpkrImport.View"; }
        }

        private ILogImportManager LogManager { get; set; }

        private ILogImport LogImport { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        private readonly HashSet<string> structElCodes = new HashSet<string>();

        public ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                if (file.Extention == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    if (rows.Count < 4)
                    {
                        throw new Exception("Не корректный формат");
                    }

                    InitLog(file.FileName);

                    InitHeader(rows[0], rows[2]);

                    var realObjStructElements = RoSeDomain.GetAll()
                        .Fetch(x => x.StructuralElement)
                        .Where(x => x.StructuralElement.Code != null && x.StructuralElement.Code != "")
                        .ToList()
                        .GroupBy(x => "{0}_{1}".FormatUsing(x.RealityObject.Id, x.StructuralElement.Code.Split('*')[0].ToUpper()))
                        .ToDictionary(x => x.Key, y => y.ToList());

                    var realObjs = RealityObjectDomain.GetAll()
                                 .ToDictionary(x => x.Id);

                    var typeProjects = TypeProjectDomain
                        .GetAll()
                        .GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key, y => y.First());

                    var realObjInfoToUpdate = new HashSet<long>();
                    var typeProjToSave = new List<TypeProject>();
                    var realObjToUpdate = new List<RealityObject>();
                    var st1ToSave = new List<RealityObjectStructuralElementInProgramm>();
                    var st2ToSave = new List<RealityObjectStructuralElementInProgrammStage2>();
                    var st3ToSave = new List<RealityObjectStructuralElementInProgrammStage3>();
                    for (var i = 3; i < rows.Count; i++)
                    {
                        var tempRow = rows[i];

                        var id = GetValue(tempRow, "ID").ToLong();
                        var buildYear = GetValue(tempRow, "MKDB02");
                        var typeProjectStr = GetValue(tempRow, "СЕРИЯ");
                        var typeProject = typeProjects.Get(typeProjectStr);

                        if (typeProject == null)
                        {
                            typeProject = new TypeProject
                            {
                                Name = typeProjectStr
                            };

                            typeProjects.Add(typeProjectStr, typeProject);
                            typeProjToSave.Add(typeProject);
                        }

                        var realObj = realObjs.Get(id);

                        if (realObj == null)
                        {
                            LogImport.Warn("Строка: {0}".FormatUsing(i), "Дом не найден");
                            continue;
                        }

                        realObj.ExternalId = GetValue(tempRow, "UID");
                        realObj.TypeProject = typeProject;

                        if (buildYear.IsEmpty())
                        {
                            realObj.BuildYear = buildYear.ToInt();
                        }

                        realObj.AreaMkd = GetValue(tempRow, "MKDC01").ToDecimal();
                        realObj.AreaLiving = GetValue(tempRow, "MKDC02").ToDecimal();
                        realObj.AreaNotLivingFunctional = GetValue(tempRow, "MKDC03").ToInt();
                        realObj.MaximumFloors = GetValue(tempRow, "MKDC06").ToInt();
                        realObj.NumberEntrances = GetValue(tempRow, "MKDC07").ToInt();
                        realObj.NumberApartments = GetValue(tempRow, "MKDC08").ToInt();
                        realObj.Points = (int)GetValue(tempRow, "ДОМ БАЛЛЫ (ПОЛНЫЕ)").ToDecimal();

                        realObjInfoToUpdate.Add(realObj.Id);

                        var number = GetValue(tempRow, "ОЧЕРЕДЬ").ToInt();

                        foreach (var structElCode in structElCodes)
                        {
                            var roSeList = realObjStructElements.Get("{0}_{1}".FormatUsing(realObj.Id, structElCode));

                            if (roSeList == null || roSeList.Count == 0)
                            {
                                LogImport.Warn("Строка: {0}".FormatUsing(i), "У дома: {0}. Не найден конструктивный элемент с кодом: {1}".FormatUsing(realObj.Address, structElCode));
                                continue;
                            }        

                            var year = GetValue(tempRow, "П_{0}".FormatUsing(structElCode)).ToInt();
                            var points = GetValue(tempRow, "БАЛЛЫ_{0}".FormatUsing(structElCode)).ToInt();

                            if (structElCode == "BALCONY" || year == 0)
                            {
                                continue;
                            }

                            var st3 = new RealityObjectStructuralElementInProgrammStage3
                            {
                                RealityObject = realObj,
                                CommonEstateObjects = roSeList.Select(x => x.StructuralElement.Group.CommonEstateObject.Name).FirstOrDefault(),
                                Year = year,
                                IndexNumber = number,
                                Point = points
                            };

                            st3ToSave.Add(st3);
                            
                            var st2 = new RealityObjectStructuralElementInProgrammStage2
                            {
                                Stage3 = st3,
                                RealityObject = realObj,
                                CommonEstateObject = roSeList.Select(x => x.StructuralElement.Group.CommonEstateObject).FirstOrDefault(),
                                Year = year,
                                StructuralElements = roSeList.Select(x => x.StructuralElement.Name).Distinct().AggregateWithSeparator(", ")
                            };

                            st2ToSave.Add(st2);

                            foreach (var roSe in roSeList)
                            {
                                var st1 = new RealityObjectStructuralElementInProgramm
                                {
                                    Stage2 = st2,
                                    StructuralElement = roSe,
                                    Year = year,
                                    LastOverhaulYear = roSe.LastOverhaulYear
                                };

                                st1ToSave.Add(st1);
                            }
                        }

                        realObjToUpdate.Add(realObj);

                        LogImport.CountChangedRows++;
                    }

                    if (realObjInfoToUpdate.Count > 0)
                    {
                        var session = SessionProvider.GetCurrentSession();

                        try
                        {
                            for (var i = 0; i < realObjInfoToUpdate.Count; i += 1000)
                            {
                                var ids = realObjInfoToUpdate.Skip(i).Take(1000).ToList();

                                session.CreateSQLQuery(@" delete from OVRHL_RO_STRUCT_EL_IN_PRG where id in(
                                        select st1.id  from OVRHL_RO_STRUCT_EL_IN_PRG st1
                                        join OVRHL_RO_STRUCT_EL_IN_PRG_2 st2 on st1.STAGE2_ID = st2.ID
                                        where st2.RO_ID in (:ids))")
                                    .SetParameterList("ids", ids)
                                    .ExecuteUpdate();

                                session.CreateSQLQuery(@" delete from OVRHL_RO_STRUCT_EL_IN_PRG_2
                                        where RO_ID in (:ids)")
                                    .SetParameterList("ids", ids)
                                    .ExecuteUpdate();

                                session.CreateSQLQuery(@" delete from OVRHL_RO_STRUCT_EL_IN_PRG_3
                                        where RO_ID in (:ids)")
                                    .SetParameterList("ids", ids)
                                    .ExecuteUpdate();
                            }
                        }
                        catch
                        {
                            LogImport.Error("Ошибка", "Не удалось удалить констуктивные элементы, т.к некоторые из них участвуют в версиях ДПКР");
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(Container, typeProjToSave, 10000, true, true);
                    TransactionHelper.InsertInManyTransactions(Container, realObjToUpdate, 10000, true, true);
                    TransactionHelper.InsertInManyTransactions(Container, st3ToSave, 10000, true, true);
                    TransactionHelper.InsertInManyTransactions(Container, st2ToSave, 10000, true, true);
                    TransactionHelper.InsertInManyTransactions(Container, st1ToSave, 10000, true, true);
                }
            }

            LogImport.ImportKey = Key;

            LogManager.FileNameWithoutExtention = file.FileName;
            LogManager.UploadDate = DateTime.Now;
            LogManager.Add(file, LogImport);
            LogManager.Save();

            var status = LogManager.CountError > 0 ? StatusImport.CompletedWithError : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, string.Empty, string.Empty, LogManager.LogFileId);
        }

        public bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void InitHeader(GkhExcelCell[] row, GkhExcelCell[] thirdRow)
        {
            headersDict["ID"] = -1;
            headersDict["UID"] = -1;
            headersDict["СЕРИЯ"] = -1;
            headersDict["MKDB02"] = -1;
            headersDict["MKDC01"] = -1;
            headersDict["MKDC02"] = -1;
            headersDict["MKDC03"] = -1;
            headersDict["MKDC06"] = -1;
            headersDict["MKDC07"] = -1;
            headersDict["MKDC08"] = -1;
            headersDict["ДОМ БАЛЛЫ (ПОЛНЫЕ)"] = -1;
            headersDict["ОЧЕРЕДЬ"] = -1;

            for (var index = 0; index < row.Length; ++index)
            {
                var header = row[index].Value.ToUpper();
                var thirdRowHeader = thirdRow[index].Value.ToUpper();
                if (headersDict.ContainsKey(header))
                {
                    headersDict[header] = index;
                }
                else if (!thirdRowHeader.IsEmpty())
                {
                    if (header.StartsWith("БАЛЛЫ"))
                    {
                        headersDict.Add("БАЛЛЫ_{0}".FormatUsing(thirdRowHeader), index);
                        structElCodes.Add(thirdRowHeader);
                    }

                    if (header.EndsWith("П"))
                    {
                        headersDict.Add("П_{0}".FormatUsing(thirdRowHeader), index);
                        structElCodes.Add(thirdRowHeader);
                    }
                }
            }
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

            return result.Trim();
        }

        public void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;

            LogImport = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            LogImport.SetFileName(fileName);
            LogImport.ImportKey = Key;
        }
    }
}