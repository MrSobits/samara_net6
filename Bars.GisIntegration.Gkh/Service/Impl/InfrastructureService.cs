namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.UI.Service;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис Infrastructure
    /// </summary>
    public class InfrastructureService : IInfrastructureService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список объектов ОКИ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetRkiList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<OkiObject>>("RkiItemDataSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var currentContragent = dataSupplierProvider.GetCurrentDataSupplier();
                baseParams.Params.Add("Contragent", currentContragent);

                var rkiList = extractor.GetExternalEntities(baseParams.Params);

                var loadParams = baseParams.GetLoadParam();

                var data = rkiList
                    .Select(x => new
                    {
                        x.Id,
                        Name = x.ObjectName,
                        Address = x.ObjectAddress?.AddressName ?? x.MoTerritory?.MoName,
                        TypeGroupName = x.OkiType?.OkiTypeGroup?.Name,
                        TypeName = x.OkiType?.Value
                    })
                .AsQueryable()
                .Order(loadParams)
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(dataSupplierProvider);
            }
        }
    }
}
