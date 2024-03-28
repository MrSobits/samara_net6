namespace Bars.Gkh.Gis.DomainService.Dict.Impl
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Gkh.Domain;
    using Gkh.Entities;
    using Entities.Kp50;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы со справочником услуг биллинга
    /// </summary>
    public class BilServiceDictionaryService : IBilServiceDictionaryService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        ///  Получить список дополнительных услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список дополнительных услуг</returns>
        public IDataResult ListAdditionalService(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var manorgId = loadParams.Filter.GetAsId("manorgId");

            var manOrgRepository = this.Container.ResolveDomain<ManagingOrganization>();
            var manOrgStorageRepository = this.Container.ResolveDomain<BilManOrgStorage>();
            var domain = this.Container.ResolveDomain<BilServiceDictionary>();

            try
            {
                var manOrg = manOrgRepository.Get(manorgId);

                if (manOrg == null || manOrg.Contragent == null)
                {
                    return new ListDataResult();
                }

                var schemaIds = manOrgStorageRepository.GetAll()
                    .Where(x => x.Schema != null)
                    .Where(x => x.ManOrgInn == manOrg.Contragent.Inn && x.ManOrgKpp == manOrg.Contragent.Kpp)
                    .Select(x => x.Schema.Id)
                    .ToList();

                var data = domain.GetAll()
                    .Where(x => x.Schema != null)
                    .Where(x => x.ServiceTypeCode != 1 && x.ServiceTypeCode != 2)
                    .ToList()
                    .Where(x => schemaIds.Contains(x.Schema.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.ServiceName,
                            x.MeasureName
                        })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(manOrgRepository);
                this.Container.Release(manOrgStorageRepository);
            }
        }

        /// <summary>
        /// Получить список коммунальных услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список дополнительных услуг</returns>
        public IDataResult ListCommunalService(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var manorgId = loadParams.Filter.GetAsId("manorgId");

            var manOrgRepository = this.Container.ResolveDomain<ManagingOrganization>();
            var manOrgStorageRepository = this.Container.ResolveDomain<BilManOrgStorage>();
            var domain = this.Container.ResolveDomain<BilServiceDictionary>();

            try
            {
                var manOrg = manOrgRepository.Get(manorgId);

                if (manOrg == null || manOrg.Contragent == null)
                {
                    return new ListDataResult();
                }

                var schemaIds = manOrgStorageRepository.GetAll()
                    .Where(x => x.Schema != null)
                    .Where(x => x.ManOrgInn == manOrg.Contragent.Inn && x.ManOrgKpp == manOrg.Contragent.Kpp)
                    .Select(x => x.Schema.Id)
                    .ToList();

                var result = domain.GetAll()
                .Where(x => x.ServiceTypeCode == 2 && x.Schema != null)
                .ToList()
                .Where(x => schemaIds.Contains(x.Schema.Id))
                .Select(x => new
                {
                    x.Id,
                    x.IsOdnService,
                    x.OrderNumber,
                    x.ServiceName
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(manOrgRepository);
                this.Container.Release(manOrgStorageRepository);
            }
        }

        /// <summary>
        /// Получить список работ и услуг
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список работ и услуг</returns>
        public IDataResult ListServiceWork(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var manorgId = loadParams.Filter.GetAsId("manorgId");

            var domain = this.Container.ResolveDomain<BilServiceDictionary>();
            var manOrgRepository = this.Container.ResolveDomain<ManagingOrganization>();
            var manOrgStorageRepository = this.Container.ResolveDomain<BilManOrgStorage>();

            try
            {
                var manOrg = manOrgRepository.Get(manorgId);

                if (manOrg == null || manOrg.Contragent == null)
                {
                    return new ListDataResult();
                }

                var schemaIds = manOrgStorageRepository.GetAll()
                    .Where(x => x.Schema != null)
                    .Where(x => x.ManOrgInn == manOrg.Contragent.Inn && x.ManOrgKpp == manOrg.Contragent.Kpp)
                    .Select(x => x.Schema.Id)
                    .ToList();

                var data = domain.GetAll()
                    .Where(x => x.Schema != null)
                .Where(x => x.ServiceTypeCode == 1)
                .ToList()
                .Where(x => schemaIds.Contains(x.Schema.Id))
                .Select(x => new
                {
                    x.Id,
                    x.ServiceName,
                    x.MeasureName,
                    x.ServiceCode
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(manOrgRepository);
                this.Container.Release(manOrgStorageRepository);
            }
        }
    }
}
