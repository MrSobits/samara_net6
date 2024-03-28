
namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class NsoActCheckServiceInterceptor : ActCheckServiceInterceptor<NsoActCheck>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<NsoActCheck> service, NsoActCheck entity)
        {
            var provDocService = this.Container.Resolve<IDomainService<ActCheckProvidedDoc>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => provDocService.GetAll().Any(x => x.ActCheck.Id == id) ? "Предоставленные документы" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(provDocService);
            }
        }
    }
    
}