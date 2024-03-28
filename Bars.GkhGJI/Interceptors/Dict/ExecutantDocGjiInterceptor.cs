namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ExecutantDocGjiInterceptor : EmptyDomainInterceptor<ExecutantDocGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ExecutantDocGji> service, ExecutantDocGji entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                  id => this.Container.Resolve<IDomainService<Prescription>>().GetAll().Any(x => x.Executant.Id == id) ? "Предписания" : null,
                                  id => this.Container.Resolve<IDomainService<Presentation>>().GetAll().Any(x => x.Executant.Id == id) ? "Представления" : null,
                                  id => this.Container.Resolve<IDomainService<Protocol>>().GetAll().Any(x => x.Executant.Id == id) ? "Протоколы" : null,
                                  id => this.Container.Resolve<IDomainService<ResolPros>>().GetAll().Any(x => x.Executant.Id == id) ? "Постановления прокуратуры" : null,
                                  id => this.Container.Resolve<IDomainService<Resolution>>().GetAll().Any(x => x.Executant.Id == id) ? "Постановления" : null
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