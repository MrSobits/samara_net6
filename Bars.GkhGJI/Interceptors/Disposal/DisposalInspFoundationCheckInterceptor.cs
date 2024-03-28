namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalInspFoundationCheckInterceptor : EmptyDomainInterceptor<DisposalInspFoundationCheck>
    {
        public override IDataResult AfterCreateAction(IDomainService<DisposalInspFoundationCheck> service, DisposalInspFoundationCheck entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DisposalInspFoundationCheck, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.Id.ToString() + " " + entity.InspFoundationCheck.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DisposalInspFoundationCheck> service, DisposalInspFoundationCheck entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DisposalInspFoundationCheck, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.InspFoundationCheck.Name);
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
                { "Disposal", "Распоряжение ГЖИ" },
                { "InspFoundationCheck", "Правовое основание проведения проверки" }
            };
            return result;
        }
    }
}