namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System.Collections.Generic;

    public class ProtocolDefinitionInterceptor : ProtocolDefinitionInterceptor<ProtocolDefinition>
    {
        
    }

    // Generic класс для определения протокола чтобы было лучше расширят ьсущности без дублирования кода через subclass
    public class ProtocolDefinitionInterceptor<T> : EmptyDomainInterceptor<T>
        where T : ProtocolDefinition
    {
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + " " + entity.DocumentNumber);
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
                { "Protocol", "Протокол" },
                { "ExecutionDate", "Дата исполнения" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Description", "Описание" },
                { "IssuedDefinition", "ДЛ, вынесшее определение" },
                { "SignedBy", "документ выдан" },
                { "TypeDefinition", "Тип определения" },
                { "TimeDefinition", "Время начала" },
                { "DateOfProceedings", "Дата рассмотрения дела" },
                { "FileInfo", "Файл" }
            };
            return result;
        }
    }
}
