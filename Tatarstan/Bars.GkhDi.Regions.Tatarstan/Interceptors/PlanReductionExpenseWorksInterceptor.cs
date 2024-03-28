namespace Bars.GkhDi.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class PlanReductionExpenseWorksInterceptor : EmptyDomainInterceptor<PlanReductionExpenseWorks>
    {
        public IDomainService<PlanReduceMeasureName> PlanReduceMeasureNameDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<PlanReductionExpenseWorks> service, PlanReductionExpenseWorks entity)
        {
            PlanReduceMeasureNameDomain.GetAll().Where(x => x.PlanReductionExpenseWorks.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => PlanReduceMeasureNameDomain.Delete(x));

            return Success();
        } 
    }
}