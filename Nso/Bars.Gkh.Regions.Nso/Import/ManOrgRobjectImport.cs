namespace Bars.Gkh.Regions.Nso.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    public class Record
    {
        public int rowNum;

        public string ExternalId;

        public string ManOrgName;

        public string Inn;

        public string Address;
    }

    public class Log
    {
        public string Text { get; set; }

        public bool Success { get; set; }
    }

    public sealed class ManOrgRobjectImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region PrivateMembers

        private ILogImport logImport;

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        private readonly Dictionary<int, Log> logDict = new Dictionary<int, Log>();

        private Dictionary<string, Dictionary<string, List<Record>>> records;

        private Dictionary<string, Dictionary<string, long>> contragentDict;

        private Dictionary<string, List<long>> realtyObjectDict;

        private Dictionary<long, ManagingOrganization> manOrgByContragentDict;

        private Dictionary<long, List<long>> exisingManOrgRobjectDictByMo;

        private Dictionary<long, List<long>> exisingManOrgContractRobjectDictByMo;

        private List<long> existingDirectManagementRoList;

        #endregion PrivateMembers

        #region Properties

        public IWindsorContainer Container { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }

        public IRepository<ManagingOrganization> ManagingOrganizationRepository { get; set; }

        public IRepository<ManagingOrgRealityObject> ManagingOrgRealityObjectRepository { get; set; }

        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        public IRepository<ManOrgContractOwners> ManOrgContractOwnersRepository { get; set; }

        public IRepository<ManOrgJskTsjContract> ManOrgJskTsjContractRepository { get; set; }
        
        public IRepository<RealityObjectDirectManagContract> RealityObjectDirectManagContractRepository { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ManOrgRobjectImport"; }
        }

        public override string Name
        {
            get { return "Импорт программы капитального ремонта"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls,xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.ManOrgRobjectImport"; }
        }

        public ILogImportManager LogManager { get; set; }

        #endregion Properties

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];
            var fileExtention = fileData.Extention;

            this.InitLog(fileData.FileName);

            this.InitDictionaries();

            var message = this.ReadData(fileData.Data, fileExtention);

            if (!string.IsNullOrEmpty(message))
            {
                return new ImportResult(StatusImport.CompletedWithError, message);
            }

            this.SaveData();

            WriteLogs();
           
            this.LogManager.Add(fileData, this.logImport);
            this.LogManager.Save();

            message += this.LogManager.GetInfo();
            var status = LogManager.CountError > 0 ? StatusImport.CompletedWithError : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
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
            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] { this.PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void InitLog(string fileName)
        {
            if (this.LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogManager.FileNameWithoutExtention = fileName;
            this.LogManager.UploadDate = DateTime.Now;

            this.logImport = this.Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (this.logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.logImport.SetFileName(fileName);
            this.logImport.ImportKey = this.Key;
        }

        private void InitDictionaries()
        {
            contragentDict = ContragentRepository.GetAll()
                .Where(x => x.ContragentState == ContragentState.Active)
                .Select(x => new { x.Id, x.Name, x.Inn })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(
                    x => x.Key, 
                    x => x.GroupBy(y => y.Inn ?? "").ToDictionary(y => y.Key, y => y.First().Id));

            realtyObjectDict = RealityObjectRepository.GetAll()
                .Where(x => x.ExternalId != null)
                .Select(x => new { x.Id, x.ExternalId })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.ExternalId))
                .GroupBy(x => x.ExternalId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            manOrgByContragentDict = ManagingOrganizationRepository.GetAll()
                .Where(x => x.Contragent != null)
                .Select(x => new
                    {
                        x.Id,
                        ContragentId = x.Contragent.Id,
                        x.TypeManagement
                    })
                .AsEnumerable()
                .GroupBy(x => x.ContragentId)
                .ToDictionary(
                    x => x.Key, 
                    x =>
                    { 
                        var first = x.First();

                        return new ManagingOrganization
                                   {
                                       Id = first.Id,
                                       TypeManagement = first.TypeManagement
                                   };
                    });

            exisingManOrgRobjectDictByMo = ManagingOrgRealityObjectRepository.GetAll()
                .Where(x => x.ManagingOrganization != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new { roId = x.RealityObject.Id, manOrgId = x.ManagingOrganization.Id })
                .AsEnumerable()
                .GroupBy(x => x.manOrgId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.roId).ToList());

            exisingManOrgContractRobjectDictByMo = ManOrgContractRealityObjectRepository.GetAll()
                .Where(x => x.RealityObject != null)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Select(x => new { roId = x.RealityObject.Id, manOrgId = x.ManOrgContract.ManagingOrganization.Id })
                .AsEnumerable()
                .GroupBy(x => x.manOrgId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.roId).ToList());

            existingDirectManagementRoList = ManOrgContractRealityObjectRepository.GetAll()
                .Where(x => x.RealityObject != null)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                .Select(x => x.RealityObject.Id)
                .ToList();
        }

        private string InitHeader(GkhExcelCell[] data)
        {
            this.headersDict["EXTERNAL_ID_HOUSE"] = -1;
            this.headersDict["NAME_UK"] = -1;
            this.headersDict["INN"] = -1;
            this.headersDict["ADRES"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (this.headersDict.ContainsKey(header))
                {
                    this.headersDict[header] = index;
                }
            }

            var missingFields = this.headersDict.Where(x => x.Value == -1).Select(x => x.Key).ToList();

            var result = missingFields.Any() 
                ? string.Format("В файле отсутствуют заголовки: {0}", string.Join(", ", missingFields)) 
                : string.Empty;

            return result;
        }

        private string ReadData(byte[] fileData, string fileExtention)
        {
            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    if (fileExtention == "xlsx")
                    {
                        excel.UseVersionXlsx();
                    }

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    var message = this.InitHeader(rows.First());

                    if (!string.IsNullOrEmpty(message))
                    {
                        return message;
                    }

                    var list = new List<Record>();

                    for (var i = 1; i < rows.Count; ++i)
                    {
                        var row = rows[i];

                        var record = new Record { rowNum = i + 1 };
                        
                        record.ExternalId = this.GetValue(row, "EXTERNAL_ID_HOUSE");

                        if (string.IsNullOrWhiteSpace(record.ExternalId))
                        {
                            logDict[record.rowNum] = new Log { Success = false, Text = "Поле 'EXTERNAL_ID_HOUSE' пустое" };
                            continue;
                        }

                        if (!realtyObjectDict.ContainsKey(record.ExternalId))
                        {
                            logDict[record.rowNum] = new Log { Success = false, Text = "В системе нет дома с ExternalId = " + record.ExternalId };
                            continue;
                        }
                        else if (realtyObjectDict[record.ExternalId].Count > 1)
                        {
                            logDict[record.rowNum] = new Log { Success = false, Text = "В системе несколько домов с ExternalId = " + record.ExternalId };
                            continue;
                        }

                        record.ManOrgName = this.GetValue(row, "NAME_UK");

                        if (string.IsNullOrWhiteSpace(record.ManOrgName))
                        {
                            logDict[record.rowNum] = new Log { Success = false, Text = "Поле 'NAME_UK' пустое" };
                            continue;
                        }

                        record.Inn = this.GetValue(row, "INN");
                        record.Address = this.GetValue(row, "ADRES");
                        
                        list.Add(record);
                    }

                    records = list.GroupBy(x => x.ManOrgName.ToUpper())
                        .ToDictionary(
                             x => x.Key,
                             x => x.GroupBy(y => y.Inn.ToUpper())
                                   .ToDictionary(y => y.Key, y => y.ToList()));
                 
                }
            }

            return string.Empty;
        }

        private void SaveData()
        {
            if (records.ContainsKey("НЕПОСРЕДСТВЕННОЕ УПРАВЛЕНИЕ"))
            {
                records["НЕПОСРЕДСТВЕННОЕ УПРАВЛЕНИЕ"].ForEach(x => InTransaction(() => this.SaveRobjectDirectManagement(x.Value)));
            }

            records.Where(x => x.Key != "НЕПОСРЕДСТВЕННОЕ УПРАВЛЕНИЕ").ForEach(x => x.Value.ForEach(y => InTransaction(() => SaveManOrgData(y.Value))));
        }

        private void SaveManOrgData(List<Record> recs)
        {
            var contragentId = 0L;

            var firstRecord = recs.First();
            var manOrgName = firstRecord.ManOrgName;
            var inn = firstRecord.Inn;
            var manOrgAddress = firstRecord.Address;

            Action createContragent = () =>
                {
                    if (!(Utils.VerifyInn(inn, true) || !Utils.VerifyInn(inn, true)))
                    {
                        recs.ForEach(x => logDict[x.rowNum] = new Log { Success = false, Text = "Конрагент не создан. Указаный ИНН не корректен: " + inn });
                        return;
                    }

                    var contragent = new Contragent
                        {
                            Name = manOrgName,
                            Inn = inn,
                            AddressOutsideSubject = manOrgAddress,
                            ContragentState = ContragentState.Active,
                            ActivityGroundsTermination = GroundsTermination.NotSet
                        };

                    ContragentRepository.Save(contragent);

                    contragentDict[manOrgName] = new Dictionary<string, long> { { inn, contragent.Id } };

                    contragentId = contragent.Id;
                };
            
            if (contragentDict.ContainsKey(manOrgName.ToUpper()))
            {
                if (contragentDict[manOrgName.ToUpper()].Any(x => x.Key == inn))
                {
                    contragentId = contragentDict[manOrgName.ToUpper()][inn];
                }
                else
                {
                    createContragent();
                }
            }
            else
            {
                createContragent();
            }

            ManagingOrganization manOrg;

            if (manOrgByContragentDict.ContainsKey(contragentId))
            {
                manOrg = manOrgByContragentDict[contragentId];
            }
            else
            {
                var typeManagement = TypeManagementManOrg.UK;

                if (manOrgName.StartsWith("ТСЖ"))
                {
                    typeManagement = TypeManagementManOrg.TSJ;
                }
                else if (manOrgName.StartsWith("ЖСК"))
                {
                    typeManagement = TypeManagementManOrg.JSK;
                }

                manOrg = new ManagingOrganization
                    {
                        Contragent = new Contragent { Id = contragentId },
                        TypeManagement = typeManagement,
                        ActivityGroundsTermination = GroundsTermination.NotSet,
                        MemberRanking = YesNoNotSet.NotSet,
                        OrgStateRole = OrgStateRole.Active
                    };

                ManagingOrganizationRepository.Save(manOrg);

                manOrgByContragentDict[contragentId] = manOrg;
            }

            var typeContract = manOrg.TypeManagement == TypeManagementManOrg.JSK || manOrg.TypeManagement == TypeManagementManOrg.TSJ
                ? TypeContractManOrg.JskTsj
                : TypeContractManOrg.ManagingOrgOwners;

            SaveManOrgRoRelation(manOrg.Id, recs, typeContract);
        }

        private void SaveRobjectDirectManagement(List<Record> recs)
        {
            foreach (var record in recs)
            {
                var roId = realtyObjectDict[record.ExternalId].First();

                if (!existingDirectManagementRoList.Contains(roId))
                {
                    var contract = new RealityObjectDirectManagContract
                        {
                            TypeContractManOrgRealObj = TypeContractManOrg.DirectManag,
                            StartDate = new DateTime(2014, 1, 1)
                        };
                 
                    RealityObjectDirectManagContractRepository.Save(contract);
                    
                    var directmanageContract = new ManOrgContractRealityObject
                    {
                        ManOrgContract = contract,
                        RealityObject = new RealityObject { Id = roId }
                    };

                    ManOrgContractRealityObjectRepository.Save(directmanageContract);
                    existingDirectManagementRoList.Add(roId);

                    logDict[record.rowNum] = new Log { Success = true, Text = "Создано непосредственное управление" };
                }
                else
                {
                    logDict[record.rowNum] = new Log { Success = false, Text = "Дом уже в непосредственном управлении" };
                }
            }
        }

        private void SaveManOrgRoRelation(long manOrgId, List<Record> recs, TypeContractManOrg typeContract)
        {
            var existingRobjects = new List<long>();
            var existingRobjectContracts = new List<long>();

            if (exisingManOrgRobjectDictByMo.ContainsKey(manOrgId))
            {
                existingRobjects = exisingManOrgRobjectDictByMo[manOrgId];
            }

            if (exisingManOrgContractRobjectDictByMo.ContainsKey(manOrgId))
            {
                existingRobjectContracts = exisingManOrgContractRobjectDictByMo[manOrgId];
            }

            foreach (var record in recs)
            {
                var roId = realtyObjectDict[record.ExternalId].First();

                if (!existingRobjects.Contains(roId))
                {
                    var manOrgRo = new ManagingOrgRealityObject
                        {
                            ManagingOrganization = new ManagingOrganization { Id = manOrgId },
                            RealityObject = new RealityObject { Id = roId }
                        };

                    ManagingOrgRealityObjectRepository.Save(manOrgRo);
                    existingRobjects.Add(roId);
                }
                
                if (!existingRobjectContracts.Contains(roId))
                {
                    ManOrgBaseContract contract;

                    if (typeContract == TypeContractManOrg.JskTsj)
                    {
                        contract = new ManOrgJskTsjContract
                        {
                            ManagingOrganization = new ManagingOrganization { Id = manOrgId },
                            TypeContractManOrgRealObj = typeContract,
                            StartDate = new DateTime(2014, 1, 1)
                        };

                        ManOrgJskTsjContractRepository.Save(contract);
                    }
                    else
                    {
                        var manOrgContractOwnersContract = new ManOrgContractOwners
                        {
                            ManagingOrganization = new ManagingOrganization { Id = manOrgId },
                            TypeContractManOrgRealObj = typeContract,
                            StartDate = new DateTime(2014, 1, 1)
                        };

                        ManOrgContractOwnersRepository.Save(manOrgContractOwnersContract);

                        contract = manOrgContractOwnersContract;
                    }
                    
                    var manOrgRoContract = new ManOrgContractRealityObject
                    {
                        ManOrgContract = contract,
                        RealityObject = new RealityObject { Id = roId }
                    };

                    ManOrgContractRealityObjectRepository.Save(manOrgRoContract);
                    existingRobjectContracts.Add(roId);

                    logDict[record.rowNum] = new Log { Success = true, Text = "Добавлено управление" };
                }
                else
                {
                    logDict[record.rowNum] = new Log { Success = false, Text = "Дом уже управляется данным УО" };
                }
            }
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

        private void WriteLogs()
        {
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                if (log.Value.Success)
                {
                    this.logImport.CountAddedRows++;
                }
                else
                {
                    this.logImport.CountWarning++;
                }
                
                this.logImport.Info("Строка: " + log.Key , log.Value.Text);
            }
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
    }
}
