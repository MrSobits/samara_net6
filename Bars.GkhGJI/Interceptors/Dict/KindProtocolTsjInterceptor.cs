namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class KindProtocolTsjInterceptor : EmptyDomainInterceptor<KindProtocolTsj>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<KindProtocolTsj> service, KindProtocolTsj entity)
        {
            if (Container.Resolve<IDomainService<ActivityTsjProtocol>>().GetAll().Any(x => x.KindProtocolTsj.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Протоколы деятельности ТСЖ;");
            }

            return this.Success();
        }
    }
}