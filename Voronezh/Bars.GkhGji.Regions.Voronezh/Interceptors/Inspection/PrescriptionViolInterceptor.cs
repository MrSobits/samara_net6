namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class PrescriptionViolInterceptor : EmptyDomainInterceptor<PrescriptionViol>
    {
        public IDomainService<InspectionGjiViol> InspectionGjiViolDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocGjiChildrenDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }
       
      
        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            if (entity.DateFactRemoval.HasValue)
            {
                var inspViol = InspectionGjiViolDomain.Get(entity.InspectionViolation.Id);
                inspViol.DateFactRemoval = entity.DateFactRemoval;
                InspectionGjiViolDomain.Update(inspViol);
            }
                var viols = PrescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == entity.Document.Id)
                    .Select(x => new
                    {
                        x.Id,
                        DatePlan = x.DatePlanExtension.HasValue ? x.DatePlanExtension : x.DatePlanRemoval,
                        x.DateFactRemoval
                    }).ToList();
                var intimecount = viols.Where(x => x.DateFactRemoval.HasValue && x.DateFactRemoval <= x.DatePlan).Select(x => x.Id).ToList().Count;
                var notExeccount = viols.Where(x => !x.DateFactRemoval.HasValue).Select(x => x.Id).ToList().Count;
                var notInTimeExecuted = viols.Where(x => x.DateFactRemoval.HasValue && x.DateFactRemoval > x.DatePlan).Select(x => x.Id).ToList().Count;

                if (viols.Count > 0)
                {
                    var prescr = PrescriptionDomain.Get(entity.Document.Id);
                    if (intimecount > 0 && (intimecount > notExeccount + notInTimeExecuted) && (intimecount < viols.Count))
                    {
                        prescr.TypePrescriptionExecution = GkhGji.Enums.TypePrescriptionExecution.Partially;
                    }
                    else if (intimecount > 0 && intimecount == viols.Count)
                    {
                        prescr.TypePrescriptionExecution = GkhGji.Enums.TypePrescriptionExecution.Executed;
                    }
                    else if (notInTimeExecuted > 0)
                    {
                        prescr.TypePrescriptionExecution = GkhGji.Enums.TypePrescriptionExecution.Violated;
                    }
                    else if (notInTimeExecuted > 0 && notInTimeExecuted <= viols.Count / 2)
                    {
                        prescr.TypePrescriptionExecution = GkhGji.Enums.TypePrescriptionExecution.NotExecuted;
                    }
                    else if (notExeccount > 0 && notExeccount > viols.Count / 2)
                    {
                        prescr.TypePrescriptionExecution = GkhGji.Enums.TypePrescriptionExecution.NotExecuted;
                    }
                    else
                    {
                        prescr.TypePrescriptionExecution = GkhGji.Enums.TypePrescriptionExecution.NotSet;
                    }

                PrescriptionDomain.Update(prescr);
                }

            return this.Success();
        }
    }
}