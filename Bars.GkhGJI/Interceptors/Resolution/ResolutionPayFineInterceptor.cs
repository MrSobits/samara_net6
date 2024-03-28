namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionPayFineInterceptor : EmptyDomainInterceptor<ResolutionPayFine>
    {
        public override IDataResult AfterCreateAction(IDomainService<ResolutionPayFine> service, ResolutionPayFine entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionPayFine, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<ResolutionPayFine> service, ResolutionPayFine entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionPayFine, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionPayFine> service, ResolutionPayFine entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ResolutionPayFine, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNum);
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
                { "Resolution", "Постановление" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Amount", "Сумма штрафа" },
                { "TypeDocumentPaid", "Тип документа оплаты штрафа" }
            };
            return result;
        }
    }
}