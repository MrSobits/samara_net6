namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.UI.Service;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис Inspection
    /// </summary>
    public class InspectionService : IInspectionService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список планов проверок
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetPlanList(BaseParams baseParams)
        {
            var extractor = this.Container.Resolve<IDataSelector<PlanJurPersonGji>>("InspectionPlanSelector");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                dataSupplierProvider.GetCurrentDataSupplier();

                baseParams.Params.Add("selectedList", "all");

                var plans = extractor.GetExternalEntities(baseParams.Params)
                    .Select(x =>
                    new
                    {
                        x.Id,
                        x.Name,
                        DateApproval = x.DateApproval ?? DateTime.MinValue,
                        DateStart = x.DateStart ?? DateTime.MinValue,
                        DateEnd = x.DateEnd ?? DateTime.MinValue
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(plans.Order(loadParams).Paging(loadParams).ToList(), plans.Count());
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(dataSupplierProvider);
            }
            return new BaseDataResult();
        }

        /// <summary>
        /// Получить список проверок (и их распоряжений)
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult GetInspectionList(BaseParams baseParams)
        {
            //todo Пока закомментировал, разобраться с модулем ГЖИ
            var examinationSelector = this.Container.Resolve<IDataSelector<Disposal>>("ExaminationExtractor");
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                dataSupplierProvider.GetCurrentDataSupplier(); // в качестве проверки текущего пользователя
                var loadParams = baseParams.GetLoadParam();

                baseParams.Params.Add("selectedList", "all");
                baseParams.Params.Add("fromWizard", true);

                var disposals = examinationSelector.GetExternalEntities(baseParams.Params).Select(
                        x => new
                        {
                            x.Id,
                            Base = x.Inspection.TypeBase.GetDisplayName(),
                            Number = x.Inspection.InspectionNumber ?? string.Empty,
                            x.DateStart,
                            x.DateEnd,
                            KindCheck = x.KindCheck?.Name ?? string.Empty,
                            ContragentName = x.Inspection.Contragent?.Name ?? string.Empty
                        })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(disposals.Order(loadParams).Paging(loadParams).ToList(), disposals.Count());
            }
            finally
            {
                this.Container.Release(examinationSelector);
                this.Container.Release(dataSupplierProvider);
            }
            return new BaseDataResult();
        }
    }
}
