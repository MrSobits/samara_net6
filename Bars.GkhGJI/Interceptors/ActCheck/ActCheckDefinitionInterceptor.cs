namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckDefinitionInterceptor : EmptyDomainInterceptor<ActCheckDefinition>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActCheckDefinition> service, ActCheckDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckDefinition> service, ActCheckDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckDefinition> service, ActCheckDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.DocumentNum);
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
                { "ActCheck", "Акт проверки" },
                { "ExecutionDate", "Дата исполнения" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Description", "Описание" },
                { "IssuedDefinition", "ДЛ, вынесшее определение" },
                { "TypeDefinition", "Тип определения" },
                { "File", "Файл" },
                { "SignedFile", "Подписанный файл" },
                { "DocumentSend", "Дата отправки" },
                { "DocumentDelivered", "Дата получения" }
            };
            return result;
        }
    }
}