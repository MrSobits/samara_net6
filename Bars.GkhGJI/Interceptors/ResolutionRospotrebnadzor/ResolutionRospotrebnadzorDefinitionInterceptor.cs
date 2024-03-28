namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionRospotrebnadzorDefinitionInterceptor : EmptyDomainInterceptor<ResolutionRospotrebnadzorDefinition>
    {
        public override IDataResult AfterCreateAction(IDomainService<ResolutionRospotrebnadzorDefinition> service, ResolutionRospotrebnadzorDefinition entity)
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

        public override IDataResult AfterUpdateAction(IDomainService<ResolutionRospotrebnadzorDefinition> service, ResolutionRospotrebnadzorDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionRospotrebnadzorDefinition> service, ResolutionRospotrebnadzorDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNum);
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
                { "Resolution", "Постановление Роспотребнадзора" },
                { "DocumentNum", "Номер документа" },
                { "DocumentDate", "Дата документа" },
                { "TypeDefinition", "Тип определения" },
                { "IssuedDefinition", "ДЛ, вынесшее определение" },
                { "ExecutionDate", "Дата исполнения" },
                { "Description", "Описание" }
            };
            return result;
        }
    }
}