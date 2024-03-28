namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Gkh.Entities.Dicts;
    using Overhaul.Entities;

    public class ListServicesDecisionInterceptor : EmptyDomainInterceptor<ListServicesDecision>
    {
        public IDomainService<StructuralElementWork> StructWorksDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public IDomainService<ListServiceDecisionWorkPlan> WorkPlanDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<ListServicesDecision> service, ListServicesDecision entity)
        {
            /*
             * Сохраняем работы дома
             */
            var roWorks =
                StructWorksDomain.GetAll()
                    .Where(x => RealityObjectStructuralElementDomain.GetAll()
                                .Any(y => y.RealityObject.Id == entity.RealityObject.Id
                                        && y.StructuralElement.Id == x.StructuralElement.Id))
                    .Select(x => x.Job.Work.Id)
                    .Distinct()
                    .ToList();

            foreach (var roWork in roWorks)
            {
                WorkPlanDomain.Save(new ListServiceDecisionWorkPlan
                {
                    ListServicesDecision = entity,
                    Work = new Work {Id = roWork}
                });
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ListServicesDecision> service, ListServicesDecision entity)
        {
            var anotherAvtualDecision = service.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                .Where(x => x.PropertyOwnerDecisionType == entity.PropertyOwnerDecisionType)
                .Where(x => x.Id != entity.Id)
                .FirstOrDefault(x => x.DateEnd == null);

            if (anotherAvtualDecision != null && entity.DateStart.HasValue)
            {
                anotherAvtualDecision.DateEnd = entity.DateStart.Value.AddDays(-1);
                service.Save(anotherAvtualDecision);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ListServicesDecision> service, ListServicesDecision entity)
        {
            WorkPlanDomain.GetAll()
                .Where(x => x.ListServicesDecision.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => WorkPlanDomain.Delete(x));

            return Success();
        }
    }
}
