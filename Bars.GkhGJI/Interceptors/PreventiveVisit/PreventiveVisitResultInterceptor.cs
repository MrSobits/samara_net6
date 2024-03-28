namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PreventiveVisitResultInterceptor : EmptyDomainInterceptor<PreventiveVisitResult>
    {
        public override IDataResult AfterCreateAction(IDomainService<PreventiveVisitResult> service, PreventiveVisitResult entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PreventiveVisitResult, entity.Id, entity.GetType(), GetPropertyValues(), entity.PreventiveVisit.Id.ToString() + " " + entity.RealityObject.Address);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<PreventiveVisitResult> service, PreventiveVisitResult entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PreventiveVisitResult, entity.Id, entity.GetType(), GetPropertyValues(), entity.PreventiveVisit.DocumentNumber + " " + entity.RealityObject.Address);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PreventiveVisitResult> service, PreventiveVisitResult entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.PreventiveVisitResult, entity.Id, entity.GetType(), GetPropertyValues(), entity.PreventiveVisit.DocumentNumber + " " + entity.RealityObject.Address);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "PreventiveVisit", "Документ ГЖИ" },
                { "RealityObject", "Проверяемый объект" },
                { "InformText", "Инспектор" }
            };
            return result;
        }
    }
}