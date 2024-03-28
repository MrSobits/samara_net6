namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;

    public class AppealCitsQuestionInterceptor : EmptyDomainInterceptor<AppealCitsQuestion>
    {
        public override IDataResult AfterCreateAction(IDomainService<AppealCitsQuestion> service, AppealCitsQuestion entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var questDomain = Container.ResolveDomain<QuestionKind>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsQuestion, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + questDomain.Get(entity.QuestionKind.Id).Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsQuestion> service, AppealCitsQuestion entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsQuestion, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.QuestionKind.Name);
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
                { "QuestionKind", "Вид вопроса" }
            };
            return result;
        }

    }
}
