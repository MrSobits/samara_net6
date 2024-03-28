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

    /// <summary>
    /// Хелпер импорта организаций для поставщиков жилищных услуг (ПЖУ), домов ПЖУ и договоров между ПЖУ и домом
    /// </summary>
    public class ServiceOrganizationImportHelper : BaseOrganizationImportHelper, IOrganizationImportHelper, IInitializable
    {
        public ServiceOrganizationImportHelper(
            IOrganizationImportCommonHelper commonHelper,
            IDictionary<string, long> headersDict,
            ILogImport logImport
        )
            : base(commonHelper, headersDict, logImport)
        {
        }

        public IRepository<ServiceOrganization> ServiceOrganizationRepository { get; set; }

        public IRepository<ServiceOrgRealityObject> ServiceOrgRealityObjectRepository { get; set; }

        public IRepository<ServiceOrgRealityObjectContract> ServiceOrgRealityObjectContractRepository { get; set; }


        private Dictionary<long, string> existingServiceOrganizations;

        private Dictionary<string, List<ServiceOrganization>> serviceOrgByContragentMixedKeyDict;
        

        private List<ServiceOrganization> serviceOrganizationsToCreate = new List<ServiceOrganization>();

        private List<ServiceOrgRealityObject> serviceOrgRealityObjectToCreate = new List<ServiceOrgRealityObject>();

        private List<ServiceOrgContract> serviceOrgContractToCreate = new List<ServiceOrgContract>();

        private List<ServiceOrgRealityObjectContract> serviceOrgRealityObjectContractToCreate = new List<ServiceOrgRealityObjectContract>();
        


        // Связь дома и поставщика жилищных услуг
        private Dictionary<string, List<long>> housingOrgRo;

        // Договор между домом и поставщиком жилищных услуг
        private Dictionary<string, List<long>> housingOrgRoContract;

        public string OrganizationType { get { return "поставщик жилищных услуг"; } }

        public void Initialize()
        {
            this.existingServiceOrganizations = this.ServiceOrganizationRepository.GetAll()
                .Select(x => new { x.Id, x.Contragent.Inn, x.Contragent.Kpp })
                .AsEnumerable()
                .ToDictionary(
                    x => x.Id, 
                    x => string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower());

            this.serviceOrgByContragentMixedKeyDict = this.ServiceOrganizationRepository.GetAll()
                .Select(x => new
                {
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    ServiceOrganization = x
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.ServiceOrganization,
                    mixedkey = string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ServiceOrganization).ToList());

            // Связь дома и поставщика жилищных услуг
            this.housingOrgRo = this.ServiceOrgRealityObjectRepository.GetAll()
                .Where(x => x.ServiceOrg != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.ServiceOrg.Contragent.Inn,
                    x.ServiceOrg.Contragent.Kpp,
                    x.ServiceOrg.Id,
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

            // Договор между домом и поставщиком жилищных услуг
            this.housingOrgRoContract = this.ServiceOrgRealityObjectContractRepository.GetAll()
                .Where(x => x.ServOrgContract.ServOrg != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new 
                { 
                    x.ServOrgContract.ServOrg.Contragent.Inn,
                    x.ServOrgContract.ServOrg.Contragent.Kpp, 
                    x.ServOrgContract.ServOrg.Id,
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
            this.serviceOrganizationsToCreate.ForEach(x => session.Insert(x));
            this.serviceOrgRealityObjectToCreate.ForEach(x => session.Insert(x));
            this.serviceOrgContractToCreate.ForEach(x => session.Insert(x));
            this.serviceOrgRealityObjectContractToCreate.ForEach(x => session.Insert(x));
        }
        
        protected override void GetOrganizationId(Record record)
        {
            record.OrganizationId = 0;

            if (this.existingServiceOrganizations.ContainsKey(record.ImportOrganizationId))
            {
                record.OrganizationId = record.ImportOrganizationId;

                record.Organization = new ServiceOrganization { Id = record.OrganizationId };
                record.ContragentMixedKey = this.existingServiceOrganizations[record.ImportOrganizationId];
                record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, record.OrganizationId);
            }
        }

        protected override IDataResult CreateOrSetOrganization(Record record)
        {
            if (this.serviceOrgByContragentMixedKeyDict.ContainsKey(record.ContragentMixedKey))
            {
                var serviceOrgs = this.serviceOrgByContragentMixedKeyDict[record.ContragentMixedKey];

                if (serviceOrgs.Count > 1)
                {
                    var msg = "Указанный контрагент имеет несколько поставщиков жилищный услуг: "
                           + string.Join(", ", serviceOrgs.Where(x => x.Contragent != null).Select(x => x.Contragent.Name));

                    return BaseDataResult.Error(msg);
                }
                else
                {
                    var serviceOrg = serviceOrgs.First();
                    record.Organization = serviceOrg;

                    // Поиск по существующим записям (контракты и связи с домом) будем искать по ключу ИНН#КПП#ИдОрганизации
                    record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, serviceOrg.Id);
                }
            }
            else
            {
                // create
                var serviceOrganization = new ServiceOrganization
                {
                    Contragent = record.Contragent,
                    OrgStateRole = OrgStateRole.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.serviceOrganizationsToCreate.Add(serviceOrganization);

                record.Organization = serviceOrganization;

                this.serviceOrgByContragentMixedKeyDict[record.ContragentMixedKey] = new List<ServiceOrganization> { serviceOrganization };

                // Поиск по добавленным во время импорта записям (контракты и связи с домом) будем искать по ключу ИНН#КПП, т.к. ИдОрганизации еще нет.
                record.ContragentOrganizationMixedKey = record.ContragentMixedKey;
            }

            return new BaseDataResult();
        }

        protected override void CreateContractIfNotExist(Record record, bool updatePeriodsManOrgs = false)
        {
            // 1. Создать связь между домом и организацией

            var serviceOrgRealityObject = new ServiceOrgRealityObject
            {
                ServiceOrg = record.Organization as ServiceOrganization,
                RealityObject = new RealityObject { Id = record.RealtyObjectId },
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            if (this.housingOrgRo.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var housingOrgRobjects = this.housingOrgRo[record.ContragentOrganizationMixedKey];

                if (!housingOrgRobjects.Contains(record.RealtyObjectId))
                {
                    this.serviceOrgRealityObjectToCreate.Add(serviceOrgRealityObject);
                    
                    housingOrgRobjects.Add(record.RealtyObjectId);
                }
            }
            else
            {
                this.serviceOrgRealityObjectToCreate.Add(serviceOrgRealityObject);

                this.housingOrgRo[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }

            // 2. Создать договор между домом и организацией
            
            Action createContract = () =>
            {
                var contract = new ServiceOrgContract
                {
                    ServOrg = record.Organization as ServiceOrganization,
                    DateStart = record.ContractStartDate,
                    DocumentDate = record.DocumentDate,
                    DocumentNumber = record.DocumentNumber,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.serviceOrgContractToCreate.Add(contract);

                var serviceOrgRealityObjectContract = new ServiceOrgRealityObjectContract
                {
                    ServOrgContract = contract,
                    RealityObject = new RealityObject { Id = record.RealtyObjectId },
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.serviceOrgRealityObjectContractToCreate.Add(serviceOrgRealityObjectContract);
            };

            if (this.housingOrgRoContract.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var housingOrgRoContracts = this.housingOrgRoContract[record.ContragentOrganizationMixedKey];
                if (!housingOrgRoContracts.Contains(record.RealtyObjectId))
                {
                    createContract();

                    housingOrgRoContracts.Add(record.RealtyObjectId);
                }
            }
            else
            {
                createContract();

                this.housingOrgRoContract[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }
        }
    }
}