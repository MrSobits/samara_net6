namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    public class PrescriptionInterceptor : EmptyDomainInterceptor<Prescription>
    {
        public IDomainService<PrescriptionRealityObject> RoDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<Prescription> service, Prescription entity)
        {
            // Удаляем все дочерние Нарушения
            var domainServiceViolation = Container.Resolve<IDomainService<PrescriptionRealityObject>>();
            var roIds = domainServiceViolation.GetAll().Where(x => x.Prescription.Id == entity.Id)
                .Select(x => x.Id).ToList();

            foreach (var id in roIds)
            {
                RoDomain.Delete(id);
            }
            
            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<Prescription> service, Prescription entity)
        {
            var primaryAppealCitsContragent = PrimaryBaseStatementAppealCitsDomainService.GetAll()
                .Where(x => x.BaseStatementAppealCits.Inspection.Id == entity.Inspection.Id)
                .Select(x => x.BaseStatementAppealCits.Inspection.Contragent)
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            if (primaryAppealCitsContragent != null)
            {
                entity.Contragent = primaryAppealCitsContragent;
            }

            return Success();
        }
    }
}