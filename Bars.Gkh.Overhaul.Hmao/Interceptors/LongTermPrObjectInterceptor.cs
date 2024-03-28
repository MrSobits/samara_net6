using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using Bars.B4;

    public class LongTermPrObjectInterceptor : EmptyDomainInterceptor<LongTermPrObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<LongTermPrObject> service, LongTermPrObject entity)
        {
            return Success();
        }
    }
}