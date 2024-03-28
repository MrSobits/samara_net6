namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class DisposalDocConfirmInterceptor : EmptyDomainInterceptor<DisposalDocConfirm>
    {
        public override IDataResult AfterCreateAction(IDomainService<DisposalDocConfirm> service, DisposalDocConfirm entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DisposalDocConfirm, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.Id.ToString() + " " + entity.DocumentName);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DisposalDocConfirm> service, DisposalDocConfirm entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DisposalDocConfirm, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.DocumentName);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DisposalDocConfirm> service, DisposalDocConfirm entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DisposalDocConfirm, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.DocumentName);
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
                { "DocumentName", "Наименвоание документа" }
            };
            return result;
        }
    }
}