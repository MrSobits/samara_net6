using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FIAS;
using Bars.B4.Modules.States;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums.Import;
using Bars.Gkh.Helpers;
using Bars.Gkh.Import.FiasHelper;
using Bars.Gkh.Import.Impl;
using Castle.Core.Internal;
using Bars.Gkh.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bars.Gkh.Import
{
    public class ManagingOrganizationImport : GkhImportBase
    {
        #region Properties

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        public IDomainService<ManOrgBaseContract> ManOrgBaseContractDomain { get; set; }

        public IDomainService<ManOrgContractOwners> ManOrgContractOwnersDomain { get; set; }

        public IDomainService<ManagingOrgRealityObject> ManagingOrgRealityObjectDomain { get; set; }

        #endregion

        #region fields

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key => Id;

        public override string CodeImport => "ManagingOrganizationImport";

        public override string Name => "Импорт лицензий";

        public override string PossibleFileExtensions => "xlsx";

        public override string PermissionName => "Import.ManagingOrganization.View";

        #endregion

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

        protected override ImportResult Import(BaseParams baseParams,
            B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var file = baseParams.Files["FileImport"];
            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = CodeImport;

            //парсинг
            Indicate(0, "Парсинг файла");
            try
            {
                var records = Parce(file, ct);
                int i = 0;
                foreach (var record in records)
                {
                    Indicate(++i * 100 / records.Count, $"Обработка записи {i} из {records.Count}");
                    try
                    {
                        Process(record, LogImport, ct);
                    }
                    catch (Exception e)
                    {
                        LogImport.Error("Ошибка обработки записи", $"Строка {record.N} Ошибка обработчика: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                LogImportManager.FileNameWithoutExtention = file.FileName;
                LogImportManager.Add(file, LogImport);
                LogImportManager.Save();
                return new ImportResult(StatusImport.CompletedWithError, e.Message + '\n' + e.StackTrace);
            }

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0 ? StatusImport.CompletedWithError :
                        LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning :
                        StatusImport.CompletedWithoutError;

            return new ImportResult(statusImport, string.Format("Импортировано {0} записей", LogImport.CountAddedRows), string.Empty, LogImportManager.LogFileId);
        }

        private List<ImportRecord> Parce(FileData file, CancellationToken ct)
        {
            var result = new List<ImportRecord>();

            var importHelper = Container.Resolve<IImportHelper>();

            using (var xlsMemoryStream = new MemoryStream(file.Data))
            {
                //---открытие файла---
                if (file.Extention == "xlsx")
                {
                    importHelper.Excel.UseVersionXlsx();
                }

                importHelper.Excel.Open(xlsMemoryStream);

                Indicate(0, "Поиск заголовка");
                var headerRow = importHelper.FindHeader(20, 3);

                //---заголовки---
                Indicate(0, "Сборка словаря заголовков");
                var headers = importHelper.GetHeaders();

                //---данные---
                var count = importHelper.Excel.GetRowsCount(0, 0);
                for (int i = headerRow + 1; i < count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    if (String.IsNullOrWhiteSpace(importHelper.Read<string>(i, "Код дома по ФИАС", true)))
                        continue;

                    Indicate(i * 100 / count, $"Парсинг записи {i - headerRow} из {count - headerRow}");

                    result.Add(new ImportRecord()
                    {
                       N = i + 1,
                       LicenseNumber = importHelper.Read<string>(i, "Номер лицензии", true),
                       LicenseDate = importHelper.ReadDateTime(i, "Дата лицензии", true),
                       IsActual = importHelper.Read<string>(i, "Статус лицензии", true) == "Действующая",
                       LicenseState = importHelper.Read<string>(i, "Статус лицензии", true),
                       LicenseIncludingDate = importHelper.ReadDateTime(i, "Дата включения лицензии в реестр", true, "dd.MM.yyyy HH:mm"),
                       DirectiveNumber = importHelper.Read<string>(i, "Номер приказа (распоряжения)", true),
                       DirectiveDate = importHelper.ReadDateTime(i, "Дата приказа (распоряжения)", true),
                       LicenseAddress = importHelper.Read<string>(i, "Адрес осуществления лицензируемого вида деятельности", true),
                       FIAS = importHelper.ReadGuid(i, "Код по ФИАС", true),
                       Name = importHelper.Read<string>(i, "Наименование/ ФИО лицензиата", true),
                       Inn = importHelper.Read<long>(i, "ИНН", true),
                       Ogrn = importHelper.Read<long>(i, "ОГРН/ ОГРНИП", true),
                       HouseFias = importHelper.ReadtGuidNullable(i, "Код дома по ФИАС", true),
                       HouseStartManagement = importHelper.ReadDateTimeNullable(i, "Дата начала полномочий по управлению домом", true),
                       HouseEndManagement = importHelper.ReadDateTimeNullable(i, "Дата окончания полномочий по управлению домом", false),
                       HouseExcludeREason = importHelper.Read<string>(i, "Основание исключения", true)                       
                    });
                }
            }

            return result;
        }

        private void Process(ImportRecord record, ILogImport logImport, CancellationToken ct)
        {
            try { 

            ct.ThrowIfCancellationRequested();

            //-----сопоставление контрагента-----
            var contragent = FindContragent(record.N, record.Inn, record.Ogrn, record.Name, logImport, record.FIAS);
            if (contragent == null)
            {
               contragent = CreateContragent(record);
            }

            var managingOrganization = GetManagingOrganization(contragent, record, logImport);

            //-----сопоставление лицензии-----
            //var license = FindLicense(record.N, contragent, record.LicenseNumber, logImport);
            //if (license == null)
            //{
            //    license = CreateNewLicense(contragent, record, logImport);
            //    if (license == null)
            //        return;
            //}

            //-----сопоставление дома-----
            var house = FindHouse(record.N, record.HouseFias, record.HouseNumber, logImport);
            if (house == null)
                return;

            //-----Жилой дом управляющей организации-----
            if (!ManagingOrgRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject.Id == house.Id)
                .Where(x => x.ManagingOrganization.Id == managingOrganization.Id)
                .Any())
            {
                ManagingOrgRealityObjectDomain.Save(new ManagingOrgRealityObject
                {
                    RealityObject = house,
                    ManagingOrganization = managingOrganization
                });
            }
            var manOrgBaseContractRepo = Container.Resolve<IRepository<ManOrgBaseContract>>();
            var manOrgContractOwnersRepo = Container.Resolve<IRepository<ManOrgContractOwners>>();

            var contracts = ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject == house)
                .Select(x => x.ManOrgContract)
                .ToList();

            if (!record.HouseEndManagement.HasValue)
            {
                //-----управляшка из экселины - текущая. Дом должен быть в ее управлении-----
                var currentContracts = contracts.Where(x => !x.StartDate.HasValue || x.StartDate.Value <= DateTime.Now)
                                                           .Where(x => !x.EndDate.HasValue)
                                                           .ToArray();

                if (currentContracts.Count() > 1)
                {
                    logImport.Error("Ошибка поиска текущей УО", $"Строка {record.N} - У дома {house.Address} {currentContracts.Count()} текущих контрактов управления: {string.Join(", ", currentContracts.Select(x => x.DocumentNumber))}");
                    return;
                }
                else if (currentContracts.Count() == 1)
                {
                    var currentContract = currentContracts[0];
                    if (currentContract.ManagingOrganization == managingOrganization)
                    {
                        //текущая управляшка совпадает с экселиной - все норм
                        return;
                    }

                    //закрыть текущее управление
                    if (!record.HouseStartManagement.HasValue)
                    {
                        logImport.Error("Ошибка присвоения текущей УО", $"Строка {record.N} - Для закрытия старого контракта должна быть проставлено значение в столбце: дата начала полномочий по управлению домом");
                        return;
                    }
                    currentContract.EndDate = record.HouseStartManagement.Value.AddDays(-1);
                    ManOrgBaseContractDomain.Update(currentContract);
                }
                //ManOrgContractRealityObjectDomain.GetAll().FirstOrDefault().ManOrgContract
                //создаем договор нового управления
                string docnumber = string.Empty;
                if (!string.IsNullOrEmpty(record.Reason) && record.Reason.Contains("№"))
                {
                    docnumber = record.Reason.Split('№')[1].Trim();

                    var pattern = @"(?<=_)(.*?)(?=-1)";
                    var input = record.Reason.Split('№')[0].Trim();
                    if (Regex.IsMatch(input, pattern))
                    {
                        var dateStr = Regex.Match(input, pattern);
                        var date = DateTime.ParseExact(dateStr.Value, "MM-dd-yyyy", null);
                    }
                }

                var newContract = new ManOrgContractOwners
                {
                    DocumentName = "реестр лицензий субъекта",
                    ContractFoundation = Enums.ManOrgContractOwnersFoundation.OwnersMeetingProtocol,
                    DocumentNumber = docnumber,
                    TypeContractManOrgRealObj = Enums.TypeContractManOrg.ManagingOrgOwners,
                    ContractStopReason = Enums.ContractStopReasonEnum.is_not_filled,
                    ManagingOrganization = managingOrganization,
                    StartDate = record.HouseStartManagement,
                    Note = "Создано автоматически при импорте лицензий",
                    RegisterReason = record.Reason
                };

                manOrgContractOwnersRepo.Save(newContract);

                ManOrgContractRealityObjectDomain.Save(new ManOrgContractRealityObject
                {
                    RealityObject = house,
                    ManOrgContract = newContract
                });

                logImport.CountAddedRows++;

            }
            else
            {
                //-----управляшка из экселины не текущая - просто чекнуть историю-----
                if (contracts.Where(x => x.ManagingOrganization == managingOrganization).Any())
                {
                    //запись есть - все норм
                    return;
                }

                string docnumber = string.Empty;
                if (!string.IsNullOrEmpty(record.Reason) && record.Reason.Contains("№"))
                {
                    docnumber = record.Reason.Split('№')[1].Trim();
                }

                //создаем новый договор
                var newContract = new ManOrgContractOwners
                {
                    DocumentName = "реестр лицензий суьъекта",
                    DocumentNumber = docnumber,
                    TypeContractManOrgRealObj = Enums.TypeContractManOrg.ManagingOrgJskTsj,
                    ContractStopReason = Enums.ContractStopReasonEnum.finished_contract,
                    ManagingOrganization = managingOrganization,
                    StartDate = record.HouseStartManagement,
                    EndDate = record.HouseEndManagement,
                    Note = "Создано автоматически при импорте лицензий",
                    RegisterReason = record.Reason
                };
                manOrgContractOwnersRepo.Save(newContract);

                    ManOrgContractRealityObjectDomain.Save(new ManOrgContractRealityObject
                    {
                        RealityObject = house,
                        ManOrgContract = newContract
                    });

                    logImport.CountAddedRows++;
            }
        }
            catch (Exception e)
            {
                logImport.Error("Ошибка обработки записи", record.FIAS + " системная ошибка " + e.Message);
            }
        }

        private Contragent CreateContragent(ImportRecord record)
        {
            var contragentRepo = Container.Resolve<IRepository<Contragent>>();
            FiasAddress newFiasAddress = CreateOrFindFiasAddress(record.FIAS);//LicenseAddress
            Contragent newcontragent = new Contragent
            {
                Ogrn = record.Ogrn.ToString(),
                IsSite = false,
                Inn = record.Inn.ToString(),
                Name = record.Name,
                ShortName = record.Name,
                FiasFactAddress = newFiasAddress,
                FiasJuridicalAddress = newFiasAddress,
                JuridicalAddress = record.LicenseAddress,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 14,
                FiasMailingAddress = newFiasAddress,
                Description = "Создан импортом лицензий",
                TypeEntrepreneurship = Gkh.Enums.TypeEntrepreneurship.NotSet,
                ActivityGroundsTermination = Gkh.Enums.GroundsTermination.NotSet,
                ContragentState = Gkh.Enums.ContragentState.Active
            };          
            ManagingOrganization manorg = new ManagingOrganization
            {
                Contragent = newcontragent,
                ActivityGroundsTermination = Enums.GroundsTermination.NotSet,
                Description = "Создан импортом лицензий",
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 14,
                TypeManagement = Enums.TypeManagementManOrg.UK

            };
            this.Container.InTransaction(() =>
            {
                contragentRepo.Save(newcontragent);
                ManagingOrganizationDomain.Save(manorg);
            });
            return newcontragent;
        }

        private RealityObject FindHouse(int rownumber, Guid? houseFias, string houseNumber, ILogImport logImport)
        {
            if (houseFias == null )
            {
                logImport.Warn("Не заполнена информация о доме", $"Строка {rownumber} - Не указан ФИАС дома");
                return null;
            }

            RealityObject[] houses;
            //поиск по фиасу
            if (houseFias != null)
            {
                houses = RealityObjectDomain.GetAll().Where(x => x.FiasAddress.HouseGuid == houseFias).ToArray();
                if (houses.Count() > 1)
                {
                    RealityObject house = houses.Where(x => x.FiasAddress.House == houseNumber).FirstOrDefault();
                    if (house != null)
                    {
                        return house;
                    }
                    logImport.Error("Ошибка сопоставления дома", $"Строка {rownumber} - Найдено {houses.Count()} домов с HouseGuid {houseFias}");
                    return null;
                }
                else if (houses.Count() == 1)
                {
                    return houses[0];
                }
                else
                {
                    logImport.Warn("Ошибка сопоставления дома", $"Строка {rownumber} - Не найден дом с HouseGuid {houseFias} ");
                }
            }

            return null;
        }

        private Contragent FindContragent(int rownumber, long inn, long ogrn, string name, ILogImport logImport, Guid? ctrFias)
        {
            if (inn == 0 || ogrn == 0)
            {
                logImport.Error("Не заполнена информация о контрагенте", $"Строка {rownumber} - Не указан ИНН, ОГРН или наименование контрагента");
                return null;
            }

            Contragent[] contragents;

            if (inn != 0)
            {
                contragents = ContragentDomain.GetAll().Where(x => x.Parent == null).Where(x => x.Inn == inn.ToString()).ToArray();
                if (contragents.Count() > 1)
                {
                    logImport.Error("Ошибка сопоставления контрагента", $"Строка {rownumber} - Найдено {contragents.Count()} контрагентов с ОГРН {ogrn}");
                    Contragent ctr =  contragents.Where(x => x.ContragentState == Enums.ContragentState.Active).FirstOrDefault();
                    if (ctr != null)
                    {
                        return ctr;
                    }
                    return null;
                }
                else if (contragents.Count() == 1)
                {
                    return contragents[0];
                }
                else
                {
                    logImport.Warn("Ошибка сопоставления контрагента", $"Строка {rownumber} - Не найден контрагент с ИНН {inn}");
                }
            }

            //if (inn != 0)
            //{
            //    contragents = ContragentDomain.GetAll().Where(x => x.Parent == null).Where(x => x.Inn == inn.ToString()).ToArray();
            //    if (contragents.Count() > 1)
            //    {
            //        logImport.Error("Ошибка сопоставления контрагента", $"Строка {rownumber} - Найдено {contragents.Count()} контрагентов с ИНН {inn}");
            //        return null;
            //    }
            //    else if (contragents.Count() == 1)
            //    {
            //        return contragents[0];
            //    }
            //    else
            //    {
            //        logImport.Warn("Ошибка сопоставления контрагента", $"Строка {rownumber} - Не найден контрагент с ОГРН {ogrn} и ИНН {inn} - попытка добавить контрагента <{name}>");
            //        try
            //        {
            //           FiasAddress newFiasAddress = CreateOrFindFiasAddress(ctrFias);

            //            Contragent newcontragent = new Contragent
            //            {
            //                Ogrn = ogrn.ToString(),
            //                IsSite = false,
            //                Inn = inn.ToString(),
            //                Name = name,
            //                ShortName = name,
            //                FiasFactAddress = newFiasAddress,
            //                FiasJuridicalAddress = newFiasAddress,
            //                ObjectCreateDate = DateTime.Now,
            //                ObjectEditDate = DateTime.Now,
            //                ObjectVersion = 14,
            //                FiasMailingAddress = newFiasAddress,
            //                Description = "Создан импортом лицензий",
            //                TypeEntrepreneurship = Gkh.Enums.TypeEntrepreneurship.NotSet,
            //                ActivityGroundsTermination = Gkh.Enums.GroundsTermination.NotSet,
            //                ContragentState = Gkh.Enums.ContragentState.Active
            //            };
            //            var contragentContainer = Container.Resolve<IRepository<Contragent>>();
            //            contragentContainer.Save(newcontragent);
            //            //ManagingOrganization manorg = new ManagingOrganization
            //            //{
            //            //    Contragent = newcontragent,
            //            //    ActivityGroundsTermination = Enums.GroundsTermination.NotSet,
            //            //    Description = "Создан импортом лицензий",
            //            //    ObjectCreateDate = DateTime.Now,
            //            //    ObjectEditDate = DateTime.Now,
            //            //    ObjectVersion = 14,
            //            //    TypeManagement = Enums.TypeManagementManOrg.UK

            //            //};
            //            //ManagingOrganizationDomain.Save(manorg);
            //            return newcontragent;
            //        }
            //        catch (Exception e)
            //        {
            //            logImport.Warn("Попытка не удалась", $"{e.Message}, значит запись по контрагенту {name} добавлена не будет");
            //        }
            //    }
            //}

            //if (!String.IsNullOrEmpty(name))
            //{
            //    contragents = ContragentDomain.GetAll().Where(x => x.Parent == null).Where(x => x.Name == name).ToArray();
            //    if (contragents.Count() > 1)
            //    {
            //        logImport.Error("Ошибка сопоставления контрагента", $"Строка {rownumber} - Найдено {contragents.Count()} контрагентов с названием <{name}>");
            //        return null;
            //    }
            //    else if (contragents.Count() == 1)
            //    {
            //        return contragents[0];
            //    }
            //    else
            //    {
            //        logImport.Error("Ошибка сопоставления контрагента", $"Строка {rownumber} - Не найден контрагент с ОГРН {ogrn}, ИНН {inn} и названием <{name}>");
            //    }
            //}

            return null;
        }

        private FiasAddress CreateOrFindFiasAddress(Guid? ctrFias)
        {
            var fiasCont = Container.Resolve<IDomainService<FiasAddress>>();
            var fiasAddressByHg = fiasCont.GetAll().Where(x => x.HouseGuid == ctrFias).FirstOrDefault();

            if (!string.IsNullOrEmpty(ctrFias.ToString()) && fiasAddressByHg == null)
            {
                var fiasHouseContainer = Container.Resolve<IDomainService<FiasHouse>>();
                var fiasHouse = fiasHouseContainer.GetAll().Where(x => x.HouseGuid == ctrFias).FirstOrDefault();
                if (fiasHouse == null)
                {
                    return null;
                }
                var fiasRecorg = Container.Resolve<IDomainService<Fias>>().GetAll().Where(x => x.AOGuid == fiasHouse.AoGuid.ToString() && x.ActStatus == FiasActualStatusEnum.Actual).FirstOrDefault();
                var streetName = string.IsNullOrWhiteSpace(fiasRecorg.ShortName)
                                 ? fiasRecorg.OffName
                                 : string.Format("{0}. {1}", fiasRecorg.ShortName, fiasRecorg.OffName);
                DynamicAddress address = new DynamicAddress
                {
                    AddressGuid = string.Format("{0}_{1}", (byte)fiasRecorg.AOLevel, fiasRecorg.AOGuid),
                    AddressName = streetName,
                    PostCode = fiasRecorg.PostalCode,
                    Name = streetName,
                    Code = fiasRecorg.CodeRecord,
                    GuidId = fiasRecorg.AOGuid
                };

                var addressName = new StringBuilder(address.AddressName);
                string letter = "";
                string building = "";
                string housing = "";
                if (!string.IsNullOrEmpty(fiasHouse.HouseNum))
                {
                    addressName.Append(", д. ");
                    addressName.Append(fiasHouse.HouseNum);

                    if (fiasHouse.StructureType == FiasStructureTypeEnum.Letter)
                    {
                        addressName.Append(", лит. ");
                        addressName.Append(fiasHouse.StrucNum);
                        letter = fiasHouse.StrucNum;
                    }

                    if (fiasHouse.StructureType == FiasStructureTypeEnum.Construction)
                    {
                        addressName.Append(", корп. ");
                        addressName.Append(fiasHouse.StrucNum);
                        housing = fiasHouse.StrucNum;
                    }

                    if (fiasHouse.StructureType == FiasStructureTypeEnum.Structure)
                    {
                        addressName.Append(", секц. ");
                        addressName.Append(fiasHouse.StrucNum);
                        building = fiasHouse.StrucNum;
                    }
                }


                var fiasAddress = new FiasAddress
                {
                    AddressGuid = address.AddressGuid,
                    AddressName = addressName.ToString(),
                    PostCode = address.PostCode,
                    StreetGuidId = address.GuidId,
                    StreetName = address.Name,
                    StreetCode = address.Code,
                    Letter = letter,
                    House = fiasHouse.HouseNum,
                    Housing = housing,
                    Building = building,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 14,
                    PlaceAddressName = address.AddressName.Replace(address.Name, string.Empty).Trim(' ').Trim(','),
                    PlaceGuidId = address.ParentGuidId,
                    PlaceName = address.ParentName,
                    PlaceCode = address.PlaceCode
                };

                fiasCont.Save(fiasAddress);

                return fiasAddress;
            }
            else if (fiasAddressByHg != null)
            {
                return fiasAddressByHg;
            }
            else
            { return null; }
        }

        private ManOrgLicense FindLicense(int rownumber, Contragent contragent, int licenseNumber, ILogImport logImport)
        {
            var licenses = LicenseDomain.GetAll()
                .Where(x => x.LicNum == licenseNumber)
                .Where(x => x.Contragent == contragent)
                .ToArray();

            if (licenses.Count() == 0)
            {
                return null;
            }
            if (licenses.Count() > 1)
            {
                logImport.Warn("Ошибка сопоставления лицензии", $"Строка {rownumber} - Найдено {licenses.Count()} лицензий у контрагента {contragent.Name} с номером {licenseNumber}");
            }
            return licenses[0];
        }

        private ManOrgLicense CreateNewLicense(Contragent contragent, ImportRecord record, ILogImport logImport)
        {
            var license = new ManOrgLicense
            {
                LicNum = 1,
                LicNumber = record.LicenseNumber.ToString(),
                DateIssued = record.LicenseDate,
                DisposalNumber = record.DirectiveNumber,
                DateDisposal = record.DirectiveDate,
                DateRegister = record.LicenseIncludingDate,
                State = GetLicenseActualState(),
                Contragent = contragent,
            };

            LicenseDomain.Save(license);
            return license;
        }

        State licenseActualState = null;
        private State GetLicenseActualState()
        {
            if (licenseActualState != null)
                return licenseActualState;

            licenseActualState = StateDomain.GetAll()
                .Where(x => x.TypeId == "gkh_manorg_license")
                .Where(x => x.StartState)
                .Where(x => !x.FinalState)
                .FirstOrDefault();

            if (licenseActualState != null)
                return licenseActualState;

            licenseActualState = new State
            {
                Name = "Действующая",
                Code = "001",
                TypeId = "gkh_manorg_license",
                StartState = true,
                FinalState = false,
                OrderValue = 0,
                Description = ""
            };

            StateDomain.Save(licenseActualState);
            return licenseActualState;
        }

        /// <summary>
        /// Получить управляшку по контрагенту
        /// </summary>
        private ManagingOrganization GetManagingOrganization(Contragent contragent, ImportRecord record, ILogImport logImport)
        {
            var manorgs = ManagingOrganizationDomain.GetAll().Where(x => x.Contragent == contragent).ToArray();

            if (manorgs.Count() == 1)
                return manorgs[0];
            else if (manorgs.Count() > 1)
            {
                //если нашлось больше, отбираем по статусам
                manorgs = manorgs.Where(x => x.OrgStateRole == Enums.OrgStateRole.Active).ToArray();
                if (manorgs.Count() > 0)
                    return manorgs[0];
            }

            var newmanorg = new ManagingOrganization
            {
                Contragent = contragent,
                OrgStateRole = Enums.OrgStateRole.Active,
                Description = "Создана автоматически при импорте лицензий",
                TypeManagement = Enums.TypeManagementManOrg.UK, //TODO: распознавание формы управляшки по имени
                MemberRanking = Enums.YesNoNotSet.NotSet,
                IsTransferredManagementTsj = false,
                OfficialSite731 = false,
                ActivityGroundsTermination = Enums.GroundsTermination.NotSet,
                IsDispatchCrrespondedFact = false
            };

            ManagingOrganizationDomain.Save(newmanorg);
            return newmanorg;
        }

        private class ImportRecord
        {
            /// <summary>
            /// Номер лицензии
            /// </summary>
            public string LicenseNumber { get; set; }

            /// <summary>
            /// Статус лицензии
            /// </summary>
            public string LicenseState { get; set; }

            /// <summary>
            /// Дата лицензии
            /// </summary>
            public DateTime LicenseDate { get; set; }

            /// <summary>
            /// Статус лицензии
            /// </summary>
            public bool IsActual { get; set; }

            /// <summary>
            /// Дата включения лицензии в реестр
            /// </summary>
            public DateTime LicenseIncludingDate { get; set; }

            /// <summary>
            /// Номер приказа (распоряжения)
            /// </summary>
            public string DirectiveNumber { get; set; }

            /// <summary>
            /// Дата приказа (распоряжения)
            /// </summary>
            public DateTime DirectiveDate { get; set; }

            /// <summary>
            /// Адрес осуществления лицензируемого вида деятельности
            /// </summary>
            public string LicenseAddress { get; set; }

            /// <summary>
            ///Основание включения
            /// </summary>
            public string Reason { get; set; }

            /// <summary>
            ///№ дома
            /// </summary>
            public string HouseNumber { get; set; }

            /// <summary>
            /// Код по ФИАС
            /// </summary>
            public Guid FIAS { get; set; }

            /// <summary>
            /// Наименование/ ФИО лицензиата
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// ИНН
            /// </summary>
            public long Inn { get; set; }

            /// <summary>
            /// ОГРН/ ОГРНИП
            /// </summary>
            public long Ogrn { get; set; }

            /// <summary>
            /// Адрес многоквартирного дома
            /// </summary>
            public string HouseAddress { get; set; }

            /// <summary>
            /// Код дома по ФИАС
            /// </summary>
            public Guid? HouseFias { get; set; }

            /// <summary>
            /// Дата включения дома в реестр
            /// </summary>
            public DateTime? HouseInclude { get; set; }

            /// <summary>
            /// Дата начала полномочий по управлению домом
            /// </summary>
            public DateTime? HouseStartManagement { get; set; }

            /// <summary>
            /// Дата окончания полномочий по управлению домом
            /// </summary>
            public DateTime? HouseEndManagement { get; set; }

            /// <summary>
            /// Дата исключения дома из реестра
            /// </summary>
            public DateTime? HouseExclude { get; set; }

            /// <summary>
            /// Основание исключения
            /// </summary>
            public string HouseExcludeREason { get; set; }

            /// <summary>
            /// Сведения по ст. 198 ЖК выгружены
            /// </summary>
            public bool Is198 { get; set; }

            /// <summary>
            /// Информация в реестре объектов жилищного фонда выгружена
            /// </summary>
            public bool IsRo { get; set; }

            /// <summary>
            /// Номер строки
            /// </summary>
            public int N { get; set; }
        }
    }
}


