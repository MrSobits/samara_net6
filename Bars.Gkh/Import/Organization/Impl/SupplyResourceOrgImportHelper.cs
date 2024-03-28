namespace Bars.Gkh.Import.Organization.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Core;

    using NHibernate;

    /// Хелпер импорта организаций для поставщиков коммунальных услуг (ПКУ), домов ПКУ и договоров между ПКУ и домом
    public class SupplyResourceOrgImportHelper : BaseOrganizationImportHelper, IOrganizationImportHelper, IInitializable
    {
        public SupplyResourceOrgImportHelper(
            IOrganizationImportCommonHelper commonHelper,
            IDictionary<string, long> headersDict,
            ILogImport logImport)
            : base(commonHelper, headersDict, logImport)
        {
        }

        public IRepository<SupplyResourceOrg> SupplyResourceOrgRepository { get; set; }

        public IRepository<SupplyResourceOrgRealtyObject> SupplyResourceOrgRealtyObjectRepository { get; set; }

        public IRepository<RealityObjectResOrg> RealityObjectResOrgRepository { get; set; }


        private Dictionary<long, string> existingSupplyResOrganizations;

        private Dictionary<string, List<SupplyResourceOrg>> supplyResOrgByContragentMixedKeyDict;


        private List<SupplyResourceOrg> supplyResourceOrgToCreate = new List<SupplyResourceOrg>();

        private List<SupplyResourceOrgRealtyObject> supplyResourceOrgRealtyObjectToCreate = new List<SupplyResourceOrgRealtyObject>();

        private List<RealityObjectResOrg> realityObjectResOrgToCreate = new List<RealityObjectResOrg>();

        // Связь дома и поставщика коммунальных услуг
        private Dictionary<string, List<long>> communalOrgRo;

        // Договор между домом и поставщиком коммунальных услуг
        private Dictionary<string, List<long>> communalOrgRoContract;


        public string OrganizationType { get { return "поставщик коммунальных услуг"; } }

        public void Initialize()
        {
            this.existingSupplyResOrganizations = this.SupplyResourceOrgRepository.GetAll()
                .Select(x => new { x.Id, x.Contragent.Inn, x.Contragent.Kpp })
                .AsEnumerable()
                .ToDictionary(
                    x => x.Id,
                    x => string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower());
            
            this.supplyResOrgByContragentMixedKeyDict = this.SupplyResourceOrgRepository.GetAll()
                .Select(x => new
                {
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    SupplyResourceOrg = x
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.SupplyResourceOrg,
                    mixedkey = string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.SupplyResourceOrg).ToList()); 

            // Связь дома и поставщика коммунальных услуг
            this.communalOrgRo = this.SupplyResourceOrgRealtyObjectRepository.GetAll()
                .Where(x => x.SupplyResourceOrg != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.SupplyResourceOrg.Contragent.Inn,
                    x.SupplyResourceOrg.Contragent.Kpp,
                    x.SupplyResourceOrg.Id,
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

            // Договор между домом и поставщиком коммунальных услуг
            this.communalOrgRoContract = this.RealityObjectResOrgRepository.GetAll()
                .Where(x => x.ResourceOrg != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.ResourceOrg.Contragent.Inn,
                    x.ResourceOrg.Contragent.Kpp,
                    x.ResourceOrg.Id,
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
            this.supplyResourceOrgToCreate.ForEach(x => session.Insert(x));
            this.supplyResourceOrgRealtyObjectToCreate.ForEach(x => session.Insert(x));
            this.realityObjectResOrgToCreate.ForEach(x => session.Insert(x));
        }
        
        protected override void GetOrganizationId(Record record)
        {
            record.OrganizationId = 0;

            if (this.existingSupplyResOrganizations.ContainsKey(record.ImportOrganizationId))
            {
                record.OrganizationId = record.ImportOrganizationId;

                record.Organization = new SupplyResourceOrg { Id = record.OrganizationId };
                record.ContragentMixedKey = this.existingSupplyResOrganizations[record.ImportOrganizationId];
                record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, record.OrganizationId);
            }
        }

        protected override IDataResult CreateOrSetOrganization(Record record)
        {
            if (this.supplyResOrgByContragentMixedKeyDict.ContainsKey(record.ContragentMixedKey))
            {
                var supplyResOrgs = this.supplyResOrgByContragentMixedKeyDict[record.ContragentMixedKey];

                if (supplyResOrgs.Count > 1)
                {
                    var msg = "Указанный контрагент имеет несколько поставщиков коммунальных услуг: "
                           + string.Join(", ", supplyResOrgs.Where(x => x.Contragent != null).Select(x => x.Contragent.Name));

                    return BaseDataResult.Error(msg);
                }
                else
                {
                    var supplyResOrg = supplyResOrgs.First();
                    record.Organization = supplyResOrg;

                    // Поиск по существующим записям (контракты и связи с домом) будем искать по ключу ИНН#КПП#ИдОрганизации
                    record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, supplyResOrg.Id);
                }
            }
            else
            {
                // create
                var supplyResourceOrg = new SupplyResourceOrg
                {
                    Contragent = record.Contragent,
                    OrgStateRole = OrgStateRole.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.supplyResourceOrgToCreate.Add(supplyResourceOrg);

                record.Organization = supplyResourceOrg;

                this.supplyResOrgByContragentMixedKeyDict[record.ContragentMixedKey] = new List<SupplyResourceOrg> { supplyResourceOrg };

                // Поиск по добавленным во время импорта записям (контракты и связи с домом) будем искать по ключу ИНН#КПП, т.к. ИдОрганизации еще нет.
                record.ContragentOrganizationMixedKey = record.ContragentMixedKey;
            }

            return new BaseDataResult();
        }

        protected override void CreateContractIfNotExist(Record record, bool updatePeriodsManOrgs = false)
        {
            // 1. Создать связь между домом и организацией

            var supplyResourceOrgRealtyObject = new SupplyResourceOrgRealtyObject
            {
                SupplyResourceOrg = record.Organization as SupplyResourceOrg,
                RealityObject = new RealityObject { Id = record.RealtyObjectId },
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            if (this.communalOrgRo.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var communalOrgRobjects = this.communalOrgRo[record.ContragentOrganizationMixedKey];

                if (!communalOrgRobjects.Contains(record.RealtyObjectId))
                {
                    this.supplyResourceOrgRealtyObjectToCreate.Add(supplyResourceOrgRealtyObject);
                    
                    communalOrgRobjects.Add(record.RealtyObjectId);
                }
            }
            else
            {
                this.supplyResourceOrgRealtyObjectToCreate.Add(supplyResourceOrgRealtyObject);

                this.communalOrgRo[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }

            // 2. Создать договор между домом и организацией
            var realityObjectResOrg = new RealityObjectResOrg
            {
                RealityObject = new RealityObject { Id = record.RealtyObjectId },
                ResourceOrg = record.Organization as SupplyResourceOrg,
                ContractDate = record.DocumentDate,
                ContractNumber = record.DocumentNumber,
                DateStart = record.ContractStartDate,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            if (this.communalOrgRoContract.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var communalOrgRoContracts = this.communalOrgRoContract[record.ContragentOrganizationMixedKey];

                if (!communalOrgRoContracts.Contains(record.RealtyObjectId))
                {
                    this.realityObjectResOrgToCreate.Add(realityObjectResOrg);
                    
                    communalOrgRoContracts.Add(record.RealtyObjectId);
                }
            }
            else
            {
                this.realityObjectResOrgToCreate.Add(realityObjectResOrg);

                this.communalOrgRoContract[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }
        }
    }
}