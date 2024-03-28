namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System.Collections.Generic;

    public class ResolutionDefinitionInterceptor : ResolutionDefinitionInterceptor<ResolutionDefinition>
    {
        
    }

    // Generic класс для определения постановления чтобы было лучше расширять сущности без дублирования кода через subclass
    public class ResolutionDefinitionInterceptor<T> : EmptyDomainInterceptor<T>
        where T : ResolutionDefinition
    {
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.Id.ToString());
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
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNumber);
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
                { "Resolution", "Постановление" },
                { "ExecutionDate", "Дата исполнения" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Description", "Описание" },
                { "FileInfo", "Файл" },
                { "IssuedDefinition", "ДЛ, вынесшее определение" },
                { "TypeDefinition", "Тип определения" }
            };
            return result;
        }
    }
}
