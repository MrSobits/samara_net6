namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class DocumentViolGroupInterceptor : EmptyDomainInterceptor<DocumentViolGroup>
    {
        public IDomainService<PrescriptionViol> prescriptionViolDomain { get; set; }

        public override IDataResult AfterDeleteAction(IDomainService<DocumentViolGroup> service, DocumentViolGroup entity)
        {
            var longTextDomain = Container.Resolve<IDomainService<DocumentViolGroupLongText>>();
            var pointDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();

            try
            {
                // удаляем Blop записи
                longTextDomain.GetAll()
                              .Where(x => x.ViolGroup.Id == entity.Id)
                              .Select(x => x.Id)
                              .ForEach(x => longTextDomain.Delete(x));

                // удаляем связи нарушения с группой нарушений
                pointDomain.GetAll()
                    .Where(x => x.ViolGroup.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => pointDomain.Delete(x));

                return this.Success();
            }
            finally
            {
                Container.Release(longTextDomain);
                Container.Release(pointDomain);
            }

        }

        public override IDataResult BeforeUpdateAction(IDomainService<DocumentViolGroup> service, DocumentViolGroup entity)
        {
            var pointsDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();
            var prescriptionViolStageDomain = Container.Resolve<IDomainService<PrescriptionViol>>();
            var actRemovalViolStageDomain = Container.Resolve<IDomainService<ActRemovalViolation>>();

            try
            {
                
                if (entity.Document.TypeDocumentGji == TypeDocumentGji.Prescription)
                {
                    
                    var pointViols = prescriptionViolStageDomain.GetAll().Where(y => pointsDomain.GetAll().Any(x => x.ViolGroup.Id == entity.Id && x.ViolStage.Id == y.Id)).ToList();

                    // Если даты изменились то необходимо пройти по связанным с этой группой нарушениям и изменить вней дату
                    foreach (var viol in pointViols)
                    {
                        if (viol.DatePlanRemoval != entity.DatePlanRemoval)
                        {
                            viol.DatePlanRemoval = entity.DatePlanRemoval;

                            prescriptionViolStageDomain.Update(viol);
                        }
                    }
                }
                else if (entity.Document.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                {
                    var pointViols = actRemovalViolStageDomain.GetAll().Where(y => pointsDomain.GetAll().Any(x => x.ViolGroup.Id == entity.Id && x.ViolStage.Id == y.Id)).ToList();

                    // Если даты изменились то необходимо пройти по связанным с этой группой нарушениям и изменить вней дату
                    foreach (var viol in pointViols)
                    {
                        if (viol.DateFactRemoval != entity.DateFactRemoval)
                        {
                            viol.DateFactRemoval = entity.DateFactRemoval;

                            actRemovalViolStageDomain.Update(viol);
                        }
                    } 
                }

                return this.Success();
            }
            finally 
            {
                Container.Release(pointsDomain);
                Container.Release(prescriptionViolStageDomain);
                Container.Release(actRemovalViolStageDomain);
            }
        }
    }
}