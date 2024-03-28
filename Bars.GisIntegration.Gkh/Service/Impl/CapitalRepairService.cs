namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис CapitalRepair
    /// </summary>
    public class CapitalRepairService : ICapitalRepairService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод получения списка МО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список МО</returns>
        public IDataResult GetMunicipalities(BaseParams baseParams)
        {
            var programCrSelector = this.Container.Resolve<IDataSelector<ProgramCrProxy>>("ProgramCrSelector");
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();

            try
            {
                var parameters = new DynamicDictionary();
                var loadParams = baseParams.GetLoadParam();

                var municipalityIds = programCrSelector.GetExternalEntities(parameters)
                    .Select(x => x.MunicipalityId)
                    .Distinct();

                var municipalities = municipalityDomain.GetAll()
                    .Where(x => municipalityIds.Contains(x.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Name
                        })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(municipalities.Order(loadParams).Paging(loadParams).ToList(), municipalities.Count());
            }
            finally 
            {
                this.Container.Release(programCrSelector);
                this.Container.Release(municipalityDomain);
            }
        }

        /// <summary>
        /// Получить список планов по кап. ремонту
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetCapitalRepairPlan(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var domain = this.Container.ResolveDomain<RisCrPlan>();
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();

                var data = domain.GetAll()
                    .Where(x => x.Contragent == currentContragent)
                    .Select(x => new
                    {
                        x.Id,
                        x.MunicipalityName,
                        x.Name,
                        x.StartMonthYear
                    })
                .Order(loadParams)
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(dataSupplierProvider);
            }
        }
    }
}
