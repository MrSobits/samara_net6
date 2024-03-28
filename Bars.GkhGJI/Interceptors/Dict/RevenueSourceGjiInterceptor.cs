namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class RevenueSourceGjiInterceptor : EmptyDomainInterceptor<RevenueSourceGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RevenueSourceGji> service, RevenueSourceGji entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<AppealCitsSource>>().GetAll().Any(x => x.RevenueSource.Id == id) ? "Источник поступления реестра обращений" : null,
                                   id => this.Container.Resolve<IDomainService<AppealCitsAnswer>>().GetAll().Any(x => x.Addressee.Id == id) ? "Ответы по обращению граждан" : null
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