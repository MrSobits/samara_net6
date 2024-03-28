namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    public class ChelyabinskProtocolSurveySubjectRequirementInterceptor : EmptyDomainInterceptor<ChelyabinskProtocolSurveySubjectRequirement>
    {
        public override IDataResult AfterCreateAction(IDomainService<ChelyabinskProtocolSurveySubjectRequirement> service, ChelyabinskProtocolSurveySubjectRequirement entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ProtocolSurveySubjectRequirement, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.Id.ToString() + " " + entity.Requirement.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ChelyabinskProtocolSurveySubjectRequirement> service, ChelyabinskProtocolSurveySubjectRequirement entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ProtocolSurveySubjectRequirement, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + entity.Requirement.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ChelyabinskProtocolSurveySubjectRequirement> service, ChelyabinskProtocolSurveySubjectRequirement entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ProtocolSurveySubjectRequirement, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + entity.Requirement.Name);
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
                { "Requirement", "Перечень требований к субъектам првоерки" },
                { "Protocol", "Протокол" }
            };
            return result;
        }
    }
}