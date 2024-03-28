namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionDisputeInterceptor : EmptyDomainInterceptor<ResolutionDispute>
    {
        public override IDataResult AfterCreateAction(IDomainService<ResolutionDispute> service, ResolutionDispute entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionDispute, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<ResolutionDispute> service, ResolutionDispute entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionDispute, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionDispute> service, ResolutionDispute entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ResolutionDispute, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNum);
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
                { "Court", "Вид суда" },
                { "Instance", "Инстанция" },
                { "CourtVerdict", "Решение суда" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Description", "Описание" },
                { "Appeal", "Постановление обжаловано" },
                { "ProsecutionProtest", "Протест прокуратуры" },
                { "Lawyer", "Юрист" },
                { "File", "Файл" }
            };
            return result;
        }
    }
}