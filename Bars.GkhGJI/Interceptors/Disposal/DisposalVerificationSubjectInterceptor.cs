namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalVerificationSubjectInterceptor : EmptyDomainInterceptor<DisposalVerificationSubject>
    {
        public override IDataResult AfterCreateAction(IDomainService<DisposalVerificationSubject> service, DisposalVerificationSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiVerificationSubject, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.Id.ToString() + " " + entity.SurveySubject.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DisposalVerificationSubject> service, DisposalVerificationSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiVerificationSubject, entity.Id, entity.GetType(), GetPropertyValues(), entity.Disposal.DocumentNumber + " " + entity.SurveySubject.Name);
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
                { "Disposal", "Приказ" },
                { "SurveySubject", "Предмет проверки" }
            };
            return result;
        }
    }
}