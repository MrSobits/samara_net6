namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyPhotoInterceptor : EmptyDomainInterceptor<ActSurveyPhoto>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActSurveyPhoto> service, ActSurveyPhoto entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ActSurveyPhoto, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActSurveyPhoto> service, ActSurveyPhoto entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ActSurveyPhoto, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActSurveyPhoto> service, ActSurveyPhoto entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ActSurveyPhoto, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.Name);
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
                { "ImageDate", "Дата изображения" },
                { "Group", "Группа" },
                { "Name", "Наименование" },
                { "Description", "Описание" },
                { "File", "Файл" }
            };
            return result;
        }
    }
}