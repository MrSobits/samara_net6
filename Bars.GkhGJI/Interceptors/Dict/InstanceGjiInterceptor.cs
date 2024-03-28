namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class InstanceGjiInterceptor : EmptyDomainInterceptor<InstanceGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<InstanceGji> service, InstanceGji entity)
        {
            if (Container.Resolve<IDomainService<ResolutionDispute>>().GetAll().Any(x => x.Instance.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Оспаривания в постановлении ГЖИ;");
            }

            return this.Success();
        }
    }
}