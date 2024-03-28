namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197SurveySubjectRequirementInterceptor : EmptyDomainInterceptor<Protocol197SurveySubjectRequirement>
    {
        public override IDataResult AfterCreateAction(IDomainService<Protocol197SurveySubjectRequirement> service, Protocol197SurveySubjectRequirement entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ProtocolSurveySubjectRequirement, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.Id.ToString() + " " + entity.Requirement.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<Protocol197SurveySubjectRequirement> service, Protocol197SurveySubjectRequirement entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ProtocolSurveySubjectRequirement, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.DocumentNumber + entity.Requirement.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<Protocol197SurveySubjectRequirement> service, Protocol197SurveySubjectRequirement entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ProtocolSurveySubjectRequirement, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.DocumentNumber + entity.Requirement.Name);
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
                { "Protocol197", "Протокол" },
                { "Requirement", "Перечень требований к субъектам проверки" }
            };
            return result;
        }
    }
}