namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using System.Linq;

    public class EstimateCalculationInterceptor : EmptyDomainInterceptor<EstimateCalculation>
    {
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<EstimateCalculation> service, EstimateCalculation entity)
        {
            return CheckTotalEstimate(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<EstimateCalculation> service, EstimateCalculation entity)
        {
            return CheckTotalEstimate(service, entity);
        }

        private IDataResult CheckTotalEstimate(IDomainService<EstimateCalculation> service, EstimateCalculation entity)
        {
            if (entity.EstimationType == EstimationType.Customer)
            {
                return Success();
            }

            var typeWorkSum =
                TypeWorkCrDomain.GetAll()
                                .Where(x => x.Work.Id == entity.TypeWorkCr.Work.Id && x.ObjectCr.Id == entity.ObjectCr.Id)
                                .Select(x => x.Sum)
                                .Sum();

            var estCalcSum =
                service.GetAll()
                       .Where(x => x.TypeWorkCr.Work.Id == entity.TypeWorkCr.Work.Id)
                       .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                       .Where(x => x.Id != entity.Id)
                       .Where(x => x.EstimationType != EstimationType.Customer)
                       .Select(x => x.TotalEstimate)
                       .Sum();

            //if (estCalcSum.ToDecimal() + entity.TotalEstimate.ToDecimal() > typeWorkSum.ToDecimal() && !entity.State.StartState)
            //{
            //    return Failure("Итого по смете не должно превышать стоимость работы в краткосрочной программе!");
            //}

            return Success();
        }
    }
}