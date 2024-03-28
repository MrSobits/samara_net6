namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class DisposalFactViolationInterceptor : EmptyDomainInterceptor<DisposalFactViolation>
    {
        public override IDataResult AfterCreateAction(IDomainService<DisposalFactViolation> service, DisposalFactViolation entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DisposalFactViolation, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.Id.ToString() + " " + entity.TypeFactViolation.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DisposalFactViolation> service, DisposalFactViolation entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DisposalFactViolation, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.TypeFactViolation.Name);
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
                { "Disposal", "Приказ" },
                { "TypeFactViolation", "Факты нарушения" }
            };
            return result;
        }
    }
}