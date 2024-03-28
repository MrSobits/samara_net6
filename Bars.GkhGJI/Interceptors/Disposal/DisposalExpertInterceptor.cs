namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalExpertInterceptor : EmptyDomainInterceptor<DisposalExpert>
    {
        public override IDataResult AfterCreateAction(IDomainService<DisposalExpert> service, DisposalExpert entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiExpert, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.Id.ToString() + " " + entity.Expert.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DisposalExpert> service, DisposalExpert entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiExpert, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.Expert.Name);
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
                { "Disposal", "Распоряжение" },
                { "Expert", "Эксперт" }
            };
            return result;
        }
    }
}