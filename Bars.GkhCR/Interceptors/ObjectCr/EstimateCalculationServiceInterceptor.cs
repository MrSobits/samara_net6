namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using B4;
    using B4.Modules.States;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;

    using Enums;
    using Entities;

    public class EstimateCalculationServiceInterceptor : EmptyDomainInterceptor<EstimateCalculation>
    {
        public override IDataResult BeforeCreateAction(IDomainService<EstimateCalculation> service, EstimateCalculation entity)
        {
            var res = CheckTypeWork(service, entity);

            if (!res.Success)
            {
                return Failure(res.Message);
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<EstimateCalculation> service, EstimateCalculation entity)
        {
            var res = CheckTypeWork(service, entity);

            if (!res.Success)
            {
                return Failure(res.Message);
            }

            return Success();
        }

        private IDataResult CheckTypeWork(IDomainService<EstimateCalculation> domain, EstimateCalculation entity)
        {
            IDataResult result = new BaseDataResult();
            var config = Container.GetGkhConfig<GkhCrConfig>();
            var cntEstimates = config.General.CountEstimatesByWork;
            if (entity.TypeWorkCr != null && cntEstimates == TypeWorkCrCountEstimations.OnlyOne)
            {
                var query =
                    domain.GetAll()
                          .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                          .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                          .Where(x => x.Id != entity.Id);

                var estimationTypeParam = config.General.EstimationTypeParam;
                if (entity.EstimationType != EstimationType.None && estimationTypeParam == EstimationTypeParam.Use)
                {
                    query = query.Where(x => x.EstimationType == entity.EstimationType);
                    if (query.Any())
                    {
                        result = new BaseDataResult(
                            false,
                            "По данному виду работ у текущего объекта КР уже существует запись с таким типом сметы!");
                    }
                }
                else
                {
                    // проверяем, есть ли у объекта кап ремонта другая смета с таким же типом работы
                    // проверка отключена, так как может быть несколько смет 
                    //if (query.Any())
                    //{
                    //    result = new BaseDataResult(
                    //        false,
                    //        "По данному виду работ у текущего объекта КР уже существует запись!");
                    //}
                }
            }

            return result;
        }

        public override IDataResult BeforeDeleteAction(IDomainService<EstimateCalculation> service, EstimateCalculation entity)
        {
            var estimateService = this.Container.Resolve<IDomainService<Estimate>>();
            var estimateCollection = estimateService.GetAll()
                    .Where(x => x.EstimateCalculation.Id == entity.Id)
                    .AsEnumerable();

            foreach (var estimate in estimateCollection)
            {
                estimateService.Delete(estimate.Id);
            }

            var resourceStatementService = this.Container.Resolve<IDomainService<ResourceStatement>>();
            var resourceCollection = resourceStatementService
                    .GetAll()
                    .Where(x => x.EstimateCalculation.Id == entity.Id)
                    .AsEnumerable();

            foreach (var resource in resourceCollection)
            {
                resourceStatementService.Delete(resource.Id);
            }

            return this.Success();
        }
    }
}