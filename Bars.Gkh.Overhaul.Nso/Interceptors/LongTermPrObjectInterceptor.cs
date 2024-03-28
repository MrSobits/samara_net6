using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Utils;

    public class LongTermPrObjectInterceptor : EmptyDomainInterceptor<LongTermPrObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<LongTermPrObject> service, LongTermPrObject entity)
        {
            return Success();
        }
    }
}