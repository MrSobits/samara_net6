namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DecisionControlMeasuresInterceptor : EmptyDomainInterceptor<DecisionControlMeasures>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionControlMeasures> service, DecisionControlMeasures entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiControlMeasures, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.Id.ToString() + " " + entity.ControlActivity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionControlMeasures> service, DecisionControlMeasures entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiControlMeasures, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.ControlActivity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionControlMeasures> service, DecisionControlMeasures entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiControlMeasures, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.ControlActivity.Name);
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
                { "Decision", "Распоряжение" },
                { "ControlActivity", "Мероприятие по контролю" },
                { "Description", "Описание мериприятия по контролю" },
                { "DateStart", "Дата начала мероприятия по контролю" },
                { "DateEnd", "Дата окончания мероприятия по контролю" }
            };
            return result;
        }
    }
}