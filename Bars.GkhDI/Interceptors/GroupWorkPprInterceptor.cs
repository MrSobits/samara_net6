namespace Bars.GkhDi.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class GroupWorkPprInterceptor : EmptyDomainInterceptor<GroupWorkPpr>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<GroupWorkPpr> service, GroupWorkPpr entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<WorkPpr>>().GetAll().Any(x => x.GroupWorkPpr.Id == id) ? "ППР" : null,
                                   id => this.Container.Resolve<IDomainService<WorkRepairList>>().GetAll().Any(x => x.GroupWorkPpr.Id == id) ? "ППР услуги ремонт" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            return this.Success();
        }
    }
}
