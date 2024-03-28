namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalInspFoundCheckNormDocItemInterceptor : EmptyDomainInterceptor<DisposalInspFoundCheckNormDocItem>
    {
        public override IDataResult AfterCreateAction(IDomainService<DisposalInspFoundCheckNormDocItem> service, DisposalInspFoundCheckNormDocItem entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DisposalInspFoundCheckNormDocItem, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.Id.ToString() + " " + entity.NormativeDocItem.Number);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<DisposalInspFoundCheckNormDocItem> service, DisposalInspFoundCheckNormDocItem entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DisposalInspFoundCheckNormDocItem, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.NormativeDocItem.Number);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DisposalInspFoundCheckNormDocItem> service, DisposalInspFoundCheckNormDocItem entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DisposalInspFoundCheckNormDocItem, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.NormativeDocItem.Number);
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
                { "DisposalInspFoundationCheck", "НПА проверки приказа ГЖИ" },
                { "NormativeDocItem", "Пункт нормативного документа" },
                { "Disposal", "Распоряжение ГЖИ" }
            };
            return result;
        }
    }
}