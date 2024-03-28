namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionRospotrebnadzorDisputeInterceptor : EmptyDomainInterceptor<ResolutionRospotrebnadzorDispute>
    {
        public override IDataResult AfterCreateAction(IDomainService<ResolutionRospotrebnadzorDispute> service, ResolutionRospotrebnadzorDispute entity)
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

        public override IDataResult AfterUpdateAction(IDomainService<ResolutionRospotrebnadzorDispute> service, ResolutionRospotrebnadzorDispute entity)
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

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionRospotrebnadzorDispute> service, ResolutionRospotrebnadzorDispute entity)
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
                { "Resolution", "Постановление Роспотребнадзора" },
                { "DocumentNum", "Номер документа" },
                { "DocumentDate", "Дата документа" },
                { "Court", "Вид суда" },
                { "Instance", "Инстанция" },
                { "CourtVerdict", "Решение суда" },
                { "Lawyer", "Юрист" },
                { "File", "Файл" },
                { "ProsecutionProtest", "Протест прокуратуры" },
                { "Description", "Описание" }
            };
            return result;
        }
    }
}