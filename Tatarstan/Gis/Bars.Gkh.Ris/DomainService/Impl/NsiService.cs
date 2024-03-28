namespace Bars.Gkh.Ris.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Gis.Entities.Kp50;
    using Bars.Gkh.Gis.Entities.ManOrg;
    using Bars.Gkh.Ris.Extractors.Nsi;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис Nsi
    /// </summary>
    public class NsiService : INsiService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список дополнительных услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetAdditionalServices(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<BilServiceDictionary>>("AdditionalServicesSelector");

            try
            {
                var addServList = extractor.GetExternalEntities(baseParams.Params);

                var loadParams = baseParams.GetLoadParam();

                var data = addServList.Select(x => new
                {
                    x.Id,
                    x.ServiceName,
                    UnitMeasure = x.MeasureName
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Получить список коммунальных услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetMunicipalServices(BaseParams baseParams)
        {
            var extractor = (MunicipalServicesDataExtractor)this.Container.Resolve<IGisIntegrationDataExtractor>("MunicipalServicesDataExtractor");

            try
            {
                var addServList = extractor.GetMunicipalServices(baseParams.Params);

                var loadParams = baseParams.GetLoadParam();

                var data = addServList.Select(x => new
                {
                    x.Id,
                    x.Name,
                    UnitMeasure = x.UnitMeasure.Name
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Получить список работ и услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetOrganizationWorks(BaseParams baseParams)
        {
            var extractor = (OrganizationWorksDataExtractor)this.Container.Resolve<IDataSelector<ManOrgBilWorkService>>("OrganizationWorksDataSelector");

            try
            {
                var orgWorkList = extractor.GetEntitiesByContragent();

                var loadParams = baseParams.GetLoadParam();

                var data = orgWorkList.Select(x => new
                {
                    x.Id,
                    Name = x.BilService != null
                        ? x.BilService.ServiceName
                        : string.Empty,
                    MeasureName = x.BilService != null
                        ? x.BilService.MeasureName
                        : string.Empty,
                    x.Description
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }
    }
}