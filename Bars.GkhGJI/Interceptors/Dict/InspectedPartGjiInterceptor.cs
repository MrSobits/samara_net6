namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class InspectedPartGjiInterceptor : EmptyDomainInterceptor<InspectedPartGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<InspectedPartGji> service, InspectedPartGji entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                  id => this.Container.Resolve<IDomainService<ActCheckInspectedPart>>().GetAll().Any(x => x.InspectedPart.Id == id) ? "Инспектируемые части в акте проверки ГЖИ" : null,
                                  id => this.Container.Resolve<IDomainService<ActSurveyInspectedPart>>().GetAll().Any(x => x.InspectedPart.Id == id) ? "Инспектируемые части в акте обследования ГЖИ" : null
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