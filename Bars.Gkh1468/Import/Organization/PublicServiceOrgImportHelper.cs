namespace Bars.Gkh1468.Import.Organization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Organization;
    using Bars.Gkh.Import.Organization.Impl;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.Entities;

    using Castle.Core;

    using NHibernate;

    /// <summary>
    /// Хелпер импорта организаций для поставщиков ресурсов (ПР), домов ПР и договоров между ПР и домом
    /// </summary>
    public class PublicServiceOrgImportHelper : BaseOrganizationImportHelper, IOrganizationImportHelper, IInitializable
    {
        public PublicServiceOrgImportHelper(
            IOrganizationImportCommonHelper commonHelper,
            IDictionary<string, long> headersDict,
            ILogImport logImport)
            : base(commonHelper, headersDict, logImport)
        {
        }

        public string OrganizationType { get { return "поставщик ресурсов"; } }

        public IRepository<PublicServiceOrg> PublicServiceOrgRepository { get; set; }

        public IRepository<PublicServiceOrgRealtyObject> PublicServiceOrgRealtyObjectRepository { get; set; }

        public IRepository<PublicServiceOrgContractRealObj> PublicServiceOrgContractRealObjRepository { get; set; }

        private List<PublicServiceOrg> publicServiceOrgToCreate = new List<PublicServiceOrg>();

        private List<PublicServiceOrgRealtyObject> publicServiceOrgRealtyObjectToCreate = new List<PublicServiceOrgRealtyObject>();

        private List<PublicServiceOrgContract> realObjPublicServiceOrgToCreate = new List<PublicServiceOrgContract>();

        private Dictionary<string, List<PublicServiceOrg>> publicServiceOrgByContragentMixedKeyDict;

        // Связь дома и поставщика ресурсов
        private Dictionary<string, List<long>> publicServiceOrgRo;

        // Договор между домом и поставщиком ресурсов
        private Dictionary<string, List<long>> publicServiceOrgRoContract;

        private Dictionary<long, string> existingPublicServiceOrgs;

        public void Initialize()
        {
            this.existingPublicServiceOrgs = this.PublicServiceOrgRepository.GetAll()
                .Select(x => new { x.Id, x.Contragent.Inn, x.Contragent.Kpp })
                .AsEnumerable()
                .ToDictionary(
                    x => x.Id,
                    x => string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower());

            this.publicServiceOrgByContragentMixedKeyDict = this.PublicServiceOrgRepository.GetAll()
                .Select(x => new
                {
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    PublicServiceOrg = x
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.PublicServiceOrg,
                    mixedkey = string.Format(
                        "{0}#{1}",
                        (x.Inn ?? string.Empty).Trim(),
                        (x.Kpp ?? string.Empty).Trim()).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => y.PublicServiceOrg).ToList()); 

            // Связь дома и поставщика ресурсов
            this.publicServiceOrgRo = this.PublicServiceOrgRealtyObjectRepository.GetAll()
                .Where(x => x.PublicServiceOrg != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.PublicServiceOrg.Contragent.Inn,
                    x.PublicServiceOrg.Contragent.Kpp,
                    x.PublicServiceOrg.Id,
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

            // Договор между домом и поставщиком ресурсов
            this.publicServiceOrgRoContract = this.PublicServiceOrgContractRealObjRepository.GetAll()
                .Where(x => x.RsoContract.PublicServiceOrg != null)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                {
                    x.RsoContract.PublicServiceOrg.Contragent.Inn,
                    x.RsoContract.PublicServiceOrg.Contragent.Kpp,
                    x.RsoContract.PublicServiceOrg.Id,
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

        protected override void GetOrganizationId(Record record)
        {
            record.OrganizationId = 0;

            if (this.existingPublicServiceOrgs.ContainsKey(record.ImportOrganizationId))
            {
                record.OrganizationId = record.ImportOrganizationId;


                record.Organization = new PublicServiceOrg { Id = record.OrganizationId };
                record.ContragentMixedKey = this.existingPublicServiceOrgs[record.ImportOrganizationId];
                record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, record.OrganizationId);
            }
        }

        protected override IDataResult CreateOrSetOrganization(Record record)
        {
            if (this.publicServiceOrgByContragentMixedKeyDict.ContainsKey(record.ContragentMixedKey))
            {
                var publicServiceOrgs = this.publicServiceOrgByContragentMixedKeyDict[record.ContragentMixedKey];

                if (publicServiceOrgs.Count > 1)
                {
                    var msg = "Указанный контрагент имеет несколько поставщиков ресурсов: "
                           + string.Join(", ", publicServiceOrgs.Where(x => x.Contragent != null).Select(x => x.Contragent.Name));

                    return BaseDataResult.Error(msg);
                }
                else
                {
                    var publicServiceOrg = publicServiceOrgs.First();
                    record.Organization = publicServiceOrg;

                    // Поиск по существующим записям (контракты и связи с домом) будем искать по ключу ИНН#КПП#ИдОрганизации
                    record.ContragentOrganizationMixedKey = string.Format("{0}#{1}", record.ContragentMixedKey, publicServiceOrg.Id);
                }
            }
            else
            {
                // create
                var publicServiceOrg = new PublicServiceOrg
                {
                    Contragent = record.Contragent,
                    OrgStateRole = OrgStateRole.Active,
                    ActivityGroundsTermination = GroundsTermination.NotSet,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.publicServiceOrgToCreate.Add(publicServiceOrg);

                record.Organization = publicServiceOrg;

                this.publicServiceOrgByContragentMixedKeyDict[record.ContragentMixedKey] = new List<PublicServiceOrg> { publicServiceOrg };

                // Поиск по добавленным во время импорта записям (контракты и связи с домом) будем искать по ключу ИНН#КПП, т.к. ИдОрганизации еще нет.
                record.ContragentOrganizationMixedKey = record.ContragentMixedKey;
            }

            return new BaseDataResult();
        }

        protected override void CreateContractIfNotExist(Record record, bool updatePeriodsManOrgs = false)
        {
            // 1. Создать связь между домом и организацией

            var publicServiceOrgRealtyObject = new PublicServiceOrgRealtyObject
            {
                PublicServiceOrg = record.Organization as PublicServiceOrg,
                RealityObject = new RealityObject { Id = record.RealtyObjectId },
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            if (this.publicServiceOrgRo.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var publicServiceOrgRobjects = this.publicServiceOrgRo[record.ContragentOrganizationMixedKey];

                if (!publicServiceOrgRobjects.Contains(record.RealtyObjectId))
                {
                    this.publicServiceOrgRealtyObjectToCreate.Add(publicServiceOrgRealtyObject);

                    publicServiceOrgRobjects.Add(record.RealtyObjectId);
                }
            }
            else
            {
                this.publicServiceOrgRealtyObjectToCreate.Add(publicServiceOrgRealtyObject);

                this.publicServiceOrgRo[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }

            // 2. Создать договор между домом и организацией

            var realObjPublicServiceOrg = new PublicServiceOrgContract
            {
                RealityObject = new RealityObject { Id = record.RealtyObjectId },
                PublicServiceOrg = record.Organization as PublicServiceOrg,
                ContractDate = record.DocumentDate,
                ContractNumber = record.DocumentNumber,
                DateStart = record.ContractStartDate,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now
            };

            if (this.publicServiceOrgRoContract.ContainsKey(record.ContragentOrganizationMixedKey))
            {
                var publicServiceOrgRoContracts = this.publicServiceOrgRoContract[record.ContragentOrganizationMixedKey];

                if (!publicServiceOrgRoContracts.Contains(record.RealtyObjectId))
                {
                    this.realObjPublicServiceOrgToCreate.Add(realObjPublicServiceOrg);

                    publicServiceOrgRoContracts.Add(record.RealtyObjectId);
                }
            }
            else
            {
                this.realObjPublicServiceOrgToCreate.Add(realObjPublicServiceOrg);

                this.publicServiceOrgRoContract[record.ContragentOrganizationMixedKey] = new List<long> { record.RealtyObjectId };
            }
        }
        
        public void SaveData(IStatelessSession session)
        {
            this.publicServiceOrgToCreate.ForEach(x => session.Insert(x));
            this.publicServiceOrgRealtyObjectToCreate.ForEach(x => session.Insert(x));
            this.realObjPublicServiceOrgToCreate.ForEach(x => session.Insert(x));
        }
    }
}