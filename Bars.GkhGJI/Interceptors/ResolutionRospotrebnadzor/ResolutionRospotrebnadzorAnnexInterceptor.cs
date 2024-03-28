namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionRospotrebnadzorAnnexInterceptor : EmptyDomainInterceptor<ResolutionRospotrebnadzorAnnex>
    {
        public override IDataResult AfterCreateAction(IDomainService<ResolutionRospotrebnadzorAnnex> service, ResolutionRospotrebnadzorAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.DocumentName);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ResolutionRospotrebnadzorAnnex> service, ResolutionRospotrebnadzorAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentName);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionRospotrebnadzorAnnex> service, ResolutionRospotrebnadzorAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentName);
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
                { "DocumentName", "Наименование" },
                { "DocumentDate", "Дата документа" },
                { "File", "Файл" },
                { "Description", "Описание" },
                { "SignedFile", "Подписанный файл" },
                { "MessageCheck", "Статус файла" }
            };
            return result;
        }
    }
}