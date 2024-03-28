namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using B4;

    using Bars.GkhGji.Entities;

    public class PrescriptionViolationServiceInterceptor : EmptyDomainInterceptor<PrescriptionViol>
    {
        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            // Была задумка проставлять даты по цепочке 
            // Но в томске надобность в этом отпала
            /*
            var datePlanRemovalStageMax = ViolStageDomain.GetAll()
               .Where(x => x.InspectionViolation.Inspection.Id == entity.InspectionViolation.Id)
               .Max(x => x.DatePlanRemoval);

            var dateDateFactRemovalStageMax = ViolStageDomain.GetAll()
               .Where(x => x.InspectionViolation.Inspection.Id == entity.InspectionViolation.Id)
               .Max(x => x.DateFactRemoval);

            var viol = InspectionViolDomain.GetAll().FirstOrDefault(x => x.Id == entity.InspectionViolation.Id);
            if (viol != null)
            {
                viol.DateFactRemoval = dateDateFactRemovalStageMax > entity.DateFactRemoval ? dateDateFactRemovalStageMax : entity.DateFactRemoval;
                viol.DatePlanRemoval = datePlanRemovalStageMax > entity.DatePlanRemoval ? datePlanRemovalStageMax : entity.DatePlanRemoval;

                InspectionViolDomain.Update(viol);    
            }
            */
            
            return Success();
        }
    }
}