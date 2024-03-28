namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DecisionVerificationSubjectInterceptor : EmptyDomainInterceptor<DecisionVerificationSubject>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionVerificationSubject> service, DecisionVerificationSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiVerificationSubject, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.Id.ToString() + " " + entity.SurveySubject.Name).Substring(0, 150));
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionVerificationSubject> service, DecisionVerificationSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiVerificationSubject, entity.Id, entity.GetType(), GetPropertyValues(), (entity.Decision.DocumentNumber + " " + entity.SurveySubject.Name).Substring(0, 150));
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
                { "Decision", "Приказ" },
                { "SurveySubject", "Предмет проверки" }
            };
            return result;
        }
    }
}