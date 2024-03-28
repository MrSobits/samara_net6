namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;


    public class ViolationGjiInterceptor : ViolationGjiInterceptor<ViolationGji>
    {
    }


    public class ViolationGjiInterceptor<T> : EmptyDomainInterceptor<T>
        where T : ViolationGji
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            return CheckViolationGjiForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            return CheckViolationGjiForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var docDomain = Container.ResolveDomain<ViolationNormativeDocItemGji>();
                
            docDomain
                .GetAll().Where(x => x.ViolationGji.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => docDomain.Delete(x));

            var refFuncs = new List<Func<long, string>>
                               {
                                   id => Container.Resolve<IDomainService<ViolationFeatureGji>>().GetAll().Any(x => x.ViolationGji.Id == id) ? "Характеристики нарушений" : null,
                                   id => Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll().Any(x => x.Violation.Id == id) ? "Нарушение проверки ГЖИ" : null
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

        private IDataResult CheckViolationGjiForm(ViolationGji entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнены обязательные поля: Текст нарушения");
            }

            if (entity.Name.Length > 2000)
            {
                return Failure("Количество знаков в поле Текст не должно превышать 2000 символов");
            }

            return Success();
        }
    }
}