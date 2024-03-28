namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolRSODefinitionInterceptor : EmptyDomainInterceptor<ProtocolRSODefinition>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolRSODefinition> service, ProtocolRSODefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolRSO.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<ProtocolRSODefinition> service, ProtocolRSODefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolRSO.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolRSODefinition> service, ProtocolRSODefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolRSO.DocumentNumber + " " + entity.DocumentNum);
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
                { "ProtocolRSO", "Протокол прокуратуры" },
                { "ExecutionDate", "Дата исполнения" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Description", "Описание" },
                { "IssuedDefinition", "ДЛ, вынесшее определение" },
                { "TypeDefinition", "Тип определения" }
            };
            return result;
        }
    }
}