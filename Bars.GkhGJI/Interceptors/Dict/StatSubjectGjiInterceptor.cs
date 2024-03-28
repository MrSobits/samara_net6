namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class StatSubjectGjiInterceptor : EmptyDomainInterceptor<StatSubjectGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<StatSubjectGji> service, StatSubjectGji entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                  id => this.Container.Resolve<IDomainService<AppealCitsStatSubject>>().GetAll().Any(x => x.Subject.Id == id) ? "Тематики реестра обращений" : null,
                                  id => Container.Resolve<IDomainService<StatSubjectSubsubjectGji>>().GetAll().Any(x => x.Subject.Id == id) ? "Подтематика тематики" : null,
                                  id => this.Container.Resolve<IDomainService<StatSubsubjectFeatureGji>>().GetAll().Any(x => x.Subsubject.Id == id) ? "Характеристики подтематики" : null
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