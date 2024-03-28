namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class StageWorkCrServiceInterceptor : EmptyDomainInterceptor<StageWorkCr>
    {
        public override IDataResult BeforeCreateAction(IDomainService<StageWorkCr> service, StageWorkCr entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<StageWorkCr> service, StageWorkCr entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<StageWorkCr> service, StageWorkCr entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<ArchiveSmr>>().GetAll().Any(x => x.StageWorkCr.Id == id) ? "Архив значений в мониторинге СМР" : null,
                                   id => this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll().Any(x => x.StageWorkCr.Id == id) ? "Виды работ объекта КР" : null
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

        private IDataResult ValidateEntity(StageWorkCr entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 50)
            {
                return Failure("Количество знаков в поле Код не должно превышать 50 символов");
            }

            return Success();
        }
    }
}