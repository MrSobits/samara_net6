namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;

    public class AppealCitsDecisionInterceptor : EmptyDomainInterceptor<AppealCitsDecision>
    {
        public override IDataResult AfterCreateAction(IDomainService<AppealCitsDecision> service, AppealCitsDecision entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsDecision, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsDecision> service, AppealCitsDecision entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsDecision, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsDecision> service, AppealCitsDecision entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsDecision, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
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
                { "DocumentName", "Документ" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentDate", "Дата документа" },
                { "IssuedBy", "Выдан" },
                { "SignedBy", "Подписан" },
                { "Resolution", "Постановление" },
                { "Apellant", "Заявитель" },
                { "ApellantPosition", "Должность заявителя" },
                { "ApellantPlaceWork", "Место работы заявителя" },
                { "TypeDecisionAnswer", "Вид КНД" },
                { "TypeAppelantPresence", "Наличие заявителя" },
                { "RepresentativeFio", "ФИО представителя" },
            };
            return result;
        }

    }
}
