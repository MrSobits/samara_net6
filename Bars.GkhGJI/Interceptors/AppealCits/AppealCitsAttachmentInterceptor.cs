namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;

    public class AppealCitsAttachmentInterceptor : EmptyDomainInterceptor<AppealCitsAttachment>
    {
        public override IDataResult AfterCreateAction(IDomainService<AppealCitsAttachment> service, AppealCitsAttachment entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsAttachment, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsAttachment> service, AppealCitsAttachment entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsAttachment, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsAttachment> service, AppealCitsAttachment entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsAttachment, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.Name);
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
                { "AppealCits", "Обращение" },
                { "FileInfo", "Файл" },
                { "Name", "Наименование" },
                { "Description", "Описание" }
            };
            return result;
        }

    }
}
