namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DecisionControlListInterceptor : EmptyDomainInterceptor<DecisionControlList>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionControlList> service, DecisionControlList entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DecisionControlList, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.Id.ToString() + " " + entity.ControlList.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionControlList> service, DecisionControlList entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DecisionControlList, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.ControlList.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionControlList> service, DecisionControlList entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DecisionControlList, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.ControlList.Name);
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
                { "ControlList", "Мероприятие по контролю" },
                { "Description", "Описание мериприятия по контролю" }
            };
            return result;
        }
    }
}