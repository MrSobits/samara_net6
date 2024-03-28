namespace Bars.GkhDi.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class TemplateServiceInterceptor : EmptyDomainInterceptor<TemplateService>
    {
        public override IDataResult AfterUpdateAction(IDomainService<TemplateService> service, TemplateService entity)
        {
            //var serviceBaseService = this.Container.Resolve<IDomainService<BaseService>>();
            //if (!entity.Changeable && entity.UnitMeasure != null)
            //{
            //    serviceBaseService.GetAll()
            //        .Where(x => x.TemplateService.Id == entity.Id)
            //        .ToList()
            //        .ForEach(x => x.UnitMeasure = new UnitMeasure { Id = entity.UnitMeasure.Id });
            //}

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TemplateService> service, TemplateService entity)
        {
            var tsOptFieldService = this.Container.Resolve<IDomainService<TemplateServiceOptionFields>>();
            var baseServService = this.Container.Resolve<IDomainService<BaseService>>();
            var groupWorkPprService = this.Container.Resolve<IDomainService<GroupWorkPpr>>();

            var refFuncs = new List<Func<long, string>>
                               {
                                   id => baseServService.GetAll().Any(x => x.TemplateService.Id == id) ? "Услуги" : null,
                                   id => groupWorkPprService.GetAll().Any(x => x.Service.Id == id) ? "Группа ППР" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            var optFields = tsOptFieldService.GetAll().Where(x => x.TemplateService.Id == entity.Id).Select(x => x.Id).ToList();
            optFields.ForEach(x => tsOptFieldService.Delete(x));

            this.Container.Release(tsOptFieldService);
            this.Container.Release(baseServService);
            this.Container.Release(groupWorkPprService);

            return this.Success();
        }
    }
}
