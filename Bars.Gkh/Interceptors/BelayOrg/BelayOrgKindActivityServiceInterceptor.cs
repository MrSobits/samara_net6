namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    public class BelayOrgKindActivityServiceInterceptor : EmptyDomainInterceptor<BelayOrgKindActivity>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<BelayOrgKindActivity> service, BelayOrgKindActivity entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<BelayPolicy>>().GetAll().Any(x => x.BelayOrgKindActivity.Id == id) ? "Страховые полисы" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            return Success();
        }
    }
}
