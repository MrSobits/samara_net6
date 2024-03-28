namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyAnnexInterceptor : EmptyDomainInterceptor<ActSurveyAnnex>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActSurveyAnnex> service, ActSurveyAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActSurveyAnnex> service, ActSurveyAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActSurveyAnnex> service, ActSurveyAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.Name);
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
                { "ActSurvey", "Акт обследования" },
                { "DocumentDate", "Дата документа" },
                { "Name", "Наименование" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "SignedFile", "Подписанный файл" },
                { "MessageCheck", "Статус файла" }
            };
            return result;
        }
    }
}