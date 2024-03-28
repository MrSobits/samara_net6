namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ProvidedDocGjiInterceptor : EmptyDomainInterceptor<ProvidedDocGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ProvidedDocGji> service, ProvidedDocGji entity)
        {
            if (Container.Resolve<IDomainService<DisposalProvidedDoc>>().GetAll().Any(x => x.ProvidedDoc.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Предоставляемые документы рапоряжения ГЖИ;");
            }

            return this.Success();
        }
    }
}