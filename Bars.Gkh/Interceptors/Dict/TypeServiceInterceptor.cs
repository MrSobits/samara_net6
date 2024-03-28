namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class TypeServiceInterceptor : EmptyDomainInterceptor<TypeService>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TypeService> service, TypeService entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TypeService> service, TypeService entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TypeService> service, TypeService entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<ServiceOrgService>>().GetAll().Any(x => x.TypeService.Id == id) ? "Услуги поставщика жилищных услуг" : null,
                                   id => this.Container.Resolve<IDomainService<SupplyResourceOrgService>>().GetAll().Any(x => x.TypeService.Id == id) ? "Услуги Поставщик коммунальных услуг" : null
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

        private IDataResult ValidateEntity(TypeService entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
