namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyInspectedPartInterceptor : EmptyDomainInterceptor<ActSurveyInspectedPart>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActSurveyInspectedPart> service, ActSurveyInspectedPart entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiInspectedPart, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.Id.ToString() + " " + entity.InspectedPart.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActSurveyInspectedPart> service, ActSurveyInspectedPart entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiInspectedPart, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.InspectedPart.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActSurveyInspectedPart> service, ActSurveyInspectedPart entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiInspectedPart, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActSurvey.DocumentNumber + " " + entity.InspectedPart.Name);
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
                { "InspectedPart", "Инспектируемая часть" },
                { "Character", "Характер и местоположение" },
                { "Description", "Примечание" }
            };
            return result;
        }
    }
}