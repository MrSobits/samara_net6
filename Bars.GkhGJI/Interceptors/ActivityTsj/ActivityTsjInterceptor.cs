namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Интерцептор для Деятельности ТСЖ (Не путать с проверкой по дейтельности ТСЖ)
    /// </summary>
    public class ActivityTsjInterceptor : EmptyDomainInterceptor<ActivityTsj>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActivityTsj> service, ActivityTsj entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<ActivityTsjProtocol>>().GetAll().Any(x => x.ActivityTsj.Id == id) ? "Протоколы деятельности ТСЖ" : null,
                                   id => this.Container.Resolve<IDomainService<ActivityTsjRealObj>>().GetAll().Any(x => x.ActivityTsj.Id == id) ? "Дома деятельности ТСЖ" : null,
                                   id => this.Container.Resolve<IDomainService<ActivityTsjStatute>>().GetAll().Any(x => x.ActivityTsj.Id == id) ? "Уставы деятельности ТСЖ" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
            }

            if (!string.IsNullOrEmpty(message))
            {
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            return Success();
        }
    }
}