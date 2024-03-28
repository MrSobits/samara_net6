namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PreventiveVisitWitnessInterceptor : EmptyDomainInterceptor<PreventiveVisitWitness>
    {
        public override IDataResult AfterCreateAction(IDomainService<PreventiveVisitWitness> service, PreventiveVisitWitness entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiWitness, entity.Id, entity.GetType(), GetPropertyValues(), entity.PreventiveVisit.Id.ToString() + " " + entity.Fio);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<PreventiveVisitWitness> service, PreventiveVisitWitness entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiWitness, entity.Id, entity.GetType(), GetPropertyValues(), entity.PreventiveVisit.DocumentNumber + " " + entity.Fio);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PreventiveVisitWitness> service, PreventiveVisitWitness entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiWitness, entity.Id, entity.GetType(), GetPropertyValues(), entity.PreventiveVisit.DocumentNumber + " " + entity.Fio);
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
                { "PreventiveVisit", "Акт проверки" },
                { "Position", "Должность" },
                { "Fio", "ФИО" }
            };
            return result;
        }
    }
}