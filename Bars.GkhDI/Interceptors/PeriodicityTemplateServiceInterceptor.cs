namespace Bars.GkhDi.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    public class PeriodicityTemplateServiceInterceptor : EmptyDomainInterceptor<PeriodicityTemplateService>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PeriodicityTemplateService> service, PeriodicityTemplateService entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PeriodicityTemplateService> service, PeriodicityTemplateService entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PeriodicityTemplateService> service, PeriodicityTemplateService entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>().GetAll().Any(x => x.PeriodicityTemplateService.Id == id) ? "Работы плана работ по содержанию и ремонту" : null,
                                   id => this.Container.Resolve<IDomainService<AdditionalService>>().GetAll().Any(x => x.Periodicity.Id == id) ? "Дополнительная услуга" : null,
                                   id => this.Container.Resolve<IDomainService<HousingService>>().GetAll().Any(x => x.Periodicity.Id == id) ? "Жилищная услуга" : null
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

        private IDataResult CheckForm(PeriodicityTemplateService entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
