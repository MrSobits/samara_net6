namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DecisionAdminRegulationInterceptor : EmptyDomainInterceptor<DecisionAdminRegulation>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionAdminRegulation> service, DecisionAdminRegulation entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAdminRegulation, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.Id.ToString() + " " + entity.AdminRegulation.Name).Substring(0, 150));
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionAdminRegulation> service, DecisionAdminRegulation entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiAdminRegulation, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.DocumentNumber + " " + entity.AdminRegulation.Name).Substring(0, 150));
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
                { "Decision", "Распоряжение ГЖИ" },
                { "AdminRegulation", "Административный регламент" }
            };
            return result;
        }
    }
}