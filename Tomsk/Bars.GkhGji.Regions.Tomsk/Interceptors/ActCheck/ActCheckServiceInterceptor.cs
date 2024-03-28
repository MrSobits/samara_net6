namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    public class ActCheckServiceInterceptor : Bars.GkhGji.Interceptors.ActCheckServiceInterceptor
    {
        public IDomainService<ActCheckTime> ActCheckTimeDomain { get; set; }

        public IDomainService<ActCheckVerificationResult> ActCheckVerificationResultDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            ActCheckTimeDomain.GetAll()
                .Where(x => x.ActCheck.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => ActCheckTimeDomain.Delete(x));

            ActCheckVerificationResultDomain.GetAll()
                .Where(x => x.ActCheck.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => ActCheckVerificationResultDomain.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}