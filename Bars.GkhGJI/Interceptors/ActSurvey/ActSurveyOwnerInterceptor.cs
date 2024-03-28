namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyOwnerInterceptor : EmptyDomainInterceptor<ActSurveyOwner>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActSurveyOwner> service, ActSurveyOwner entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ActSurveyOwner, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.Id.ToString() + " " + entity.Fio);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActSurveyOwner> service, ActSurveyOwner entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ActSurveyOwner, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.Fio);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActSurveyOwner> service, ActSurveyOwner entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ActSurveyOwner, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.Fio);
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
                { "Fio", "ФИО" },
                { "Position", "Должность" },
                { "WorkPlace", "Место работы" },
                { "DocumentName", "Правоустанавливающий документ" }
            };
            return result;
        }
    }
}