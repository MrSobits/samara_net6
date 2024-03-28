namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseViolInterceptor : EmptyDomainInterceptor<AdministrativeCaseViolation>
    {
        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }

        public IRepository<ActCheckViolation> ActCheckViolDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ProtocolViolation> ProtocolViolDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<AdministrativeCaseViolation> service, AdministrativeCaseViolation entity)
        {
            // проверяем существует ли нарушение в дочерних предписаниях
            var existInPrescription = PrescriptionViolDomain.GetAll()
                                                .Where(x => x.Document.Stage.Parent.Id == (entity.Document.Stage != null ? entity.Document.Stage.Id : 0))
                                                .Any(x => x.InspectionViolation.Id == entity.InspectionViolation.Id);

            if (existInPrescription)
            {
                return Failure("Нарушение не может быть удалено, поскольку содержится в дочернем предписании");
            }

            // проверяем существует ли нарушение в дочерних протоколах
            var existInProtocol = ProtocolViolDomain.GetAll()
                                                .Where(x => x.Document.Stage.Parent.Id == (entity.Document.Stage != null ? entity.Document.Stage.Id : 0))
                                                .Any(x => x.InspectionViolation.Id == entity.InspectionViolation.Id);

            if (existInProtocol)
            {
                return Failure("Нарушение не может быть удалено, поскольку содержится в дочернем протоколе");
            }

            // Перед удалением загружаем
            entity.InspectionViolation = InspectionViolDomain.Load(entity.InspectionViolation.Id);

            return Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AdministrativeCaseViolation> service, AdministrativeCaseViolation entity)
        {
            // Если на базовое нарушение больше нет ссылок то тоже удаляем данное нарушение
            if (!ViolStageDomain.GetAll().Any(x => x.Id != entity.Id && x.InspectionViolation.Id == entity.InspectionViolation.Id))
            {
                InspectionViolDomain.Delete(entity.InspectionViolation.Id);
            }

            return Success();
        }
    }
}