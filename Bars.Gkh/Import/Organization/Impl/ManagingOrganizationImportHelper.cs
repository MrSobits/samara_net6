namespace Bars.Gkh.Import.Organization.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhExcel;

    using Castle.Core;

    using NHibernate;

    /// <summary>
    /// Хелпер импорта организаций для УО, домав УО и договоров между УО и домом
    /// </summary>
    public class ManagingOrganizationImportHelper : BaseOrganizationImportHelper, IOrganizationImportHelper, IInitializable
    {
        public ManagingOrganizationImportHelper(
            IOrganizationImportCommonHelper commonHelper,
            IDictionary<string, long> headersDict,
            ILogImport logImport)
            : base(commonHelper, headersDict, logImport)
        {
        }

        public sealed class ContragentProxy
        {
            public string mixedKey;

            public string Name;
        }
        
        public IRepository<ManagingOrganization> ManagingOrganizationRepository { get; set; }

        public IRepository<ManagingOrgRealityObject> ManagingOrgRealityObjectRepository { get; set; }

        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }
        
        private List<ManagingOrganization> managingOrganizationToCreate = new List<ManagingOrganization>();

        private List<ManagingOrgRealityObject> managingOrgRealityObjectToCreate = new List<ManagingOrgRealityObject>();

        private List<ManOrgContractRealityObject> manOrgContractRealityObjectToCreate = new List<ManOrgContractRealityObject>();

        private List<ManOrgContractOwners> manOrgContractOwnersToCreate = new List<ManOrgContractOwners>();

        private List<ManOrgJskTsjContract> manOrgJskTsjContractToCreate = new List<ManOrgJskTsjContract>();

        private List<ManOrgBaseContract> manOrgContractOwnersToUpdate = new List<ManOrgBaseContract>();

        // Связь дома и Ук
        private Dictionary<string, List<long>> manOrgRo;

        // Договор между ук и домом
        private Dictionary<string, List<long>> manOrgRoContract;

        private Dictionary<string, List<ManagingOrganization>> manOrgByContragentMixedKeyDict;

        private Dictionary<long, ContragentProxy> existingManagingOrganizations;

        public string OrganizationType { get { return "ук"; } }

        public void Initialize()
        {
            this.existingManagingOrganizations = this.ManagingOrganizationRepository.GetAll()
                .Select(x => new { x.Id, x.Contragent.Inn, x.Contragent.Kpp, x.Contragent.Name })
                .AsEnumerable()
                .ToDictionary(
                    x => x.Id,
                    x => new ContragentProxy
                    {
                        mixedKey = string.Format(
                            "{0}#{1}",
                            (x.Inn ?? string.Empty).Trim(),
                            (x.Kpp ?? string.Empty).Trim()).ToLower(),
                        Name = x.Name
                    });

            this.manOrgByContragentMixedKeyDict = this.ManagingOrganizationRepository.GetAll()
                 .Select(x => new
                 {
                     x.Contragent.Inn,
                     x.Contragent.Kpp,
                     ManagingOrganization = x
                 })
                .AsEnumerable()
                .Select(x => new
                {
                    x.ManagingOrganization,
                    mixedkey = string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ManagingOrganization).ToList()); 

            // Связь дома и Ук
            this.manOrgRo = this.ManagingOrgRealityObjectRepository.GetAll()
                .Where(x => x.ManagingOrganization != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.ManagingOrganization.Contragent.Inn,
                    x.ManagingOrganization.Contragent.Kpp,
                    x.ManagingOrganization.Id,
                    roId = x.RealityObject.Id
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.roId,
                    mixedkey = string.Format(
                        "{0}#{1}#{2}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim(),
                        x.Id).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.roId).ToList());

            // Договор между ук и домом
            this.manOrgRoContract = this.ManOrgContractRealityObjectRepository.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                    x.ManOrgContract.ManagingOrganization.Contragent.Kpp,
                    x.ManOrgContract.ManagingOrganization.Id,
                    roId = x.RealityObject.Id
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.roId,
                    mixedkey = string.Format(
                        "{0}#{1}#{2}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim(),
                        x.Id).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.roId).ToList());
        }

        public void SaveData(IStatelessSession session)
        {
            this.managingOrganizationToCreate.ForEach(x => session.Insert(x));
            this.managingOrgRealityObjectToCreate.ForEach(x => session.Insert(x));
            this.manOrgContractOwnersToCreate.ForEach(x => session.Insert(x));
            this.manOrgJskTsjContractToCreate.ForEach(x => session.Insert(x));
            this.manOrgContractRealityObjectToCreate.ForEach(x => session.Insert(x));
            this.manOrgContractOwnersToUpdate.ForEach(x => session.Update(x));
        }

        public override IDataResult ProcessLine(GkhExcelCell[] data, Record record)
        {
            var result = base.ProcessLine(data, record);

            if (!result.Success)
            {
                return result;
            }

            var rowNumber = record.RowNumber.ToString();
            //не совсем понятно, зачем нужна эта проверка, возможно, что в регионах бывает ситуация,
            //когда используется этот хелпер, и там другой OrgType,
            if (record.OrganizationType == OrgType.ManagingOrganization)
            {
                var managementType = this.GetValue(data, "TYPE_COM");
                if (string.IsNullOrEmpty(managementType))
                {
                    this.WriteLog(rowNumber, "Не задан тип управления.", LogMessageType.Error);
                }
                else
                {
                    switch (managementType.ToLower())
                    {
                        case "ук":
                            record.TypeManagement = TypeManagementManOrg.UK;
                            break;

                        case "тсж":
                            record.TypeManagement = TypeManagementManOrg.TSJ;
                            break;

                        case "жск":
                            record.TypeManagement = TypeManagementManOrg.JSK;
                            break;

                        default:
                            var msg = "Неизвестный тип управления: " + managementType;
                            this.WriteLog(rowNumber, msg, LogMessageType.Error);
                            return BaseDataResult.Error(msg);
                    }
                }
            }
            return new BaseDataResult();
        }
        
        protected override void GetOrganizationId(Record record)
        {
            record.OrganizationId = 0;

            if (this.existingManagingOrganizations.ContainsKey(record.ImportOrganizationId))
            {
                record.OrganizationId = record.ImportOrganizationId;

                var data = this.existingManagingOrganizations[record.ImportOrganizationId];

                record.Organization = new ManagingOrganization { Id = record.OrganizationId };
                record.Contragent = new Contragent { Name = data.Name };
                record.ContragentMixedKey = data.mixedKey;
                record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, record.OrganizationId);
            }
        }

        protected override IDataResult CreateOrSetOrganization(Record record)
        {
            if (this.manOrgByContragentMixedKeyDict.ContainsKey(record.ContragentMixedKey))
            {
                var manOrgs = this.manOrgByContragentMixedKeyDict[record.ContragentMixedKey];

                if (manOrgs.Count > 1)
                {
                    var msg = "Указанный контрагент имеет несколько управляющих организаций: "
                           + string.Join(", ", manOrgs.Where(x => x.Contragent != null).Select(x => x.Contragent.Name));

                    return BaseDataResult.Error(msg);
                }
                else
                {
                    var manOrg = manOrgs.First();
                    record.Organization = manOrg;

                    // Поиск по существующим записям (контракты и связи с домом) будем искать по ключу ИНН#КПП#ИдОрганизации
                    record.ContragentOrganizationMixedKey = $"{record.ContragentMixedKey}#{manOrg.Id}";
                }
            }
            else
            {
                // create
                var managingOrganization = new ManagingOrganization
                {
                    Contragent = record.Contragent,
                    TypeManagement = record.TypeManagement,
                    OrgStateRole = OrgStateRole.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.managingOrganizationToCreate.Add(managingOrganization);

                record.Organization = managingOrganization;

                this.manOrgByContragentMixedKeyDict[record.ContragentMixedKey] = new List<ManagingOrganization> { managingOrganization };

                // Поиск по добавленным во время импорта записям (контракты и связи с домом) будем искать по ключу ИНН#КПП, т.к. ИдОрганизации еще нет.
                record.ContragentOrganizationMixedKey = record.ContragentMixedKey;
            }

            return new BaseDataResult();
        }

        protected override void CreateContractIfNotExist(Record record, bool updatePeriodsManOrgs)
        {
            // 1. Создать связь между домом и организацией

            var managingOrgRealityObject = new ManagingOrgRealityObject
            {
                ManagingOrganization = record.Organization as ManagingOrganization,
                RealityObject = new RealityObject { Id = record.RealtyObjectId },
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            if (this.manOrgRo.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var manOrgRobjects = this.manOrgRo[record.ContragentOrganizationMixedKey];
                if (!manOrgRobjects.Contains(record.RealtyObjectId))
                {
                    this.managingOrgRealityObjectToCreate.Add(managingOrgRealityObject);
                    
                    manOrgRobjects.Add(record.RealtyObjectId);
                }
            }
            else
            {
                this.managingOrgRealityObjectToCreate.Add(managingOrgRealityObject);

                this.manOrgRo[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }

            // 2. Создать договор между домом и организацией
            
            Action createContract = () =>
            {
                string typeContractName;
                ManOrgBaseContract contract;

                if (record.TypeManagement == TypeManagementManOrg.UK)
                {
                    typeContractName = TypeContractManOrg.ManagingOrgOwners.GetEnumMeta().Display;

                    DateTime? endDateContract = null;

                    if (updatePeriodsManOrgs)
                    {
                        var manOrgs = this.ManOrgContractRealityObjectService.GetAll()
                            .Where(x => x.RealityObject.Id == record.RealtyObjectId)
                            .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                            .OrderByDescending(x => x.ManOrgContract.StartDate)
                            .Select(x => x.ManOrgContract)
                            .ToList();

                        var manOrgBeforeStartDate = manOrgs?.FirstOrDefault(x => x.StartDate < record.ContractStartDate);
                        var manOrgAfterStartDate  = manOrgs?.FirstOrDefault(x => x.StartDate > record.ContractStartDate);

                        manOrgBeforeStartDate.EndDate = record.ContractStartDate.AddDays(-1);
                        endDateContract = manOrgAfterStartDate?.StartDate?.AddDays(-1);

                        this.manOrgContractOwnersToUpdate.Add(manOrgBeforeStartDate);
                    }

                    var manOrgContractOwners = new ManOrgContractOwners
                    {
                        ManagingOrganization = record.Organization as ManagingOrganization,
                        TypeContractManOrgRealObj = TypeContractManOrg.ManagingOrgOwners,
                        StartDate = record.ContractStartDate,
                        EndDate = endDateContract,
                        DocumentDate = record.DocumentDate,
                        DocumentNumber = record.DocumentNumber,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now
                    };

                    this.manOrgContractOwnersToCreate.Add(manOrgContractOwners);

                    contract = manOrgContractOwners;
                }
                else
                {
                    typeContractName = TypeContractManOrg.JskTsj.GetEnumMeta().Display;

                    var manOrgJskTsjContract = new ManOrgJskTsjContract
                    {
                        ManagingOrganization = record.Organization as ManagingOrganization,
                        TypeContractManOrgRealObj = TypeContractManOrg.JskTsj,
                        StartDate = record.ContractStartDate,
                        DocumentDate = record.DocumentDate,
                        DocumentNumber = record.DocumentNumber,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now
                    };

                    this.manOrgJskTsjContractToCreate.Add(manOrgJskTsjContract);

                    contract = manOrgJskTsjContract;
                }

                var manOrgContractRealityObject = new ManOrgContractRealityObject
                {
                    ManOrgContract = contract,
                    RealityObject = new RealityObject { Id = record.RealtyObjectId },
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.manOrgContractRealityObjectToCreate.Add(manOrgContractRealityObject);

                // Обновить уо и вид управления дома
                this.CommonHelper.UpdateRealtyObjectManOrg(
                    roId: record.RealtyObjectId,
                    contractStartDate: record.ContractStartDate,
                    manOrgName: record.Contragent.Name,
                    manInn: record.Contragent.Inn,
                    startControlDate: contract.StartDate?.ToString("dd.MM.YYYY"),
                    typeContract: typeContractName);
            };

            if (this.manOrgRoContract.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var manOrgContracts = this.manOrgRoContract[record.ContragentOrganizationMixedKey];
                if (!manOrgContracts.Contains(record.RealtyObjectId))
                {
                    createContract();

                    manOrgContracts.Add(record.RealtyObjectId);
                }
            }
            else
            {
                createContract();

                this.manOrgRoContract[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }
        }
    }
}